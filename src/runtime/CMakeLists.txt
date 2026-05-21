# src/runtime/CMakeLists.txt

# ---- Dependency discovery ----

find_package(PkgConfig QUIET)

set(_deps "${CMAKE_SOURCE_DIR}/deps")

# Helper: abort with a clear message when pkg-config is absent on non-Windows.
macro(require_pkgconfig _feature)
    if(NOT PKG_CONFIG_FOUND)
        message(FATAL_ERROR
            "pkg-config is required to locate '${_feature}' on this platform. "
            "Install pkg-config and re-run cmake.")
    endif()
endmacro()

# ------------------------------------------------------------------
# GLib / eglib
# On Windows we use the in-tree eglib (Mono's minimal glib subset).
# Everywhere else we use the system glib-2.0.
# ------------------------------------------------------------------
if(WIN32)
    find_path(EGLIB_INCLUDE_DIR glib.h REQUIRED
        HINTS "${_deps}/eglib/eglib/src")
    find_library(EGLIB_LIBRARY NAMES eglib REQUIRED
        HINTS "${_deps}/eglib/${MOON_ARCH}/Release")
    add_library(_moon_glib UNKNOWN IMPORTED)
    set_target_properties(_moon_glib PROPERTIES
        IMPORTED_LOCATION             "${EGLIB_LIBRARY}"
        INTERFACE_INCLUDE_DIRECTORIES "${EGLIB_INCLUDE_DIR}")
else()
    require_pkgconfig(glib-2.0)
    pkg_check_modules(GLIB REQUIRED IMPORTED_TARGET glib-2.0)
    add_library(_moon_glib ALIAS PkgConfig::GLIB)
endif()

# ------------------------------------------------------------------
# Cairo
# ------------------------------------------------------------------
if(WIN32)
    find_path(CAIRO_INCLUDE_DIR cairo.h REQUIRED
        HINTS "${_deps}/cairo/include")
    find_library(CAIRO_LIBRARY NAMES cairo REQUIRED
        HINTS "${_deps}/cairo/lib/${MOON_ARCH}")
    add_library(_moon_cairo UNKNOWN IMPORTED)
    set_target_properties(_moon_cairo PROPERTIES
        IMPORTED_LOCATION             "${CAIRO_LIBRARY}"
        INTERFACE_INCLUDE_DIRECTORIES "${CAIRO_INCLUDE_DIR}")
else()
    require_pkgconfig(cairo)
    pkg_check_modules(CAIRO REQUIRED IMPORTED_TARGET cairo)
    add_library(_moon_cairo ALIAS PkgConfig::CAIRO)
endif()

# ------------------------------------------------------------------
# FreeType
# ------------------------------------------------------------------
if(WIN32)
    find_path(FREETYPE_INCLUDE_DIR ft2build.h REQUIRED
        HINTS "${_deps}/freetype/include")
    find_library(FREETYPE_LIBRARY NAMES freetype REQUIRED
        HINTS "${_deps}/freetype/release/${MOON_ARCH}")
    add_library(_moon_freetype UNKNOWN IMPORTED)
    set_target_properties(_moon_freetype PROPERTIES
        IMPORTED_LOCATION             "${FREETYPE_LIBRARY}"
        INTERFACE_INCLUDE_DIRECTORIES "${FREETYPE_INCLUDE_DIR}")
else()
    find_package(Freetype REQUIRED)
    add_library(_moon_freetype ALIAS Freetype::Freetype)
endif()

# ------------------------------------------------------------------
# Expat
# ------------------------------------------------------------------
if(WIN32)
    find_path(EXPAT_INCLUDE_DIR expat.h REQUIRED
        HINTS "${_deps}/expat/include")
    find_library(EXPAT_LIBRARY NAMES libexpat expat REQUIRED
        HINTS "${_deps}/expat/lib/${MOON_ARCH}/Release")
    add_library(_moon_expat UNKNOWN IMPORTED)
    set_target_properties(_moon_expat PROPERTIES
        IMPORTED_LOCATION             "${EXPAT_LIBRARY}"
        INTERFACE_INCLUDE_DIRECTORIES "${EXPAT_INCLUDE_DIR}")
