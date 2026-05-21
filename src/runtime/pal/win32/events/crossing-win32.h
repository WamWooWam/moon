#pragma once
#include "pal.h"

using namespace Moonlight;

class MoonCrossingEventWin32 : public MoonCrossingEvent {
public:
    MoonCrossingEventWin32(bool is_enter, float x, float y) {
        this->is_enter = is_enter;
        this->x = x;
        this->y = y;
    }

    virtual ~MoonCrossingEventWin32() {
    }

    virtual MoonEvent *Clone() {
        return new MoonCrossingEventWin32(is_enter, x, y);
    }

    virtual gpointer GetPlatformEvent() {
        return NULL;
    }

    virtual Point GetPosition() {
        return Point(x, y);
    }

    virtual double GetPressure() {
        return 0.0;
    }

    virtual void GetStylusInfo(TabletDeviceType *type, bool *is_inverted) {
        // FIXME
    }

    virtual bool IsEnter() {
        return is_enter;
    }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;

        return window->GetSurface()->HandleUICrossing(this);
    }

private:
    bool is_enter;
    float x;
    float y;
};
