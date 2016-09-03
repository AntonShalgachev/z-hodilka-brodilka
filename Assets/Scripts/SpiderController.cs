using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpiderController : NetworkBehaviour
{
	[SerializeField]
	private float velocity;
	[SerializeField]
	private float angularVelocity;

	[SerializeField]
	private float eps = 0.01f;
	[SerializeField]
	private float epsAng = 1.0f;

	private Vector2 target;

	private Rigidbody2D body;
	private Animator animator;

	private Rect terrainRect;

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		//var camera = GameObject.FindGameObjectWithTag("MainCamera");
		var camera = Camera.main;

		var follower = camera.GetComponent<Follower>();
		Debug.Assert(follower != null, "Camera has to have Follower component");

		follower.subject = gameObject;

		target = transform.position;

		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();

		var terrainObject = GameObject.Find("Terrain");
		Debug.Assert(terrainObject != null, "Can't find Terrain object");
		var terrain = terrainObject.GetComponent<TerrainGenerator>();
		Debug.Assert(terrain != null, "Terrain doesn't contain TerrainGenerator");

		terrainRect = terrain.GetRect();
	}

	void Start ()
	{

	}
	
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (Input.GetMouseButtonUp(0))
		{
			var clickPosition = Input.mousePosition;
			var worldPosition = (Vector2)Camera.main.ScreenToWorldPoint(clickPosition);

			if (terrainRect.Contains(worldPosition))
			{
				target = worldPosition;
				animator.SetTrigger("StartMoving");
			}
		}
	}

	void FixedUpdate()
	{
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
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, 0.0f, targetAngle), angularVelocity * Time.deltaTime);

			if (Mathf.Abs(dangle) < epsAng)
			{
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);

				transform.position = Vector2.MoveTowards(transform.position, target, velocity * Time.deltaTime);

				if ((target - (Vector2)transform.position).magnitude < eps)
				{
					transform.position = target;
					animator.SetTrigger("StopMoving");
				}
			}
		}
	}
}
