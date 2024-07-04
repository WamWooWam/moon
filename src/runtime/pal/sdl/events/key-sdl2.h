#pragma once
#include "pal.h"

using namespace Moonlight;

static Key KeyFromVirtualKey(int virtualKey);
static Key KeyFromChar(int32_t uc);

enum {
    CTRL_BASE = 'A' - 1,
    SELECT_ALL = 'A' - CTRL_BASE, // Ctrl+A
    COPY = 'C' - CTRL_BASE,       // Ctrl+C
    CUT = 'X' - CTRL_BASE,        // Ctrl+X
    PASTE = 'V' - CTRL_BASE,      // Ctrl+V
    UNDO = 'Z' - CTRL_BASE,       // Ctrl+Z
    REDO = 'Y' - CTRL_BASE,       // Ctrl+Y
};

class MoonKeyEventSDL2 : public MoonKeyEvent {
public:
    MoonKeyEventSDL2(bool down, bool has_char, int32_t keycode, int32_t scancode, int32_t metastate, MoonModifier modifiers = (MoonModifier)0) {
        this->down = down;
        this->has_char = has_char;

        this->key = has_char ? KeyFromChar(keycode) : KeyFromVirtualKey(keycode);
        this->keycode = keycode;
        this->scancode = scancode;
        this->metastate = metastate;
        this->modifiers = modifiers;

        if (has_char) {
            switch (keycode) {
            case SELECT_ALL:
                this->key = KeyA;
                this->modifiers = MoonModifier_Control;
                break;
            case COPY:
                this->key = KeyC;
                this->modifiers = MoonModifier_Control;
                break;
            case CUT:
                this->key = KeyX;
                this->modifiers = MoonModifier_Control;
                break;
            case PASTE:
                this->key = KeyV;
                this->modifiers = MoonModifier_Control;
                break;
            case UNDO:
                this->key = KeyZ;
                this->modifiers = MoonModifier_Control;
                break;
            case REDO:
                this->key = KeyY;
                this->modifiers = MoonModifier_Control;
                break;
            default:
                break;
            }
        }

        if (GetKeyState(VK_CONTROL) < 0) {
            this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Control);
        }
        if (GetKeyState(VK_MENU) < 0) {
            this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Alt);
        }
        if (GetKeyState(VK_SHIFT) < 0) {
            this->modifiers = (MoonModifier)(this->modifiers | MoonModifier_Shift);
        }
    }

    virtual ~MoonKeyEventSDL2() {
    }

    virtual MoonEvent *Clone() {
        return new MoonKeyEventSDL2(down, has_char, keycode, scancode, metastate, modifiers);
    }

    virtual gpointer GetPlatformEvent() {
        // FIXME we can't cache the native event in this
        // wrapper class. this is used by gtk to deal with the
        // input method support.  is this necessary on
        // SDL2?
        return NULL;
    }

    virtual Key GetSilverlightKey() {
        return key;
    }

    virtual int GetPlatformKeycode() {
        return scancode;
    }

    virtual int GetPlatformKeyval() {
        return keycode;
    }

    virtual gunichar GetUnicode() {
        return has_char ? keycode : 0;
    }

    virtual bool HasModifiers() { return this->modifiers != 0; }

    virtual MoonModifier GetModifiers() {
        return this->modifiers;
    }

    virtual bool IsModifier() {
        return false;
    }

    bool IsRelease() {
        return !down;
    }

    virtual MoonEventStatus DispatchToWindow(MoonWindow *window) {
        if (!window || !window->GetSurface())
            return MoonEventNotHandled;

        return IsRelease() ? window->GetSurface()->HandleUIKeyRelease(this) : window->GetSurface()->HandleUIKeyPress(this);
    }

private:
    bool down;
    bool has_char;
    Key key;
    int32_t keycode;
    int32_t scancode;
    int32_t metastate;
    MoonModifier modifiers;
};

