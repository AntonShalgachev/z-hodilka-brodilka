using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Name))]
public class DisplayName : MonoBehaviour
{
	[SerializeField]
	private Text nameText;
	private Name nameComponent;

	void Start ()
	{
		nameComponent = GetComponent<Name>();
	}
	
	void Update ()
	{
		nameText.text = nameComponent.GetName();
	}
}
