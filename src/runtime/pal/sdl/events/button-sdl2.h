/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"
#include <SDL.h>

using namespace Moonlight;

class MoonButtonEventSDL2 : public MoonButtonEvent {
public:
    MoonButtonEventSDL2(int button, bool is_release, float pressure, float x, float y, int numClicks, SDL_Keymod sdlmod = KMOD_NONE) {
        this->button = button;
        this->is_release = is_release;
        this->pressure = pressure;
        this->x = x;
        this->y = y;
        this->numClicks = numClicks;
        this->modifiers = (MoonModifier)0;
        if (sdlmod & KMOD_CTRL)  this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Control);
        if (sdlmod & KMOD_SHIFT) this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Shift);
        if (sdlmod & KMOD_ALT)   this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Alt);
    }

    virtual ~MoonButtonEventSDL2() {}

    virtual MoonEvent *Clone() {
        return new MoonButtonEventSDL2(button, is_release, pressure, x, y, numClicks);
    }

    virtual gpointer GetPlatformEvent() { return NULL; }

    virtual Point GetPosition() { return Point(x, y); }
    virtual double GetPressure() { return pressure; }

    virtual void GetStylusInfo(TabletDeviceType *type, bool *is_inverted) {}

    virtual bool HasModifiers() { return true; }
    virtual MoonModifier GetModifiers() { return modifiers; }

    bool IsRelease() { return is_release; }
    int GetButton() { return button; }
    virtual int GetNumberOfClicks() { return numClicks; }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;
        return IsRelease() ? window->GetSurface()->HandleUIButtonRelease(this)
                           : window->GetSurface()->HandleUIButtonPress(this);
    }

private:
    int button;
    bool is_release;
    float pressure, x, y;
    int numClicks;
    MoonModifier modifiers;
};
