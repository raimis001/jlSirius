using UnityEngine;
using System.Collections;
using System;

public class SniperScope : Scope
{
	[Range(0,150)]
	public float speed;

	private CanvasGroup group;

	void Awake()
	{
		group = GetComponent<CanvasGroup>();
		
	}
	void Update()
	{
		transform.position = Input.mousePosition;
		if (active )
		{
			if (Camera.main.fieldOfView > 35)
			{
				Camera.main.fieldOfView -= Time.deltaTime*speed;
			}
			if (group.alpha < 1)
			{
				group.alpha += Time.deltaTime *2;
			}
		}

	}

	internal override void SetActive(bool active)
	{
		if (!active)
		{
			Camera.main.fieldOfView = 60;
		}
		else
		{
			GetComponent<CanvasGroup>().alpha = 0;
		}
		base.SetActive(active);
	}
}