else()
    find_package(EXPAT REQUIRED)
    add_library(_moon_expat ALIAS EXPAT::EXPAT)
endif()

# ------------------------------------------------------------------
# zlib
# ------------------------------------------------------------------
if(WIN32)
    find_path(ZLIB_INCLUDE_DIR zlib.h REQUIRED
        HINTS "${_deps}/zlib/include")
    find_library(ZLIB_LIBRARY NAMES zlib zlibstatic REQUIRED
        HINTS "${_deps}/zlib/binaries/${MOON_ARCH}/Release")
    add_library(_moon_zlib UNKNOWN IMPORTED)
    set_target_properties(_moon_zlib PROPERTIES
        IMPORTED_LOCATION             "${ZLIB_LIBRARY}"
        INTERFACE_INCLUDE_DIRECTORIES "${ZLIB_INCLUDE_DIR}")
else()
    find_package(ZLIB REQUIRED)
    add_library(_moon_zlib ALIAS ZLIB::ZLIB)
endif()

# ------------------------------------------------------------------
# OpenGL
# ------------------------------------------------------------------
find_package(OpenGL REQUIRED)

# ------------------------------------------------------------------
# libcurl  (optional)
# ------------------------------------------------------------------
if(MOON_ENABLE_CURL)
    if(WIN32)
        # Prebuilt static curl in the convention used by the deps tree.
        set(_curl_triplet
            "libcurl-vc-${MOON_ARCH}-release-static-ipv6-sspi-schannel")
        find_path(CURL_INCLUDE_DIR curl/curl.h REQUIRED
            HINTS "${_deps}/curl/builds/${_curl_triplet}/include")
        find_library(CURL_LIBRARY NAMES libcurl_a REQUIRED
            HINTS "${_deps}/curl/builds/${_curl_triplet}/lib")
        add_library(_moon_curl UNKNOWN IMPORTED)
        set_target_properties(_moon_curl PROPERTIES
            IMPORTED_LOCATION             "${CURL_LIBRARY}"
            INTERFACE_INCLUDE_DIRECTORIES "${CURL_INCLUDE_DIR}"
            # Static curl on Windows needs these system libs.
            INTERFACE_LINK_LIBRARIES      "ws2_32;crypt32;wldap32;normaliz")
    else()
        find_package(CURL REQUIRED)
        add_library(_moon_curl ALIAS CURL::libcurl)
    endif()
endif()

# ------------------------------------------------------------------
# GLEW  (win32 and sdl2 PALs)
# ------------------------------------------------------------------
if(MOON_PAL STREQUAL "win32" OR MOON_PAL STREQUAL "sdl2")
    if(WIN32)
        find_path(GLEW_INCLUDE_DIR GL/glew.h REQUIRED
            HINTS "${_deps}/glew/include")
        find_library(GLEW_LIBRARY NAMES glew32 glew32s GLEW REQUIRED
            HINTS "${_deps}/glew/lib/Release/${MOON_ARCH}")
        add_library(_moon_glew UNKNOWN IMPORTED)
        set_target_properties(_moon_glew PROPERTIES
            IMPORTED_LOCATION             "${GLEW_LIBRARY}"
            INTERFACE_INCLUDE_DIRECTORIES "${GLEW_INCLUDE_DIR}")
    else()
        find_package(GLEW REQUIRED)
        add_library(_moon_glew ALIAS GLEW::GLEW)
    endif()
    set(_moon_use_glew ON)
else()
    set(_moon_use_glew OFF)
endif()

