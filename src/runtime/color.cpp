/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * color.cpp: Colors
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#include <string.h>
#include <stdlib.h>
#include <math.h>

#include "color.h"
#include "utils.h"

namespace Moonlight {

    // match System.Windows.Media.Colors properties
    typedef struct {
        const char *name;
        const unsigned int color;
    } named_colors_t;

    static const named_colors_t named_colors[] = {
        // NOTE: samples shows that XAML supports more than the colors defined in System.Windows.Media.Colors
        // in fact tests shows that all System.Drawing.Color seems to be available

        // NOTE: make sure to keep this list SORTED, as we use a binary search to find colors.
        { "aliceblue", 0xFFF7FBFF },
        { "antiquewhite", 0xFFFAEBD7 },
        { "aqua", 0xFF00FFFF },
        { "aquamarine", 0xFF7FFFD4 },
        { "azure", 0xFFF7FFFF },
        { "beige", 0xFFF5F5DC },
        { "bisque", 0xFFFFE4C4 },
        { "black", 0xFF000000 },
        { "blanchedalmond", 0xFFFFEBCD },
        { "blue", 0xFF0000FF },
        { "blueviolet", 0xFF8A2BE2 },
        { "brown", 0xFFA52A2A },
        { "burlywood", 0xFFDEB887 },
        { "cadetblue", 0xFF5F9EA0 },
        { "chartreuse", 0xFF7FFF00 },
        { "chocolate", 0xFFD2691E },
        { "coral", 0xFFFF7F50 },
        { "cornflowerblue", 0xFF6495ED },
        { "cornsilk", 0xFFFFF8DC },
        { "crimson", 0xFFDC143C },
        { "cyan", 0xFF00FFFF },
        { "darkblue", 0xFF00008B },
        { "darkcyan", 0xFF008B8B },
        { "darkgoldenrod", 0xFFB8860B },
        { "darkgray", 0xFFA9A9A9 },
        { "darkgreen", 0xFF006400 },
        { "darkkhaki", 0xFFBDB76B },
        { "darkmagenta", 0xFF8B008B },
        { "darkolivegreen", 0xFF556B2F },
        { "darkorange", 0xFFFF8C00 },
        { "darkorchid", 0xFF9932CC },
        { "darkred", 0xFF8B0000 },
        { "darksalmon", 0xFFE9967A },
        { "darkseagreen", 0xFF8FBC8B },
        { "darkslateblue", 0xFF483D8B },
        { "darkslategray", 0xFF2F4F4F },
        { "darkturquoise", 0xFF00CED1 },
        { "darkviolet", 0xFF9400D3 },
        { "deeppink", 0xFFFF1493 },
        { "deepskyblue", 0xFF00BFFF },
        { "dimgray", 0xFF696969 },
        { "dodgerblue", 0xFF1E90FF },
        { "firebrick", 0xFFB22222 },
        { "floralwhite", 0xFFFFFBF7 },
        { "forestgreen", 0xFF228B22 },
        { "fuchsia", 0xFFFF00FF },
        { "gainsboro", 0xFFDCDCDC },
        { "ghostwhite", 0xFFF8F8FF },
        { "gold", 0xFFFFD700 },
        { "goldenrod", 0xFFDAA520 },
        { "gray", 0xFF808080 },
        { "green", 0xFF008000 },
        { "greenyellow", 0xFFADFF2F },
        { "honeydew", 0xFFF0FFF0 },
        { "hotpink", 0xFFFF69B4 },
        { "indianred", 0xFFCD5C5C },
        { "indigo", 0xFF4B0082 },
        { "ivory", 0xFFFFFFF7 },
        { "khaki", 0xFFF0E68C },
        { "lavender", 0xFFE6E6FA },
        { "lavenderblush", 0xFFFFF0F5 },
        { "lawngreen", 0xFF7CFC00 },
        { "lemonchiffon", 0xFFFFFACD },
        { "lightblue", 0xFFADD8E6 },
        { "lightcoral", 0xFFF08080 },
        { "lightcyan", 0xFFE0FFFF },
        { "lightgoldenrodyellow", 0xFFFAFAD2 },
        { "lightgray", 0xFFD3D3D3 },
        { "lightgreen", 0xFF90EE90 },
        { "lightpink", 0xFFFFB6C1 },
        { "lightsalmon", 0xFFFFA07A },
        { "lightseagreen", 0xFF20B2AA },
        { "lightskyblue", 0xFF87CEFA },
        { "lightslategray", 0xFF778899 },
        { "lightsteelblue", 0xFFB0C4DE },
        { "lightyellow", 0xFFFFFFE0 },
        { "lime", 0xFF00FF00 },
        { "limegreen", 0xFF32CD32 },
        { "linen", 0xFFFAF0E6 },
        { "magenta", 0xFFFF00FF },
        { "maroon", 0xFF800000 },
        { "mediumaquamarine", 0xFF66CDAA },
        { "mediumblue", 0xFF0000CD },
        { "mediumorchid", 0xFFBA55D3 },
        { "mediumpurple", 0xFF9370DB },
        { "mediumseagreen", 0xFF3CB371 },
        { "mediumslateblue", 0xFF7B68EE },
        { "mediumspringgreen", 0xFF00FA9A },
        { "mediumturquoise", 0xFF48D1CC },
        { "mediumvioletred", 0xFFC71585 },
        { "midnightblue", 0xFF191970 },
        { "mintcream", 0xFFF7FFFF },
        { "mistyrose", 0xFFFFE4E1 },
        { "moccasin", 0xFFFFE4B5 },
        { "navajowhite", 0xFFFFDEAD },
        { "navy", 0xFF000080 },
        { "oldlace", 0xFFFDF5E6 },
        { "olive", 0xFF808000 },
        { "olivedrab", 0xFF6B8E23 },
        { "orange", 0xFFFFA500 },
        { "orangered", 0xFFFF4500 },
        { "orchid", 0xFFDA70D6 },
        { "palegoldenrod", 0xFFEEE8AA },
        { "palegreen", 0xFF98FB98 },
        { "paleturquoise", 0xFFAFEEEE },
        { "palevioletred", 0xFFDB7093 },
        { "papayawhip", 0xFFFFEFD5 },
        { "peachpuff", 0xFFFFDAB9 },
        { "peru", 0xFFCD853F },
        { "pink", 0xFFFFC0CB },
        { "plum", 0xFFDDA0DD },
        { "powderblue", 0xFFB0E0E6 },
        { "purple", 0xFF800080 },
        { "red", 0xFFFF0000 },
        { "rosybrown", 0xFFBC8F8F },
        { "royalblue", 0xFF4169E1 },
        { "saddlebrown", 0xFF8B4513 },
        { "salmon", 0xFFFA8072 },
        { "sandybrown", 0xFFF4A460 },
        { "seagreen", 0xFF2E8B57 },
        { "seashell", 0xFFFFF5EE },
        { "sienna", 0xFFA0522D },
        { "silver", 0xFFC0C0C0 },
        { "skyblue", 0xFF87CEEB },
        { "slateblue", 0xFF6A5ACD },
        { "slategray", 0xFF708090 },
        { "snow", 0xFFFFFAFA },
        { "springgreen", 0xFF00FF7F },
        { "steelblue", 0xFF4682B4 },
        { "tan", 0xFFD2B48C },
        { "teal", 0xFF008080 },
        { "thistle", 0xFFD8BFD8 },
        { "tomato", 0xFFFF6347 },
        { "transparent", 0x00FFFFFF },
        { "turquoise", 0xFF40E0D0 },
        { "violet", 0xFFEE82EE },
        { "wheat", 0xFFF5DEB3 },
        { "white", 0xFFFFFFFF },
        { "whitesmoke", 0xFFF5F5F5 },
        { "yellow", 0xFFFFFF00 },
        { "yellowgreen", 0xFF9ACD32 },
    };

