/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * size.h
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#ifndef __MOON_SIZE_H__
#define __MOON_SIZE_H__

#include <glib.h>
#include "pal.h"
#include "thickness.h"

namespace Moonlight {

    /* @IncludeInKinds */
    struct Size {
        double width, height;

        Size() : width(0), height(0) {}

        Size(int zero) : width(0), height(0) {}

        Size(double w, double h) {
            this->width = w;
            this->height = h;
        }

        bool IsEmpty() const {
            return (isinf(width) && isinf(height));
        }

        Size Max(double w, double h) const {
            return Size(width < w ? w : width, height < h ? h : height);
        }

        Size Max(const Size &s) const {
            return Max(s.width, s.height);
        }

        Size Min(double w, double h) const {
            return Size(width > w ? w : width, height > h ? h : height);
        }

        Size Min(const Size &s) const {
            return Min(s.width, s.height);
        }

        Size GrowBy(const double w, const double h) const {
            const double hh = isinf(height) ? height : height + h;
            const double ww = isinf(width) ? width : width + w;

            return Size(ww > 0 ? ww : 0, hh > 0 ? hh : 0);
        }

        Size GrowBy(const Thickness &t) {
            return GrowBy(t.left + t.right, t.top + t.bottom);
        }

        bool operator==(const Size &size) const {
            return fabs(size.width - width) < DBL_EPSILON && fabs(size.height - height) < DBL_EPSILON;
        }

        bool operator!=(const Size &size) const {
            return !(*this == size);
        }

        //
        // FromStr
        //   Parses @s and return a new size in @size. Returns true if
        //   this was successful, false otherwise.
        //
        static bool FromStr(const char *s, Size *size);
    };

};
#endif /* __MOON_POINT_H__ */
