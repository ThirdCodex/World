using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientScriptEngine;

public class Movement : MonoBehaviour
{
    private Lua L;
    private Entity E;

    void Awake ()
    {
        E = new Entity(gameObject);
        L = new Lua(Debug.Log);
        L.Init(E);
    }

    // Start is called before the first frame update
    void Start ()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        L.Step(E, Time.deltaTime);
    }
}
