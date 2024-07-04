/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * input.h: Simulate user input.
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007-2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#ifndef __INPUT_H__
#define __INPUT_H__

#include <glib.h>

#define Visual _XVisual
#include <X11/X.h>
#include <X11/extensions/XTest.h>
#undef Visual

#include "netscape.h"

enum VirtualKeys {
    VK_LBUTTON       = 0x01,
    VK_RBUTTON       = 0x02,
    VK_CANCEL        = 0x03,
    VK_MBUTTON       = 0x04,
    VK_XBUTTON1      = 0x05,
    VK_XBUTTON2      = 0x06,
    VK_BACK          = 0x08,
    VK_TAB           = 0x09,
    VK_CLEAR         = 0x0C,
    VK_RETURN        = 0x0D,
    VK_SHIFT         = 0x10,
    VK_CONTROL       = 0x11,
    VK_MENU          = 0x12,
    VK_PAUSE         = 0x13,
    VK_CAPITAL       = 0x14,
    VK_KANA          = 0x15,
    VK_HANGEUL       = 0x15,
    VK_HANGUL        = 0x15,
    VK_JUNJA         = 0x17,
    VK_FINAL         = 0x18,
    VK_HANJA         = 0x19,
    VK_KANJI         = 0x19,
    VK_ESCAPE        = 0x1B,
    VK_CONVERT       = 0x1C,
    VK_NONCONVERT    = 0x1D,
    VK_ACCEPT        = 0x1E,
    VK_MODECHANGE    = 0x1F,
    VK_SPACE         = 0x20,
    VK_PRIOR         = 0x21,
    VK_NEXT          = 0x22,
    VK_END           = 0x23,
    VK_HOME          = 0x24,
    VK_LEFT          = 0x25,
    VK_UP            = 0x26,
    VK_RIGHT         = 0x27,
    VK_DOWN          = 0x28,
    VK_SELECT        = 0x29,
    VK_PRINT         = 0x2A,
    VK_EXECUTE       = 0x2B,
    VK_SNAPSHOT      = 0x2C,
    VK_INSERT        = 0x2D,
    VK_DELETE        = 0x2E,
    VK_HELP          = 0x2F,
    VK_0             = 0x30,
    VK_1             = 0x31,
    VK_2             = 0x32,
    VK_3             = 0x33,
    VK_4             = 0x34,
    VK_5             = 0x35,
    VK_6             = 0x36,
    VK_7             = 0x37,
    VK_8             = 0x38,
    VK_9             = 0x39,
    VK_A             = 0x41,
    VK_B             = 0x42,
    VK_C             = 0x43,
    VK_D             = 0x44,
    VK_E             = 0x45,
    VK_F             = 0x46,
    VK_G             = 0x47,
    VK_H             = 0x48,
    VK_I             = 0x49,
    VK_J             = 0x4A,
    VK_K             = 0x4B,
    VK_L             = 0x4C,
    VK_M             = 0x4D,
    VK_N             = 0x4E,
    VK_O             = 0x4F,
    VK_P             = 0x50,
    VK_Q             = 0x51,
    VK_R             = 0x52,
    VK_S             = 0x53,
    VK_T             = 0x54,
    VK_U             = 0x55,
    VK_V             = 0x56,
    VK_W             = 0x57,
    VK_X             = 0x58,
    VK_Y             = 0x59,
    VK_Z             = 0x5A,
    VK_LWIN          = 0x5B,
    VK_RWIN          = 0x5C,
    VK_APPS          = 0x5D,
    VK_SLEEP         = 0x5F,
    VK_NUMPAD0       = 0x60,
    VK_NUMPAD1       = 0x61,
    VK_NUMPAD2       = 0x62,
    VK_NUMPAD3       = 0x63,
    VK_NUMPAD4       = 0x64,
    VK_NUMPAD5       = 0x65,
    VK_NUMPAD6       = 0x66,
    VK_NUMPAD7       = 0x67,
    VK_NUMPAD8       = 0x68,
    VK_NUMPAD9       = 0x69,
    VK_MULTIPLY      = 0x6A,
    VK_ADD           = 0x6B,
    VK_SEPARATOR     = 0x6C,
    VK_SUBTRACT      = 0x6D,
    VK_DECIMAL       = 0x6E,
    VK_DIVIDE        = 0x6F,
    VK_F1            = 0x70,
    VK_F2            = 0x71,
    VK_F3            = 0x72,
    VK_F4            = 0x73,
    VK_F5            = 0x74,
    VK_F6            = 0x75,
    VK_F7            = 0x76,
    VK_F8            = 0x77,
    VK_F9            = 0x78,
    VK_F10           = 0x79,
    VK_F11           = 0x7A,
    VK_F12           = 0x7B,
    VK_F13           = 0x7C,
    VK_F14           = 0x7D,
    VK_F15           = 0x7E,
    VK_F16           = 0x7F,
    VK_F17           = 0x80,
    VK_F18           = 0x81,
    VK_F19           = 0x82,
    VK_F20           = 0x83,
    VK_F21           = 0x84,
    VK_F22           = 0x85,
    VK_F23           = 0x86,
    VK_F24           = 0x87,
    VK_NUMLOCK       = 0x90,
    VK_SCROLL        = 0x91,
    VK_LSHIFT        = 0xA0,
    VK_RSHIFT        = 0xA1,
    VK_LCONTROL      = 0xA2,
    VK_RCONTROL      = 0xA3,
    VK_LMENU         = 0xA4,
    VK_RMENU         = 0xA5,
    VK_OEM_1         = 0xBA,
    VK_OEM_PLUS      = 0xBB,
    VK_OEM_COMMA     = 0xBC,
    VK_OEM_MINUS     = 0xBD,
    VK_OEM_PERIOD    = 0xBE,
    VK_OEM_2         = 0xBF,
    VK_OEM_3         = 0xC0,
    VK_KP_ADD        = 0xFFBB
};

