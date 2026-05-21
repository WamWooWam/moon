/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"

using namespace Moonlight;

class MoonFocusEventSDL2 : public MoonFocusEvent {
public:
    MoonFocusEventSDL2(bool is_in) {
        this->is_in = is_in;
    }

    virtual ~MoonFocusEventSDL2() {}

    virtual MoonEvent *Clone() {
        return new MoonFocusEventSDL2(is_in);
    }

    virtual gpointer GetPlatformEvent() { return NULL; }
    virtual bool IsIn() { return is_in; }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;
        return is_in ? window->GetSurface()->HandleUIFocusIn(this)
                     : window->GetSurface()->HandleUIFocusOut(this);
    }

private:
    bool is_in;
};