# ------------------------------------------------------------------
# SDL2  (sdl2 PAL only)
# ------------------------------------------------------------------
if(MOON_PAL STREQUAL "sdl2")
    if(WIN32)
        find_path(SDL2_INCLUDE_DIR SDL.h REQUIRED
            HINTS "${_deps}/sdl2/include")
        find_library(SDL2_LIBRARY NAMES SDL2 REQUIRED
            HINTS "${_deps}/sdl2/lib/${MOON_ARCH}")
        add_library(_moon_sdl2 UNKNOWN IMPORTED)
        set_target_properties(_moon_sdl2 PROPERTIES
            IMPORTED_LOCATION             "${SDL2_LIBRARY}"
            INTERFACE_INCLUDE_DIRECTORIES "${SDL2_INCLUDE_DIR}")
    else()
        # Prefer the official CMake config shipped with SDL2 2.x;
        # fall back to pkg-config for older distro packages.
        find_package(SDL2 QUIET CONFIG)
        if(TARGET SDL2::SDL2)
            add_library(_moon_sdl2 ALIAS SDL2::SDL2)
        else()
            require_pkgconfig(sdl2)
            pkg_check_modules(SDL2 REQUIRED IMPORTED_TARGET sdl2)
            add_library(_moon_sdl2 ALIAS PkgConfig::SDL2)
        endif()
    endif()
endif()

# ------------------------------------------------------------------
# SDL2_image  (sdl2 PAL only)
# ------------------------------------------------------------------
if(MOON_PAL STREQUAL "sdl2")
    if(WIN32)
        find_path(SDL2_IMAGE_INCLUDE_DIR SDL_image.h REQUIRED
            HINTS "${_deps}/sdl2_image/include")
        find_library(SDL2_IMAGE_LIBRARY NAMES SDL2_image REQUIRED
            HINTS "${_deps}/sdl2_image/lib/${MOON_ARCH}")
        add_library(_moon_sdl2_image UNKNOWN IMPORTED)
        set_target_properties(_moon_sdl2_image PROPERTIES
            IMPORTED_LOCATION             "${SDL2_IMAGE_LIBRARY}"
            INTERFACE_INCLUDE_DIRECTORIES "${SDL2_IMAGE_INCLUDE_DIR}")
    else()
        find_package(SDL2_image QUIET CONFIG)
        if(TARGET SDL2_image::SDL2_image)
            add_library(_moon_sdl2_image ALIAS SDL2_image::SDL2_image)
        else()
            require_pkgconfig(SDL2_image)
            pkg_check_modules(SDL2_IMAGE REQUIRED IMPORTED_TARGET SDL2_image)
            add_library(_moon_sdl2_image ALIAS PkgConfig::SDL2_IMAGE)
        endif()
    endif()
endif()

# ------------------------------------------------------------------
# GTK3 + fontconfig  (gtk PAL; also fontconfig for SDL2 on non-Windows)
# ------------------------------------------------------------------
if(MOON_PAL STREQUAL "gtk")
    require_pkgconfig("gtk+-2.0 / fontconfig")
    pkg_check_modules(GTK2       REQUIRED IMPORTED_TARGET gtk+-2.0)
    pkg_check_modules(FONTCONFIG REQUIRED IMPORTED_TARGET fontconfig)
elseif(MOON_PAL STREQUAL "sdl2" AND NOT WIN32)
    require_pkgconfig(fontconfig)
    pkg_check_modules(FONTCONFIG REQUIRED IMPORTED_TARGET fontconfig)
endif()

# ------------------------------------------------------------------
# Audio backends
# ------------------------------------------------------------------
if(MOON_AUDIO_ALSA)
    find_package(ALSA QUIET)
    if(TARGET ALSA::ALSA)
        add_library(_moon_alsa ALIAS ALSA::ALSA)
    else()
        require_pkgconfig(alsa)
        pkg_check_modules(ALSA REQUIRED IMPORTED_TARGET alsa)
        add_library(_moon_alsa ALIAS PkgConfig::ALSA)
    endif()
endif()

