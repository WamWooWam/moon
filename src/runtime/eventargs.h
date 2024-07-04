/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * eventargs.h
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __EVENTARGS_H__
#define __EVENTARGS_H__

#include <glib.h>
#include <cairo.h>
#include "dependencyobject.h"
#include "keyboard.h"
#include "enums.h"
#include "size.h"
#include "pal.h"
#include "error.h"

namespace Moonlight {

class StylusInfo;
class StylusPointCollection;
class UIElement;

/*
 * EventArgs needs to be ref counted since js can keep objects around until
 * after the event has been emitted.
 */

/* @Namespace=None */
class EventArgs : public DependencyObject {
public:
	EventArgs ();

protected:
	virtual ~EventArgs ();
	/* @SkipFactories */
	EventArgs (Type::Kind type);
};

/* @Namespace=None */
class PropertyChangedEventArgs : public EventArgs {
public:
	PropertyChangedEventArgs (DependencyProperty *p, int pid, Value *ov, Value *nv, bool copy_values = false) : obj (p), id (pid), copy_values(copy_values), old_value(ov), new_value (nv)
	{
		if (copy_values) {
			if (old_value)
				old_value = new Value(*old_value);
			if (new_value)
				new_value = new Value(*new_value);
		}
	}

	PropertyChangedEventArgs () : obj (NULL), id (0), copy_values (false), old_value(NULL), new_value (NULL) { }


	virtual ~PropertyChangedEventArgs ()
	{
		if (copy_values) {
			delete old_value;
			delete new_value;
		}
		new_value = NULL;
		old_value = NULL;
	}

	/* @GeneratePInvoke */
	DependencyProperty *GetProperty () { return obj; }
	/* @GeneratePInvoke */
	int GetId () { return id; }
	/* @GeneratePInvoke */
	Value* GetOldValue () { return old_value; }
	/* @GeneratePInvoke */
	Value* GetNewValue () { return new_value; }

	void SetProperty (DependencyProperty *prop) { obj = prop; }
	void SetId (int id) { this->id = id; }
	void SetOldValue (Value* old_value) { if (copy_values) delete this->old_value; this->old_value = old_value; if (copy_values && old_value) this->old_value = new Value (*old_value); }
	void SetNewValue (Value* new_value) { if (copy_values) delete this->new_value; this->new_value = new_value; if (copy_values && new_value) this->new_value = new Value (*new_value); }
private:
	DependencyProperty *obj;
	int id;
	bool copy_values;

	Value *old_value;
	Value *new_value;
};

/* @Namespace=None */
class RenderingEventArgs : public EventArgs {
public:
	RenderingEventArgs ();

	/* @GeneratePInvoke */
	TimeSpan GetRenderingTime ();

	void SetRenderingTime (TimeSpan renderingTime);

protected:
	virtual ~RenderingEventArgs ();

private:

	TimeSpan renderingTime;
};

enum CollectionChangedAction {
	CollectionChangedActionAdd,
	CollectionChangedActionRemove,
	CollectionChangedActionReplace,
	CollectionChangedActionClearing,
	CollectionChangedActionCleared,
};

/* @Namespace=None */
class CollectionChangedEventArgs : public EventArgs {
public:
	/* @GeneratePInvoke,SkipTypeConstructor */
	CollectionChangedEventArgs ();

	/* @SkipFactories */
	CollectionChangedEventArgs (CollectionChangedAction action, Value *new_value, Value *old_value, int index);
	
	/* @GeneratePInvoke */
	void SetChangedAction (CollectionChangedAction action);
	
	/* @GeneratePInvoke */
	CollectionChangedAction GetChangedAction ();
	
	/* @GeneratePInvoke */
	void SetNewItem (Value *item);
	
	/* @GeneratePInvoke */
	Value *GetNewItem ();
	
	/* @GeneratePInvoke */
	void SetOldItem (Value *item);
	
	/* @GeneratePInvoke */
	Value *GetOldItem ();
	
	/* @GeneratePInvoke */
	void SetIndex (int index);
	
	/* @GeneratePInvoke */
	int GetIndex ();

protected:
	virtual ~CollectionChangedEventArgs ();


private:
	CollectionChangedAction action;
	Value *old_item;
	Value *new_item;
	int index;
};

/* @Namespace=None */
class CollectionItemChangedEventArgs : public EventArgs {
public:
	CollectionItemChangedEventArgs (DependencyObject *collectionItem,
					DependencyProperty *property,
					Value *oldValue,
					Value *newValue)
		: EventArgs (Type::COLLECTIONITEMCHANGEDEVENTARGS)
	{
		this->collectionItem = collectionItem;
		this->property = property;
		this->oldValue = oldValue;
		this->newValue = newValue;
	}

