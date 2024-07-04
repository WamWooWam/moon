/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * validators.cpp:
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 */

#include <config.h>

#include <math.h>

#include "validators.h"
#include "thickness.h"
#include "cornerradius.h"
#include "style.h"
#include "frameworkelement.h"
#include "animation.h"
#include "propertypath.h"
#include "namescope.h"
#include "deployment.h"
#include "fonts.h"
#include "designmode.h"
#include "runtime.h"
#include "security.h"
#include "notificationwindow.h"

namespace Moonlight {

    bool
    Validators::AudioStreamIndexValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        // A value of -1 is converted to null. Any other value is left as-is
        if (value && value->AsInt32() == -1)
            value->SetIsNull(true);

        return true;
    }

    bool
    Validators::BalanceValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (value) {
            if (value->AsDouble() > 1.0) {
                *value = Value(1.0);
            }
            else if (value->AsDouble() < -1.0) {
                *value = Value(-1.0);
            }
        }

        return true;
    }

    bool
    Validators::VolumeValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (value) {
            if (value->AsDouble() > 1.0) {
                *value = Value(1.0);
            }
            else if (value->AsDouble() < 0.0) {
                *value = Value(0.0);
            }
        }

        return true;
    }

    bool
    Validators::CursorValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        return true;
    }

    bool
    Validators::default_validator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        return true;
    }

    bool
    Validators::PositiveIntValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (value->AsInt32() < 0) {
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Value must be greater than or equal to zero");
            return false;
        }
        return true;
    }

    bool
    Validators::IntGreaterThanZeroValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (value->AsInt32() < 1) {
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Value must be greater than zero");
            return false;
        }
        return true;
    }

    bool
    Validators::IsInputMethodEnabledValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!instance->Is(Type::TEXTBOX) && !instance->Is(Type::RICHTEXTBOX)) {
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Target object must be a TextBox");
            return false;
        }
        return true;
    }

    bool
    Validators::DoubleGreaterThanZeroValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (value->AsDouble() <= 0) {
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Value must be greater than zero");
            return false;
        }
        return true;
    }

    bool
    Validators::DoubleNotNegativeValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (value->AsDouble() < 0) {
            // Silverlight 1.0 apps are allowed set negative sizes here.
            if (instance->GetDeployment()->GetRuntimeVersion()[0] == '1')
                return true;

            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Value must be greater than zero");
            return false;
        }
        return true;
    }

    bool
    Validators::NonNullValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!value || value->GetIsNull()) {
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Value cannot be null");
            return false;
        }

        return true;
    }

    bool
    Validators::NonNullExceptionValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!value || value->GetIsNull()) {
            MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Value cannot be null");
            return false;
        }

        return true;
    }
    bool
    Validators::NotNullOrEmptyValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!value || value->GetIsNull() || strlen(value->AsString()) == 0) {
            MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Value cannot be null");
            return false;
        }

        return true;
    }

    bool
    Validators::MediaAttributeCollectionValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!value || value->GetIsNull()) {
            MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Value cannot be null");
            return false;
        }
        return true;
    }

    bool
    Validators::TemplateValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (instance->Is(Type::USERCONTROL)) {
            MoonError::FillIn(error, MoonError::INVALID_OPERATION, 1001, "Cannot set the template property on a UserControl");
            return false;
        }
        if (!Value::IsNull(value)) {
            Types *types = instance->GetDeployment()->GetTypes();
            ControlTemplate *t = value->AsControlTemplate(types);
            if (!types->IsSubclassOf(instance->GetObjectType(), t->GetTargetType()->kind)) {
                MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "ControlTemplate.TargetType is not compatible with the Controls type");
                return false;
            }
        }
        return true;
    }

    bool
    Validators::LanguageValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        const char *lang = value ? value->AsXmlLanguage() : NULL;

        if (!lang || !IsValidLang(lang)) {
            // FIXME: is 2203 the correct error number?
            MoonError::FillIn(error, MoonError::EXCEPTION, 2203, "Language");
            return false;
        }

        return true;
    }

    bool
    Validators::NameValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        NameScope *scope = instance->FindNameScope();
        if (scope && value) {
            DependencyObject *o = scope->FindName(value->AsString());
            if (o && o != instance) {
                MoonError::FillIn(error, MoonError::ARGUMENT, 2028,
                    g_strdup_printf("The name already exists in the tree: %s (%p %p).",
                        value->AsString(), o, instance));
                return false;
            }
        }
        // TODO: Name validation
        // This doesn't happen in 1.0 or 2.0b according to my tests, but according to the 2.0 docs
        // names need to start with a '_' or letter.  They can't start with a _.  Also characters
        // should be limited to a-z A-Z 0-9 and _.  Once a newer beta actually enforces this
        // I'll implement the validation method.
        return true;
    }

    bool RangeCheck(double d) {
        bool b = (d > -(1E300)) && (d < (1E300));
        return b;
    }

    bool
    Validators::BorderThicknessValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        Thickness t = *value->AsThickness();

        if (!RangeCheck(t.left) || !RangeCheck(t.right) || !RangeCheck(t.top) || !RangeCheck(t.bottom)) {
            MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Value is out of range");
            return false;
        }

        if (t.left < 0 || t.right < 0 || t.top < 0 || t.bottom < 0) {
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Value is out of range");
            return false;
        }
        return true;
    }

    bool
    Validators::CornerRadiusValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        CornerRadius t = *value->AsCornerRadius();

        if (!RangeCheck(t.topLeft) || !RangeCheck(t.topRight) || !RangeCheck(t.bottomLeft) || !RangeCheck(t.bottomRight)) {
            MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Value is out of range");
            return false;
        }

        if (t.topLeft < 0 || t.topRight < 0 || t.bottomLeft < 0 || t.bottomRight < 0) {
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Value is out of range");
            return false;
        }
        return true;
    }

    bool
    Validators::BufferingTimeValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (value->AsTimeSpan() > 21427200000000000LL) {
            MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Value is out of range");
            return false;
        }

        return true;
    }

    bool
    Validators::IsTimelineValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!instance->Is(Type::TIMELINE)) {
            MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Instance is not a Timeline");
            return false;
        }

        return true;
    }

    bool
    Validators::StoryboardTargetPropertyValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!IsTimelineValidator(instance, property, value, error))
            return false;

        PropertyPath *existing = Storyboard::GetTargetProperty(instance);

        if (existing && existing->property != NULL) {
            // it was initialized using a DP, we only allow it to be overriden with another DP.
            PropertyPath *new_path = value->AsPropertyPath();
            if (new_path->property == NULL)
                return false;
        }

        return true;
    }

    bool
    Validators::IsSetterSealedValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (instance->Is(Type::SETTERBASE)) {
            if (((SetterBase *)instance)->GetIsSealed()) {
                MoonError::FillIn(error, MoonError::UNAUTHORIZED_ACCESS, "Cannot modify a setter after it is used");
                return false;
            }
        }

        return true;
    }


    bool
    Validators::ContentControlContentValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (value->Is(instance->GetDeployment(), Type::FRAMEWORKELEMENT)) {
            FrameworkElement *fwe = value->AsFrameworkElement();

            if (fwe->GetLogicalParent() && fwe->GetLogicalParent()->Is(Type::PANEL)) {
                MoonError::FillIn(error, MoonError::ARGUMENT, "Content is already a child of another element");
                return false;
            }
        }

        return true;
    }

    bool
    Validators::FloatValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        double d = value->AsDouble();

        switch (fpclassify(d)) {
        case FP_SUBNORMAL:
        case FP_INFINITE:
        case FP_NAN:
            MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Value is out of range");
            return false;
        default:
            if ((float)d < -HUGE || (float)d > HUGE) {
                MoonError::FillIn(error, MoonError::EXCEPTION, 1001, "Value is out of range");
                return false;
            }
        }

        return true;
    }

    bool
    Validators::BasedOnValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        // You can't do Style s = new Style (); s.BasedOn = s;
        if (!Value::IsNull(value) && value->AsStyle() == instance) {
            MoonError::FillIn(error, MoonError::ARGUMENT, "A Style cannot be based on itself.");
            return false;
        }

        return true;
    }

    bool
    Validators::StyleValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        Type::Kind target_kind = instance->GetObjectType();
        Types *types = instance->GetDeployment()->GetTypes();
        char *error_message = NULL;

        if (!Value::IsNull(value)) {
            Style *root = NULL;
            Style *style = value->AsStyle(types);
            // 0) Styles are only sealable once they're fully
            // validated internally and correct so if it's sealed,
            // we only need to verify that we can apply it to this
            // object.
            if (style->GetIsSealed()) {
                if (!types->IsSubclassOf(target_kind, style->GetTargetType()->kind)) {
                    error_message = g_strdup_printf("Style.TargetType ('%s') is not a subclass of ('%s')",
                        types->Find(style->GetTargetType()->kind)->GetName(),
                        types->Find(target_kind)->GetName());
                    MoonError::FillIn(error, MoonError::XAML_PARSE_EXCEPTION, error_message);
                    g_free(error_message);
                    return false;
                }
                return true;
            }

            // 1) Check for circular references in the BasedOn tree
            GHashTable *cycles = g_hash_table_new(g_direct_hash, g_direct_equal);
            root = style;
            while (root) {
                if (g_hash_table_lookup(cycles, root)) {
                    MoonError::FillIn(error, MoonError::INVALID_OPERATION, 1001, "Circular reference in Style.BasedOn");
                    g_hash_table_destroy(cycles);
                    return false;
                }
                g_hash_table_insert(cycles, root, GINT_TO_POINTER(1));
                root = root->GetBasedOn();
            }
            g_hash_table_destroy(cycles);

            // 2) Check that the instance is a subclass of Style::TargetType and also all the styles TargetTypes are
            // subclasses of their BasedOn styles TargetType.
            root = style;
            while (root) {
                MoonError::ExceptionType exception;
                ManagedTypeInfo *target_type = root->GetTargetType();

                if (root == style) {
                    if (!target_type) {
                        exception = MoonError::INVALID_OPERATION;
                        error_message = g_strdup("TargetType cannot be null");
                    }
                    else if (!types->IsSubclassOf(target_kind, target_type->kind)) {
                        exception = MoonError::XAML_PARSE_EXCEPTION;
                        error_message = g_strdup_printf("Style.TargetType ('%s') is not a subclass of ('%s')",
                            types->Find(target_type->kind)->GetName(),
                            types->Find(target_kind)->GetName());
                    }
                }
                else if (!target_type || !types->IsSubclassOf(target_kind, target_type->kind)) {
                    exception = MoonError::INVALID_OPERATION;
                    error_message = g_strdup_printf("Style.TargetType ('%s') is not a subclass of ('%s')",
                        target_type ? types->Find(target_type->kind)->GetName() : "<Not Specified>",
                        types->Find(target_kind)->GetName());
                }
                if (error_message) {
                    MoonError::FillIn(error, exception, error_message);
                    g_free(error_message);
                    return false;
                }
                target_kind = target_type->kind;
                root = root->GetBasedOn();
            }

            // 3) This style is now OK and never needs to be checked again.
            style->Seal();
        }

        return true;
    }

    bool
    Validators::OnlyDuringInitializationValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!instance->GetDeployment()->IsInitializing()) {
            char *msg = g_strdup_printf("Property %s cannot be changed.", property->GetName());
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, msg);
            g_free(msg);
            return false;
        }
        return true;
    }

    bool
    Validators::NonNullOnlyDuringInitializationValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (OnlyDuringInitializationValidator(instance, property, value, error))
            return NonNullValidator(instance, property, value, error);
        return false;
    }

    bool
    Validators::NullOrInDesignMode(DependencyObject *instance, DependencyProperty *GetIsInDesignModeproperty, Value *value, MoonError *error) {
        if (!value || value->GetIsNull())
            return true;

        // this state is kept only in "Application.Current.RootVisual"
        UIElement *top = instance->GetDeployment()->GetSurface()->GetToplevel();
        if (!top || !top->GetValue(DesignerProperties::IsInDesignModeProperty)->AsBool()) {
            MoonError::FillIn(error, MoonError::NOT_IMPLEMENTED_EXCEPTION, 1001, "Value can only be null if not in design mode");
            return false;
        }
        return true;
    }

    bool
    Validators::UserInitiatedValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (instance->GetDeployment()->GetSurface()->IsUserInitiatedEvent())
            return true;

        MoonError::FillIn(error, MoonError::SECURITY_EXCEPTION, 1001, "Value can only be set from a user-initiated event");
        return false;
    }

    bool
    Validators::AssemblyPartSourceValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!value || value->GetIsNull())
            *value = Value("", Type::STRING);
        return true;
    }

    static bool
    IsRunningOutOfBrowser(Deployment *deployment, MoonError *error) {
        // fill in a NotSupportedException if not running Out-Of-Browser
        if (deployment->GetCurrentApplication() == NULL) {
            MoonError::FillIn(error, MoonError::NOT_SUPPORTED_EXCEPTION, "No current application");
            return false;
        }

        if (!deployment->GetCurrentApplication()->IsRunningOutOfBrowser()) {
            MoonError::FillIn(error, MoonError::NOT_SUPPORTED_EXCEPTION, "Can only be set when Out-Of-Browser");
            return false;
        }
        return true;
    }

    static bool
    IsElevatedPermission(Deployment *deployment) {
        OutOfBrowserSettings *oob = deployment->GetOutOfBrowserSettings();
        if (!oob)
            return false;

        SecuritySettings *ss = oob->GetSecuritySettings();
        return (ss && (ss->GetElevatedPermissions() == ElevatedPermissionsRequired));
    }

    static bool
    IsWindow(DependencyObject *instance, MoonError *error) {
        if (!instance->Is(Type::WINDOW)) {
            MoonError::FillIn(error, MoonError::ARGUMENT, 1001, "Target object must be a Window");
            return false;
        }
        return true;
    }

    bool
    Validators::WindowTopLeftValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!IsWindow(instance, error))
            return false;

        Deployment *d = instance->GetDeployment();

        // must be running out of browser
        if (!IsRunningOutOfBrowser(d, error))
            return false;

        // can only be set for WindowStartupLocation == Manual -> otherwise it is ignored (no error)
        OutOfBrowserSettings *oob = d->GetOutOfBrowserSettings();
        if (!oob)
            return false;
        WindowSettings *ws = oob->GetWindowSettings();
        if (!ws || ws->GetWindowStartupLocation() != WindowStartupLocationManual)
            return false;

        // can only be set before the end of Application.Startup -> otherwise it is ignored (no error)
        return d->IsInitializing();
    }

    bool
    Validators::WindowElevatedValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        Deployment *d = instance->GetDeployment();

        // must be running out of browser
        if (!IsRunningOutOfBrowser(d, error))
            return false;

        // if Application.Setup is over then...
        if (!d->IsInitializing()) {
            Surface *s = d->GetSurface();
            //  this can only occurs for user-initiated events *AND* if running with elevated permissions
            if (!s->IsUserInitiatedEvent() && !IsElevatedPermission(d)) {
                MoonError::FillIn(error, MoonError::SECURITY_EXCEPTION, 1001, "Value can only be set from a user-initiated event");
                return false;
            }
        }

        return true;
    }

    bool
    Validators::WindowHeightValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!IsWindow(instance, error))
            return false;

        if (!WindowElevatedValidator(instance, property, value, error))
            return false;

        MoonWindowingSystem *ws = Runtime::GetWindowingSystem();
        Window *window = (Window *)instance;
        guint screen_height = ws->GetScreenHeight(window->GetMoonWindow());

        double height = value ? value->AsDouble() : 0;
        if (height > screen_height - window->GetValue(Window::TopProperty)->AsDouble()) {
            char *msg = g_strdup_printf("Invalid Window Height (%g)", height);
            MoonError::FillIn(error, MoonError::ARGUMENT_OUT_OF_RANGE, 1001, msg);
            return false;
        }
        return true;
    }

    bool
    Validators::WindowWidthValidator(DependencyObject *instance, DependencyProperty *property, Value *value, MoonError *error) {
        if (!IsWindow(instance, error))
            return false;

        if (!WindowElevatedValidator(instance, property, value, error))
            return false;

        MoonWindowingSystem *ws = Runtime::GetWindowingSystem();
        Window *window = (Window *)instance;
        guint screen_width = ws->GetScreenWidth(window->GetMoonWindow());

        double width = value ? value->AsDouble() : 0;

        if (width > screen_width - window->GetValue(Window::LeftProperty)->AsDouble()) {
            char *msg = g_strdup_printf("Invalid Window Width (%g)", width);
            MoonError::FillIn(error, MoonError::ARGUMENT_OUT_OF_RANGE, 1001, msg);
            g_free(msg);
            return false;
        }
        return true;
    }

};
