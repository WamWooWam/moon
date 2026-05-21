/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"

using namespace Moonlight;

class MoonCrossingEventSDL2 : public MoonCrossingEvent {
public:
    MoonCrossingEventSDL2(bool is_enter, float x, float y) {
        this->is_enter = is_enter;
        this->x = x;
        this->y = y;
    }

    virtual ~MoonCrossingEventSDL2() {}

    virtual MoonEvent *Clone() {
        return new MoonCrossingEventSDL2(is_enter, x, y);
    }

    virtual gpointer GetPlatformEvent() { return NULL; }

    virtual Point GetPosition() { return Point(x, y); }
    virtual double GetPressure() { return 0.0; }
    virtual void GetStylusInfo(TabletDeviceType *type, bool *is_inverted) {}
    virtual bool IsEnter() { return is_enter; }

    virtual bool HasModifiers() { return false; }
    virtual MoonModifier GetModifiers() { return (MoonModifier)0; }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;
        return window->GetSurface()->HandleUICrossing(this);
    }

private:
    bool is_enter;
    float x, y;
};
