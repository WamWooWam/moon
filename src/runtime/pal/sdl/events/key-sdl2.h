/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"
#include <SDL.h>

using namespace Moonlight;

static Key KeyFromSDLKeycode(SDL_Keycode sym);
static Key KeyFromChar(int32_t uc);
static MoonModifier ModifiersFromSDLMod(SDL_Keymod mod);

class MoonKeyEventSDL2 : public MoonKeyEvent {
public:
    MoonKeyEventSDL2(bool down, bool has_char, int32_t keycode, int32_t scancode, SDL_Keymod sdlmod) {
        this->down = down;
        this->has_char = has_char;
        this->keycode = keycode;
        this->scancode = scancode;
        this->modifiers = ModifiersFromSDLMod(sdlmod);
        this->key = has_char ? KeyFromChar(keycode) : KeyFromSDLKeycode((SDL_Keycode)keycode);

        // Ctrl+letter: SDL_TEXTINPUT suppresses these, but just in case
        if (has_char && (sdlmod & KMOD_CTRL) && keycode >= 1 && keycode <= 26) {
            this->key = (Key)(KeyA + (keycode - 1));
            this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Control);
        }
    }

    virtual ~MoonKeyEventSDL2() {}

    virtual MoonEvent *Clone() {
        MoonKeyEventSDL2 *c = new MoonKeyEventSDL2(down, has_char, keycode, scancode, KMOD_NONE);
        c->modifiers = this->modifiers;
        c->key = this->key;
        return c;
    }

    virtual gpointer GetPlatformEvent() { return NULL; }
    virtual Key GetSilverlightKey() { return key; }
    virtual int GetPlatformKeycode() { return scancode; }
    virtual int GetPlatformKeyval() { return keycode; }
    virtual gunichar GetUnicode() { return has_char ? (gunichar)keycode : 0; }
    virtual bool HasModifiers() { return this->modifiers != 0; }
    virtual MoonModifier GetModifiers() { return this->modifiers; }
    virtual bool IsModifier() { return false; }

    bool IsRelease() { return !down; }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;
        return IsRelease() ? window->GetSurface()->HandleUIKeyRelease(this)
                           : window->GetSurface()->HandleUIKeyPress(this);
    }

private:
    bool down, has_char;
    Key key;
    int32_t keycode, scancode;
    MoonModifier modifiers;
};

static MoonModifier ModifiersFromSDLMod(SDL_Keymod mod) {
    MoonModifier result = (MoonModifier)0;
    if (mod & KMOD_CTRL)  result = (MoonModifier)(result | MoonModifier_Control);
    if (mod & KMOD_SHIFT) result = (MoonModifier)(result | MoonModifier_Shift);
    if (mod & KMOD_ALT)   result = (MoonModifier)(result | MoonModifier_Alt);
    return result;
}

