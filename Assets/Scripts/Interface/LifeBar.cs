using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour
{

	public Gradient lifeColors;

	private Material material;
	void Start()
	{
		material = GetComponent<MeshRenderer>().material;
	}

	void Update()
	{
		material.color = lifeColors.Evaluate(PlayerControler.Life/100);
	}
}
