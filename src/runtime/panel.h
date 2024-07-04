/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * panel.h
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_PANEL_H__
#define __MOON_PANEL_H__

#include <glib.h>
#include "frameworkelement.h"

namespace Moonlight {

/* @ContentProperty="Children" */
/* @Namespace=System.Windows.Controls */
class Panel : public FrameworkElement {
 private:
	//
	// Contains the last element where the mouse entered
	//
	UIElement *mouse_over;

 protected:
	virtual ~Panel ();

	virtual bool EmptyBackground ();
 public:
 	/* @PropertyType=Brush,GenerateAccessors */
	const static int BackgroundProperty;
 	/* @PropertyType=UIElementCollection,AutoCreator=Panel::CreateChildren,ManagedFieldAccess=Internal,ManagedSetterAccess=Internal,GenerateAccessors */
	const static int ChildrenProperty;
	/* @PropertyType=bool,DefaultValue=false,ManagedSetterAccess=Internal,GenerateAccessors */
	const static int IsItemsHostProperty;
	
 	/* @GeneratePInvoke,ManagedAccess=Protected */
	Panel ();

	virtual void OnIsAttachedChanged (bool attached);

	virtual void ComputeBounds ();
	virtual Size MeasureOverrideWithError (Size availableSize, MoonError *error);

	virtual Rect GetCoverageBounds ();
	virtual void Render (Context *ctx, Region *region);

	virtual bool InsideObject (cairo_t *cr, double x, double y);
	
	virtual bool CanFindElement () { return GetBackground () != NULL; }

	virtual bool IsLayoutContainer () { return true; }
	virtual bool IsContainer () { return true; }

	virtual void ShiftPosition (Point p);
	
	virtual void OnPropertyChanged (PropertyChangedEventArgs *args, MoonError *error);
	virtual void OnSubPropertyChanged (DependencyProperty *prop, DependencyObject *obj, PropertyChangedEventArgs *subobj_args);
	
	virtual void OnCollectionChanged (Collection *col, CollectionChangedEventArgs *args);
	virtual void OnCollectionItemChanged (Collection *col, DependencyObject *obj, PropertyChangedEventArgs *args);

	virtual void ElementAdded (UIElement *item);
	virtual void ElementRemoved (UIElement *item);

	//
	// Property Accessors
	//
	void SetBackground (Brush *background);
	Brush *GetBackground ();
	
	void SetChildren (UIElementCollection *children);
	UIElementCollection *GetChildren ();
	
	void SetIsItemsHost (bool value);
	bool GetIsItemsHost ();

	// Autocreator for the Children property
	static Value *CreateChildren (Type::Kind kind, DependencyProperty *property, DependencyObject *forObj);
};

};
#endif /* __MOON_PANEL_H__ */
