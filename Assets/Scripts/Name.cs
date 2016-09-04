using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Name : NetworkBehaviour
{
	[SyncVar]
	private string playerName;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	public void SetName(string name)
	{
		Debug.Log("Call to SetName(\"" +name + "\")");
		Debug.Log("isServer = " + isServer);

		playerName = name;
	}

	public string GetName()
	{
		return playerName;
	}
}
