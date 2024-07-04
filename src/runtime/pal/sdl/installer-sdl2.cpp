
#include <config.h>

#include "pal.h"
#include "installer-sdl2.h"

namespace Moonlight {
    MoonInstallerServiceSDL2::MoonInstallerServiceSDL2() {
    }

    MoonInstallerServiceSDL2::~MoonInstallerServiceSDL2() {
    }

    const char *MoonInstallerServiceSDL2::GetBaseInstallDir() {
        g_assert_not_reached();
    }

    bool MoonInstallerServiceSDL2::Install(Deployment *deployment, bool unattended) {
        g_assert_not_reached();
    }

    bool MoonInstallerServiceSDL2::Uninstall(Deployment *deployment) {
        g_assert_not_reached();
    }
}