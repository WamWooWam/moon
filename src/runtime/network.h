/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * network.h
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 */
 
#ifndef __MOON_NETWORK_H__
#define __MOON_NETWORK_H__

#include <glib.h>

#include "pal.h"
#include "dependencyobject.h"
#include "eventargs.h"
#include "downloader.h"

namespace Moonlight {

class HttpHandler;
class HttpRequest;
class HttpResponse;

/*
 * HttpRequestProgressChangedEventArgs
 */

/* @Namespace=None */
class MOON_API HttpRequestProgressChangedEventArgs : public EventArgs {
private:
	double progress;

public:
	HttpRequestProgressChangedEventArgs (double progress)
	{
		this->progress = progress;
	}

	double GetProgress () { return progress; }
};

/*
 * HttpRequestStoppedEventArgs
 */

/* @Namespace=None */
class MOON_API HttpRequestStoppedEventArgs : public EventArgs {
private:
	char *error_msg;

public:
	HttpRequestStoppedEventArgs (const char *error_msg)
	{
		this->error_msg = error_msg != NULL ?  g_strdup (error_msg) : NULL;
	}
	virtual ~HttpRequestStoppedEventArgs ()
	{
		g_free (error_msg);
	}

	const char *GetErrorMessage () { return error_msg; }
	bool IsSuccess () { return error_msg == NULL; }
};

/*
 * HttpRequestWriteEventArgs
 */

/* @Namespace=None */
class MOON_API HttpRequestWriteEventArgs : public EventArgs {
private:
	void *data;
	gint64 offset;
	gint32 count;
public:
	HttpRequestWriteEventArgs (void *data, gint64 offset, gint32 count)
	{
		this->data = data;
		this->offset = offset;
		this->count = count;
	}
	/* @GeneratePInvoke */
	void *GetData () { return data; }
	void SetData (void *value) { data = value; }
	/* @GeneratePInvoke */
	guint64 GetOffset () { return offset; }
	void SetOffset (gint64 value) { offset = value; }
	/* @GeneratePInvoke */
	guint32 GetCount () { return count; }
	void SetCount (gint32 value) { count = value; }
};

/*
 * HttpRequest
 */

/* @Namespace=None,ManagedEvents=Manual */
class MOON_API HttpRequest : public EventObject {
public:
	enum Options {
		OptionsNone = 0,
		CustomHeaders = 1,
		/* This will prevent file to be stored in browser cache (required for mms where there are several
		 * simultaneous requests for the same url. If the url is cached, firefox will only download 1
		 * request at a time since the cache entry for that url is busy) */
		DisableCache = 2,
		/* Data will not be written to disk. User must listen to the Write event */
		DisableFileStorage = 4,
		/* Disables async Send. At least the Downloader class should use this, since Downloader already has an async Send. */
		DisableAsyncSend = 8,
		/* Forces HTTP/1.0. This is required for media logs sent to windows media servers, since curl will confuse the server otherwise. */
		ForceHttp_1_0 = 16,
		/* Keep in sync with HttpRequestOptions in NativeMethods.cs */
	};

	virtual void Dispose ();

	/* Public API */
	/* @GeneratePInvoke */
	void Open (const char *verb, const Uri *uri, DownloaderAccessPolicy policy);
	void Open (const char *verb, const Uri *uri, const Uri *resource_base, DownloaderAccessPolicy policy);
	/* @GeneratePInvoke */
	void Send ();
	/* @GeneratePInvoke */
	void Abort ();
	/* @GeneratePInvoke */
	void SetBody (/* @MarshalAs=byte[] */ const void *body, gint32 length);
	/* @GeneratePInvoke */
	void SetHeader (const char *header, const char *value, bool disable_folding);
	void SetHeaderFormatted (const char *header, char *value, bool disable_folding);
	/* @GeneratePInvoke */
	HttpResponse *GetResponse ();

	Options GetOptions () { return options; }

	bool IsAborted () { return is_aborted; }
	bool IsCompleted () { return is_completed; }

	HttpHandler *GetHandler () { return handler; }

	const Uri  *GetUri () { return request_uri; }
	const char *GetVerb () { return verb; }
	const Uri  *GetOriginalUri () { return original_uri; }
	const Uri  *GetFinalUri () { return final_uri; }
	const char *GetFilename () { return tmpfile; }
	gint64 GetNotifiedSize () { return notified_size; }
	bool IsCrossDomain () { return is_cross_domain; }