	CollectionItemChangedEventArgs ()
		: EventArgs (Type::COLLECTIONITEMCHANGEDEVENTARGS)
	{
		this->collectionItem = NULL;
		this->property = NULL;
		this->oldValue = NULL;
		this->newValue = NULL;
	}

	DependencyObject*   GetCollectionItem() { return collectionItem; }
	DependencyProperty* GetProperty()       { return property; }
	Value*              GetOldValue ()      { return oldValue; }
	Value*              GetNewValue ()      { return newValue; }

	void SetCollectionItem(DependencyObject* collectionItem) { this->collectionItem = collectionItem; }
	void SetProperty(DependencyProperty* property)           { this->property = property; }
	void SetOldValue (Value* oldValue)                       { this->oldValue = oldValue; }
	void SetNewValue (Value* newValue)                       { this->newValue = newValue; }

private:
	DependencyObject *collectionItem;
	DependencyProperty *property;
	Value *oldValue;
	Value *newValue;
};



/* @Namespace=None */
class ResourceDictionaryChangedEventArgs : public EventArgs {
public:
	/* @GeneratePInvoke,SkipTypeConstructor */
	ResourceDictionaryChangedEventArgs ();

	/* @SkipFactories */
	ResourceDictionaryChangedEventArgs (CollectionChangedAction action, Value *new_value, Value *old_value, const char *key);
	
	/* @GeneratePInvoke */
	void SetChangedAction (CollectionChangedAction action);
	
	/* @GeneratePInvoke */
	CollectionChangedAction GetChangedAction ();
	
	/* @GeneratePInvoke */
	void SetNewItem (Value *item);
	
	/* @GeneratePInvoke */
	Value *GetNewItem ();
	
	/* @GeneratePInvoke */
	void SetOldItem (Value *item);
	
	/* @GeneratePInvoke */
	Value *GetOldItem ();
	
	/* @GeneratePInvoke */
	void SetKey (const char *key);
	
	/* @GeneratePInvoke */
	const char* GetKey ();

protected:
	virtual ~ResourceDictionaryChangedEventArgs ();


private:
	CollectionChangedAction action;
	Value *old_item;
	Value *new_item;
	const char *key;
};

/* @Namespace=None */
class DownloadProgressEventArgs : public EventArgs {
 private:
	double progress;

 protected:
	virtual ~DownloadProgressEventArgs ();
 
 public:
	DownloadProgressEventArgs ();
	DownloadProgressEventArgs (double progress);
	
	void SetProgress (double progress);
	
	/* @GeneratePInvoke */
	double GetProgress ();
};

/* @Namespace=None */
class RoutedEventArgs : public EventArgs {
public:
	/* @GeneratePInvoke */
	DependencyObject *GetSource ();
	
	/* @GeneratePInvoke */
	void SetSource(DependencyObject *el);
	
	/* @GeneratePInvoke */
	void SetHandled (bool handled);
	
	/* @GeneratePInvoke */
	bool GetHandled ();

	/* @SkipFactories */
	RoutedEventArgs (DependencyObject *source);

	const static void *SourceWeakRef;

protected:
 	/* @GeneratePInvoke */
	RoutedEventArgs ();

	/* @SkipFactories */
	RoutedEventArgs (DependencyObject *source, Type::Kind kind);

	/* @SkipFactories */
	RoutedEventArgs (Type::Kind kind);

	virtual ~RoutedEventArgs ();

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

private:
	WeakRef<DependencyObject> source;
	bool handled;
};

/* @Namespace=None */
class KeyEventArgs : public RoutedEventArgs {
public:
 	/* @SkipFactories */
	KeyEventArgs (MoonKeyEvent *event);
	
	/* @GeneratePInvoke */
	int GetKey ();
	
	/* @GeneratePInvoke */
	int GetPlatformKeyCode ();
	
	MoonKeyEvent *GetEvent () { return event; }
protected:
 	/* @GeneratePInvoke */
	KeyEventArgs ();

	virtual ~KeyEventArgs ();

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
private:
	MoonKeyEvent *event;
};

/* @Namespace=None */
class MouseEventArgs : public RoutedEventArgs {
public:
	/* @SkipFactories */
	MouseEventArgs (MoonMouseEvent *event);
	
	MoonMouseEvent *GetEvent () { return event; }

	/* @GeneratePInvoke */
	void GetPosition (UIElement *relative_to, double *x, double *y);
	
	/* @GeneratePInvoke */
	StylusInfo *GetStylusInfo ();
	
	/* @GeneratePInvoke */
	StylusPointCollection *GetStylusPoints (UIElement *ink_presenter);
	
protected:
 	/* @GeneratePInvoke */
	MouseEventArgs ();

	/* @SkipFactories */
	MouseEventArgs (Type::Kind kind, MoonMouseEvent *event);

