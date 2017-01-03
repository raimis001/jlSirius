using UnityEngine;
using System.Collections;

public class Scope : MonoBehaviour
{
	public bool active {
		get { return gameObject.activeSelf; }
	}

	virtual internal void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}


}
