set(PROJECT_NAME libmoon)

################################################################################
# Source groups
################################################################################
set(no_group_source_files
    "../../.clang-format"
)
source_group("" FILES ${no_group_source_files})

set(Header_Files
    "animation.h"
    "application.h"
    "applier.h"
    "asxparser.h"
    "audio-alsa.h"
    "audio-opensles.h"
    "audio-pulse.h"
    "audio.h"
    "authors.h"
    "bitmapcache.h"
    "bitmapimage.h"
    "bitmapsource.h"
    "border.h"
    "brush.h"
    "canvas.h"
    "capture.h"
    "cbinding.h"
    "clock.h"
    "collection.h"
    "color.h"
    "consent.h"
    "contentcontrol.h"
    "contentpresenter.h"
    "context-cairo.h"
    "context-cgl.h"
    "context-egl.h"
    "context-gallium.h"
    "context-gl.h"
    "context-opengl.h"
    "context.h"
    "control.h"
    "cornerradius.h"
    "cpu.h"
    "debug.h"
    "deepzoomimagetilesource.h"
    "dependencyobject.h"
    "dependencyproperty.h"
    "deployment.h"
    "designmode.h"
    "dirty.h"
    "downloader.h"
    "drm.h"
    "easing.h"
    "effect.h"
    "enums.h"
    "error.h"
    "eventargs.h"
    "factory.h"
    "font-utils.h"
    "fontfamily.h"
    "fontmanager.h"
    "fonts.h"
    "fontsource.h"
    "fontstretch.h"
    "fontstyle.h"
    "fontweight.h"
    "framerate-display.h"
    "frameworkelement.h"
    "fullscreen.h"
    "gchandle.h"
    "geometry.h"
    "gkeyfile.h"
    "glyphs.h"
    "grid.h"
    "http-streaming.h"
    "imagesource.h"
    "incomplete-support.h"
    "inputmethod.h"
    "inputscope.h"
    "jpeg.h"
    "keyboard.h"
    "layoutinformation.h"
    "libmoon.h"
    "list.h"
    "managedtypeinfo.h"
    "media.h"
    "mediaelement.h"
    "medialog.h"
    "mediaplayer.h"
    "messaging.h"
    "moon-curves.h"
    "moon-path.h"
    "moonlightconfiguration.h"
    "mp3.h"
    "multiscaleimage.h"
    "multiscalesubimage.h"
    "namescope.h"
    "network-curl.h"
    "network.h"
    "notificationwindow.h"
    "openfile.h"
    "pal/pal-threads.h"
    "pal/pal.h"
    "pal/window.h"
    "panel.h"
    "pipeline-asf.h"
    "pipeline-ffmpeg.h"
    "pipeline-mp4.h"
    "pipeline-vda.h"
    "pipeline.h"
    "playlist.h"
    "point.h"
    "popup.h"
    "printing.h"
    "projection.h"
    "propertypath.h"
    "provider.h"
    "ptr.h"
    "rect.h"
    "region.h"
    "resources.h"
    "richtextbox.h"
    "richtextlayout.h"
    "runtime.h"
    "security.h"
    "shape.h"
    "size.h"
    "style.h"
    "stylus.h"
    "surface-cairo.h"
    "surface-cgl.h"
    "surface-egl.h"
    "surface-gallium-egl.h"
    "surface-gallium.h"
    "surface-gl.h"
    "surface-opengl.h"
    "surface.h"
    "tabnavigationwalker.h"
    "template.h"
    "textblock.h"
    "textbox.h"
    "textelement.h"
    "textlayout.h"
    "textoptions.h"
    "textpointer.h"
    "textselection.h"
    "thickness.h"
    "tilesource.h"
    "timeline.h"
    "timemanager.h"
    "timesource.h"
    "transform.h"
    "trigger.h"
    "type.h"
    "uielement.h"
    "uri.h"
    "usercontrol.h"
    "utils.h"
    "validators.h"
    "value.h"
    "weakrefmanager.h"
    "webbrowser.h"
    "writeablebitmap.h"
    "xaml.h"
    "xap.h"
    "yuv-converter.h"
    "zip/crypt.h"
    "zip/ioapi.h"
    "zip/unzip.h"
)
source_group("Header Files" FILES ${Header_Files})

