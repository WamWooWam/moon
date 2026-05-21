/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"
#include <SDL.h>

using namespace Moonlight;

class MoonMotionEventSDL2 : public MoonMotionEvent {
public:
    MoonMotionEventSDL2(float pressure, float x, float y, SDL_Keymod sdlmod = KMOD_NONE) {
        this->pressure = pressure;
        this->x = x;
        this->y = y;
        this->modifiers = (MoonModifier)0;
        if (sdlmod & KMOD_CTRL)  this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Control);
        if (sdlmod & KMOD_SHIFT) this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Shift);
        if (sdlmod & KMOD_ALT)   this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Alt);
    }

    virtual ~MoonMotionEventSDL2() {}

    virtual MoonEvent *Clone() {
        return new MoonMotionEventSDL2(pressure, x, y);
    }

    virtual gpointer GetPlatformEvent() { return NULL; }

    virtual Point GetPosition() { return Point(x, y); }
    virtual double GetPressure() { return pressure; }
    virtual void GetStylusInfo(TabletDeviceType *type, bool *is_inverted) {}

    virtual bool HasModifiers() { return true; }
    virtual MoonModifier GetModifiers() { return modifiers; }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;
        return window->GetSurface()->HandleUIMotion(this);
    }

private:
    float pressure, x, y;
    MoonModifier modifiers;
};
