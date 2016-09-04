using UnityEngine;
using System.Collections;

public class DesignScaler : MonoBehaviour
{
	[SerializeField]
	private int designWidth;
	[SerializeField]
	private int designHeight;

	void Start ()
	{
		var rx = (float)Screen.width / designWidth;
		var ry = (float)Screen.height / designHeight;

		GetComponent<RectTransform>().localScale = new Vector3(rx, ry, 1.0f);
	}
	
	void Update ()
	{
		
	}
}
