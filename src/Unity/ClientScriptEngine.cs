using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;

namespace ClientScriptEngine
{
    public class Entity
    {
        public GameObject obj;
        public DynValue _internal;
        
        public Entity (GameObject obj)
        {
            this.obj = obj;
        }
    }

    public class EntityProxy
    {
        Entity target;

		[MoonSharpHidden]
		public EntityProxy (Entity e)
		{
			this.target = e;
		}

        public DynValue _internal
        {
            get { return target._internal; }
            set { target._internal = value; }
        }

        public void position_set (float x, float y, float z)
        {
            target.obj.transform.position = new Vector3(x, y, z);
        }
    }

    public class Lua
    {
        MoonSharp.Interpreter.Script S;

        public Lua (System.Action<string> print)
        {
            // Supposed to make loading faster?
            MoonSharp.Interpreter.Script.WarmUp();
            
            UserData.RegisterProxyType<EntityProxy, Entity>(
                    e => new EntityProxy(e));

            string[] paths = new string[] { "Scripts/?.lua" };
            S = new MoonSharp.Interpreter.Script();
            S.Options.ScriptLoader = new FileSystemScriptLoader();
            ((ScriptLoaderBase)S.Options.ScriptLoader).ModulePaths = paths;
            S.Options.DebugPrint = print;

            S.DoFile("Scripts/Movement.lua");
        }

        public void Test ()
        {
            S.DoFile("Scripts/Test.lua");
        }

        public void Init (Entity E)
        {
            S.Call(S.Globals["Init"], UserData.Create(E));
        }

        public void Step (Entity E, float dt)
        {
            S.Call(S.Globals["Step"], UserData.Create(E), dt);
        }
    }
}