class InputProvider {
public:
	void MoveMouseLogarithmic (int x, int y);
	void MoveMouse (int x, int y);
	void MoveMouseDirect (int x, int y);
	void MouseDoubleClick (unsigned int delay);
	void MouseLeftClick (unsigned int delay);
	void MouseRightClick (unsigned int delay);
	void MouseLeftButtonDown (unsigned int delay);
	void MouseLeftButtonUp (unsigned int delay);
	void MouseWheel (gint16 clicks);
	bool MouseIsAtPosition (int x, int y);
	void SendKeyInput (guint32 key_code, bool key_down, bool extended, bool unicode);
	void SetKeyboardInputSpeed (unsigned int keyboard_input_speed);

	static InputProvider *GetInstance ();

private:
	InputProvider ();
	~InputProvider ();

	static InputProvider *instance;
	Window root_window;
	bool xtest_available;
	unsigned int keyboard_speed; // input speed from 1 (slow) to 10 (fast), default is 10

	GSList* down_keys;

	void GetCursorPos (int &x, int &y);
	int MapToKeysym (int key);
	Display *GetDisplay ();
};

G_BEGIN_DECLS
int InputHelper_MoveMouseLogarithmic (int x, int y);
int InputHelper_MoveMouseDirect (int x, int y);
int InputHelper_MouseLeftClick (unsigned int delay);
int InputHelper_MouseRightClick (unsigned int delay);
int InputHelper_MouseDoubleClick (unsigned int delay);
int InputHelper_MouseLeftButtonUp (unsigned int delay);
int InputHelper_MouseLeftButtonDown (unsigned int delay);
int InputHelper_MouseWheel (gint16 clicks);
int InputHelper_MouseIsAtPosition (int x, int y, guint8 *result);
int InputHelper_SendKeyInput (int vkey, bool down, bool extended, bool unicode);
int InputHelper_SetKeyboardInputSpeed (unsigned int keyboardInputSpeed);
int InputHelper_InjectPenEventFromBuffer (/* TODO */);
G_END_DECLS

#endif  // __INPUT_H__

