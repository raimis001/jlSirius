using UnityEngine;
using System.Collections;

public class AmmoCounter : MonoBehaviour
{

	public int ammoCount;
	public int ammoMax;

	public float shotDistance;
	public float shotHp;

	public float coolDownTime;
	private float coolDown;

	// Update is called once per frame
	void Update()
	{
		int i = 0;
		float p = ((float) ammoCount/ammoMax);
		int c = (int) (p*transform.childCount);

		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(i < c);
			i++;
		}

		if (coolDown > 0)
		{
			coolDown -= Time.deltaTime;
		}

	}

	public bool CanShot()
	{
		return ammoCount > 0 && coolDown <= 0;
	}

	public void Shot()
	{
		ammoCount--;
		coolDown = coolDownTime;
	}
}