static Key KeyFromVirtualKey(int virtualKey) {
    Key key = (Key)0;

    switch (virtualKey) {

    case VK_BACK:
        key = Key::KeyBACKSPACE;
        break;

    case VK_TAB:
        key = Key::KeyTAB;
        break;

    case VK_RETURN:
        key = Key::KeyENTER;
        break;

    case VK_ESCAPE:
        key = Key::KeyESCAPE;
        break;

    case VK_SPACE:
        key = Key::KeySPACE;
        break;

    case VK_END:
        key = Key::KeyEND;
        break;

    case VK_HOME:
        key = Key::KeyHOME;
        break;

    case VK_LEFT:
        key = Key::KeyLEFT;
        break;

    case VK_UP:
        key = Key::KeyUP;
        break;

    case VK_RIGHT:
        key = Key::KeyRIGHT;
        break;

    case VK_DOWN:
        key = Key::KeyDOWN;
        break;

    case VK_INSERT:
        key = Key::KeyINSERT;
        break;

    case VK_DELETE:
        key = Key::KeyDELETE;
        break;

    case '0':
        key = Key::KeyDIGIT0;
        break;

    case '1':
        key = Key::KeyDIGIT1;
        break;

    case '2':
        key = Key::KeyDIGIT2;
        break;

    case '3':
        key = Key::KeyDIGIT3;
        break;

    case '4':
        key = Key::KeyDIGIT4;
        break;

    case '5':
        key = Key::KeyDIGIT5;
        break;

    case '6':
        key = Key::KeyDIGIT6;
        break;

    case '7':
        key = Key::KeyDIGIT7;
        break;

    case '8':
        key = Key::KeyDIGIT8;
        break;

    case '9':
        key = Key::KeyDIGIT9;
        break;

    case 'A':
        key = Key::KeyA;
        break;

    case 'B':
        key = Key::KeyB;
        break;

    case 'C':
        key = Key::KeyC;
        break;

    case 'D':
        key = Key::KeyD;
        break;

    case 'E':
        key = Key::KeyE;
        break;

    case 'F':
        key = Key::KeyF;
        break;

    case 'G':
        key = Key::KeyG;
        break;

    case 'H':
        key = Key::KeyH;
        break;

    case 'I':
        key = Key::KeyI;
        break;

    case 'J':
        key = Key::KeyJ;
        break;

    case 'K':
        key = Key::KeyK;
        break;

    case 'L':
        key = Key::KeyL;
        break;

    case 'M':
        key = Key::KeyM;
        break;

    case 'N':
        key = Key::KeyN;
        break;

    case 'O':
        key = Key::KeyO;
        break;

    case 'P':
        key = Key::KeyP;
        break;

    case 'Q':
        key = Key::KeyQ;
        break;

    case 'R':
        key = Key::KeyR;
        break;

    case 'S':
        key = Key::KeyS;
        break;

    case 'T':
        key = Key::KeyT;
        break;

    case 'U':
        key = Key::KeyU;
        break;

    case 'V':
        key = Key::KeyV;
        break;

    case 'W':
        key = Key::KeyW;
        break;

    case 'X':
        key = Key::KeyX;
        break;

    case 'Y':
        key = Key::KeyY;
        break;

    case 'Z':
        key = Key::KeyZ;
        break;

    case VK_NUMPAD0:
        key = Key::KeyNUMPAD0;
        break;

    case VK_NUMPAD1:
        key = Key::KeyNUMPAD1;
        break;

    case VK_NUMPAD2:
        key = Key::KeyNUMPAD2;
        break;

    case VK_NUMPAD3:
        key = Key::KeyNUMPAD3;
        break;

    case VK_NUMPAD4:
        key = Key::KeyNUMPAD4;
        break;

    case VK_NUMPAD5:
        key = Key::KeyNUMPAD5;
        break;

    case VK_NUMPAD6:
        key = Key::KeyNUMPAD6;
        break;

    case VK_NUMPAD7:
        key = Key::KeyNUMPAD7;
        break;

    case VK_NUMPAD8:
        key = Key::KeyNUMPAD8;
        break;

    case VK_NUMPAD9:
        key = Key::KeyNUMPAD9;
        break;

    case VK_MULTIPLY:
        key = Key::KeyMULTIPLY;
        break;

    case VK_ADD:
        key = Key::KeyADD;
        break;

    case VK_SUBTRACT:
        key = Key::KeySUBTRACT;
        break;

    case VK_DECIMAL:
        key = Key::KeyDECIMAL;
        break;

    case VK_DIVIDE:
        key = Key::KeyDIVIDE;
        break;

    case VK_F1:
        key = Key::KeyF1;
        break;

    case VK_F2:
        key = Key::KeyF2;
        break;

    case VK_F3:
        key = Key::KeyF3;
        break;

    case VK_F4:
        key = Key::KeyF4;
        break;

    case VK_F5:
        key = Key::KeyF5;
        break;

    case VK_F6:
        key = Key::KeyF6;
        break;

    case VK_F7:
        key = Key::KeyF7;
        break;

    case VK_F8:
        key = Key::KeyF8;
        break;

    case VK_F9:
        key = Key::KeyF9;
        break;

    case VK_F10:
        key = Key::KeyF10;
        break;

    case VK_F11:
        key = Key::KeyF11;
        break;

    case VK_F12:
        key = Key::KeyF12;
        break;

    case VK_SHIFT:
    case VK_LSHIFT:
        key = Key::KeySHIFT;
        break;

    case VK_RSHIFT:
        key = Key::KeySHIFT;
        break;

    case VK_CONTROL:
    case VK_LCONTROL:
        key = Key::KeyCTRL;
        break;

    case VK_RCONTROL:
        key = Key::KeyCTRL;
        break;

    case VK_MENU:
    case VK_LMENU:
        key = Key::KeyALT;
        break;

    case VK_RMENU:
        key = Key::KeyALT;
        break;
    default:
        key = Key::KeyUNKNOWN;
        break;
    }

    return key;
}

static Key KeyFromChar(int32_t uc) {
    if (uc >= (int)'a' && uc <= (int)'z')
        return (Key)(KeyA + ((char)uc - 'a'));

    if (uc >= (int)'A' && uc <= (int)'Z')
        return (Key)(KeyA + ((char)uc - 'A'));

    if (uc >= (int)'0' && uc <= (int)'9')
        return (Key)(KeyDIGIT0 + ((char)uc - '0'));

    if (uc == 0x20)
        return KeySPACE;

    return KeyUNKNOWN;
}
