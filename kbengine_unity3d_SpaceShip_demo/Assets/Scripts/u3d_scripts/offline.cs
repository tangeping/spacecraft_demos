using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offline :MonoBehaviour
{

    private bool startRelogin = false;
    public int ui_state = 0;

    public  void installEvents()
    {
        // common
        KBEngine.Event.registerOut("onDisconnected", this, "onDisconnected");
        KBEngine.Event.registerOut("onConnectionState", this, "onConnectionState");
        KBEngine.Event.registerIn("onConnectionState", this, "onConnectionState");
        KBEngine.Event.registerOut("onKicked", this, "onKicked");
        KBEngine.Event.registerIn("onKicked", this, "onKicked");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