set(Header_Files__pal__win32
    "pal/win32/pal-win32.h"
    "pal/win32/window-win32.h"
    "pal/win32/clipboard-win32.h"
    "pal/win32/fonts-win32.h"
    "pal/win32/im-win32.h"
    "pal/win32/installer-win32.h"
    "pal/win32/pixbuf-win32.h"
    "pal/win32/helpers/dialogeventhandler.h"
    "pal/win32/events/button-win32.h"
    "pal/win32/events/wheel-win32.h"
    "pal/win32/events/motion-win32.h"
    "pal/win32/events/key-win32.h"
)
source_group("Header Files\\pal\\win32" FILES ${Header_Files__pal__win32})

set(Header_Files__pal__sdl
    "pal/sdl/pal-sdl2.h"
    "pal/sdl/window-sdl2.h"
    "pal/sdl/clipboard-sdl2.h"
    "pal/sdl/fonts-sdl2.h"
    "pal/sdl/im-sdl2.h"
    "pal/sdl/installer-sdl2.h"
    "pal/sdl/pixbuf-sdl2.h"
    "pal/sdl/events/button-sdl2.h"
    "pal/sdl/events/wheel-sdl2.h"
    "pal/sdl/events/motion-sdl2.h"
    "pal/sdl/events/key-sdl2.h"
)
source_group("Header Files\\pal\\sdl" FILES ${Header_Files__pal__sdl})

set(Source_Files
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
    "context-wgl.cpp"
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
    "network-curl.cpp"
    "network.cpp"
    "notificationwindow.cpp"
    "openfile.cpp"
    "pal/window.cpp"
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
    "surface-wgl.cpp"
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
)
source_group("Source Files" FILES ${Source_Files})

set(Source_Files__pal
    "pal/pal.cpp"
)
source_group("Source Files\\pal" FILES ${Source_Files__pal})

if(PAL_WIN32)
set(Source_Files__pal__win32
    "pal/win32/fonts-win32.cpp"
    "pal/win32/im-win32.cpp"
    "pal/win32/pal-win32.cpp"
    "pal/win32/pixbuf-win32.cpp"
    "pal/win32/threads-win32.cpp"
    "pal/win32/window-win32.cpp"
    "pal/win32/clipboard-win32.cpp"
    "pal/win32/fonts-win32.cpp"
    "pal/win32/installer-win32.cpp"
    "pal/win32/helpers/dialogeventhandler.cpp"
)
endif()
source_group("Source Files\\pal\\win32" FILES ${Source_Files__pal__win32})

if(PAL_SDL2)
set(Source_Files__pal__sdl2
    "pal/sdl/fonts-sdl2.cpp"
    "pal/sdl/im-sdl2.cpp"
    "pal/sdl/pal-sdl2.cpp"
    "pal/sdl/pixbuf-sdl2.cpp"
    "pal/sdl/threads-sdl2.cpp"
    "pal/sdl/window-sdl2.cpp"
    "pal/sdl/clipboard-sdl2.cpp"
    "pal/sdl/fonts-sdl2.cpp"
    "pal/sdl/installer-sdl2.cpp"
    "pal/sdl/gl/context-sdl2.cpp"
    "pal/sdl/gl/surface-sdl2.cpp"
)
endif()
source_group("Source Files\\pal\\sdl" FILES ${Source_Files__pal__sdl2})

set(Source_Files__zip
    "zip/ioapi.c"
    "zip/unzip.c"
)
source_group("Source Files\\zip" FILES ${Source_Files__zip})

set(ALL_FILES
    ${no_group_source_files}
    ${Header_Files}
    ${Header_Files__pal__win32}
    ${Header_Files__pal__sdl}
    ${Source_Files}
    ${Source_Files__pal}
    ${Source_Files__pal__win32}
    ${Source_Files__pal__sdl2}
    ${Source_Files__zip}
)

################################################################################
# Target
################################################################################
add_library(${PROJECT_NAME} SHARED ${ALL_FILES})