if(MOON_AUDIO_PULSE)
    require_pkgconfig(libpulse)
    pkg_check_modules(PULSE REQUIRED IMPORTED_TARGET libpulse)
endif()

# ---- Source files --------------------------------------------------------

# Core runtime — platform-independent.
set(Source_Files_Core
    "animation.cpp"
    "application.cpp"
    "applier.cpp"
    "asxparser.cpp"
    "audio.cpp"
    "bitmapcache.cpp"
    "bitmapimage.cpp"
    "bitmapsource.cpp"
    "border.cpp"
    "brush.cpp"
    "canvas.cpp"
    "capture.cpp"
    "cbinding.cpp"
    "clock.cpp"
    "collection.cpp"
    "color.cpp"
    "consent.cpp"
    "contentcontrol.cpp"
    "contentpresenter.cpp"
    "context-cairo.cpp"
    "context-gl.cpp"
    "context.cpp"
    "control.cpp"
    "cornerradius.cpp"
    "cpu.cpp"
    "debug.cpp"
    "deepzoomimagetilesource.cpp"
    "dependencyobject.cpp"
    "dependencyproperty.cpp"
    "dependencyproperty.g.cpp"
    "deployment.cpp"
    "dirty.cpp"
    "downloader.cpp"
    "easing.cpp"
    "effect.cpp"
    "enums.cpp"
    "error.cpp"
    "eventargs.cpp"
    "factory.cpp"
    "font-utils.cpp"
    "fontmanager.cpp"
    "fonts.cpp"
    "frameworkelement.cpp"
    "gchandle.cpp"
    "geometry.cpp"
    "gkeyfile.c"
    "glyphs.cpp"
    "grid.cpp"
    "http-streaming.cpp"
    "imagesource.cpp"
    "keyboard.cpp"
    "layoutinformation.cpp"
    "list.cpp"
    "managedtypeinfo.cpp"
    "media.cpp"
    "mediaelement.cpp"
    "medialog.cpp"
    "mediaplayer.cpp"
    "messaging.cpp"
    "moon-curves.cpp"
    "moon-path.cpp"
    "moonlightconfiguration.cpp"
    "mp3.cpp"
    "multiscaleimage.cpp"
    "multiscalesubimage.cpp"
    "namescope.cpp"
    "network.cpp"
    "notificationwindow.cpp"
    "openfile.cpp"
    "panel.cpp"
    "pipeline-asf.cpp"
    "pipeline-logo.cpp"
    "pipeline-mp4.cpp"
    "pipeline.cpp"
    "playlist.cpp"
    "point.cpp"
    "popup.cpp"
    "printing.cpp"
    "projection.cpp"
    "provider.cpp"
    "rect.cpp"
    "region.cpp"
    "resources.cpp"
    "richtextbox.cpp"
    "richtextlayout.cpp"
    "runtime.cpp"
    "security.cpp"
    "shape.cpp"
    "size.cpp"
    "style.cpp"
    "stylus.cpp"
    "surface-cairo.cpp"
    "surface-gl.cpp"
    "surface.cpp"
    "tabnavigationwalker.cpp"
    "template.cpp"
    "textblock.cpp"
    "textbox.cpp"
    "textelement.cpp"
    "textlayout.cpp"
    "textpointer.cpp"
    "textselection.cpp"
    "thickness.cpp"
    "tilesource.cpp"
    "timeline.cpp"
    "timemanager.cpp"
    "timesource.cpp"
    "transform.cpp"
    "trigger.cpp"
    "type-generated.cpp"
    "type.cpp"
    "uielement.cpp"
    "uri.cpp"
    "usercontrol.cpp"
    "utils.cpp"
    "validators.cpp"
    "value.cpp"
    "weakrefmanager.cpp"
    "webbrowser.cpp"
    "writeablebitmap.cpp"
    "xaml.cpp"
    "xap.cpp"
    "yuv-converter.cpp"
    # PAL base (not backend-specific)
    "pal/pal.cpp"
    "pal/window.cpp"
    # Shared OpenGL base (used by WGL, SDL-GL, EGL — not Cairo-only builds)
    "context-opengl.cpp"
    "surface-opengl.cpp"
)

