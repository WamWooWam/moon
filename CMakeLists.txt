cmake_minimum_required(VERSION 3.21 FATAL_ERROR)
project(Moonlight C CXX)

# ---- Language standard ----
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

# ---- MSVC runtime ----
# Use the DLL runtime (MD/MDd) so we match the rest of the dependency tree.
set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreadedDLL"
    CACHE STRING "MSVC runtime library selection")

# ---- Build configurations ----
# Explicitly enumerate the three configs we care about so multi-config
# generators (VS, Ninja Multi-Config) don't sprout MinSizeRel by default.
get_property(_is_multi GLOBAL PROPERTY GENERATOR_IS_MULTI_CONFIG)
if(_is_multi)
    set(CMAKE_CONFIGURATION_TYPES "Debug;RelWithDebInfo;Release"
        CACHE STRING "Build configurations" FORCE)
endif()

# ---- PAL backend ----
if(WIN32)
    set(_default_pal "win32")
elseif(APPLE)
    set(_default_pal "cocoa")
else()
    set(_default_pal "gtk")
endif()

set(MOON_PAL "${_default_pal}" CACHE STRING
    "Platform abstraction layer backend. One of: win32  sdl2  gtk  cocoa")
set_property(CACHE MOON_PAL PROPERTY STRINGS win32 sdl2 gtk cocoa)

if(MOON_PAL STREQUAL "win32" AND NOT WIN32)
    message(FATAL_ERROR "MOON_PAL=win32 requires a Windows host")
endif()
if(MOON_PAL STREQUAL "cocoa" AND NOT APPLE)
    message(FATAL_ERROR "MOON_PAL=cocoa requires macOS")
endif()

message(STATUS "Moonlight PAL: ${MOON_PAL}")

# ---- Feature flags ----
option(MOON_ENABLE_CURL    "Enable libcurl for networking"       ON)
option(MOON_ENABLE_FFMPEG  "Enable FFmpeg media pipeline"        OFF)
option(MOON_AUDIO_ALSA     "Enable ALSA audio backend"           OFF)
option(MOON_AUDIO_PULSE    "Enable PulseAudio audio backend"     OFF)
option(MOON_AUDIO_OPENSLES "Enable OpenSLES audio (Android)"     OFF)

# ---- Architecture token ----
# Used to locate prebuilt Windows deps laid out as deps/<lib>/{x64,x86}/...
if(CMAKE_SIZEOF_VOID_P EQUAL 8)
    set(MOON_ARCH "x64")
else()
    set(MOON_ARCH "x86")
endif()

set_property(GLOBAL PROPERTY USE_FOLDERS ON)

add_subdirectory(src/runtime)
