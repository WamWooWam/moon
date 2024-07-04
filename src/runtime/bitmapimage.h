/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * bitmapimage.h
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007-2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __BITMAPIMAGE_H__
#define __BITMAPIMAGE_H__

#include "utils.h"
#include "dependencyobject.h"
#include "downloader.h"
#include "bitmapsource.h"

namespace Moonlight {

/* @Namespace=System.Windows.Media.Imaging */
class BitmapImage : public BitmapSource {
 private:
	Downloader *downloader;
	MoonPixbufLoader *loader;
	MoonError *moon_error;
	char *part_name;
	Cancellable *get_res_aborter;
	DownloaderAccessPolicy policy;

 protected:
	/* @GeneratePInvoke */
	BitmapImage ();

	virtual ~BitmapImage ();

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

 public:
	virtual void Dispose ();

	/* @PropertyType=Uri,IsConstPropertyType,AlwaysChange,GenerateAccessors,DefaultValue=new Uri() */
	const static int UriSourceProperty;
	/* @PropertyType=double,DefaultValue=0.0,ManagedAccess=Internal,GenerateAccessors */
	const static int ProgressProperty;
	/* @PropertyType=BitmapCreateOptions,DefaultValue=BitmapCreateOptionsDelayCreation,GenerateAccessors */
	const static int CreateOptionsProperty;

	void SetUriSource (const Uri* value);
	const Uri* GetUriSource ();
	
	void SetProgress (double progress);
	double GetProgress ();

	void SetCreateOptions (BitmapCreateOptions value);
	BitmapCreateOptions GetCreateOptions ();

	void SetDownloadPolicy (DownloaderAccessPolicy dlpolicy)
	{ 
		policy = dlpolicy;
	}
	
	void CleanupLoader ();
	void CreateLoader (unsigned char *buffer);
	/* @GeneratePInvoke */
	void PixbufWrite (gpointer buffer, gint32 offset, gint32 n);
	/* @GeneratePInvoke */
	void PixmapComplete ();

	virtual void OnPropertyChanged (PropertyChangedEventArgs *args, MoonError *error);

	/* @DelegateType=EventHandler<DownloadProgressEventArgs> */
	const static int DownloadProgressEvent;
	/* @DelegateType=EventHandler<ExceptionRoutedEventArgs> */
	const static int ImageFailedEvent;
	/* @DelegateType=EventHandler<RoutedEventArgs> */
	const static int ImageOpenedEvent;

	void SetDownloader (Downloader *downloader, Uri *uri, const char *part_name);
	void CleanupDownloader ();
	void DownloaderProgressChanged ();
	void DownloaderComplete ();
	void DownloaderFailed ();
	void UriSourceChanged ();
	void Abort ();

	static void uri_source_changed_callback (EventObject *user_data);
	static void downloader_progress_changed (EventObject *sender, EventArgs *calldata, gpointer closure);
	static void downloader_complete (EventObject *sender, EventArgs *calldata, gpointer closure);
	static void downloader_failed (EventObject *sender, EventArgs *calldata, gpointer closure);
	static void pixbuf_write (EventObject *sender, EventArgs *calldata, gpointer data);
};

};

#endif /* __BITMAPIMAGE_H__ */
