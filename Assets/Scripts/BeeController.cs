using UnityEngine;
using System.Collections;

public class BeeController : MonoBehaviour
{
	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	void OnAnimationEnd()
	{
		Debug.Log("Bee got slaughtered");
		Destroy(gameObject);
	}
}
