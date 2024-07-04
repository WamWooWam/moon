/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * error.h: ErrorEventArgs and its subclasses
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_ERROR_H__
#define __MOON_ERROR_H__

namespace Moonlight {
class ErrorEventArgs;
};

#include "pal.h"
#include "enums.h"
#include "gchandle.h"

namespace Moonlight {

class ParserErrorEventArgs;

#ifdef NO_ERROR
#undef NO_ERROR
#endif

class MOON_API MoonError {
public:
	enum ExceptionType {
		NO_ERROR = 0,
		EXCEPTION = 1,
		ARGUMENT = 2,
		ARGUMENT_NULL = 3,
		ARGUMENT_OUT_OF_RANGE = 4,
		INVALID_OPERATION = 5,
		XAML_PARSE_EXCEPTION = 6,
		UNAUTHORIZED_ACCESS = 7,
		EXECUTION_ENGINE_EXCEPTION = 8,
		GCHANDLE_EXCEPTION = 9,
		LISTEN_FAILED = 10,
		SEND_FAILED = 11,
		NOT_IMPLEMENTED_EXCEPTION = 12,
		SECURITY_EXCEPTION = 13,
		NOT_SUPPORTED_EXCEPTION = 14
	};

	// non-zero if an error occurred.
	ExceptionType number;

	// the silverlight error code
	int code;

	// Used for xaml parsing exceptions
	int char_position;
	int line_number;

	char *message;

	// managed code has thrown an exception, we store a gchandle
	// to the exception here.
	GCHandle gchandle;
	
	MoonError ();
	MoonError (ExceptionType type, int code, const char *message);
	~MoonError ();

	MoonError (const MoonError &e);
	MoonError& operator= (const MoonError& other);

	void Clear ();
	
  	static void FillIn (MoonError *error, ExceptionType type, int code, const char *message);
  	static void FillIn (MoonError *error, ExceptionType type, const char *message);
	static void FillIn (MoonError *error, ParserErrorEventArgs *error_args);
};

};
#endif /* __MOON_ERROR_H__ */
