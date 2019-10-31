using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using NNanomsg;
using NNanomsg.Protocols;
using System.Diagnostics;

//using EventHandler = System.Func<Script.Event, System.Collections.Generic.Dictionary<string, string>, bool>;
using EventHandler = System.Func<string, bool>;

public struct WorldEvent
{
    public Script.Event type;
    public Dictionary<string, string> properties;
}

public class Client
{
    protected static bool running = true;

    protected String url;
    protected bool connected;
    protected RequestSocket interaction;
    protected SubscribeSocket world;
    protected String id;
    protected String req_register;
    protected String req_access;
    protected String req_end;
    protected String acknowledged;
    protected String denied;
    protected Thread listenThread;

    protected ManualResetEvent listenEvent;

    public static string RandomString (int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public Client (String url)
    {
        this.url = url;
        Setup(RandomString(10));
    }

    ~Client ()
    {
        if (connected)
            Disconnect();
    }

    public void Disconnect ()
    {
        // First we set connected to false to prevent any new negotiations
        // while we are trying to disconnect. Then we request to end the
        // connection cleanly. This is a request to send the 'client end'
        // event from the world server which also send it to ourselves to
        // wakeup any errant listener threads
        connected = false;
        Request(req_end);
        listenThread.Join();

        // Finally dispose of any sockets
        world.Dispose();
        interaction.Dispose();
    }

    public void Setup (String id)
    {
        this.id = id;
        this.interaction = new RequestSocket();
        this.world = new SubscribeSocket();
        this.req_register = "REG " + id;
        this.req_access = "ACCESS " + id;
        this.req_end = "END " + id;
        this.acknowledged = id + "-ACK";
        this.denied = id + "-DENIED";
        this.connected = false;
        this.listenThread = null;
        this.listenEvent = new ManualResetEvent(false);
    }

    protected void Request (String data)
    {
        interaction.Send(Encoding.UTF8.GetBytes(data));
        String reply = Response();
        if (reply != acknowledged)
            throw new Exception("Cannot register: " + reply);
    }

    protected string Response ()
    {
        return Encoding.ASCII.GetString(interaction.Receive());
    }

    // Get the next
    protected WorldEvent NextEvent ()
    {
        string data = Encoding.UTF8.GetString(world.Receive());
        Console.WriteLine("received: `{0}`", data);
        string[] e = data.Split(" ".ToCharArray());
        var worldEvent = new WorldEvent();
        worldEvent.type = Event.Default;
        worldEvent.properties = null;

        if (e[0] == id && e[1] == "new") {
            //  This message has the format:
            //      <hash> new
            //         id <id>
            //         <key0> <value0>
            //         <key1> <value1>
            //         ...
            //  (Not including newlines -- formatted for readability)
            //  This is building that dictionary to pass to the script
            //  so it can update from the network.
            Debug.Assert(e.Length >= 4);
            worldEvent.properties = new Dictionary<string,string>();
            for (int i = 2; i < e.Length; i += 2)
                worldEvent.properties[e[i]] = e[i+1];
        }
        return worldEvent;
    }

    public void Connect (EventHandler handler)
    {
        // Request our id from the server.
        interaction.Connect(url + ":8889");
        Request(req_register);

        // Connection must be made before requesting access or otherwise
        // we may miss some key events that have already been sent by the
        // time we connected.
        Console.WriteLine("Accessing...");
        world.Subscribe(id);
        world.Connect(url + ":8888");

        // `Connected` doesn't mean we are actually fully listening. Start
        // the listen thread and then wait for the listening event
        this.connected = true;
        this.listenThread = new Thread(() => ListenProc(handler));
        this.listenThread.Start();
        this.listenEvent.WaitOne();

        // Finally request access from the world server
        Request(req_access);
        Console.WriteLine("Connected with id " + id);
    }

    // While connected listen for events from the World server
    private void ListenProc (EventHandler handler)
    {
        var listener = new NanomsgListener();
        listener.AddSocket(world);
        listener.ReceivedMessage += socketId =>
            {
                // If called while not connected simply do nothing, we just
                // want the listener to wakeup to then see that it isn't
                // connected so it can join the main thread
                if (!connected)
                    return;

                var worldEvent = NextEvent();
                handler(worldEvent.type, worldEvent.properties);
            };
        this.listenEvent.Set();
        // TODO: Put a timeout on the Listen so we can handle a bad 
        // connection
        while (connected)
            listener.Listen(null);
    }
}

