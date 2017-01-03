using UnityEngine;
using System.Collections;

public class ActorMines : ActorAbstract
{

	private AudioSource Sound;

	private float beepTime;

	void Start()
	{
		Sound = GetComponent<AudioSource>();
		beepTime = 1 + Random.value;
	}

	public void Update()
	{
		if (!active) return;


		beepTime -= Time.deltaTime;
		if (beepTime <= 0)
		{
			Helper.PlaySound(Sound);
			beepTime = 0.7f + Random.value;
		}
	}

	public override void EnterZone()
	{
		Debug.Log("Enter ded zone");
		PlayerControler.Life = 0;
		ActivateZone();
	}

	public override void ActivateZone()
	{
		Collider collider = GetComponent<Collider>();
		if (collider) collider.enabled = false;
		StartCoroutine(Explode());
	}

	public override void Restart()
	{
		Mine[] mines = GetComponentsInChildren<Mine>();
		foreach (Mine mine in mines)
		{
			mine.Restart();
		}
		Collider collider = GetComponent<Collider>();
		if (collider) collider.enabled = true;
		active = true;
	}

	IEnumerator Explode()
	{
		Mine[] mines = GetComponentsInChildren<Mine>();
		foreach (Mine mine in mines)
		{
			mine.Explode();
			yield return new WaitForSeconds(0.1f);
		}
	}
}