    static const int num_named_colors = sizeof(named_colors) / sizeof(named_colors[0]);

    static int
    compare_color_names(const void *v1, const void *v2) {
        named_colors_t *c1 = (named_colors_t *)v1;
        named_colors_t *c2 = (named_colors_t *)v2;

        return g_ascii_strcasecmp(c1->name, c2->name);
    }

    /**
     * see: http://msdn2.microsoft.com/en-us/library/system.windows.media.solidcolorbrush.aspx
     *
     * If no color is found, NULL is returned.
     */
    Color *
    Color::FromStr(const char *name) {
        size_t len;

        if (!name)
            return new Color(0x00FFFFFF);

        if ((len = strlen(name)) == 0)
            return new Color(0x00000000);

        if (name[0] == '#') {
            char a[3] = "FF";
            char r[3] = "FF";
            char g[3] = "FF";
            char b[3] = "FF";

            // Relaxed parsing with some it's-the-web-and-it's-broken
            // "error tolerance"
            int real_len = len - 1;
            if (real_len >= 8) {
                // aarrggbb
                a[0] = name[1];
                a[1] = name[2];
                r[0] = name[3];
                r[1] = name[4];
                g[0] = name[5];
                g[1] = name[6];
                b[0] = name[7];
                b[1] = name[8];
            }
            else if (real_len >= 6) {
                // rrggbb
                r[0] = name[1];
                r[1] = name[2];
                g[0] = name[3];
                g[1] = name[4];
                b[0] = name[5];
                b[1] = name[6];
            }
            else if (real_len >= 4) {
                // argb
                a[1] = a[0] = name[1];
                r[1] = r[0] = name[2];
                g[1] = g[0] = name[3];
                b[1] = b[0] = name[4];
            }
            else if (real_len == 3) {
                // rgb
                r[1] = r[0] = name[1];
                g[1] = g[0] = name[2];
                b[1] = b[0] = name[3];
            }

            return new Color(strtol(r, NULL, 16) / 255.0F, strtol(g, NULL, 16) / 255.0F,
                strtol(b, NULL, 16) / 255.0F, strtol(a, NULL, 16) / 255.0F);
        }

        if (name[0] == 's' && name[1] == 'c' && name[2] == '#') {
            char *iter = (char *)name + 3;
            double a = 1.0, r = 1.0, g = 1.0, b = 1.0;
            GArray *channels = double_garray_from_str(iter, 0);

            if (channels != NULL) {
                int i = 0;
                if (channels->len >= 4) {
                    a = g_array_index(channels, double, 0);
                    i++;
                }

                if (channels->len >= 3) {
                    r = g_array_index(channels, double, 0 + i);
                    g = g_array_index(channels, double, 1 + i);
                    b = g_array_index(channels, double, 2 + i);
                }

                g_array_free(channels, TRUE);
            }

            /* Clamp values. scRGB theoretically supports negative numbers
             * ('darker than black') as a refference to another source but here
             * it doesn't make too much sense. */
            r = MIN(1.0, r);
            r = MAX(0.0, r);
            g = MIN(1.0, g);
            g = MAX(0.0, g);
            b = MIN(1.0, b);
            b = MAX(0.0, b);
            a = MIN(1.0, a);
            a = MAX(0.0, a);

            /* NOTE: This is not a fully accurate scRGB -> sRGB conversion.
             * The real stuff has slightly different/more complex band shaping.
             * But I'd call it "good enough" for now. */
            r = powf(r, 0.4545);
            g = powf(g, 0.46);
            b = powf(b, 0.4545);

            return new Color(r, g, b, a);
        }

        if (name[0] >= '0' && name[0] <= '9') {
            guint32 color = strtoul(name, NULL, 10);

            return new Color(color);
        }

        named_colors_t key = { name, 0 };

        named_colors_t *result = (named_colors_t *)bsearch(&key, named_colors, num_named_colors, sizeof(named_colors_t),
            compare_color_names);

        if (result)
            return new Color(result->color);

        return NULL;
    }