# libcurl networking — only when curl is enabled.
if(MOON_ENABLE_CURL)
    list(APPEND Source_Files_Core "network-curl.cpp")
endif()

# WGL (Windows OpenGL) — win32 PAL only.
if(MOON_PAL STREQUAL "win32")
    list(APPEND Source_Files_Core
        "context-wgl.cpp"
        "surface-wgl.cpp"
    )
endif()

# FFmpeg pipeline — opt-in.
if(MOON_ENABLE_FFMPEG)
    list(APPEND Source_Files_Core "pipeline-ffmpeg.cpp")
endif()

# Audio backends — each opt-in independently.
set(Source_Files_Audio)
if(MOON_AUDIO_ALSA)
    list(APPEND Source_Files_Audio "audio-alsa.cpp")
endif()
if(MOON_AUDIO_PULSE)
    list(APPEND Source_Files_Audio "audio-pulse.cpp")
endif()
if(MOON_AUDIO_OPENSLES)
    list(APPEND Source_Files_Audio "audio-opensles.cpp")
endif()

# Bundled zip library.
set(Source_Files_Zip
    "zip/ioapi.c"
    "zip/unzip.c"
)

# PAL backend sources.
if(MOON_PAL STREQUAL "win32")
    set(Source_Files_PAL
        "pal/win32/clipboard-win32.cpp"
        "pal/fonts/dwrite/fonts-dwrite.cpp"
        "pal/win32/helpers/dialogeventhandler.cpp"
        "pal/win32/im-win32.cpp"
        "pal/win32/installer-win32.cpp"
        "pal/win32/pal-win32.cpp"
        "pal/win32/pixbuf-win32.cpp"
        "pal/win32/threads-win32.cpp"
        "pal/win32/window-win32.cpp"
    )
elseif(MOON_PAL STREQUAL "sdl2")
    set(Source_Files_PAL
        "pal/sdl/clipboard-sdl2.cpp"
        "pal/sdl/gl/context-sdl2.cpp"
        "pal/sdl/gl/surface-sdl2.cpp"
        "pal/sdl/im-sdl2.cpp"
        "pal/sdl/installer-sdl2.cpp"
        "pal/sdl/pal-sdl2.cpp"
        "pal/sdl/pixbuf-sdl2.cpp"
        "pal/sdl/threads-sdl2.cpp"
        "pal/sdl/window-sdl2.cpp"
    )
    if(WIN32)
        list(APPEND Source_Files_PAL "pal/fonts/dwrite/fonts-dwrite.cpp")
    else()
        list(APPEND Source_Files_PAL "pal/fonts/fontconfig/fonts-fontconfig.cpp")
    endif()
