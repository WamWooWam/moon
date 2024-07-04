/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * style.cpp:
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */


#include <config.h>
#include "style.h"
#include "error.h"
#include "deployment.h"
#include "provider.h"

namespace Moonlight {

    //
    // Style
    //

    Style::Style() {
        SetObjectType(Type::STYLE);
    }

    Style::~Style() {
    }

    void
    Style::OnPropertyChanged(PropertyChangedEventArgs *args, MoonError *error) {
        if (args->GetProperty()->GetOwnerType() != Type::STYLE) {
            DependencyObject::OnPropertyChanged(args, error);
            return;
        }

        if (args->GetId() == Style::BasedOnProperty) {
            if (GetIsSealed()) {
                MoonError::FillIn(error, MoonError::INVALID_OPERATION, "Cannot change BasedOn on a sealed style");
                return;
            }
        }

        NotifyListenersOfPropertyChange(args, error);
    }

    void
    Style::OnIsAttachedChanged(bool value) {
        // printf ("Style::OnIsAttachedChanged\n");
        DependencyObject::OnIsAttachedChanged(value);
        if (!value) {
            // printf ("emitting Style::DetachedEvent\n");
            Emit(Style::DetachedEvent);
        }
    }

    void
    Style::OnCollectionChanged(Collection *col, CollectionChangedEventArgs *args) {
        // printf ("OnCollectionChanged");
        switch (args->GetChangedAction()) {
        case CollectionChangedActionAdd:
            // printf (" (Add)\n");
            break;
        case CollectionChangedActionRemove:
            // printf (" (Remove)\n");
            break;
        case CollectionChangedActionReplace:
            // printf (" (Replace)\n");
            break;
        case CollectionChangedActionClearing:
            // printf (" (Clearing)\n");
            break;
        case CollectionChangedActionCleared:
            // printf (" (Cleared)\n");
            break;
        }
    }

    void
    Style::OnCollectionItemChanged(Collection *col, DependencyObject *obj, PropertyChangedEventArgs *args) {
        // printf ("OnCollectionItemChanged (%s)\n", args->GetProperty()->GetName());
    }

    void
    Style::Seal() {
        Application *application;

        if (GetIsSealed())
            return;

        application = Application::GetCurrent();
        if (application == NULL)
            return;

        application->ConvertSetterValues(this);
        SetIsSealed(true);
        GetSetters()->Seal();

        Style *s = GetBasedOn();
        if (s)
            s->Seal();
    }

    //
    // SetterBaseCollection
    //

    SetterBaseCollection::SetterBaseCollection() {
        SetObjectType(Type::SETTERBASE_COLLECTION);
    }

    void
    SetterBaseCollection::Seal() {
        SetIsSealed(true);

        CollectionIterator *iter = GetIterator();
        MoonError err;

        Value *current;
        Types *types = Deployment::GetCurrent()->GetTypes();

        while (iter->Next(&err) && (current = iter->GetCurrent(&err))) {
            SetterBase *setter = current->AsSetterBase(types);
            setter->Seal();
        }

        delete iter;
    }

    bool
    SetterBaseCollection::AddedToCollection(Value *value, MoonError *error) {
        if (!value || !ValidateSetter(value, error))
            return false;

        SetterBase *setter = value->AsSetterBase();
        if (setter) {
            setter->SetAttached(true);
            setter->Seal();
        }

        return DependencyObjectCollection::AddedToCollection(value, error);
    }

    void
    SetterBaseCollection::RemovedFromCollection(Value *value, bool is_value_safe) {
        if (is_value_safe) {
            SetterBase *setter = value->AsSetterBase();
            if (setter)
                setter->SetAttached(false);
        }
        DependencyObjectCollection::RemovedFromCollection(value, is_value_safe);
    }

    bool
    SetterBaseCollection::ValidateSetter(Value *value, MoonError *error) {
        Types *types = Deployment::GetCurrent()->GetTypes();

        if (types->IsSubclassOf(value->GetKind(), Type::SETTER)) {
            Setter *s = value->AsSetter(types);
            if (!s->GetValue(Setter::PropertyProperty)) {
                MoonError::FillIn(error, MoonError::EXCEPTION, "Cannot have a null PropertyProperty value");
                return false;
            }
            if (!s->ReadLocalValue(Setter::ValueProperty)) {
                MoonError::FillIn(error, MoonError::EXCEPTION, "Cannot have a null ValueProperty value");
                return false;
            }
        }

        if (types->IsSubclassOf(value->GetKind(), Type::SETTERBASE)) {
            SetterBase *s = value->AsSetterBase(types);
            if (s->GetAttached()) {
                MoonError::FillIn(error, MoonError::INVALID_OPERATION, "Setter is currently attached to another style");
                return false;
            }
        }

        if (GetIsSealed()) {
            MoonError::FillIn(error, MoonError::EXCEPTION, "Cannot add a setter to a sealed style");
            return false;
        }

        return true;
    }


    //
    // SetterBase
    //

    SetterBase::SetterBase() {
        SetObjectType(Type::SETTERBASE);
        attached = false;
    }

