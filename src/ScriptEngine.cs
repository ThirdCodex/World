using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace ScriptEngine
{
    public class Lua<Proxy, Target>
        where Proxy : class, new ()
        where Target : class
    {
        Script S;

        // Accepts a function to print things out within Lua and a delegate
        // that creates a new Proxy with type Target as a parameter. We cannot
        // declare that delegate within this class constructor because it is
        // a limitation of C#
        public Lua (System.Action<string> print, Func<Target, Proxy> createDel)
        {
            // Supposed to make loading faster?
            Script.WarmUp();

            UserData.RegisterProxyType<Proxy, Target>(createDel);

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

        public void Init (Target T)
        {
            S.Call(S.Globals["Init"], UserData.Create(T));
        }

        public void Step (Target T, float dt)
        {
            S.Call(S.Globals["Step"], UserData.Create(T), dt);
        }
    }
}