elseif(MOON_PAL STREQUAL "gtk")
    set(Source_Files_PAL
        # GTK windowing
        "pal/gtk/clipboard-gtk.cpp"
        "pal/gtk/config-dialog-gtk.cpp"
        "pal/gtk/debug-ui-gtk.cpp"
        "pal/gtk/im-gtk.cpp"
        "pal/gtk/install-dialog-gtk.cpp"
        "pal/gtk/nocodec-ui-gtk.cpp"
        "pal/gtk/pal-gtk.cpp"
        "pal/gtk/pixbuf-gtk.cpp"
        "pal/gtk/window-gtk.cpp"
        # windowless-gtk.cpp is the NPAPI browser-plugin path; it depends on
        # plugin.h / moonlight.h that are not part of the standalone runtime.
        # Threading (pthreads)
        "pal/gtk/pal-threads-pthreads.cpp"
        # Font discovery
        "pal/fonts/fontconfig/fonts-fontconfig.cpp"
        # Linux capture (V4L2 video, ALSA/pulse audio capture)
        "pal/gtk/capture/pal-linux-capture.cpp"
        "pal/gtk/capture/pal-linux-audio-capture.cpp"
        "pal/gtk/capture/v4l2/pal-v4l2-video-capture.cpp"
        # GLib-based inter-process messaging
        "pal/gtk/messaging/glib/pal-glib-msg.cpp"
        # Linux network (generic; D-Bus optional)
        "pal/gtk/network/pal-linux-network.cpp"
        # EGL rendering path
        "context-egl.cpp"
        "surface-egl.cpp"
    )

    # D-Bus network backend — requires dbus-glib which is not always present.
    if(PKG_CONFIG_FOUND)
        pkg_check_modules(DBUS_GLIB QUIET dbus-glib-1)
    endif()
    if(DBUS_GLIB_FOUND)
        list(APPEND Source_Files_PAL "pal/gtk/network/dbus/pal-dbus-network.cpp")
        set(_moon_gtk_extra_includes ${DBUS_GLIB_INCLUDE_DIRS})
        set(_moon_gtk_extra_libs     ${DBUS_GLIB_LIBRARIES})
    endif()

    # Gallium3D software renderer — requires Mesa Gallium headers.
    option(MOON_ENABLE_GALLIUM "Enable Gallium3D software renderer (requires Mesa dev headers)" OFF)
    if(MOON_ENABLE_GALLIUM)
        list(APPEND Source_Files_PAL
            "context-gallium.cpp"
            "surface-gallium.cpp"
            "surface-gallium-egl.cpp"
        )
    endif()
endif()

# IDE source grouping (harmless on non-VS generators).
source_group("Source Files"           FILES ${Source_Files_Core})
source_group("Source Files\\audio"    FILES ${Source_Files_Audio})
source_group("Source Files\\zip"      FILES ${Source_Files_Zip})
source_group("Source Files\\pal"      FILES ${Source_Files_PAL})

# ---- Target --------------------------------------------------------------

add_library(libmoon SHARED
    ${Source_Files_Core}
    ${Source_Files_Audio}
    ${Source_Files_Zip}
    ${Source_Files_PAL}
)

# ---- Include directories -------------------------------------------------

target_include_directories(libmoon PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}"
    "${CMAKE_CURRENT_SOURCE_DIR}/pal"
)

# Each PAL subdirectory contains a config.h that activates its feature flags.
if(MOON_PAL STREQUAL "win32")
    target_include_directories(libmoon PRIVATE
        "${CMAKE_CURRENT_SOURCE_DIR}/pal/win32"
        "${CMAKE_CURRENT_SOURCE_DIR}/pal/win32/events"
    )
elseif(MOON_PAL STREQUAL "sdl2")
    target_include_directories(libmoon PRIVATE
        "${CMAKE_CURRENT_SOURCE_DIR}/pal/sdl"
        "${CMAKE_CURRENT_SOURCE_DIR}/pal/sdl/events"
    )
elseif(MOON_PAL STREQUAL "gtk")
    target_include_directories(libmoon PRIVATE
        "${CMAKE_CURRENT_SOURCE_DIR}/pal/gtk"
    )
endif()

# ---- Compile definitions -------------------------------------------------

target_compile_definitions(libmoon PRIVATE
    BUILDING_LIBMOON
    HAVE_CONFIG_H          # tells sources that config.h exists and can be included
    # LOGGING must be defined globally so that runtime.h's #if LOGGING guard is
    # satisfied before debug.h includes runtime.h and then later sets #define LOGGING 1.
    # Without this, debug_flags is undeclared in Release/RelWithDebInfo builds.
    LOGGING=1
    $<$<CONFIG:Debug>:DEBUG _DEBUG>
    $<$<NOT:$<CONFIG:Debug>>:NDEBUG>
)

if(WIN32)
    target_compile_definitions(libmoon PRIVATE
        WIN32
        _WINDOWS
        _USRDLL
        UNICODE
        _UNICODE
        # NOMINMAX is defined inside pal.h already; don't duplicate it here
    )
