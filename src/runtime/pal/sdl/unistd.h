/**
 * unistd.h: Emulation of some standard UNIX APIs on Windows.
 * On POSIX platforms this header chains to the real <unistd.h>.
 **/

#pragma once

#ifdef _WIN32

#include <io.h>

typedef int ssize_t;

#define close(fd)        _close(fd)
#define read(fd, buf, n) _read(fd, buf, n)
#define write(fd, buf, n) _write(fd, buf, n)

#define S_ISDIR(mode)  ((mode) & _S_IFDIR)
#define S_ISREG(mode)  ((mode) & _S_IFREG)
#define S_ISLINK(mode) (false)

#define symlink(target, link) CreateSymbolicLinkA(link, target, 0)

#else
/* We're shadowing the system <unistd.h> because we're in a -I path.
   Use #include_next to pull in the real one. */
#include_next <unistd.h>
#endif
