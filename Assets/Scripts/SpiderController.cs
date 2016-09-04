using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Name), typeof(Animator))]
public class SpiderController : NetworkBehaviour
{
	[SerializeField]
	private float velocity;
	[SerializeField]
	private float angularVelocity;

	[SerializeField]
	private float eps;
	[SerializeField]
	private float epsAng;

	private Vector2 target;
	private Animator animator;
	private Rect terrainRect;
	private MatchManager matchManager;

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		var camera = Camera.main;
		var follower = camera.GetComponent<Follower>();
		Debug.Assert(follower != null, "Camera has to have Follower component");

		var terrainObject = GameObject.Find("Terrain");
		Debug.Assert(terrainObject != null, "Can't find Terrain object");
		var terrain = terrainObject.GetComponent<TerrainGenerator>();
		Debug.Assert(terrain != null, "Terrain doesn't contain TerrainGenerator");

		follower.subject = gameObject;
		target = transform.position;

		terrainRect = terrain.GetRect();

		var nameObject = GameObject.FindGameObjectWithTag("PlayerName") as GameObject;
		Debug.Assert(nameObject != null);
		var nameText = nameObject.GetComponent<Text>();
		Debug.Assert(nameText != null);

		if (nameText.text != "")
			CmdSetName(nameText.text);
		else
			CmdSetName("Unknown Player");

		matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
		matchManager.OnMatchStart();
	}

	[Command]
	void CmdSetName(string name)
	{
		GetComponent<Name>().SetName(name);
	}

	void Start ()
	{
		animator = GetComponent<Animator>();
	}
	
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (matchManager.MatchEnded())
			return;

		if (Input.GetMouseButtonUp(0))
		{
			var clickPosition = Input.mousePosition;
			var worldPosition = (Vector2)Camera.main.ScreenToWorldPoint(clickPosition);

			if (terrainRect.Contains(worldPosition))
			{
				target = worldPosition;
				CmdSetMoving(true);
			}
		}
	}

	void FixedUpdate()
	{
		if (!isLocalPlayer)
			return;

		var direction = target - (Vector2)transform.position;

		if (direction.magnitude > eps)
		{
			var forward = transform.up;
			var dangle = Vector2.Angle(forward, direction);
			var z = Vector3.Cross(forward, direction).z;
			if (z < 0.0f)
				dangle *= -1.0f;

			var currentAngle = transform.rotation.eulerAngles.z;
			var targetAngle = currentAngle + dangle;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, 0.0f, targetAngle), angularVelocity * Time.fixedDeltaTime);

			if (Mathf.Abs(dangle) < epsAng)
			{
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);

				transform.position = Vector2.MoveTowards(transform.position, target, velocity * Time.fixedDeltaTime);

				if ((target - (Vector2)transform.position).magnitude < eps)
				{
					transform.position = target;
					CmdSetMoving(false);
				}
			}
		}
	}

	[Command]
	void CmdSetMoving(bool moving)
	{
		RpcSetMoving(moving);
	}

	[ClientRpc]
	void RpcSetMoving(bool moving)
	{
		animator.SetBool("Moving", moving);
	}
}
