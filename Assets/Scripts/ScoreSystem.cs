using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ScoreSystem : NetworkBehaviour
{
	[SyncVar]
	private int score;

	void Start ()
	{
		if (!isServer)
			return;

		score = 0;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!isServer)
			return;

		score++;

		var tag = GetComponent<Name>().GetName();
		Debug.Log(tag + " has now " + score + " points");
	}

	public int GetScore()
	{
		return score;
	}
}
