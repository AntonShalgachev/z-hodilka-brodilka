using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
	public GameObject subject
	{
		get; set;
	}

	void Start ()
	{
		
	}
	
	void Update ()
	{
		if (subject != null)
		{
			var position = subject.transform.position;
			transform.position = new Vector3(position.x, position.y, transform.position.z);
		}
	}
}