else()
    target_compile_definitions(libmoon PRIVATE
        HAVE_SYS_TIME_H    # available on every POSIX target we care about
    )
endif()

if(MOON_ENABLE_CURL)
    target_compile_definitions(libmoon PRIVATE HAVE_CURL=1)
endif()

if(_moon_use_glew)
    target_compile_definitions(libmoon PRIVATE USE_GLEW=1)
endif()

# ---- Compile options -----------------------------------------------------

if(MSVC)
    target_compile_options(libmoon PRIVATE
        /W1         # Minimal warnings — bump to /W3 when the tree is clean
        /Gd         # __cdecl calling convention
        /MP         # Parallel compilation
        /Y-         # Disable precompiled headers
        $<$<CONFIG:Debug>:          /Od /RTC1 /Zi>
        $<$<CONFIG:RelWithDebInfo>: /O2 /Oi  /Gy /Zi>
        $<$<CONFIG:Release>:        /O2 /Oi  /Gy /Zi>
    )
    target_link_options(libmoon PRIVATE
        /DEBUG
        $<$<CONFIG:Debug>:          /INCREMENTAL>
        $<$<CONFIG:RelWithDebInfo>: /OPT:REF /OPT:ICF /INCREMENTAL:NO>
        $<$<CONFIG:Release>:        /OPT:REF /OPT:ICF /INCREMENTAL:NO>
    )
else()
    # GCC / Clang
    target_compile_options(libmoon PRIVATE
        -Wall
        -Wextra
        -Wno-unused-parameter
        $<$<CONFIG:Debug>:          -g -O0>
        $<$<CONFIG:RelWithDebInfo>: -O2 -g>
        $<$<CONFIG:Release>:        -O2>
    )
    # Export only symbols explicitly marked MOON_API.
    target_compile_options(libmoon PRIVATE -fvisibility=hidden)
endif()

# ---- Link libraries ------------------------------------------------------

target_link_libraries(libmoon PRIVATE
    _moon_glib
    _moon_cairo
    _moon_freetype
    _moon_expat
    _moon_zlib
    OpenGL::GL
)

if(MOON_ENABLE_CURL)
    target_link_libraries(libmoon PRIVATE _moon_curl)
endif()

if(_moon_use_glew)
    target_link_libraries(libmoon PRIVATE _moon_glew)
endif()

# PAL-specific link deps.
if(MOON_PAL STREQUAL "win32")
    target_link_libraries(libmoon PRIVATE
        comctl32
        dwrite
        shlwapi
        windowscodecs
        winmm
    )
elseif(MOON_PAL STREQUAL "sdl2")
    target_link_libraries(libmoon PRIVATE _moon_sdl2 _moon_sdl2_image)
    if(WIN32)
        target_link_libraries(libmoon PRIVATE dwrite)
    else()
        target_link_libraries(libmoon PRIVATE PkgConfig::FONTCONFIG)
    endif()
elseif(MOON_PAL STREQUAL "gtk")
    target_link_libraries(libmoon PRIVATE
        PkgConfig::GTK2
        PkgConfig::FONTCONFIG
    )
    if(_moon_gtk_extra_includes)
        target_include_directories(libmoon PRIVATE ${_moon_gtk_extra_includes})
    endif()
    if(_moon_gtk_extra_libs)
        target_link_libraries(libmoon PRIVATE ${_moon_gtk_extra_libs})
    endif()
    if(MOON_ENABLE_GALLIUM)
        target_compile_definitions(libmoon PRIVATE USE_GALLIUM=1)
    endif()
endif()

# Audio.
if(MOON_AUDIO_ALSA)
    target_link_libraries(libmoon PRIVATE _moon_alsa)
endif()
if(MOON_AUDIO_PULSE)
    target_link_libraries(libmoon PRIVATE PkgConfig::PULSE)
endif()
