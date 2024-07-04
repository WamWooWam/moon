#pragma once
#include "pal.h"

using namespace Moonlight;

class MoonButtonEventWin32 : public MoonButtonEvent {
public:
    MoonButtonEventWin32(int button, bool is_release, float pressure, float x, float y, int numClicks) {
        this->button = button;
        this->is_release = is_release;
        this->pressure = pressure;
        this->x = x;
        this->y = y;
        this->numClicks = numClicks;
    }

    virtual ~MoonButtonEventWin32() {
    }

    virtual MoonEvent *Clone() {
        return new MoonButtonEventWin32(button, is_release, pressure, x, y, numClicks);
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

    bool IsRelease() {
        return is_release;
    }

    int GetButton() {
        return button;
    }

    // the number of clicks.  gdk provides them as event->type ==
    // GDK_3BUTTON_PRESS/GDK_2BUTTON_PRESS/GDK_BUTTON_PRESS
    virtual int GetNumberOfClicks() {
        return numClicks;
    }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;

        return IsRelease() ? window->GetSurface()->HandleUIButtonRelease(this) : window->GetSurface()->HandleUIButtonPress(this);
    }

private:
    int button;
    bool is_release;
    float pressure;
    float x;
    float y;
    int numClicks;
};
