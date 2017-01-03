using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
	[Range(0, 50)]
	public float speed = 10;

	public MovingBlock[] blocks;

	public TerrainBlock[] prefabControl;

	private List<TerrainBlock> activeBlocks = new List<TerrainBlock>();
	  
	void Awake()
	{
		foreach (TerrainBlock block in prefabControl)
		{
			activeBlocks.Add(block);
		}

		foreach (MovingBlock block in blocks)
		{
			block.terrain = GetRandomBlock();
		}

	}

	void Update()
	{
		foreach (MovingBlock block in blocks)
		{
			block.transform.Translate(0,0,-Time.deltaTime * speed);
			if (block.transform.localPosition.z <= -70)
			{
				float delta = block.transform.localPosition.z + 50;
				block.transform.localPosition = new Vector3(0,0,(blocks.Length - 1) * 50  + delta);

				if (block.terrain) activeBlocks.Add(block.terrain);
				block.terrain = GetRandomBlock();

			}
		}
	}

	TerrainBlock GetRandomBlock()
	{
		activeBlocks.Shuffle();
		TerrainBlock block = activeBlocks[0];
		activeBlocks.RemoveAt(0);

		return block;

	}
}
