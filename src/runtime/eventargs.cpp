/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * eventargs.cpp: specialized code for dealing with mouse/stylus/keyboard event args.
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#include <glib.h>

#include "eventargs.h"
#include "uielement.h"
#include "collection.h"
#include "stylus.h"
#include "runtime.h"
#include "timeline.h"
#include "deployment.h"
#include "writeablebitmap.h"
#include "factory.h"

namespace Moonlight {

EventArgs::EventArgs ()
	: DependencyObject (Type::EVENTARGS)
{
}

EventArgs::EventArgs (Type::Kind kind)
	: DependencyObject (kind)
{
}

EventArgs::~EventArgs ()
{
}

RenderingEventArgs::RenderingEventArgs ()
	: EventArgs (Type::RENDERINGEVENTARGS)
{
}

RenderingEventArgs::~RenderingEventArgs ()
{
}

TimeSpan
RenderingEventArgs::GetRenderingTime ()
{
	return renderingTime;
}

void
RenderingEventArgs::SetRenderingTime (TimeSpan renderingTime)
{
	this->renderingTime = renderingTime;
}

CollectionChangedEventArgs::CollectionChangedEventArgs ()
	: EventArgs (Type::COLLECTIONCHANGEDEVENTARGS)
{
	action = CollectionChangedActionAdd;
	old_item = NULL;
	new_item = NULL;
	index = -1;
}

CollectionChangedEventArgs::CollectionChangedEventArgs (CollectionChangedAction action, Value *new_item, Value *old_item, int index)
	: EventArgs (Type::COLLECTIONCHANGEDEVENTARGS)
{
	this->action = action;
	this->new_item = new_item;
	this->old_item = old_item;
	this->index = index;

	EnsureManagedPeer ();
}

CollectionChangedEventArgs::~CollectionChangedEventArgs ()
{
}

void
CollectionChangedEventArgs::SetChangedAction (CollectionChangedAction action)
{
	this->action = action;
}
	
CollectionChangedAction
CollectionChangedEventArgs::GetChangedAction ()
{
	return action;
}
	
void
CollectionChangedEventArgs::SetNewItem (Value *item)
{
	new_item = item;
}
	
Value *
CollectionChangedEventArgs::GetNewItem ()
{
	return new_item;
}
	
void
CollectionChangedEventArgs::SetOldItem (Value *item)
{
	old_item = item;
}
	
Value *
CollectionChangedEventArgs::GetOldItem ()
{
	return old_item;
}
	
void
CollectionChangedEventArgs::SetIndex (int index)
{
	this->index = index;
}
	
int
CollectionChangedEventArgs::GetIndex ()
{
	return index;
}

ResourceDictionaryChangedEventArgs::ResourceDictionaryChangedEventArgs ()
	: EventArgs (Type::RESOURCE_DICTIONARYCHANGEDEVENTARGS)
{
	action = CollectionChangedActionAdd;
	old_item = NULL;
	new_item = NULL;
	key = NULL;
}

ResourceDictionaryChangedEventArgs::ResourceDictionaryChangedEventArgs (CollectionChangedAction action, Value *new_item, Value *old_item, const char *key)
	: EventArgs (Type::RESOURCE_DICTIONARYCHANGEDEVENTARGS)
{
	this->action = action;
	this->new_item = new_item;
	this->old_item = old_item;
	this->key = key;

	EnsureManagedPeer ();
}

ResourceDictionaryChangedEventArgs::~ResourceDictionaryChangedEventArgs ()
{
}

void
ResourceDictionaryChangedEventArgs::SetChangedAction (CollectionChangedAction action)
{
	this->action = action;
}
	
CollectionChangedAction
ResourceDictionaryChangedEventArgs::GetChangedAction ()
{
	return action;
}
	
void
ResourceDictionaryChangedEventArgs::SetNewItem (Value *item)
{
	new_item = item;
}
	
Value *
ResourceDictionaryChangedEventArgs::GetNewItem ()
{
	return new_item;
}
	
void
ResourceDictionaryChangedEventArgs::SetOldItem (Value *item)
{
	old_item = item;
}
	
Value *
ResourceDictionaryChangedEventArgs::GetOldItem ()
{
	return old_item;
}
	
void
ResourceDictionaryChangedEventArgs::SetKey (const char *key)
{
	this->key = key;
}
	
const char *
ResourceDictionaryChangedEventArgs::GetKey ()
{
	return key;
}

DownloadProgressEventArgs::DownloadProgressEventArgs (double progress)
	: EventArgs (Type::DOWNLOADPROGRESSEVENTARGS)
{
	this->progress = progress;
}

DownloadProgressEventArgs::DownloadProgressEventArgs ()
	: EventArgs (Type::DOWNLOADPROGRESSEVENTARGS)
{
	progress = 0.0;
}

DownloadProgressEventArgs::~DownloadProgressEventArgs ()
{
}

void
DownloadProgressEventArgs::SetProgress (double progress)
{
	this->progress = progress;
}
	
double
DownloadProgressEventArgs::GetProgress ()
{
	return progress;
}

RoutedEventArgs::RoutedEventArgs (DependencyObject *source)
	: EventArgs (Type::ROUTEDEVENTARGS), source (this, SourceWeakRef)
{
	EnsureManagedPeer ();
	this->source = source;
	handled = false;
}

RoutedEventArgs::RoutedEventArgs ()
	: EventArgs (Type::ROUTEDEVENTARGS), source (this, SourceWeakRef)
{
	source = NULL;
	handled = false;
}

RoutedEventArgs::RoutedEventArgs (Type::Kind kind)
	: EventArgs (kind), source (this, SourceWeakRef)
{
	source = NULL;
	handled = false;
}

RoutedEventArgs::RoutedEventArgs (DependencyObject *source, Type::Kind kind)
	: EventArgs (Type::ROUTEDEVENTARGS), source (this, SourceWeakRef)
{
	EnsureManagedPeer ();
	this->source = source;
	handled = false;
}

RoutedEventArgs::~RoutedEventArgs ()
{
	source = NULL;
}

void
RoutedEventArgs::SetHandled (bool handled)
{
	this->handled = handled;
}
	
bool
RoutedEventArgs::GetHandled ()
{
	return handled;
}

DependencyObject *
RoutedEventArgs::GetSource ()
{
	return source;
}

void
RoutedEventArgs::SetSource (DependencyObject *el)
{
	source = el;
}

LogReadyRoutedEventArgs::LogReadyRoutedEventArgs ()
	: RoutedEventArgs (Type::LOGREADYROUTEDEVENTARGS)
{
	log = NULL;
	log_source = (LogSource) 0;
}

LogReadyRoutedEventArgs::~LogReadyRoutedEventArgs ()
{
	g_free (log);
}

MouseEventArgs::MouseEventArgs (MoonMouseEvent *event)
	: RoutedEventArgs (Type::MOUSEEVENTARGS)
{
	if (event != NULL) {
		this->event = (MoonMouseEvent*)event->Clone();
	} else {
		this->event = NULL;
	}
	EnsureManagedPeer ();
}

MouseEventArgs::MouseEventArgs (Type::Kind kind, MoonMouseEvent *event)
	: RoutedEventArgs (kind)
{
	if (event != NULL) {
		this->event = (MoonMouseEvent *) event->Clone();
	} else {
		this->event = NULL;
	}
}

MouseEventArgs::MouseEventArgs ()
	: RoutedEventArgs (Type::MOUSEEVENTARGS)
{
	event = NULL;
}

MouseEventArgs::~MouseEventArgs ()
{
	delete event;
}

void
MouseEventArgs::GetPosition (UIElement *relative_to, double *x, double *y)
{
       if (event) {
	       Point p = event->GetPosition ();
	       *x = p.x;
	       *y = p.y;
       }

       if (relative_to) {
	       // FIXME this a nasty place to do this we should be able to
	       // reduce the problem for this kind of hit testing.
	       if (relative_to->IsAttached ())
		       relative_to->GetDeployment ()->GetSurface ()->ProcessDirtyElements ();
	       relative_to->TransformPoint (x, y);
       }
}

StylusInfo*
MouseEventArgs::GetStylusInfo ()
{
	TabletDeviceType type = TabletDeviceTypeMouse;
	bool is_inverted = false;

	GetEvent()->GetStylusInfo (&type, &is_inverted);

	StylusInfo *info = MoonUnmanagedFactory::CreateStylusInfo ();

	info->SetDeviceType (type);
	info->SetIsInverted (is_inverted);

	return info;
}

StylusPointCollection*
MouseEventArgs::GetStylusPoints (UIElement *ink_presenter)
{
	StylusPointCollection *points = MoonUnmanagedFactory::CreateStylusPointCollection ();
	double pressure;
	double x, y;
	
	GetPosition (ink_presenter, &x, &y);

	pressure = GetEvent()->GetPressure ();

	StylusPoint *point = new StylusPoint ();
	point->SetValue (StylusPoint::XProperty, Value(x));
	point->SetValue (StylusPoint::YProperty, Value(y));
	point->SetValue (StylusPoint::PressureFactorProperty, Value(pressure));

	points->Add (point);

	point->unref ();

	return points;
}

MouseButtonEventArgs::MouseButtonEventArgs ()
	: MouseEventArgs (Type::MOUSEBUTTONEVENTARGS, NULL)
{
}

MouseButtonEventArgs::MouseButtonEventArgs (MoonButtonEvent *event)
	: MouseEventArgs (Type::MOUSEBUTTONEVENTARGS, event)
{
	EnsureManagedPeer ();
}

MouseButtonEventArgs::~MouseButtonEventArgs ()
{
}

MouseWheelEventArgs::MouseWheelEventArgs ()
	: MouseEventArgs (Type::MOUSEWHEELEVENTARGS, NULL)
{
}

MouseWheelEventArgs::MouseWheelEventArgs (MoonScrollWheelEvent *event)
	: MouseEventArgs (Type::MOUSEWHEELEVENTARGS, event)
{
	EnsureManagedPeer ();
}

MouseWheelEventArgs::~MouseWheelEventArgs ()
{
}

int
MouseWheelEventArgs::GetWheelDelta ()
{
	MoonScrollWheelEvent *event = (MoonScrollWheelEvent*)GetEvent();
	return event ? event->GetWheelDelta () : 0;
}

KeyEventArgs::KeyEventArgs (MoonKeyEvent *event)
	: RoutedEventArgs (Type::KEYEVENTARGS)
{
	this->event = (MoonKeyEvent*)event->Clone ();

	EnsureManagedPeer ();
}

KeyEventArgs::KeyEventArgs ()
	: RoutedEventArgs (Type::KEYEVENTARGS)
{
	this->event = NULL;
}

KeyEventArgs::~KeyEventArgs ()
{
	delete event;
}

int
KeyEventArgs::GetKey ()
{
	return event ? event->GetSilverlightKey() : KeyUNKNOWN;
}

int
KeyEventArgs::GetPlatformKeyCode ()
{
	return event ? event->GetPlatformKeycode() : 0;
}

//
// ErrorEventArgs
//
ErrorEventArgs::ErrorEventArgs (DependencyObject *original_source, ErrorEventArgs *args)
	: EventArgs (args->GetObjectType ())
{
	Initialize (original_source, args->error_type, *args->error, args->extended_code, args->extended_message);
}

ErrorEventArgs::ErrorEventArgs (Type::Kind kind, ErrorEventArgsType type, const MoonError error)
	: EventArgs (kind)
{
	Initialize (NULL, type, error, 0, NULL);
}

ErrorEventArgs::ErrorEventArgs (DependencyObject *original_source, Type::Kind kind, ErrorEventArgsType type, const MoonError error)
	: EventArgs (kind)
{
	Initialize (original_source, type, error, 0, NULL);
}

ErrorEventArgs::ErrorEventArgs (ErrorEventArgsType type, MoonError error)
	: EventArgs (Type::ERROREVENTARGS)
{
	Initialize (NULL, type, error, 0, NULL);
}

ErrorEventArgs::ErrorEventArgs (DependencyObject *original_source, ErrorEventArgsType type, MoonError error)
	: EventArgs (Type::ERROREVENTARGS)
{
	Initialize (original_source, type, error, 0, NULL);
}

ErrorEventArgs::ErrorEventArgs (ErrorEventArgsType type, MoonError error, int extended_error_code, const char *extended_msg)
	: EventArgs (Type::ERROREVENTARGS)
{
	Initialize (NULL, type, error, extended_error_code, extended_msg);
}

void
ErrorEventArgs::Initialize (DependencyObject *original_source, ErrorEventArgsType type, const MoonError &error, int extended_error_code, const char *extended_msg)
{
	error_type = type;
	this->error = new MoonError (error);
	this->original_source = original_source;
	if (original_source)
		original_source->ref ();
	extended_message = g_strdup (extended_msg);
	extended_code = extended_error_code;
#if DEBUG
	printf ("Moonlight: ErrorEventArgs created with message: '%s'\n", error.message);
#endif
}

ErrorEventArgs::~ErrorEventArgs ()
{
	delete error;
	g_free (extended_message);
	if (original_source)
		original_source->unref ();
}


//
// ImageErrorEventArgs
//

ImageErrorEventArgs::ImageErrorEventArgs (DependencyObject *original_source, MoonError error)
  : ErrorEventArgs (original_source, Type::IMAGEERROREVENTARGS, ImageError, error)
{
}

ImageErrorEventArgs::~ImageErrorEventArgs ()
{
}

//
// MessageReceivedEventArgs
//

MessageReceivedEventArgs::MessageReceivedEventArgs (const char* message,
						    ReceiverNameScope namescope,
						    const char* receiverName,
						    const char* senderDomain)
	: EventArgs (Type::MESSAGERECEIVEDEVENTARGS)
{
	this->message = g_strdup (message);
	this->namescope = namescope;
	this->receiverName = g_strdup (receiverName);
	this->response = NULL;
	this->senderDomain = g_strdup (senderDomain);
}

MessageReceivedEventArgs::~MessageReceivedEventArgs ()
{
	g_free (message);
	g_free (receiverName);
	g_free (response);
	g_free (senderDomain);
}

//
// SendCompletedEventArgs
//

SendCompletedEventArgs::SendCompletedEventArgs (MoonError *error,
						const char* message,
						const char* receiverName,
						const char* receiverDomain,
						const char* response,
						GCHandle managedUserState)
	: EventArgs (Type::SENDCOMPLETEDEVENTARGS)
{
	this->error = error ? new MoonError (*error) : NULL;
	this->message = g_strdup (message);
	this->receiverName = g_strdup (receiverName);
	this->receiverDomain = g_strdup (receiverDomain);
	this->response = g_strdup (response);
	this->managedUserState = Deployment::GetCurrent ()->CloneGCHandle (managedUserState);
}

SendCompletedEventArgs::~SendCompletedEventArgs ()
{
	delete error;
	g_free (message);
	g_free (receiverName);
	g_free (receiverDomain);
	g_free (response);

	GetDeployment ()->FreeGCHandle (managedUserState);
}

//
// CaptureImageCompletedEventArgs
//
CaptureImageCompletedEventArgs::CaptureImageCompletedEventArgs ()
	: EventArgs (Type::CAPTUREIMAGECOMPLETEDEVENTARGS),
	  source (this, SourceWeakRef)
{
	this->error = NULL;
}

CaptureImageCompletedEventArgs::CaptureImageCompletedEventArgs (MoonError *error)
	: EventArgs (Type::CAPTUREIMAGECOMPLETEDEVENTARGS),
	  source (this, SourceWeakRef)
{
	EnsureManagedPeer ();
	this->error = error ? new MoonError (*error) : NULL;
}

CaptureImageCompletedEventArgs::~CaptureImageCompletedEventArgs ()
{
	delete error;
}

void
CaptureImageCompletedEventArgs::SetSource (BitmapSource *source)
{
	this->source = source;
}

//
//
// ParserErrorEventArgs
//

ParserErrorEventArgs::ParserErrorEventArgs (DependencyObject *original_source, const char *msg, const char *file,
					    int line, int column, int error_code, 
					    const char *element, const char *attribute)
  : ErrorEventArgs (original_source, Type::PARSERERROREVENTARGS, ParserError, MoonError (MoonError::XAML_PARSE_EXCEPTION, error_code, msg))
{
	xml_attribute = g_strdup (attribute);
	xml_element = g_strdup (element);
	xaml_file = g_strdup (file);
	char_position = column;
	line_number = line;
}

ParserErrorEventArgs::~ParserErrorEventArgs ()
{
	g_free (xaml_file);
	g_free (xml_element);
	g_free (xml_attribute);
}

//
// TimelineMarkerRoutedEventArgs
//

TimelineMarkerRoutedEventArgs::TimelineMarkerRoutedEventArgs ()
	: RoutedEventArgs (Type::TIMELINEMARKERROUTEDEVENTARGS), marker (this, MarkerWeakRef)
{

}

TimelineMarkerRoutedEventArgs::TimelineMarkerRoutedEventArgs (TimelineMarker *marker)
	: RoutedEventArgs (Type::TIMELINEMARKERROUTEDEVENTARGS), marker (this, MarkerWeakRef)
{
	EnsureManagedPeer ();
	this->marker = marker;
}

TimelineMarker *
TimelineMarkerRoutedEventArgs::GetMarker ()
{
	return marker;
}

void
TimelineMarkerRoutedEventArgs::SetMarker (TimelineMarker *value)
{
	marker = value;
}

TimelineMarkerRoutedEventArgs::~TimelineMarkerRoutedEventArgs ()
{
	marker = NULL;
}


//
// CheckAndDownloadUpdateAsyncCompletedEventArgs
//

CheckAndDownloadUpdateCompletedEventArgs::CheckAndDownloadUpdateCompletedEventArgs (bool updated, const char *error)
{
	this->updated = updated;
	this->error = g_strdup (error);
}

CheckAndDownloadUpdateCompletedEventArgs::~CheckAndDownloadUpdateCompletedEventArgs ()
{
	g_free (error);
}

//
// SizeChangedEventArgs
//

SizeChangedEventArgs::SizeChangedEventArgs()
{
	SetObjectType(Type::SIZECHANGEDEVENTARGS);
	prev_size = Size (); new_size = Size ();
}

SizeChangedEventArgs::SizeChangedEventArgs (Size prev_size, Size new_size)
{
	SetObjectType(Type::SIZECHANGEDEVENTARGS);
	this->prev_size = prev_size;
	this->new_size = new_size;

	EnsureManagedPeer ();
}

void
SizeChangedEventArgs::GetPrevSize (SizeChangedEventArgs *args, Size *prev_size)
{
	*prev_size = args->GetPrevSize ();
}

void
SizeChangedEventArgs::GetNewSize (SizeChangedEventArgs *args, Size *new_size)
{
	*new_size = args->GetNewSize ();
}


};
