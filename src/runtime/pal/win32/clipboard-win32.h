
#ifndef MOON_CLIPBOARD_WIN32_H
#define MOON_CLIPBOARD_WIN32_H

#include "config.h"
#include "window-win32.h"
#include "pal-win32.h"

namespace Moonlight {

class MoonClipboardWin32 : public MoonClipboard {
public:
	MoonClipboardWin32 (MoonWindowWin32 *win, MoonClipboardType clipboardType);

	virtual bool ContainsText ();
	virtual void SetText (const char *text);
	virtual void AsyncGetText (MoonClipboardGetTextCallback cb, gpointer data);
	virtual char* GetText ();

private:
    HWND hwnd;
};

};

#endif /* MOON_CLIPBOARD_WIN32_H */