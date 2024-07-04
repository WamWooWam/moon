/**
 * unistd.h: Emulation of some standard UNIX APIs
 **/

#pragma once

#include <io.h>

/* map ssize_t to int */
typedef int ssize_t;

/* Map the Windows equivalents to the POSIX function names */

#define close(fd) _close(fd)
#define read(fd, buf, n) _read(fd, buf, n)
#define write(fd, buf, n) _write(fd, buf, n)

/* Implement some sys/stat.h macros that are missing in Windows */
#define S_ISDIR(mode) ((mode) & _S_IFDIR)
#define S_ISREG(mode) ((mode) & _S_IFREG)
#define S_ISLINK(mode) (false)

#define symlink(target, link) CreateSymbolicLinkA(link, target, 0)