use_props(${PROJECT_NAME} "${CMAKE_CONFIGURATION_TYPES}" "${DEFAULT_CXX_PROPS}")
set(ROOT_NAMESPACE libmoon)

set_target_properties(${PROJECT_NAME} PROPERTIES
    VS_GLOBAL_KEYWORD "Win32Proj"
)
################################################################################
# Output directory
################################################################################
if("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x64")
    set_target_properties(${PROJECT_NAME} PROPERTIES
        OUTPUT_DIRECTORY_DEBUG   "${CMAKE_CURRENT_SOURCE_DIR}/bin/$<CONFIG>/${CMAKE_VS_PLATFORM_NAME}/"
        OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}/bin/$<CONFIG>/${CMAKE_VS_PLATFORM_NAME}/"
    )
elseif("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x86")
    set_target_properties(${PROJECT_NAME} PROPERTIES
        OUTPUT_DIRECTORY_DEBUG   "${CMAKE_CURRENT_SOURCE_DIR}/bin/$<CONFIG>/${CMAKE_VS_PLATFORM_NAME}/"
        OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}/bin/$<CONFIG>/${CMAKE_VS_PLATFORM_NAME}/"
    )
endif()
if("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x64")
    set_target_properties(${PROJECT_NAME} PROPERTIES
        INTERPROCEDURAL_OPTIMIZATION_RELEASE "TRUE"
    )
elseif("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x86")
    set_target_properties(${PROJECT_NAME} PROPERTIES
        INTERPROCEDURAL_OPTIMIZATION_RELEASE "TRUE"
    )
endif()
################################################################################
# MSVC runtime library
################################################################################
get_property(MSVC_RUNTIME_LIBRARY_DEFAULT TARGET ${PROJECT_NAME} PROPERTY MSVC_RUNTIME_LIBRARY)
if("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x64")
    string(CONCAT "MSVC_RUNTIME_LIBRARY_STR"
        $<$<CONFIG:Debug>:
            MultiThreadedDebugDLL
        >
        $<$<CONFIG:Release>:
            MultiThreadedDLL
        >
        $<$<NOT:$<OR:$<CONFIG:Debug>,$<CONFIG:Release>>>:${MSVC_RUNTIME_LIBRARY_DEFAULT}>
    )
elseif("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x86")
    string(CONCAT "MSVC_RUNTIME_LIBRARY_STR"
        $<$<CONFIG:Debug>:
            MultiThreadedDebugDLL
        >
        $<$<CONFIG:Release>:
            MultiThreadedDLL
        >
        $<$<NOT:$<OR:$<CONFIG:Debug>,$<CONFIG:Release>>>:${MSVC_RUNTIME_LIBRARY_DEFAULT}>
    )
endif()
set_target_properties(${PROJECT_NAME} PROPERTIES MSVC_RUNTIME_LIBRARY ${MSVC_RUNTIME_LIBRARY_STR})

################################################################################
# Include directories
################################################################################
target_include_directories(${PROJECT_NAME} PUBLIC
    "${CMAKE_CURRENT_SOURCE_DIR}/.;"
    "${CMAKE_CURRENT_SOURCE_DIR}/pal;"
    "${CMAKE_CURRENT_SOURCE_DIR}/pal/win32;"
    "${CMAKE_CURRENT_SOURCE_DIR}/pal/win32/events;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/curl/builds/libcurl-vc-${CMAKE_VS_PLATFORM_NAME}-release-static-ipv6-sspi-schannel/include;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/cairo/include;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/expat/include;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/freetype/include;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/eglib/eglib/src;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/zlib/include;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/glew/include"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/sdl2/include"
)

################################################################################
# Compile definitions
################################################################################
if("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x64")
    target_compile_definitions(${PROJECT_NAME} PRIVATE
        "$<$<CONFIG:Debug>:"
            "_DEBUG"
        ">"
        "$<$<CONFIG:Release>:"
            "NDEBUG"
        ">"
        "WIN32;"
        "BUILDING_LIBMOON;"
        "DEBUG;"
        "_WINDOWS;"
        "_USRDLL;"
        "LIBMOON_EXPORTS;"
        "UNICODE;"
        "_UNICODE"
    )
