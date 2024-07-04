/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * provider.h: an api for PropertyValue providers (for property inheritance)
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_PROVIDER_H__
#define __MOON_PROVIDER_H__

#include <glib.h>

#include "type.h"
#include "value.h"

namespace Moonlight {

class EventObject;
class EventArgs;
class PropertyChangedEventArgs;
class FrameworkElement;
class DependencyObject;
class DependencyProperty;
class UIElement;
class Surface;
class Style;
struct Value;
struct MoonError;

class InheritedIsEnabledValueProvider;
class LocalPropertyValueProvider;
class PropertyValueProvider;
class StylePropertyValueProvider;
class ImplicitStylePropertyValueProvider;
class InheritedPropertyValueProvider;
class InheritedDataContextValueProvider;
class DefaultValueProvider;
class AutoCreatePropertyValueProvider;

enum PropertyPrecedence {
	PropertyPrecedence_IsEnabled,
	PropertyPrecedence_LocalValue,
	PropertyPrecedence_DynamicValue, // use this level for types that need to compute property values lazily

	PropertyPrecedence_LocalStyle,
	PropertyPrecedence_ImplicitStyle,

	PropertyPrecedence_Inherited,
	PropertyPrecedence_InheritedDataContext,
	PropertyPrecedence_DefaultValue,
	PropertyPrecedence_AutoCreate,

	PropertyPrecedence_Count,

	PropertyPrecedence_Highest = PropertyPrecedence_IsEnabled,
	PropertyPrecedence_Lowest = PropertyPrecedence_AutoCreate,
};

// this has to be in the same order as the precedence enum above
struct PropertyValueProviderVTable {
	InheritedIsEnabledValueProvider *isenabled;
	LocalPropertyValueProvider *localvalue;
	PropertyValueProvider *dynamicvalue; // base class pointer since DO subclasses will define their own.
	StylePropertyValueProvider *localstyle;
	ImplicitStylePropertyValueProvider *implicitstyle;
	InheritedPropertyValueProvider *inherited;
	InheritedDataContextValueProvider *inheriteddatacontext;
	DefaultValueProvider *defaultvalue;
	AutoCreatePropertyValueProvider *autocreate;
};

enum ProviderFlags {
	ProviderFlags_RecomputesOnLowerPriorityChange  = 1<<0,
	ProviderFlags_RecomputesOnHigherPriorityChange = 1<<1,
	ProviderFlags_RecomputesOnClear                = 1<<2,
	ProviderFlags_ProvidesLocalValue               = 1<<3
};

class PropertyValueProvider {
public:
	PropertyValueProvider (DependencyObject *_obj, PropertyPrecedence _precedence, int _flags = 0)
		: obj(_obj), precedence(_precedence), flags((ProviderFlags)_flags)
	{
	}
	virtual ~PropertyValueProvider () { }

	virtual Value *GetPropertyValue (DependencyProperty *property) = 0;

	virtual void RecomputePropertyValue (DependencyProperty *property, ProviderFlags flags, MoonError *error) { }

	ProviderFlags GetFlags () { return flags; }

	virtual void ForeachValue (GHFunc func, gpointer data) { }

protected:
	DependencyObject *obj;
	PropertyPrecedence precedence;
	ProviderFlags flags;
};


class LocalPropertyValueProvider : public PropertyValueProvider {
public:
	LocalPropertyValueProvider (DependencyObject *obj, PropertyPrecedence _precedence, GHRFunc dispose_value);
	virtual ~LocalPropertyValueProvider ();

	virtual Value *GetPropertyValue (DependencyProperty *property);

	virtual void ForeachValue (GHFunc func, gpointer data);
	void SetValue (DependencyProperty *property, Value *value);
	void ClearValue (DependencyProperty *property);


 private:
	GHashTable *local_values;
	GHRFunc     dispose_value;
};

class StylePropertyValueProvider : public PropertyValueProvider {
public:
	StylePropertyValueProvider (DependencyObject *obj, PropertyPrecedence _precedence, GHRFunc dispose_value);
	virtual ~StylePropertyValueProvider ();

	virtual Value *GetPropertyValue (DependencyProperty *property);

	virtual void RecomputePropertyValue (DependencyProperty *property, ProviderFlags flags, MoonError *error);

	void UpdateStyle (Style *style, MoonError *error);

	virtual void ForeachValue (GHFunc func, gpointer data);

private:

	GHashTable *style_hash;
	GHRFunc dispose_value;
	Style *style;
};

class ImplicitStylePropertyValueProvider : public PropertyValueProvider {
public:
	enum StyleIndex {
		StyleIndexVisualTree,
		StyleIndexApplicationResources,
		StyleIndexGenericXaml,

		StyleIndexCount
	};

	enum StyleMask {
		StyleMaskVisualTree = 1 << StyleIndexVisualTree,
		StyleMaskApplicationResources = 1 << StyleIndexApplicationResources,
		StyleMaskGenericXaml = 1 << StyleIndexGenericXaml,

		StyleMaskAll = (StyleMaskGenericXaml | StyleMaskVisualTree | StyleMaskApplicationResources),
		StyleMaskNone = 0
	};


	ImplicitStylePropertyValueProvider (DependencyObject *obj, PropertyPrecedence _precedence, GHRFunc dispose_value);
	virtual ~ImplicitStylePropertyValueProvider ();

	virtual Value *GetPropertyValue (DependencyProperty *property);

	virtual void RecomputePropertyValue (DependencyProperty *property, ProviderFlags flags, MoonError *error);

	virtual void ForeachValue (GHFunc func, gpointer data);

	void SetStyles (StyleMask style_mask, Style **styles, MoonError *error);
	void ClearStyles (StyleMask style_mask, MoonError *error);

private:

