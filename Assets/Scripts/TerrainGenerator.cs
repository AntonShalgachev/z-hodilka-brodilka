using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour
{
	[SerializeField]
	private GameObject groundPrefab;
	[SerializeField]
	private GameObject grassPrefab;

	[SerializeField]
	private int terrainColumns;
	[SerializeField]
	private int terrainRows;
	[SerializeField]
	private int grassBufferSize;

	private float tileWidth;
	private float tileHeight;

	private Vector2 terrainSize;

	void Start ()
	{
		GenerateTerrain();
		GenerateBorder();
	}
	
	void Update ()
	{
		
	}

	void GenerateTerrain()
	{
		tileWidth = groundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
		tileHeight = groundPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
		var tileSize = new Vector2(tileWidth, tileHeight);

		var fullTerrainColumns = terrainColumns + 2 * grassBufferSize;
		var fullTerrainRows = terrainRows + 2 * grassBufferSize;

		var terrainWidth = tileWidth * fullTerrainColumns;
		var terrainHeight = tileHeight * fullTerrainRows;
		terrainSize = new Vector2(terrainWidth, terrainHeight);

		var lowerLeft = -0.5f * terrainSize + 0.5f * tileSize;

		for (var col = 0; col < fullTerrainColumns; col++)
		{
			for (var row = 0; row < fullTerrainRows; row++)
			{
				var tilePosition = lowerLeft + new Vector2(col * tileWidth, row * tileHeight);

				GameObject prefab;
				if (col >= grassBufferSize && row >= grassBufferSize && col < fullTerrainColumns - grassBufferSize && row < fullTerrainRows - grassBufferSize)
					prefab = groundPrefab;
				else
					prefab = grassPrefab;

				var tile = Object.Instantiate(prefab, tilePosition, Quaternion.identity) as GameObject;
				tile.transform.parent = transform;
			}
		}
	}

	void GenerateBorder()
	{
		var halfWidth = 0.5f * tileWidth * terrainColumns;
		var halfHeight = 0.5f * tileHeight * terrainRows;

		// Corner points
		var ll = new Vector2(-halfWidth, -halfHeight);
		var lr = new Vector2(halfWidth, -halfHeight);
		var ur = new Vector2(halfWidth, halfHeight);
		var ul = new Vector2(-halfWidth, halfHeight);

		var edges = new Vector2[4][] { new Vector2[2] {ll, ul}, new Vector2[2] { ul, ur}, new Vector2[2] { ur, lr}, new Vector2[2] { ll, lr} };

		foreach (var edge in edges)
		{
			var collider = gameObject.AddComponent<EdgeCollider2D>();
			collider.points = edge;
		}

		gameObject.layer = LayerMask.NameToLayer("Border");
	}

	public Rect GetRect()
	{
		var width = tileWidth * terrainColumns;
		var height = tileHeight * terrainRows;
		var size = new Vector2(width, height);

		return new Rect(-0.5f * size, size);
	}
}
