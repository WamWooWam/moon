/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * pal-linux-capture.h
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef MOON_PAL_LINUX_CAPTURE_H
#define MOON_PAL_LINUX_CAPTURE_H

#include "pal.h"

namespace Moonlight {

class MoonCaptureServiceLinux : public MoonCaptureService {
public:
	MoonCaptureServiceLinux ();
	virtual ~MoonCaptureServiceLinux ();

	virtual MoonVideoCaptureService *GetVideoCaptureService();
	virtual MoonAudioCaptureService *GetAudioCaptureService();
private:

	MoonVideoCaptureService *video_service;
	MoonAudioCaptureService *audio_service;
};

};
#endif /* MOON_PAL_LINUX_CAPTURE_H */
