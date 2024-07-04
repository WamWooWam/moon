/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * notificationwindow.h
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 */

#ifndef __MOON_NOTIFICATION_WINDOW_H__
#define __MOON_NOTIFICATION_WINDOW_H__
 
#include "dependencyobject.h"
#include "pal/window.h"

namespace Moonlight {

/* @Namespace=System.Windows */ 
class Window : public DependencyObject {
private:
	MoonWindow *moon_window;

protected:
	virtual ~Window () {}

public:
	/* @GeneratePInvoke */
	Window ();
	
	/* @PropertyType=double,DefaultValue=100.0,ManagedFieldAccess=Internal,Validator=WindowHeightValidator */
	const static int HeightProperty;
	/* @PropertyType=double,DefaultValue=100.0,ManagedFieldAccess=Internal,Validator=WindowWidthValidator */
	const static int WidthProperty;
	/* @PropertyType=double,DefaultValue=50.0,ManagedFieldAccess=Internal,Validator=WindowTopLeftValidator */
	const static int LeftProperty;
	/* @PropertyType=double,DefaultValue=50.0,ManagedFieldAccess=Internal,Validator=WindowTopLeftValidator */
	const static int TopProperty;
	/* @PropertyType=bool,ManagedFieldAccess=Internal,ManagedSetterAccess=Internal */
	const static int IsActiveProperty;
	/* @PropertyType=bool,ManagedFieldAccess=Internal,Validator=WindowElevatedValidator */
	const static int TopMostProperty;
	/* @PropertyType=WindowState,ManagedFieldAccess=Internal,Validator=WindowElevatedValidator */
	const static int WindowStateProperty;

	/* @DelegateType=EventHandler<ClosingEventArgs> */
	const static int ClosingEvent;

	/* @GeneratePInvoke */
	bool ActivateWithError (MoonError *error);

	/* @GeneratePInvoke */
	void CloseWithError (MoonError *error);

	/* @GeneratePInvoke */
	void DragMoveWithError (MoonError *error);

	/* @GeneratePInvoke */
	void DragResizeWithError (MoonError *error);

	/* @GeneratePInvoke */
	void SetMoonWindow (MoonWindow *window);
	/* @GeneratePInvoke */
	MoonWindow *GetMoonWindow ();

	/* @GeneratePInvoke */
	void SetTitle (const char *title);

	/* @GeneratePInvoke */
	void SetStyle (const WindowStyle style);

	virtual void OnPropertyChanged (PropertyChangedEventArgs *args, MoonError *error);
};

/* @Namespace=System.Windows */
/* @CallInitialize */
class NotificationWindow : public DependencyObject {
protected:
	virtual ~NotificationWindow () {}

public:
	/* @GeneratePInvoke */
	NotificationWindow ();

	/* @PropertyType=FrameworkElement */
	const static int ContentProperty;
	/* @PropertyType=double */
	const static int HeightProperty;
	/* @PropertyType=Visibility,DefaultValue=VisibilityVisible,GenerateAccessors,ManagedSetterAccess=Private,ManagedFieldAccess=Internal*/
	const static int VisibilityProperty;
	/* @PropertyType=double */
	const static int WidthProperty;

	/* @DelegateType=EventHandler */
	const static int ClosedEvent;

	void SetVisible (bool value);
	bool GetVisible ();

	void SetVisibility (Visibility value);
	Visibility GetVisibility ();
};

};
#endif /* __MOON_NOTIFICATION_WINDOW_H__ */
