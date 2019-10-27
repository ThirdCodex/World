using System;
using System.Collections.Generic;
using Utilities;
using Script;

// 
// A dummy class for immitating Player interactions with a server.
//
public class Player
{
    public static bool Handler (Event type, Dictionary<string, string> props)
    {
        MoveScript ms = new MoveScript(); 
        ms.Update(props);
        Console.WriteLine(ms.location.ToString());
        return true;
    }

    public static int Main (String[] args)
    {
        Client client = new Client("tcp://127.0.0.1");
        client.Connect(Handler);
        client.Disconnect();
        return 0;
    }
}
