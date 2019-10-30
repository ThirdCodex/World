using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace ScriptEngine
{
    public class Lua
    {
        Script S;

        public Lua (System.Action<string> print)
        {
            /* Supposed to make loading faster? */
            Script.WarmUp();

            string[] paths = new string[] { "Scripts/?.lua" };
            S = new Script();
            S.Options.ScriptLoader = new FileSystemScriptLoader();
            ((ScriptLoaderBase)S.Options.ScriptLoader).ModulePaths = paths;
            S.Options.DebugPrint = print;
        }

        public void Run ()
        {
            S.DoFile("Scripts/Test.lua");
        }
    }
}
