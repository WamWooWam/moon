#pragma once
#include "pal.h"

using namespace Moonlight;

class MoonMotionEventSDL2 : public MoonMotionEvent {
public:
    MoonMotionEventSDL2(int32_t metastate, float pressure, float x, float y) {
        this->metastate = metastate;
        this->pressure = pressure;
        this->x = x;
        this->y = y;
    }

    virtual ~MoonMotionEventSDL2() {
    }

    virtual MoonEvent *Clone() {
        return new MoonMotionEventSDL2(metastate, pressure, x, y);
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
        // FIXME
    }

    virtual bool HasModifiers() { return true; }

    virtual MoonModifier GetModifiers() {
        return (MoonModifier)0;
    }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;

        // g_debug("MoonMotionEvent (pressure= %g, x= %g, y= %g)",
        //     pressure,
        //     x, y);

        return window->GetSurface()->HandleUIMotion(this);
    }

private:
    int32_t metastate;
    float pressure;
    float x;
    float y;
};
