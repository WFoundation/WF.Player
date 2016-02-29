# makefile for building Lua
# see ../INSTALL for installation instructions
# see ../Makefile and ../include/luaconf.h for further customization

# == CHANGE THE SETTINGS BELOW TO SUIT YOUR ENVIRONMENT =======================

# Your platform. See PLATS for possible values.
PLAT= none

CC= gcc
CFLAGS= -O2 -Wall $(MYCFLAGS)
AR= ar rcu
RANLIB= ranlib
RM= rm -f
LIBS= -lm $(MYLIBS)

MYCFLAGS=
MYLDFLAGS=
MYLIBS=

# == END OF USER SETTINGS. NO NEED TO CHANGE ANYTHING BELOW THIS LINE =========

PLATS= aix ansi bsd freebsd generic linux macosx mingw posix solaris

LUA_A=	liblua.a
CORE_O=	lapi.o lcode.o ldebug.o ldo.o ldump.o lfunc.o lgc.o llex.o lmem.o \
	lobject.o lopcodes.o lparser.o lstate.o lstring.o ltable.o ltm.o  \
	lundump.o lvm.o lzio.o
LIB_O=	lauxlib.o lbaselib.o ldblib.o liolib.o lmathlib.o loslib.o ltablib.o \
	lstrlib.o loadlib.o linit.o

LUA_T=	lua
LUA_O=	lua.o

LUAC_T=	luac
LUAC_O=	luac.o print.o

ALL_O= $(CORE_O) $(LIB_O) $(LUA_O) $(LUAC_O)
ALL_T= $(LUA_A) $(LUA_T) $(LUAC_T)
ALL_A= $(LUA_A)

default: $(PLAT)

all:	$(ALL_T)

o:	$(ALL_O)

a:	$(ALL_A)

$(LUA_A): $(CORE_O) $(LIB_O)
	$(AR) $@ $(CORE_O) $(LIB_O)
	$(RANLIB) $@

$(LUA_T): $(LUA_O) $(LUA_A)
	$(CC) -o $@ $(MYLDFLAGS) $(LUA_O) $(LUA_A) $(LIBS)

$(LUAC_T): $(LUAC_O) $(LUA_A)
	$(CC) -o $@ $(MYLDFLAGS) $(LUAC_O) $(LUA_A) $(LIBS)

clean:
	$(RM) $(ALL_T) $(ALL_O)

depend:
	@$(CC) $(CFLAGS) -MM l*.cpp print.cpp

echo:
	@echo "PLAT = $(PLAT)"
	@echo "CC = $(CC)"
	@echo "CFLAGS = $(CFLAGS)"
	@echo "AR = $(AR)"
	@echo "RANLIB = $(RANLIB)"
	@echo "RM = $(RM)"
	@echo "MYCFLAGS = $(MYCFLAGS)"
	@echo "MYLDFLAGS = $(MYLDFLAGS)"
	@echo "MYLIBS = $(MYLIBS)"

# convenience targets for popular platforms

none:
	@echo "Please choose a platform:"
	@echo "   $(PLATS)"

aix:
	$(MAKE) all CC="xlc" CFLAGS="-O2 -DLUA_USE_POSIX -DLUA_USE_DLOPEN" MYLIBS="-ldl" MYLDFLAGS="-brtl -bexpall"

ansi:
	$(MAKE) all MYCFLAGS=-DLUA_ANSI

bsd:
	$(MAKE) all MYCFLAGS="-DLUA_USE_POSIX -DLUA_USE_DLOPEN" MYLIBS="-Wl,-E"

freebsd:
	$(MAKE) all MYCFLAGS="-DLUA_USE_LINUX" MYLIBS="-Wl,-E -lreadline"

generic:
	$(MAKE) all MYCFLAGS=

linux:
	$(MAKE) all MYCFLAGS=-DLUA_USE_LINUX MYLIBS="-Wl,-E -ldl -lreadline -lhistory -lncurses"

macosx:
	$(MAKE) all MYCFLAGS=-DLUA_USE_LINUX MYLIBS="-lreadline"
# use this on Mac OS X 10.3-
#	$(MAKE) all MYCFLAGS=-DLUA_USE_MACOSX

