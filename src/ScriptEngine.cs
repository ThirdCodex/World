using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace ScriptEngine
{
    public class Vector
    {
        public float x, y, z;

        public Vector (int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class Entity
    {
        public string name;
        public DynValue _internal;

        public Vector position;
        public Vector direction;

        public Entity (string name)
        {
            this.name = name;
            this.position = new Vector(0,0,0);
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

        public string name
        {
            get { return target.name; }
            set { target.name = value; }
        }

        public void position_set (float x, float y, float z)
        {
            target.position.x = x;
            target.position.y = y;
            target.position.z = z;
        }
    }

    public class Lua
    {
        Script S;

        public Lua (System.Action<string> print)
        {
            // Supposed to make loading faster?
            Script.WarmUp();

            // Automatically register all MoonSharpUserData types
            //UserData.RegisterAssembly();
            
            UserData.RegisterProxyType<EntityProxy, Entity>(
                    e => new EntityProxy(e));

            string[] paths = new string[] { "Scripts/?.lua" };
            S = new Script();
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