	virtual ~MouseEventArgs ();

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
private:
	MoonMouseEvent *event;
};

/* @Namespace=None */
class LogReadyRoutedEventArgs : public RoutedEventArgs {
public:
	/* @GeneratePInvoke,ManagedAccess=Internal */
	LogReadyRoutedEventArgs ();

	/* @GeneratePInvoke */
	const char *GetLog () { return log; }
	/* @GeneratePInvoke */
	LogSource GetLogSource () { return log_source; }

	/* We receive ownership of the log and will free it when deleted */
	void GiveLog (char *value) { g_free (log); log = value; }
	void SetLogSource (LogSource value) { log_source = value; }

protected:
	
	virtual ~LogReadyRoutedEventArgs ();

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

private:
	char *log;
	LogSource log_source;
};

/* @Namespace=None */
class MouseButtonEventArgs : public MouseEventArgs {
public:
 	/* @GeneratePInvoke */
	MouseButtonEventArgs ();

	/* @SkipFactories */
	MouseButtonEventArgs (MoonButtonEvent *event);

protected:
	virtual ~MouseButtonEventArgs ();
};

/* @Namespace=None */
class MouseWheelEventArgs : public MouseEventArgs {
public:
 	/* @GeneratePInvoke */
	MouseWheelEventArgs ();

	/* @SkipFactories */
	MouseWheelEventArgs (MoonScrollWheelEvent *event);
	
	/* @GeneratePInvoke */
	int GetWheelDelta ();
	
protected:
	virtual ~MouseWheelEventArgs ();
};


/* @Namespace=None,ManagedDependencyProperties=None */
class ErrorEventArgs : public EventArgs  {
private:
	MoonError *error;
	void Initialize (DependencyObject *original_source, ErrorEventArgsType type, const MoonError &error, int extended_error_code, const char *extended_msg);
	DependencyObject *original_source;
	int extended_code;
	char *extended_message;
	ErrorEventArgsType error_type;

protected:
	virtual ~ErrorEventArgs ();

public:
	/* @SkipFactories */
	ErrorEventArgs (DependencyObject *original_source, ErrorEventArgs *args);
	/* @SkipFactories */
	ErrorEventArgs (Type::Kind kind, ErrorEventArgsType type, const MoonError error);
	/* @SkipFactories */
	ErrorEventArgs (DependencyObject *original_source, Type::Kind kind, ErrorEventArgsType type, const MoonError error);
	/* @SkipFactories */
	ErrorEventArgs (ErrorEventArgsType type, MoonError error);
	/* @SkipFactories */
	ErrorEventArgs (DependencyObject *original_source, ErrorEventArgsType type, MoonError error);
	/* @SkipFactories */
	ErrorEventArgs (ErrorEventArgsType type, MoonError error, int extended_code, const char *extended_msg);

	/* @GeneratePInvoke */
	gpointer GetMoonError () { return error; }

	/* @GeneratePInvoke */
	const char *GetErrorMessage () { return error->message; }
	/* @GeneratePInvoke */
	int GetErrorCode () { return error->code; }
	/* @GeneratePInvoke */
	int GetErrorType () { return error_type; }

	// To match SL behaviour we need to match SL error messages, which aren't all that helpful
	// This is here to keep extra error information we have, 
	// but don't want to report to the user.
	// extended_code:
	//  3 (MEDIA_UNKNOWN_CODEC): used by playlist to determine if we should raise a MediaFailed event or just continue to play the next entry.
	int GetExtendedCode () { return extended_code; }
	const char *GetExtendedMessage () { return extended_message; }

	DependencyObject *GetOriginalSource () { return original_source; }
};

/* @Namespace=None,ManagedDependencyProperties=None */
class ImageErrorEventArgs : public ErrorEventArgs {
public:
	/* @SkipFactories */
	ImageErrorEventArgs (DependencyObject *original_source, MoonError error);

protected:
	virtual ~ImageErrorEventArgs ();

};


/* @Namespace=None,ManagedDependencyProperties=None */
class MessageReceivedEventArgs : public EventArgs {
public:
	MessageReceivedEventArgs (const char* message,
				  ReceiverNameScope namescope,
				  const char* receiverName,
				  const char* senderDomain);

	/* @GeneratePInvoke */
	const char *GetMessage () { return message; }

	/* @GeneratePInvoke */
	ReceiverNameScope GetNamescope () { return namescope; }

	/* @GeneratePInvoke */
	const char *GetReceiverName () { return receiverName; }

	/* @GeneratePInvoke */
	const char *GetResponse () { return response; }

	/* @GeneratePInvoke */
	void SetResponse (const char *value) { g_free (response); response = g_strdup (value); }

