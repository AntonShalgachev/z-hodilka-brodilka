using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpawnGenerator : NetworkBehaviour
{
	[SerializeField]
	private GameObject spawnPrefab;
	[SerializeField]
	private int numberOfSpawns;

	[SerializeField]
	private TerrainGenerator terrain;

	public override void OnStartServer()
	{
		base.OnStartServer();

		var terrainRect = terrain.GetRect();

		var xfrom = terrainRect.xMin;
		var xto = terrainRect.xMax;
		var yfrom = terrainRect.yMin;
		var yto = terrainRect.yMax;

		for (var i = 0; i < numberOfSpawns; i++)
		{
			var position = new Vector2(Random.Range(xfrom, xto), Random.Range(yfrom, yto));
			var spawnPos = Object.Instantiate(spawnPrefab, position, Quaternion.identity) as GameObject;
			spawnPos.transform.parent = transform;
		}
	}
}
