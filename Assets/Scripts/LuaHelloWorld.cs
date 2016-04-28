using UnityEngine;
using System.Collections;
using LuaInterface;

public class LuaHelloWorld : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LuaState l = new LuaState();
        string path = Application.dataPath + "/Resources/lua/HelloWorldLua.lua";
        l.DoFile(path);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
