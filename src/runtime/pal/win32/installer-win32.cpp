
#include <config.h>

#include "pal.h"
#include "installer-win32.h"

namespace Moonlight {
    MoonInstallerServiceWin32::MoonInstallerServiceWin32() {
    }

    MoonInstallerServiceWin32::~MoonInstallerServiceWin32() {
    }

    const char *MoonInstallerServiceWin32::GetBaseInstallDir() {
        g_assert_not_reached();
    }

    bool MoonInstallerServiceWin32::Install(Deployment *deployment, bool unattended) {
        g_assert_not_reached();
    }

    bool MoonInstallerServiceWin32::Uninstall(Deployment *deployment) {
        g_assert_not_reached();
    }
}