mingw:
	$(MAKE) "LUA_A=lua51.dll" "LUA_T=lua.exe" \
	"AR=$(CC) -shared -o" "RANLIB=strip --strip-unneeded" \
	"MYCFLAGS=-DLUA_BUILD_AS_DLL" "MYLIBS=" "MYLDFLAGS=-s" lua.exe
	$(MAKE) "LUAC_T=luac.exe" luac.exe

posix:
	$(MAKE) all MYCFLAGS=-DLUA_USE_POSIX

solaris:
	$(MAKE) all MYCFLAGS="-DLUA_USE_POSIX -DLUA_USE_DLOPEN" MYLIBS="-ldl"

# list targets that do not create files (but not all makes understand .PHONY)
.PHONY: all $(PLATS) default o a clean depend echo none

# DO NOT DELETE

lapi.o: lapi.cpp ../include/lua.h ../include/luaconf.h ../include/lapi.h ../include/lobject.h ../include/llimits.h ../include/ldebug.h \
  ../include/lstate.h ../include/ltm.h ../include/lzio.h ../include/lmem.h ../include/ldo.h ../include/lfunc.h ../include/lgc.h ../include/lstring.h ../include/ltable.h \
  ../include/lundump.h ../include/lvm.h
lauxlib.o: lauxlib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h
lbaselib.o: lbaselib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
lcode.o: lcode.cpp ../include/lua.h ../include/luaconf.h ../include/lcode.h ../include/llex.h ../include/lobject.h ../include/llimits.h \
  ../include/lzio.h ../include/lmem.h ../include/lopcodes.h ../include/lparser.h ../include/ldebug.h ../include/lstate.h ../include/ltm.h ../include/ldo.h ../include/lgc.h \
  ../include/ltable.h
ldblib.o: ldblib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
ldebug.o: ldebug.cpp ../include/lua.h ../include/luaconf.h ../include/lapi.h ../include/lobject.h ../include/llimits.h ../include/lcode.h \
  ../include/llex.h ../include/lzio.h ../include/lmem.h ../include/lopcodes.h ../include/lparser.h ../include/ldebug.h ../include/lstate.h ../include/ltm.h ../include/ldo.h \
  ../include/lfunc.h ../include/lstring.h ../include/lgc.h ../include/ltable.h ../include/lvm.h
ldo.o: ldo.cpp ../include/lua.h ../include/luaconf.h ../include/ldebug.h ../include/lstate.h ../include/lobject.h ../include/llimits.h ../include/ltm.h \
  ../include/lzio.h ../include/lmem.h ../include/ldo.h ../include/lfunc.h ../include/lgc.h ../include/lopcodes.h ../include/lparser.h ../include/lstring.h \
  ../include/ltable.h ../include/lundump.h ../include/lvm.h
ldump.o: ldump.cpp ../include/lua.h ../include/luaconf.h ../include/lobject.h ../include/llimits.h ../include/lstate.h ../include/ltm.h \
  ../include/lzio.h ../include/lmem.h ../include/lundump.h
lfunc.o: lfunc.cpp ../include/lua.h ../include/luaconf.h ../include/lfunc.h ../include/lobject.h ../include/llimits.h ../include/lgc.h ../include/lmem.h \
  ../include/lstate.h ../include/ltm.h ../include/lzio.h
lgc.o: lgc.cpp ../include/lua.h ../include/luaconf.h ../include/ldebug.h ../include/lstate.h ../include/lobject.h ../include/llimits.h ../include/ltm.h \
  ../include/lzio.h ../include/lmem.h ../include/ldo.h ../include/lfunc.h ../include/lgc.h ../include/lstring.h ../include/ltable.h
linit.o: linit.cpp ../include/lua.h ../include/luaconf.h ../include/lualib.h ../include/lauxlib.h
liolib.o: liolib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
llex.o: llex.cpp ../include/lua.h ../include/luaconf.h ../include/ldo.h ../include/lobject.h ../include/llimits.h ../include/lstate.h ../include/ltm.h \
  ../include/lzio.h ../include/lmem.h ../include/llex.h ../include/lparser.h ../include/lstring.h ../include/lgc.h ../include/ltable.h