	/* @GeneratePInvoke */
	const char *GetSenderDomain () { return senderDomain; }

protected:
	virtual ~MessageReceivedEventArgs ();

private:
	char *message;
	ReceiverNameScope namescope;
	char *receiverName;
	char *response;
	char *senderDomain;
};

/* @Namespace=None,ManagedDependencyProperties=None */
class SendCompletedEventArgs : public EventArgs {
public:
	SendCompletedEventArgs (MoonError *error,
				const char* message,
				const char *receiverName,
				const char* receiverDomain,
				const char *response,
				GCHandle managedUserState);

	/* @GeneratePInvoke */
	void GetError (MoonError *error) {
		if (this->error) {
			*error = MoonError (*this->error);
		}
		else {
			MoonError::FillIn (error, MoonError::NO_ERROR, "");
		}
	}

	/* @GeneratePInvoke */
	const char *GetMessage () { return message; }

	/* @GeneratePInvoke */
	const char *GetReceiverName () { return receiverName; }

	/* @GeneratePInvoke */
	const char *GetReceiverDomain () { return receiverDomain; }

	/* @GeneratePInvoke */
	const char *GetResponse () { return response; }

	/* @GeneratePInvoke */
	const GCHandle GetManagedUserState () { return managedUserState; }


protected:
	virtual ~SendCompletedEventArgs ();

private:
	MoonError *error;
	char *message;
	char *receiverName;
	char *receiverDomain;
	char *response;
	GCHandle managedUserState;
};

/* @Namespace=None,ManagedDependencyProperties=None */
class CaptureImageCompletedEventArgs : public EventArgs {
public:
	/* @SkipFactories */
	CaptureImageCompletedEventArgs (MoonError *error);

	/* @GeneratePInvoke */
	void GetError (MoonError *error) {
		if (this->error) {
			*error = MoonError (*this->error);
		}
		else {
			MoonError::FillIn (error, MoonError::NO_ERROR, "");
		}
	}

	/* @GeneratePInvoke */
	BitmapSource *GetSource () { return source; }

	void SetSource (BitmapSource *source);

	const static void *SourceWeakRef;

protected:
	/* @GeneratePInvoke */
	CaptureImageCompletedEventArgs ();
	virtual ~CaptureImageCompletedEventArgs ();

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

private:
	MoonError *error;
	WeakRef<BitmapSource> source;
};

/* @Namespace=None,ManagedDependencyProperties=None */
class ParserErrorEventArgs : public ErrorEventArgs {
protected:
	virtual ~ParserErrorEventArgs ();

public:
	ParserErrorEventArgs (DependencyObject *original_source, const char *msg, const char *file,
			      int line, int column, int error_code, 
			      const char *element, const char *attribute);
	
	int char_position;
	int line_number;
	char *xaml_file;
	char *xml_element;
	char *xml_attribute;
};

/* @Namespace=None */
class TimelineMarkerRoutedEventArgs : public RoutedEventArgs {
	WeakRef<TimelineMarker> marker;
	
 protected:
	/* @GeneratePInvoke */
	TimelineMarkerRoutedEventArgs ();
	virtual ~TimelineMarkerRoutedEventArgs ();

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

 public:
	/* @SkipFactories */
	TimelineMarkerRoutedEventArgs (TimelineMarker *marker);

	/* @GeneratePInvoke */
	TimelineMarker *GetMarker ();
	/* @GeneratePInvoke */
	void SetMarker (TimelineMarker *value);

	const static void *MarkerWeakRef;
};

/* @Namespace=None */
class CheckAndDownloadUpdateCompletedEventArgs : public EventArgs {
	bool updated;
	char *error;
	
 protected:
	virtual ~CheckAndDownloadUpdateCompletedEventArgs ();
	
 public:
	/* @GeneratePInvoke */
	CheckAndDownloadUpdateCompletedEventArgs (bool updated, const char *error);
	
	/* @GeneratePInvoke */
	bool GetUpdateAvailable () { return updated; }
	
	/* @GeneratePInvoke */
	const char *GetError () { return error; }
};

/* @Namespace=None */
class SizeChangedEventArgs : public RoutedEventArgs {
	Size prev_size;
	Size new_size;

protected:
	/* @GeneratePInvoke */
	SizeChangedEventArgs ();

	virtual ~SizeChangedEventArgs () {}

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
public:
	/* @SkipFactories */
	SizeChangedEventArgs (Size prev_size, Size new_size);
	
	Size GetPrevSize () { return prev_size; }
	Size GetNewSize () { return new_size; }

	/* @GeneratePInvoke */
	static void GetPrevSize (SizeChangedEventArgs *args, /* @MarshalAs=Size,IsRef */ Size *prev_size);
	/* @GeneratePInvoke */
	static void GetNewSize  (SizeChangedEventArgs *args, /* @MarshalAs=Size,IsRef */ Size *new_size);
};

};
#endif /* __EVENTARGS_H__ */