    const char *
    Color::ToString(Color *color) {
        static char hex[16] = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        static char buf[10];
        guint8 v;

        buf[0] = '#';


        v = (guint8)(color->a * 255);
        if (v > 0) {
            buf[1] = hex[(v >> 4) & 0x0f];
            buf[2] = hex[v & 0x0f];
        }
        else {
            buf[1] = '\0';
            buf[2] = '\0';
        }

        v = (guint8)(color->r * 255);
        buf[3] = hex[(v >> 4) & 0x0f];
        buf[4] = hex[v & 0x0f];

        v = (guint8)(color->g * 255);
        buf[5] = hex[(v >> 4) & 0x0f];
        buf[6] = hex[v & 0x0f];

        v = (guint8)(color->b * 255);
        buf[7] = hex[(v >> 4) & 0x0f];
        buf[8] = hex[v & 0x0f];

        buf[9] = '\0';

        return buf;
    }

#define RGB_MAX 255
#define HSL_MAX 255

    static void
    rgb_to_hsl(int red, int green, int blue, int *hue, int *saturation, int *luminance) {
        int rDelta, gDelta, bDelta;
        int cMax, cMin;

        // r = (int) (color->r * 255.0);
        // g = (int) (color->g * 255.0);
        // b = (int) (color->b * 255.0);

        cMax = MAX(MAX(red, green), blue);
        cMin = MIN(MIN(red, green), blue);

        // calculate luminance
        *luminance = (((cMax + cMin) * HSL_MAX) + RGB_MAX) / (2 * RGB_MAX);

        if (cMax == cMin) {
            // color is achromatic
            *saturation = 0;
            *hue = 0;
            return;
        }

        // calculate saturation
        if (*luminance <= (HSL_MAX / 2)) {
            *saturation = (((cMax - cMin) * HSL_MAX) + ((cMax + cMin) / 2)) / (cMax + cMin);
        }
        else {
            *saturation = (((cMax - cMin) * HSL_MAX) + ((2 * RGB_MAX - cMax - cMin) / 2)) / (2 * RGB_MAX - cMax - cMin);
        }

        // calculate hue
        rDelta = (((cMax - red) * (HSL_MAX / 6)) + ((cMax - cMin) / 2)) / (cMax - cMin);
        gDelta = (((cMax - green) * (HSL_MAX / 6)) + ((cMax - cMin) / 2)) / (cMax - cMin);
        bDelta = (((cMax - blue) * (HSL_MAX / 6)) + ((cMax - cMin) / 2)) / (cMax - cMin);

        if (red == cMax) {
            *hue = bDelta - gDelta;
        }
        else if (green == cMax) {
            *hue = (HSL_MAX / 3) + rDelta - bDelta;
        }
        else { /* b == cMax */
            *hue = ((2 * HSL_MAX) / 3) + gDelta - rDelta;
        }

        if (*hue < 0)
            *hue += HSL_MAX;
        else if (*hue > HSL_MAX)
            *hue -= HSL_MAX;
    }

