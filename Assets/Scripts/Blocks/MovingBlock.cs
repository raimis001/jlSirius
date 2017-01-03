using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
	public Transform terrainBlock;

	private TerrainBlock _terrain;

	public TerrainBlock terrain
	{
		get { return _terrain; }
		set
		{
			if (_terrain)
			{
				_terrain.transform.SetParent(null);
				_terrain.transform.position = new Vector3(0, 0, -1000);
				_terrain.gameObject.SetActive(false);

			}

			_terrain = value;

			if (!_terrain) return;

			_terrain.transform.SetParent(terrainBlock);
			_terrain.transform.localPosition = Vector3.zero;
			_terrain.gameObject.SetActive(true);
			_terrain.Restart();
		}
	}

}