static Key KeyFromSDLKeycode(SDL_Keycode sym) {
    switch (sym) {
    case SDLK_BACKSPACE:    return KeyBACKSPACE;
    case SDLK_TAB:          return KeyTAB;
    case SDLK_RETURN:       return KeyENTER;
    case SDLK_ESCAPE:       return KeyESCAPE;
    case SDLK_SPACE:        return KeySPACE;
    case SDLK_DELETE:       return KeyDELETE;
    case SDLK_END:          return KeyEND;
    case SDLK_HOME:         return KeyHOME;
    case SDLK_LEFT:         return KeyLEFT;
    case SDLK_UP:           return KeyUP;
    case SDLK_RIGHT:        return KeyRIGHT;
    case SDLK_DOWN:         return KeyDOWN;
    case SDLK_INSERT:       return KeyINSERT;
    case SDLK_PAGEUP:       return KeyPAGEUP;
    case SDLK_PAGEDOWN:     return KeyPAGEDOWN;
    case SDLK_0:            return KeyDIGIT0;
    case SDLK_1:            return KeyDIGIT1;
    case SDLK_2:            return KeyDIGIT2;
    case SDLK_3:            return KeyDIGIT3;
    case SDLK_4:            return KeyDIGIT4;
    case SDLK_5:            return KeyDIGIT5;
    case SDLK_6:            return KeyDIGIT6;
    case SDLK_7:            return KeyDIGIT7;
    case SDLK_8:            return KeyDIGIT8;
    case SDLK_9:            return KeyDIGIT9;
    case SDLK_a:            return KeyA;
    case SDLK_b:            return KeyB;
    case SDLK_c:            return KeyC;
    case SDLK_d:            return KeyD;
    case SDLK_e:            return KeyE;
    case SDLK_f:            return KeyF;
    case SDLK_g:            return KeyG;
    case SDLK_h:            return KeyH;
    case SDLK_i:            return KeyI;
    case SDLK_j:            return KeyJ;
    case SDLK_k:            return KeyK;
    case SDLK_l:            return KeyL;
    case SDLK_m:            return KeyM;
    case SDLK_n:            return KeyN;
    case SDLK_o:            return KeyO;
    case SDLK_p:            return KeyP;
    case SDLK_q:            return KeyQ;
    case SDLK_r:            return KeyR;
    case SDLK_s:            return KeyS;
    case SDLK_t:            return KeyT;
    case SDLK_u:            return KeyU;
    case SDLK_v:            return KeyV;
    case SDLK_w:            return KeyW;
    case SDLK_x:            return KeyX;
    case SDLK_y:            return KeyY;
    case SDLK_z:            return KeyZ;
    case SDLK_KP_0:         return KeyNUMPAD0;
    case SDLK_KP_1:         return KeyNUMPAD1;
    case SDLK_KP_2:         return KeyNUMPAD2;
    case SDLK_KP_3:         return KeyNUMPAD3;
    case SDLK_KP_4:         return KeyNUMPAD4;
    case SDLK_KP_5:         return KeyNUMPAD5;
    case SDLK_KP_6:         return KeyNUMPAD6;
    case SDLK_KP_7:         return KeyNUMPAD7;
    case SDLK_KP_8:         return KeyNUMPAD8;
    case SDLK_KP_9:         return KeyNUMPAD9;
    case SDLK_KP_MULTIPLY:  return KeyMULTIPLY;
    case SDLK_KP_PLUS:      return KeyADD;
    case SDLK_KP_MINUS:     return KeySUBTRACT;
    case SDLK_KP_DECIMAL:   return KeyDECIMAL;
    case SDLK_KP_DIVIDE:    return KeyDIVIDE;
    case SDLK_F1:           return KeyF1;
    case SDLK_F2:           return KeyF2;
    case SDLK_F3:           return KeyF3;
    case SDLK_F4:           return KeyF4;
    case SDLK_F5:           return KeyF5;
    case SDLK_F6:           return KeyF6;
    case SDLK_F7:           return KeyF7;
    case SDLK_F8:           return KeyF8;
    case SDLK_F9:           return KeyF9;
    case SDLK_F10:          return KeyF10;
    case SDLK_F11:          return KeyF11;
    case SDLK_F12:          return KeyF12;
    case SDLK_LSHIFT:
    case SDLK_RSHIFT:       return KeySHIFT;
    case SDLK_LCTRL:
    case SDLK_RCTRL:        return KeyCTRL;
    case SDLK_LALT:
    case SDLK_RALT:         return KeyALT;
    case SDLK_CAPSLOCK:     return KeyCAPSLOCK;
    default:                return KeyUNKNOWN;
    }
}

static Key KeyFromChar(int32_t uc) {
    if (uc >= 'a' && uc <= 'z') return (Key)(KeyA + (uc - 'a'));
    if (uc >= 'A' && uc <= 'Z') return (Key)(KeyA + (uc - 'A'));
    if (uc >= '0' && uc <= '9') return (Key)(KeyDIGIT0 + (uc - '0'));
    if (uc == 0x20) return KeySPACE;
    return KeyUNKNOWN;
}