    static int
    hue_to_rgb(int n1, int n2, int hue) {
        if (hue < 0)
            hue += HSL_MAX;
        else if (hue > HSL_MAX)
            hue -= HSL_MAX;

        /* return r, g, or b value from this tridrant */
        if (hue < (HSL_MAX / 6))
            return (n1 + (((n2 - n1) * hue + (HSL_MAX / 12)) / (HSL_MAX / 6)));

        if (hue < (HSL_MAX / 2))
            return n2;

        if (hue < ((HSL_MAX * 2) / 3))
            return (n1 + (((n2 - n1) * (((HSL_MAX * 2) / 3) - hue) + (HSL_MAX / 12)) / (HSL_MAX / 6)));

        return n1;
    }

    static void
    hsl_to_rgb(int hue, int saturation, int luminance, int *red, int *green, int *blue) {
        int magic1, magic2;

        if (saturation == 0) {
            // color is achromatic
            *red = *green = *blue = (luminance * RGB_MAX) / HSL_MAX;
        }
        else {
            if (luminance <= (HSL_MAX / 2))
                magic2 = (luminance * (HSL_MAX + saturation) + (HSL_MAX / 2)) / HSL_MAX;
            else
                magic2 = saturation + luminance - ((saturation * luminance) + (HSL_MAX / 2)) / HSL_MAX;

            magic1 = (luminance * 2) - magic2;

            *red = MIN(255, (hue_to_rgb(magic1, magic2, hue + (HSL_MAX / 3)) * RGB_MAX + (HSL_MAX / 2)) / HSL_MAX);
            *green = MIN(255, (hue_to_rgb(magic1, magic2, hue) * RGB_MAX + (HSL_MAX / 2)) / HSL_MAX);
            *blue = MIN(255, (hue_to_rgb(magic1, magic2, hue - (HSL_MAX / 3)) * RGB_MAX + (HSL_MAX / 2)) / HSL_MAX);
        }
    }


    Color::Color(int hue, int saturation, int luminance) {
        int red, green, blue;

        hsl_to_rgb(hue, saturation, luminance, &red, &green, &blue);

        r = red / 255.0;
        g = green / 255.0;
        b = blue / 255.0;
        a = 1.0;
    }

    void
    Color::Lighten() {
        int red = (int)(r * 255.0);
        int green = (int)(g * 255.0);
        int blue = (int)(b * 255.0);
        int hue, sat, lum;

        rgb_to_hsl(red, green, blue, &hue, &sat, &lum);

        // lighten the color
        lum = MIN(255, lum + ((255 - lum) / 4));

        hsl_to_rgb(hue, sat, lum, &red, &green, &blue);

        r = red / 255.0;
        g = green / 255.0;
        b = blue / 255.0;
    }

    void
    Color::Darken() {
        int red = (int)(r * 255.0);
        int green = (int)(g * 255.0);
        int blue = (int)(b * 255.0);
        int hue, sat, lum;

        rgb_to_hsl(red, green, blue, &hue, &sat, &lum);

        // darken the color
        lum = MAX(0, lum - (int)(lum * 0.333f));
        lum = MAX(0, lum - (lum / 2));

        hsl_to_rgb(hue, sat, lum, &red, &green, &blue);

        r = red / 255.0;
        g = green / 255.0;
        b = blue / 255.0;
    }

};
