using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MatchManager : NetworkBehaviour
{
	[SerializeField]
	private float matchTime;
	[SerializeField]
	private GameObject matchOverCanvas;
	[SerializeField]
	private Text scoresText;
	[SerializeField]
	private Text timeLeftText;

	[SerializeField]
	private GameObject inputNameCanvas;
	[SerializeField]
	private GameObject timeLeftCanvas;

	private float timeLeft;
	private bool matchEnded;

	void Start ()
	{
		if (!isServer)
			return;

		timeLeft = matchTime;
		matchEnded = false;
	}
	
	void Update ()
	{
		if (!isServer)
			return;

		if (!matchEnded)
			timeLeft -= Time.deltaTime;
			
		RpcSetTimeLeft(timeLeft);

		if (timeLeft < 0.0f && !matchEnded)
		{
			Debug.Log("Match ended on server!");

			var text = "";
			foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
			{
				var scoreSystem = player.GetComponent<ScoreSystem>();
				var nameComponent = player.GetComponent<Name>();
				Debug.Assert(scoreSystem != null);
				Debug.Assert(nameComponent != null);

				text += nameComponent.GetName() + "\t" + scoreSystem.GetScore() + "\n";
			}

			RpcOnMatchEnded(text);
			matchEnded = true;
			timeLeft = 0.0f;
		}
	}

	[ClientRpc]
	void RpcOnMatchEnded(string text)
	{
		Debug.Log("Match ended on client!");

		scoresText.text = text;
		matchOverCanvas.SetActive(true);
		matchEnded = true;
	}

	[ClientRpc]
	void RpcSetTimeLeft(float timeLeft)
	{
		timeLeftText.text = Mathf.CeilToInt(timeLeft).ToString();
	}

	public bool MatchEnded()
	{
		return matchEnded;
	}

	public void OnMatchStart()
	{
		inputNameCanvas.SetActive(false);
		timeLeftCanvas.SetActive(true);
	}
}
