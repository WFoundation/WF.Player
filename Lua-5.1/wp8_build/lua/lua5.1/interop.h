#pragma once

#include <lua.h>
#include <lualib.h>
#include <lauxlib.h>
#include <lnet.h>

#include <string>
#include <objbase.h>

using namespace Platform;

namespace lua52
{	
	public ref class Interop sealed
	{
	public:
		// Lua API
		typedef String^      string;

	private:
		Interop();

		inline static lua_State * TS (intptr_t  ptr) {return (lua_State*)(void*) ptr;}
		inline static intptr_t TC(const char * ptr) { return intptr_t ((void*) ptr); }
		inline static intptr_t TS(void * ptr) {return intptr_t (ptr); };

		inline static std::string TC (string str) 
		{
			return std::string (str->Begin (), str->End ());
		}

		inline static std::wstring TW(string str)
		{
			return std::wstring(str->Begin(), str->End());
		}

	public:


		static int LuaGC(intptr_t state, int what, int data)
		{
			return ::lua_gc (TS(state), what, data);
		}

		static intptr_t LuaTypeName (intptr_t luaState, int type)
		{
			return TC(::lua_typename (TS(luaState), type));
		}

		static void LuaLError(intptr_t luaState, string message)
		{
			::luaL_error (TS(luaState), TC (message).c_str ());
		}

		static void LuaLWhere(intptr_t luaState, int level)
		{
			::luaL_where(TS(luaState), level);
		}

		static intptr_t LuaLNewState ()
		{
			return TS(::luaL_newstate ());
		}

		static void LuaClose(intptr_t luaState)
		{
			::lua_close (TS(luaState));
		}

		static void LuaLOpenLibs(intptr_t luaState)
		{
			::luaL_openlibs(TS(luaState));
		}

		static int LuaLLoadString(intptr_t luaState, string chunk)
		{
			return ::luaL_loadstring(TS(luaState), TC(chunk).c_str());
		}

		static int LuaLLoadStringArray(intptr_t luaState,const Platform::Array<uint8>^ chunk)
		{
			const char * data = (char *)chunk->begin();
			return ::luaL_loadstring(TS(luaState), data);
		}

		static void LuaCreateTable(intptr_t luaState, int narr, int nrec)
		{
			::lua_createtable(TS(luaState), narr, nrec);
		}

		static void LuaGetTable(intptr_t luaState, int index)
		{
			::lua_gettable(TS(luaState), index);
		}

		static void LuaSetTop(intptr_t luaState, int newTop)
		{
			::lua_settop (TS(luaState), newTop);
		}

		static void LuaInsert(intptr_t luaState, int newTop)
		{
			::lua_insert(TS(luaState), newTop);
		}

		static void LuaRemove(intptr_t luaState, int index)
		{
			::lua_remove(TS(luaState), index);
		}

		static void LuaRawGet(intptr_t luaState, int index)
		{
			::lua_rawget(TS(luaState), index);
		}

		static void LuaSetTable(intptr_t luaState, int index)
		{
			::lua_settable(TS(luaState), index);
		}

		static void LuaRawSet(intptr_t luaState, int index)
		{
			::lua_rawset(TS(luaState), index);
		}


		static void LuaSetMetatable(intptr_t luaState, int objIndex)
		{
			::lua_setmetatable(TS(luaState), objIndex);
		}


		static int LuaGetMetatable(intptr_t luaState, int objIndex)
		{
			return ::lua_getmetatable(TS(luaState), objIndex);
		}


		static int LuaNetEqual(intptr_t luaState, int index1, int index2)
		{
			return ::luanet_equal(TS(luaState), index1, index2);
		}


		static void LuaPushValue(intptr_t luaState, int index)
		{
			::lua_pushvalue(TS(luaState), index);
		}


		static void LuaReplace(intptr_t luaState, int index)
		{
			::lua_replace(TS(luaState), index);
		}


		static int LuaGetTop(intptr_t luaState)
		{
			return ::lua_gettop(TS(luaState));
		}


		static int LuaType(intptr_t luaState, int index)
		{
			return ::lua_type(TS(luaState), index);
		}


		static int LuaLRef(intptr_t luaState, int registryIndex)
		{
			return ::luaL_ref(TS(luaState), registryIndex);
		}


		static void LuaRawGetI(intptr_t luaState, int tableIndex, int index)
		{
			::lua_rawgeti(TS(luaState), tableIndex, index);
		}

		static void LuaRawSetI(intptr_t luaState, int tableIndex, int index)
		{
			::lua_rawseti(TS(luaState), tableIndex, index);
		}


		static intptr_t LuaNewUserData(intptr_t luaState, unsigned int size)
		{
			return TS(::lua_newuserdata(TS(luaState), size));
		}

		static intptr_t LuaToUserData(intptr_t luaState, int index)
		{
			return TS(::lua_touserdata(TS(luaState), index));
		}


		static void LuaLUnref(intptr_t luaState, int registryIndex, int reference)
		{
			::luaL_unref(TS(luaState), registryIndex, reference);
		}


		static int LuaIsString(intptr_t luaState, int index)
		{
			return ::lua_isstring(TS(luaState), index);
		}


		static int LuaIsCFunction(intptr_t luaState, int index)
		{
			return ::lua_iscfunction(TS(luaState), index);
		}


		static void LuaPushNil(intptr_t luaState)
		{
			::lua_pushnil(TS(luaState));
		}


		static int LuaCall(intptr_t luaState, int nArgs, int nResults)
		{
			::lua_call(TS(luaState), nArgs, nResults);
			return 0;
		}


		static int LuaNetPCall(intptr_t luaState, int nArgs, int nResults, int errfunc)
		{
			return ::luanet_pcall(TS(luaState), nArgs, nResults, errfunc);
		}


		static intptr_t LuaToCFunction(intptr_t luaState, int index)
		{
			return TS(::lua_tocfunction(TS(luaState), index));
		}


		static double LuaNetToNumber(intptr_t luaState, int index)
		{
			return ::luanet_tonumber(TS(luaState), index);
		}

		static int LuaToBoolean(intptr_t luaState, int index)
		{
			return ::lua_toboolean(TS(luaState), index);
		}

		static intptr_t LuaToLString(intptr_t luaState, int index, size_t * strLen)
		{
			return TC(::lua_tolstring(TS(luaState), index, strLen));
		}

		static void LuaAtPanic(intptr_t luaState, intptr_t panicf)
		{
			::lua_atpanic(TS(luaState), (lua_CFunction) (void*)panicf);
		}

		static void LuaPushStdCallCFunction(intptr_t luaState, intptr_t function)
		{
			::lua_pushstdcallcfunction(TS(luaState), (lua_stdcallCFunction) (void*) function);
		}

		static void LuaPushNumber(intptr_t luaState, double number)
		{
			::lua_pushnumber(TS(luaState), number);
		}

		static void LuaPushBoolean(intptr_t luaState, int value)
		{
			::lua_pushboolean(TS(luaState), value);
		}

		static void LuaNetPushLString(intptr_t luaState, string str, size_t size)
		{
			::luanet_pushlwstring(TS(luaState), TW(str).c_str(), size);
		}

		static void LuaPushString(intptr_t luaState, string str)
		{
			::luanet_pushwstring(TS(luaState), TW(str).c_str());
		}

		static int LuaLNewMetatable(intptr_t luaState, string meta)
		{
			return ::luaL_newmetatable(TS(luaState), TC(meta).c_str ());
		}

		static void LuaGetField(intptr_t luaState, int stackPos, string meta)
		{
			::lua_getfield(TS(luaState), stackPos, TC(meta).c_str());
		}

		static intptr_t LuaLCheckUData(intptr_t luaState, int stackPos, string meta)
		{
			return TS(::luaL_checkudata(TS(luaState), stackPos, TC(meta).c_str ()));
		}

		static int LuaLGetMetafield(intptr_t luaState, int stackPos, string field)
		{
			return ::luaL_getmetafield(TS(luaState), stackPos, TC(field).c_str());
		}

		static int LuaNetLoadBuffer(intptr_t luaState, string buff, size_t size, string name)
		{
			return ::luanet_loadbuffer(TS(luaState), TC(buff).c_str(), size, TC(name).c_str());
		}

		static int LuaNetLoadBufferArray(intptr_t luaState, const Array<uint8>^ buff, size_t size, string name)
		{
			return ::luanet_loadbuffer(TS(luaState), (char *)buff->begin (), size, TC(name).c_str());
		}

		static int LuaNetLoadFile(intptr_t luaState, string filename)
		{
			return ::luanet_loadfile(TS(luaState), TC(filename).c_str());
		}

		static void LuaError(intptr_t luaState)
		{
			::lua_error(TS(luaState));
		}

		static int LuaCheckStack(intptr_t luaState, int extra)
		{
			return ::lua_checkstack(TS(luaState), extra);
		}

		static  int LuaNext (intptr_t luaState, int index)
		{
			return ::lua_next(TS(luaState), index);
		}

		static void LuaPushLightUserData(intptr_t luaState, intptr_t udata)
		{
			::lua_pushlightuserdata(TS(luaState), (void *)udata);
		}

		static int LuaLCheckMetatable(intptr_t luaState, int obj)
		{
			return ::luaL_checkmetatable(TS(luaState), obj);
		}

		static int LuaGetHookMask(intptr_t luaState)
		{
			return ::lua_gethookmask(TS(luaState));
		}

		static int LuaSetHook(intptr_t luaState, intptr_t func, int mask, int count)
		{
			return ::lua_sethook(TS(luaState), (lua_Hook)(void *)func, mask, count);
		}

		static int LuaGetHookCount(intptr_t luaState)
		{
			return ::lua_gethookcount(TS(luaState));
		}

		static intptr_t LuaGetLocal(intptr_t luaState, intptr_t ar, int n)
		{
			return TC(::lua_getlocal(TS(luaState), (lua_Debug *)(void *)ar, n));
		}

		static intptr_t LuaSetLocal(intptr_t luaState, intptr_t ar, int n)
		{
			return TC(::lua_setlocal(TS(luaState), (lua_Debug*)(void *) ar, n));
		}

		static intptr_t LuaGetUpValue(intptr_t luaState, int funcindex, int n)
		{
			return TC(::lua_getupvalue(TS(luaState), funcindex, n));
		}

		static intptr_t LuaSetUpValue(intptr_t luaState, int funcindex, int n)
		{
			return TC(::lua_setupvalue(TS(luaState), funcindex, n));
		}

		static int LuaNetToNetObject(intptr_t luaState, int index)
		{
			return ::luanet_tonetobject(TS(luaState), index);
		}

		static void LuaNetNewUData(intptr_t luaState, int val)
		{
			::luanet_newudata(TS(luaState), val);
		}

		static int LuaNetRawNetObj(intptr_t luaState, int obj)
		{
			return ::luanet_rawnetobj(TS(luaState), obj);
		}

		static int LuaNetCheckUData(intptr_t luaState, int ud, string tname)
		{
			return ::luanet_checkudata(TS(luaState), ud, TC(tname).c_str());
		}

		static intptr_t LuaNetGetTag()
		{
			return TS(::luanet_gettag());
		}

		static void LuaNetPushGlobalTable(intptr_t luaState)
		{
			::luanet_pushglobaltable(TS(luaState));
		}

		static void LuaNetPopGlobalTable(intptr_t luaState)
		{
			::luanet_popglobaltable(TS(luaState));
		}

		static void LuaNetGetGlobal(intptr_t luaState, string name)
		{
			::luanet_getglobal(TS(luaState), TC(name).c_str());
		}

		static void LuaNetSetGlobal(intptr_t luaState, string name)
		{
			::luanet_setglobal(TS(luaState), TC(name).c_str());
		}																							

		static int LuaNetRegistryIndex()
		{
			return ::luanet_registryindex();
		}

		static intptr_t LuaNetGetMainState(intptr_t luaState)
		{
			return TS(::luanet_get_main_state(TS(luaState)));
		}

		static int LuaNetIsStringStrict(intptr_t luaState, int idx)
		{
			return ::luanet_isstring_strict(TS(luaState), idx);
		}

		static int LuaGetInfo(intptr_t luaState, string what, intptr_t pDebug)
		{
			return ::lua_getinfo(TS(luaState), TC(what).c_str(), (lua_Debug*) (void*) pDebug);
		}

		static int LuaGetStack(intptr_t luaState, int level, intptr_t pDebug)
		{
			return ::lua_getstack(TS(luaState), level, (lua_Debug*) (void*) pDebug);
		}
	};
}