lmathlib.o: lmathlib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
lmem.o: lmem.cpp ../include/lua.h ../include/luaconf.h ../include/ldebug.h ../include/lstate.h ../include/lobject.h ../include/llimits.h \
  ../include/ltm.h ../include/lzio.h ../include/lmem.h ../include/ldo.h
loadlib.o: loadlib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
lobject.o: lobject.cpp ../include/lua.h ../include/luaconf.h ../include/ldo.h ../include/lobject.h ../include/llimits.h ../include/lstate.h \
  ../include/ltm.h ../include/lzio.h ../include/lmem.h ../include/lstring.h ../include/lgc.h ../include/lvm.h
lopcodes.o: lopcodes.cpp ../include/lopcodes.h ../include/llimits.h ../include/lua.h ../include/luaconf.h
loslib.o: loslib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
lparser.o: lparser.cpp ../include/lua.h ../include/luaconf.h ../include/lcode.h ../include/llex.h ../include/lobject.h ../include/llimits.h \
  ../include/lzio.h ../include/lmem.h ../include/lopcodes.h ../include/lparser.h ../include/ldebug.h ../include/lstate.h ../include/ltm.h ../include/ldo.h \
  ../include/lfunc.h ../include/lstring.h ../include/lgc.h ../include/ltable.h
lstate.o: lstate.cpp ../include/lua.h ../include/luaconf.h ../include/ldebug.h ../include/lstate.h ../include/lobject.h ../include/llimits.h \
  ../include/ltm.h ../include/lzio.h ../include/lmem.h ../include/ldo.h ../include/lfunc.h ../include/lgc.h ../include/llex.h ../include/lstring.h ../include/ltable.h
lstring.o: lstring.cpp ../include/lua.h ../include/luaconf.h ../include/lmem.h ../include/llimits.h ../include/lobject.h ../include/lstate.h \
  ../include/ltm.h ../include/lzio.h ../include/lstring.h ../include/lgc.h
lstrlib.o: lstrlib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
ltable.o: ltable.cpp ../include/lua.h ../include/luaconf.h ../include/ldebug.h ../include/lstate.h ../include/lobject.h ../include/llimits.h \
  ../include/ltm.h ../include/lzio.h ../include/lmem.h ../include/ldo.h ../include/lgc.h ../include/ltable.h
ltablib.o: ltablib.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
ltm.o: ltm.cpp ../include/lua.h ../include/luaconf.h ../include/lobject.h ../include/llimits.h ../include/lstate.h ../include/ltm.h ../include/lzio.h \
  ../include/lmem.h ../include/lstring.h ../include/lgc.h ../include/ltable.h
lua.o: lua.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/lualib.h
luac.o: luac.cpp ../include/lua.h ../include/luaconf.h ../include/lauxlib.h ../include/ldo.h ../include/lobject.h ../include/llimits.h \
  ../include/lstate.h ../include/ltm.h ../include/lzio.h ../include/lmem.h ../include/lfunc.h ../include/lopcodes.h ../include/lstring.h ../include/lgc.h \
  ../include/lundump.h
lundump.o: lundump.cpp ../include/lua.h ../include/luaconf.h ../include/ldebug.h ../include/lstate.h ../include/lobject.h \
  ../include/llimits.h ../include/ltm.h ../include/lzio.h ../include/lmem.h ../include/ldo.h ../include/lfunc.h ../include/lstring.h ../include/lgc.h ../include/lundump.h
lvm.o: lvm.cpp ../include/lua.h ../include/luaconf.h ../include/ldebug.h ../include/lstate.h ../include/lobject.h ../include/llimits.h ../include/ltm.h \
  ../include/lzio.h ../include/lmem.h ../include/ldo.h ../include/lfunc.h ../include/lgc.h ../include/lopcodes.h ../include/lstring.h ../include/ltable.h ../include/lvm.h
lzio.o: lzio.cpp ../include/lua.h ../include/luaconf.h ../include/llimits.h ../include/lmem.h ../include/lstate.h ../include/lobject.h ../include/ltm.h \
  ../include/lzio.h
print.o: print.cpp ../include/ldebug.h ../include/lstate.h ../include/lua.h ../include/luaconf.h ../include/lobject.h ../include/llimits.h \
  ../include/ltm.h ../include/lzio.h ../include/lmem.h ../include/lopcodes.h ../include/lundump.h

# (end of Makefile)
