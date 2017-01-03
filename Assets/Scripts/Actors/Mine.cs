using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour
{

	[Header("Light")]
	public new Light light;
	public float maxIntensity;
	public float minIntensity;
	public float speed = 3;

	[Header("Objects")]
	public GameObject explosion;
	public GameObject graphics;

	internal bool active = true;

	private int sign = 1;
	private float randomTime = 1;


	void Start()
	{
		randomTime = Random.Range(0.7f, 1.3f);
	}

	// Update is called once per frame
	void Update()
	{
		if (!active)
		{
			light.intensity = 0;
			return;
		}

		light.intensity += sign * Time.deltaTime * randomTime * speed;

		if (sign > 0 && light.intensity >= maxIntensity)
		{
			sign = -1;
			randomTime = Random.Range(0.8f, 1.2f);
		}

		if (sign < 0 && light.intensity <= minIntensity)
		{
			sign = 1;
			randomTime = Random.Range(0.8f, 1.2f);
		}
	}

	public void Explode()
	{
		active = false;
		explosion.SetActive(true);
		graphics.SetActive(false);
	}

	public void Restart()
	{
		active = true;
		explosion.SetActive(false);
		graphics.SetActive(true);
	}

}
