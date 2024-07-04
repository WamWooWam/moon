/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#pragma once
#include "pal.h"

namespace Moonlight {
    class MoonInstallerServiceSDL2 : public MoonInstallerService {
        char *base_install_dir;

    protected:
        virtual const char *GetBaseInstallDir();

    public:
        MoonInstallerServiceSDL2();
        virtual ~MoonInstallerServiceSDL2();

        virtual bool Install(Deployment *deployment, bool unattended);
        virtual bool Uninstall(Deployment *deployment);
    };
};
