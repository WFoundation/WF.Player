LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE    := Lua51
LOCAL_SRC_FILES := ../../src/lapi.cpp ../../src/lauxlib.cpp ../../src/lbaselib.cpp ../../src/lcode.cpp ../../src/ldblib.cpp ../../src/ldebug.cpp ../../src/ldo.cpp ../../src/ldump.cpp ../../src/lfunc.cpp ../../src/lgc.cpp ../../src/linit.cpp ../../src/liolib.cpp ../../src/llex-android.cpp ../../src/lmathlib.cpp ../../src/lmem.cpp ../../src/lnet.cpp ../../src/loadlib.cpp ../../src/lobject.cpp ../../src/lopcodes.cpp ../../src/loslib.cpp ../../src/lparser.cpp ../../src/lstate.cpp ../../src/lstring.cpp ../../src/lstrlib.cpp ../../src/ltable.cpp ../../src/ltablib.cpp ../../src/ltm.cpp ../../src/lundump.cpp ../../src/lvm.cpp ../../src/lzio.cpp
LOCAL_C_INCLUDES := $(LOCAL_PATH)/../../include/
LOCAL_LDLIBS    := -lm

include $(BUILD_SHARED_LIBRARY)
