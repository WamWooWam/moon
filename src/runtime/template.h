/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * template.h:
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_TEMPLATE_H__
#define __MOON_TEMPLATE_H__

#include <glib.h>

#include "xaml.h"
#include "dependencyobject.h"
#include "control.h"

namespace Moonlight {

class XamlContext;

//
// FrameworkTemplate
//
/* @Namespace=System.Windows */
class FrameworkTemplate : public DependencyObject {
public:
	virtual void Dispose ();

	/* @GeneratePInvoke */
	/* This method always returns a reffed object */
	DependencyObject *GetVisualTreeWithError (FrameworkElement *templateBindingSource, MoonError *error);

	/* @GeneratePInvoke */
	void SetXamlBuffer (parse_template_func parse_template, Value *parse_template_data, const char *buffer, bool holdManagedRef);

	const static void *XamlContextWeakRef;
protected:
	/* @GeneratePInvoke,ManagedAccess=Protected */
	FrameworkTemplate ();

	virtual ~FrameworkTemplate () {}

	char *xaml_buffer;
	parse_template_func *parse_template;
	Value *parse_template_data;
	Value *parse_template_data2;

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

private:
	bool holdManagedRef;
	EVENTHANDLER (FrameworkTemplate, ShuttingDownEvent, Deployment, EventArgs);
	void ClearXamlBuffer ();
};

//
// ControlTemplate
//
/* @Namespace=System.Windows.Controls */
class ControlTemplate : public FrameworkTemplate {
public:
	/* @PropertyType=ManagedTypeInfo,AutoCreator=ControlTypeCreator,GenerateAccessors,ManagedPropertyType=System.Type,Access=Internal,ManagedAccessorAccess=Public,ManagedFieldAccess=Private */
	const static int TargetTypeProperty;

	void SetTargetType (ManagedTypeInfo *info);
	ManagedTypeInfo *GetTargetType ();

protected:
	/* @GeneratePInvoke */
	ControlTemplate ();
	virtual ~ControlTemplate () {}

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
};

//
// DataTemplate
//
/* @Namespace=System.Windows */
class DataTemplate : public FrameworkTemplate {
protected:
	/* @GeneratePInvoke */
	DataTemplate ();

	virtual ~DataTemplate () {}
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
};

};
#endif /* __MOON_TEMPLATE_H__ */
