#pragma once
#include "pal.h"

using namespace Moonlight;

class MoonWheelEventSDL2 : public MoonScrollWheelEvent {
public:
    MoonWheelEventSDL2(float pressure, float x, float y, float delta) {
        this->pressure = pressure;
        this->x = x;
        this->y = y;
        this->delta = delta;
    }

    virtual ~MoonWheelEventSDL2() {
    }

    virtual MoonEvent *Clone() {
        return new MoonWheelEventSDL2(pressure, x, y, delta);
    }

    virtual gpointer GetPlatformEvent() {
        return NULL;
    }

    virtual Point GetPosition() {
        return Point(x, y);
    }

    virtual double GetPressure() {
        return pressure;
    }

    virtual void GetStylusInfo(TabletDeviceType *type, bool *is_inverted) {
    }

    virtual bool HasModifiers() { return true; }

    virtual MoonModifier GetModifiers() {
        return (MoonModifier)0;
    }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;

        return window->GetSurface()->HandleUIScroll(this);
    }

#define MOON_SCROLL_WHEEL_DELTA 10

    virtual int GetWheelDelta() {
        return delta * MOON_SCROLL_WHEEL_DELTA;
    }

private:
    float pressure, x, y, delta;
};