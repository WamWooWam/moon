/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * pal-linux-network.h: network notifications for for linux
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef MOON_PAL_LINUX_NETWORK_H
#define MOON_PAL_LINUX_NETWORK_H

#include "pal.h"

#ifdef PAL_DBUS_NETWORKAVAILABILITY
#include "network/dbus/pal-dbus-network.h"
#endif

namespace Moonlight {

class MoonNetworkServiceLinux : public MoonNetworkService {
public:
	MoonNetworkServiceLinux ();
	virtual ~MoonNetworkServiceLinux ();

	virtual bool SetNetworkStateChangedCallback (MoonCallback callback, gpointer data);

	virtual bool GetIsNetworkAvailable ();

	void PollNetworkState ();

private:
#ifdef PAL_DBUS_NETWORKAVAILABILITY
	MoonNetworkServiceDbus *dbus_network_service;
#else
	void *dbus_network_service;
#endif
	MoonCallback callback;
	gpointer user_data;
	bool using_dbus;
	bool is_avail;
	guint idle;
};

};
#endif /* MOON_PAL_LINUX_NETWORK_H */