elseif("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x86")
    target_compile_definitions(${PROJECT_NAME} PRIVATE
        "$<$<CONFIG:Debug>:"
            "_DEBUG"
        ">"
        "$<$<CONFIG:Release>:"
            "NDEBUG"
        ">"
        "WIN32;"
        "BUILDING_LIBMOON;"
        "DEBUG;"
        "_WINDOWS;"
        "_USRDLL;"
        "LIBMOON_EXPORTS;"
        "UNICODE;"
        "_UNICODE"
    )
endif()

################################################################################
# Compile and link options
################################################################################
if(MSVC)
    if("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x64")
        target_compile_options(${PROJECT_NAME} PRIVATE
            $<$<CONFIG:Debug>:
                /std:c++17;
                /Gm-;
                /Od;
                /RTC1
            >
            $<$<CONFIG:Release>:
                /GL-;
                /O2;
                /Oi;
                /Gy
            >
            /MP;
            /Gd;
            /W1;
            /Zi;
            ${DEFAULT_CXX_EXCEPTION_HANDLING};
            /Y-
        )
    elseif("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x86")
        target_compile_options(${PROJECT_NAME} PRIVATE
            $<$<CONFIG:Debug>:
                /Gm-;
                /Od;
                /RTC1;
                /ZI
            >
            $<$<CONFIG:Release>:
                /GL-;
                /O2;
                /Oi;
                /Gy;
                /Zi
            >
            /MP;
            /Gd;
            /W1;
            ${DEFAULT_CXX_EXCEPTION_HANDLING};
            /Y-
        )
    endif()
    if("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x64")
        target_link_options(${PROJECT_NAME} PRIVATE
            $<$<CONFIG:Debug>:
                /INCREMENTAL
            >
            $<$<CONFIG:Release>:
                /OPT:REF;
                /LTCG;
                /OPT:ICF;
                /INCREMENTAL:NO
            >
            /DEBUG;
            /MACHINE:X64;
            /SUBSYSTEM:WINDOWS
        )
    elseif("${CMAKE_VS_PLATFORM_NAME}" STREQUAL "x86")
        target_link_options(${PROJECT_NAME} PRIVATE
            $<$<CONFIG:Debug>:
                /INCREMENTAL
            >
            $<$<CONFIG:Release>:
                /OPT:REF;
                /LTCG;
                /OPT:ICF;
                /INCREMENTAL:NO
            >
            /DEBUG;
            /MACHINE:X86;
            /SUBSYSTEM:WINDOWS
        )
    endif()
endif()

################################################################################
# Dependencies
################################################################################
set(ADDITIONAL_LIBRARY_DEPENDENCIES
    "$(CoreLibraryDependencies);"
    "zlib;"
    "cairo;"
    "libexpat;"
    "eglib;"
    "freetype;"
    "libcurl_a;"
    "ws2_32;"
    "crypt32;"
    "wldap32;"
    "normaliz;"
    "comctl32;"
    "shlwapi;"
    "windowscodecs;"
    "winmm;"
    "opengl32;"
    "glew32"
)
target_link_libraries(${PROJECT_NAME} PUBLIC "${ADDITIONAL_LIBRARY_DEPENDENCIES}")

target_link_directories(${PROJECT_NAME} PUBLIC
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/eglib/${CMAKE_VS_PLATFORM_NAME}/Release"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/cairo/lib/${CMAKE_VS_PLATFORM_NAME};"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/freetype/release/${CMAKE_VS_PLATFORM_NAME};"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/expat/lib/${CMAKE_VS_PLATFORM_NAME}/Release;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/glew/lib/Release/${CMAKE_VS_PLATFORM_NAME};"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/curl/builds/libcurl-vc-${CMAKE_VS_PLATFORM_NAME}-release-static-ipv6-sspi-schannel/lib;"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/zlib/binaries/${CMAKE_VS_PLATFORM_NAME}/Release"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../deps/sdl2/lib/${CMAKE_VS_PLATFORM_NAME}"
)