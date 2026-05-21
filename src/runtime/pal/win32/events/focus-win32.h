#pragma once
#include "pal.h"

using namespace Moonlight;

class MoonFocusEventWin32 : public MoonFocusEvent {
public:
    MoonFocusEventWin32(bool is_in) {
        this->is_in = is_in;
    }

    virtual ~MoonFocusEventWin32() {
    }

    virtual MoonEvent *Clone() {
        return new MoonFocusEventWin32(is_in);
    }

    virtual gpointer GetPlatformEvent() {
        return NULL;
    }

    virtual bool IsIn() {
        return is_in;
    }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;

        return is_in ? window->GetSurface()->HandleUIFocusIn(this)
                     : window->GetSurface()->HandleUIFocusOut(this);
    }

private:
    bool is_in;
};
