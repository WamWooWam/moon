/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * style.h:
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_STYLE_H__
#define __MOON_STYLE_H__

#include <glib.h>

#include "dependencyobject.h"
#include "collection.h"

//
// Style
//
/* @SilverlightVersion="2" */
/* @ContentProperty="Setters" */
/* @Namespace=System.Windows */
class Style : public DependencyObject {
public:
  	/* @PropertyType=bool,DefaultValue=false,ManagedSetterAccess=Private,GenerateAccessors */
	static DependencyProperty *IsSealedProperty;
 	/* @PropertyType=SetterBaseCollection,Access=Internal,ManagedFieldAccess=Private,ManagedAccess=Public,ManagedSetterAccess=Private,GenerateAccessors */
	static DependencyProperty *SettersProperty;
 	/* @PropertyType=ManagedTypeInfo,ManagedPropertyType=System.Type,Access=Internal,ManagedAccess=Public,ManagedFieldAccess=Internal */
	static DependencyProperty *TargetTypeProperty;
	
	/* @GenerateCBinding,GeneratePInvoke */
	Style ();
	
	/* @GenerateCBinding,GeneratePInvoke */
	void Seal ();
	
	//
	// Property Accessors
	//
	void SetSetters (SetterBaseCollection *setters);
	SetterBaseCollection *GetSetters ();
	
	void SetIsSealed (bool sealed);
	bool GetIsSealed ();
	
protected:
	virtual ~Style ();
};

//
// SetterBaseCollection
//
/* @SilverlightVersion="2" */
/* @Namespace=System.Windows */
class SetterBaseCollection : public DependencyObjectCollection {
public:
	/* @GenerateCBinding,GeneratePInvoke */
	SetterBaseCollection ();
	
  	/* @PropertyType=bool,DefaultValue=false,ManagedSetterAccess=Private,GenerateAccessors */
	static DependencyProperty *IsSealedProperty;

	virtual bool AddedToCollection (Value *value, MoonError *error);
	virtual void RemovedFromCollection (Value *value);
	
	virtual Type::Kind GetElementType () { return Type::SETTERBASE; }
	
	void Seal ();
	
	//
	// Property Accessors
	//
	void SetIsSealed (bool sealed);
	bool GetIsSealed ();
	
protected:
	virtual ~SetterBaseCollection () { }

private:
	bool ValidateSetter (Value *value, MoonError *error);
};

//
// SetterBase
//
/* @SilverlightVersion="2" */
/* @Namespace=System.Windows */
class SetterBase : public DependencyObject {
 protected:
	virtual ~SetterBase () { }
	
 public:
   	/* @PropertyType=bool,DefaultValue=false,ManagedSetterAccess=Private,GenerateAccessors */
	static DependencyProperty *IsSealedProperty;
	
	/* @GenerateCBinding,GeneratePInvoke,ManagedAccess=Internal */
	SetterBase ();
	
	void Seal ();
	
	//
	// Property Accessors
	//
	void SetAttached (bool attached);
	bool GetAttached ();
	
	void SetIsSealed (bool sealed);
	bool GetIsSealed ();
	
 private:
	bool attached;
};

//
// Setter
//
/* @SilverlightVersion="2" */
/* @Namespace=System.Windows */
/* @ManagedDependencyProperties=Manual */
class Setter : public SetterBase {
 protected:
	virtual ~Setter ();
	
 public:
	/* @PropertyType=DependencyProperty,Validator=IsSetterSealedValidator */
	static DependencyProperty *PropertyProperty;
	/* @PropertyType=object,Validator=IsSetterSealedValidator */
	static DependencyProperty *ValueProperty;

	/* @PropertyType=object */
	static DependencyProperty *ConvertedValueProperty;

	/* @GenerateCBinding,GeneratePInvoke */
	Setter ();
};

#endif /* __MOON_STYLE_H__ */
