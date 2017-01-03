using UnityEngine;
using System.Collections;

public class TerrainBlock : MonoBehaviour
{


	public void Restart()
	{
		ActorAbstract[] actors = GetComponentsInChildren<ActorAbstract>();
		foreach (ActorAbstract actor in actors)
		{
			actor.Restart();
		}
	}

}