	/* Events */
	const static int StartedEvent; /* HttpResponse (and its headers/response status) is available when this event is emitted */
	const static int StoppedEvent; /* You'll always get this event (and only once) after calling Send, even if the request is aborted */
	const static int ProgressChangedEvent;
	const static int WriteEvent;

protected:
	/* @SkipFactories */
	HttpRequest (Type::Kind type, HttpHandler *handler, Options options);
	virtual ~HttpRequest ();

	/* API to be overridden */
	virtual void OpenImpl () = 0;
	virtual void SendImpl () = 0;
	virtual void AbortImpl () = 0;
	virtual void SetBodyImpl (const void *body, guint32 length) = 0;
	virtual void SetHeaderImpl (const char *header, const char *value, bool disable_folding) = 0;

	/* This method must be called before starting to serve any data */
	void Started (HttpResponse *response);
	/* Optional method to notify the size of the file */
	void NotifySize (gint64 size);
	/* NotifyFinalUri must be called to validate the final uri (redirection policies) */
	void NotifyFinalUri (const Uri *value);
	void NotifyFinalUri (const char *value);
	/* offset might be -1 to write at the current position */
	void Write (gint64 offset, void *buffer, gint32 length);
	/* either Succeeded or Failed must be called (unless the request is aborted)*/
	void Succeeded ();
	void Failed (const char *error_message);

private:
	const char *GetDownloadDir ();

	HttpHandler *handler;
	HttpResponse *response;
	Options options;
	bool is_aborted;
	bool is_completed; /* does not say anything about whether it failed or not */
	char *verb;
	/* uri of what we were requested to fetch */
	Uri *original_uri;
	/* uri we actually requested */
	Uri *request_uri;
	/* final_uri is what was retrieved (might be different from above due to redirections) */
	Uri *final_uri;
	char *tmpfile;
	int tmpfile_fd;
	gint64 notified_size;
	gint64 written_size;
	DownloaderAccessPolicy access_policy;
	char *local_file; /* the local file we're to serve */
	bool is_cross_domain;

	bool CheckRedirectionPolicy (const Uri *url);
	static void SendAsyncCallback (EventObject *obj);
	void SendAsync ();
};

/*
 * HttpHeader
 */

class MOON_API HttpHeader : public List::Node {
private:
	char *header;
	char *value;

public:
	HttpHeader (const char *header, const char *value)
	{
		this->header = g_strdup (header);
		this->value = g_strdup (value);
	}
	virtual ~HttpHeader ()
	{
		g_free (header);
		g_free (value);
	}
	const char *GetHeader () { return header; }
	const char *GetValue () { return value; }
};

/* @CBindingRequisite */
typedef void (* HttpHeaderVisitor) (gpointer context, const char *header, const char *value);

/*
 * HttpResponse
 */

class MOON_API HttpResponse : public EventObject {
private:
	List *headers;
	gint32 response_status;
	char *response_status_text;
	guint64 content_length;

protected:
	/* @SkipFactories */
	HttpResponse (Type::Kind type, HttpRequest *request);
	virtual ~HttpResponse () {}

public:
	/* @SkipFactories */
	HttpResponse (HttpRequest *request);
	virtual void Dispose ();

	/* @GeneratePInvoke */
	void VisitHeaders (HttpHeaderVisitor visitor, void *context);

	/* List of HttpHeader */
	List *GetHeaders ();
	void ParseHeaders (const char *value);
	void AppendHeader (const char *header, const char *value);
	bool ContainsHeader (const char *header, const char *value);

	/* @GeneratePInvoke */
	gint32 GetResponseStatus () { return response_status; }

	/* @GeneratePInvoke */
	const char *GetResponseStatusText () { return response_status_text; }

	void SetStatus (gint32 status, const char *status_text);

	guint64 GetContentLength () { return content_length; }
};

/*
 * HttpHandler
 *   To provide a custom HttpHandler, you need to create a subclass of this class, and call Deployment::SetHttpHandler.
 *   You also need to provide an implementation of HttpRequest.
 */

class MOON_API HttpHandler : public EventObject {
private:
	char *download_dir;

protected:
	/* @SkipFactories */
	HttpHandler (Type::Kind type);

public:
	virtual ~HttpHandler () {}
	/* The implementation may return NULL if it can't create a request that can honor the options */
	virtual HttpRequest *CreateRequest (HttpRequest::Options options) = 0;
	const char *GetDownloadDir ();
};

};
#endif /* __MOON_NETWORK_H__ */
