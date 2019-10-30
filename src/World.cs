using System;
using System.Threading;
using ScriptEngine;

public class World
{
    public World ()
    {
    }
}

public static class Program
{
    public static int Main (String[] args)
    {
        Lua L = new Lua(Console.WriteLine);
        Entity E = new Entity("leroy");
        L.Init(E);

        while (true) {
            Thread.Sleep(1000 / 60);
            L.Step(E, 1/60f);
        }

        return 0;
    }
}
