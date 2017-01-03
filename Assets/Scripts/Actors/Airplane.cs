using UnityEngine;
using System.Collections;

public class Airplane : MonoBehaviour
{

	[Range(-10, 10)]
	public float speed;

	void Update()
	{
		transform.Translate(Vector3.back * speed);
	}
}
