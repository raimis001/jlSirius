using UnityEngine;
using System.Collections;

public class RpgScope : Scope
{

	public Transform leftCross;
	public Transform rightCross;

	internal Transform target;

	void Update()
	{
		if (!target)
		{
			GetComponent<RectTransform>().anchoredPosition = Vector2.down*100;
		}
		else
		{
			Vector2 t = Camera.main.WorldToScreenPoint(target.position);
			transform.position = Vector2.MoveTowards(transform.position, t, 7);

			float dist = (Vector3.Distance(PlayerControler.Position, target.position) - 20) / 680;
			if (dist > 1) dist = 1;
			dist = (1f - dist) * 370 + 80;
			//80 = 1; 450 = 0
			rightCross.GetComponent<RectTransform>().anchoredPosition = new Vector2(dist,0);
			leftCross.GetComponent<RectTransform>().anchoredPosition = new Vector2(-dist, 0);
			//Debug.Log(dist);
		}
	}

	
}
