using System;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    void Awake ()
    {
        //client = new Client("tcp://127.0.0.1");
    }

    void OnDestroy ()
    {
        //client.Disconnect();
    }

    // Start is called before the first frame update
    void Start ()
    {
        //client.Connect(Handler);
    }

    // Update is called once per frame
    void Update ()
    {
    }
}
