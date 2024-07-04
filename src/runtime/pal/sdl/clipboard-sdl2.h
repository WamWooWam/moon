
#ifndef MOON_CLIPBOARD_SDL2_H
#define MOON_CLIPBOARD_SDL2_H

#include "config.h"
#include "window-sdl2.h"
#include "pal-sdl2.h"

namespace Moonlight {

class MoonClipboardSDL2 : public MoonClipboard {
public:
	MoonClipboardSDL2 (MoonWindowSDL2 *win, MoonClipboardType clipboardType);

	virtual bool ContainsText ();
	virtual void SetText (const char *text);
	virtual void AsyncGetText (MoonClipboardGetTextCallback cb, gpointer data);
	virtual char* GetText ();
};

};

#endif /* MOON_CLIPBOARD_SDL2_H */