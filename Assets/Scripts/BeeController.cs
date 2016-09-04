using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BeeController : NetworkBehaviour
{
	private Animator animator;
	void Start ()
	{
		animator = GetComponent<Animator>();
		Debug.Assert(animator != null);
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
}