	void ApplyStyles (StyleMask style_mask, Style **styles, MoonError *error);

	static void style_detached (EventObject *sender, EventArgs *calldata, gpointer closure);
	void StyleDetached (Style *style);

	GHashTable *style_hash;
	GHRFunc dispose_value;
	Style **styles;
	StyleMask style_mask;
};

class InheritedContext;

class InheritedPropertyValueProvider : public PropertyValueProvider {
public:
	enum Inheritable {
		Foreground = 1 << 0,
		FontFamily = 1 << 1,
		FontStretch = 1 << 2,
		FontStyle = 1 << 3,
		FontWeight = 1 << 4,
		FontSize = 1 << 5,
		Language = 1 << 6,
		FlowDirection = 1 << 7,
		UseLayoutRounding = 1 << 8,
		TextDecorations = 1 << 9,
		FontResource = 1 << 10,

		InheritableAll = 0x7ff,
		InheritableNone = 0
	};

	InheritedPropertyValueProvider (DependencyObject *obj, PropertyPrecedence _precedence);
	virtual ~InheritedPropertyValueProvider ();

	virtual Value *GetPropertyValue (DependencyProperty *property);

	DependencyObject* GetPropertySource (DependencyProperty *property);
	DependencyObject* GetPropertySource (Inheritable inheritableProperty);
	void SetPropertySource (Inheritable inheritableProperty, DependencyObject *source);

	static bool IsPropertyInherited (DependencyObject *obj, int propertyId);


	// this method is used when a property changes on an object
	// and that notification needs to propagate down the tree
	void PropagateInheritedProperty (DependencyProperty *property, DependencyObject *source, DependencyObject *subtree);

	// this method is used when you add a subtree into a
	// pre-existing tree.  it propagates all inheritable
	// properties throughout the tree
	void PropagateInheritedPropertiesOnAddingToTree (DependencyObject *subtree);

	void ClearInheritedPropertiesOnRemovingFromTree (DependencyObject *subtree);

	static Inheritable InheritablePropertyFromPropertyId (DependencyObject *obj, int propertyId);
	static int InheritablePropertyToPropertyId (Types *types, Inheritable property, Type::Kind objectType);

private:
	void MaybePropagateInheritedValue (Types *types, DependencyObject *source, Inheritable prop, Inheritable props, DependencyObject *element);
	void MaybeRemoveInheritedValue (Types *types, DependencyObject *source, Inheritable prop, Inheritable props, DependencyObject *element);

	void WalkSubtree (Types *types, DependencyObject *rootParent, DependencyObject *element, InheritedContext *context, Inheritable props, bool adding);
	void WalkTree (Types *types, DependencyObject *rootParent, DependencyObject *element, InheritedContext *context, Inheritable props, bool adding);

	GHashTable *propertyToSupplyingAncestor;
};

class DefaultValueProvider : public PropertyValueProvider {
 public:
	DefaultValueProvider (DependencyObject *obj, PropertyPrecedence _precedence);
	virtual ~DefaultValueProvider ();

	virtual Value *GetPropertyValue (DependencyProperty *property);
};

typedef Value* AutoCreator  (Type::Kind kind, DependencyProperty *property, DependencyObject *forObj);

class AutoCreators {
public:
	static AutoCreator default_autocreator;
	static AutoCreator CreateDefaultFontSize;
	static AutoCreator CreateBlackBrush;
	static AutoCreator ControlTypeCreator;
};

class AutoCreatePropertyValueProvider : public PropertyValueProvider {
 public:
	AutoCreatePropertyValueProvider (DependencyObject *obj, PropertyPrecedence _precedence, GHRFunc dispose_value);
	virtual ~AutoCreatePropertyValueProvider ();

	virtual Value *GetPropertyValue (DependencyProperty *property);
	
	Value *ReadLocalValue (DependencyProperty *property);
	void ClearValue (DependencyProperty *property);

	virtual void RecomputePropertyValue (DependencyProperty *property, ProviderFlags flags, MoonError *error);

	virtual void ForeachValue (GHFunc func, gpointer data);

 private:
	GHashTable *auto_values;
	GHRFunc dispose_value;
};

class InheritedDataContextValueProvider : public PropertyValueProvider {
public:
	InheritedDataContextValueProvider (DependencyObject *obj, PropertyPrecedence precedence);
	virtual ~InheritedDataContextValueProvider ();

	void EmitChanged ();
	virtual Value *GetPropertyValue (DependencyProperty *property);
	void SetDataSource (FrameworkElement *source);
private:
	static void source_data_context_changed (DependencyObject *sender, PropertyChangedEventArgs *args, MoonError *error, gpointer closure);
	static void source_destroyed (EventObject *sender, EventArgs *args, gpointer closure);
	void AttachListener ();
	void DetachListener ();
	FrameworkElement *source;
};

class InheritedIsEnabledValueProvider : public PropertyValueProvider {
public:
	InheritedIsEnabledValueProvider (DependencyObject *obj, PropertyPrecedence precedence);
	virtual ~InheritedIsEnabledValueProvider ();

	bool LocalValueChanged (DependencyProperty *property);
	virtual Value *GetPropertyValue (DependencyProperty *property);
	void SetDataSource (DependencyObject *source);
private:
	static void is_enabled_changed (DependencyObject *sender, PropertyChangedEventArgs *args, MoonError *error, gpointer closure);
	static void source_destroyed (EventObject *sender, EventArgs *args, gpointer closure);
	void AttachListener (DependencyObject *source);
	void DetachListener (DependencyObject *source);
	DependencyObject *source;
	Value current_value;
};

};
#endif /* __MOON_PROVIDER_H__ */
