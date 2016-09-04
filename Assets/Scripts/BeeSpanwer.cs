using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BeeSpanwer : NetworkBehaviour
{
	[SerializeField]
	private GameObject beePrefab;
	[SerializeField]
	private int numberOfBees;

	[SerializeField]
	private TerrainGenerator terrain;

	public override void OnStartServer()
	{
		base.OnStartServer();

		//var terrainObject = GameObject.Find("Terrain");
		//Debug.Assert(terrainObject != null, "Can't find Terrain object");
		//var terrain = terrainObject.GetComponent<TerrainGenerator>();
		//Debug.Assert(terrain != null, "Terrain doesn't contain TerrainGenerator");

		var terrainRect = terrain.GetRect();
		
		var xfrom = terrainRect.xMin;
		var xto = terrainRect.xMax;
		var yfrom = terrainRect.yMin;
		var yto = terrainRect.yMax;

		for (var i = 0; i < numberOfBees; i++)
		{
			var position = new Vector2(Random.Range(xfrom, xto), Random.Range(yfrom, yto));
			var bee = Object.Instantiate(beePrefab, position, Quaternion.identity) as GameObject;
			NetworkServer.Spawn(bee);

			bee.transform.parent = transform;
		}
	}
}
