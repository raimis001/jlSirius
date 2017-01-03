using UnityEngine;
using System.Collections;

public class AutoScope : Scope
{
	void Update()
	{
		transform.position = Input.mousePosition;
	}

}