    void
    SetterBase::Seal() {
        if (GetIsSealed())
            return;
        SetIsSealed(true);
    }

    bool
    SetterBase::GetAttached() {
        return attached;
    }

    void
    SetterBase::SetAttached(bool value) {
        attached = value;
    }


    //
    // Setter
    //

    Setter::Setter() {
        SetObjectType(Type::SETTER);
    }

    Setter::~Setter() {
    }

    class StyleNode : public List::Node {
    public:
        StyleNode(Style *style) {
            this->style = style;
        }
        Style *style;
    };

    DeepStyleWalker::DeepStyleWalker(Style *style, Types *types) {
        // Create a list of all Setters in the style sorted by their DP.
        // Use the hashtable to ensure that we only take the first setter
        // declared for each DP (i.e. if the BasedOn style and main style
        // have setters for the same DP, we ignore the BasedOn one

        // NOTE: This can be pre-computed and cached as once a style is
        // sealed it can never be changed.

        this->offset = 0;
        this->types = types || !style ? types : style->GetDeployment()->GetTypes();
        this->setter_list = g_ptr_array_new();
        GHashTable *dps = g_hash_table_new(g_direct_hash, g_direct_equal);

        while (style != NULL) {
            SetterBaseCollection *setters = style->GetSetters();
            int count = setters ? setters->GetCount() : 0;
            for (int i = count - 1; i >= 0; i--) {
                Value *v = setters->GetValueAt(i);
                if (Value::IsNull(v) || !types->IsSubclassOf(v->GetKind(), Type::SETTER))
                    continue;

                Setter *setter = v->AsSetter();
                Value *dpVal = setter->GetValue(Setter::PropertyProperty);
                if (Value::IsNull(dpVal))
                    continue;

                DependencyProperty *prop = dpVal->AsDependencyProperty();
                if (!g_hash_table_lookup_extended(dps, prop, NULL, NULL)) {
                    g_hash_table_insert(dps, prop, setter);
                    g_ptr_array_add(setter_list, setter);
                }
            }
            style = style->GetBasedOn();
        }

        g_hash_table_destroy(dps);
        g_ptr_array_sort(setter_list, SetterComparer);
    }

    DeepStyleWalker::DeepStyleWalker(Style **styles, Types *types) {
        // Create a list of all Setters in the style sorted by their DP.
        // Use the hashtable to ensure that we only take the first setter
        // declared for each DP (i.e. if the BasedOn style and main style
        // have setters for the same DP, we ignore the BasedOn one

        // NOTE: This can be pre-computed and cached as once a style is
        // sealed it can never be changed.

        this->offset = 0;
        this->types = types || !styles[0] ? types : styles[0]->GetDeployment()->GetTypes();
        this->setter_list = g_ptr_array_new();

        if (!styles)
            return;

        GHashTable *styles_seen = g_hash_table_new(g_direct_hash, g_direct_equal);
        GHashTable *dps = g_hash_table_new(g_direct_hash, g_direct_equal);

        for (int i = 0; i < ImplicitStylePropertyValueProvider::StyleIndexCount; i++) {
            Style *style = styles[i];
            while (style != NULL) {
                if (g_hash_table_lookup(styles_seen, style))
                    continue;

                SetterBaseCollection *setters = style->GetSetters();
                int count = setters ? setters->GetCount() : 0;
                for (int i = count - 1; i >= 0; i--) {
                    Value *v = setters->GetValueAt(i);
                    if (Value::IsNull(v) || !types->IsSubclassOf(v->GetKind(), Type::SETTER))
                        continue;

                    Setter *setter = v->AsSetter();
                    Value *dpVal = setter->GetValue(Setter::PropertyProperty);
                    if (Value::IsNull(dpVal))
                        continue;

                    DependencyProperty *prop = dpVal->AsDependencyProperty();
                    if (!g_hash_table_lookup_extended(dps, prop, NULL, NULL)) {
                        g_hash_table_insert(dps, prop, setter);
                        g_ptr_array_add(setter_list, setter);
                    }
                }

                g_hash_table_insert(styles_seen, style, style);
                style = style->GetBasedOn();
            }
        }

        g_hash_table_destroy(dps);
        g_hash_table_destroy(styles_seen);
        g_ptr_array_sort(setter_list, SetterComparer);
    }

    gint
    DeepStyleWalker::SetterComparer(gconstpointer left, gconstpointer right) {
        Setter *l = *(Setter **)left;
        Setter *r = *(Setter **)right;

        DependencyProperty *lprop = l->GetValue(Setter::PropertyProperty)->AsDependencyProperty();
        DependencyProperty *rprop = r->GetValue(Setter::PropertyProperty)->AsDependencyProperty();

        if (lprop == rprop)
            return 0;
        return lprop > rprop ? 1 : -1;
    }

    DeepStyleWalker::~DeepStyleWalker() {
        g_ptr_array_free(setter_list, true);
    }

    Setter *
    DeepStyleWalker::Step() {
        if (offset < (int)setter_list->len)
            return (Setter *)setter_list->pdata[offset++];
        return NULL;
    }

};
