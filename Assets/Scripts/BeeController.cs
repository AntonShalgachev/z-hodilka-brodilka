using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BeeController : NetworkBehaviour
{
	[SerializeField]
	private float minSpeed;
	[SerializeField]
	private float maxSpeed;
	[SerializeField]
	private float minSpeedDist;
	[SerializeField]
	private float maxSpeedDist;

	private Animator animator;

	void Start ()
	{
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (!isServer)
			return;

		var minDist = Mathf.Infinity;

		foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
		{
			var dist = (player.transform.position - transform.position).magnitude;

			minDist = Mathf.Min(minDist, dist);
		}

		var t = Mathf.InverseLerp(minSpeedDist, maxSpeedDist, minDist);
		var speed = Mathf.Lerp(minSpeed, maxSpeed, t);
		RpcSetSpeed(speed);
	}

	void OnAnimationEnd()
	{
		if (isServer)
			Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!isServer)
			return;

		RpcTriggerDeathAnimation();
	}

	[ClientRpc]
	void RpcTriggerDeathAnimation()
	{
		animator.SetTrigger("Die");
	}

	[ClientRpc]
	void RpcSetSpeed(float speed)
	{
		animator.SetFloat("Speed", speed);
	}
}
