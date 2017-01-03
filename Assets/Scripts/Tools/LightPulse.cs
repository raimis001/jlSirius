using UnityEngine;
using System.Collections;

public class LightPulse : MonoBehaviour
{
	public float maxIntensity;
	public float minIntensity;

	public float speed = 3;

	internal bool active = true;

	private Light Light;
	private int sign = 1;
	private float randomTime = 1;


	void Start()
	{
		Light = GetComponent<Light>();
		randomTime = Random.Range(0.8f, 1.2f);
	}

	void Update()
	{
		if (!active)
		{
			Light.intensity = minIntensity;
			return;
		}

		Light.intensity += sign*Time.deltaTime * randomTime * speed;
		if (sign > 0 && Light.intensity >= maxIntensity)
		{
			sign = -1;
			randomTime = Random.Range(0.8f, 1.2f);
		}

		if (sign < 0 && Light.intensity <= minIntensity)
		{
			sign = 1;
			randomTime = Random.Range(0.8f, 1.2f);
		}
	}
}
