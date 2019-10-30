using System;
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
        L.Run();
        return 0;
    }
}
