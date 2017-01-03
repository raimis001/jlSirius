using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLine : MonoBehaviour
{

	public Transform Position;
	public Vector3 Offset;
	public Vector3 Target;

	[Range(0,0.5f)]
	public float speedShot = 0.1f;
	[Range(0, 0.5f)]
	public float speedWait = 0.05f;

	private LineRenderer Line;

	private bool _auto;

	void Start ()
	{
		Line = GetComponent<LineRenderer>();
	}

	public Vector3 RandomTarget()
	{
		return new Vector3(Random.Range(-10,10),0,40);
	}


	public void StartAuto()
	{
		if (_auto) return;
		StartCoroutine(ShotAuto());
	}

	public void StopAuto()
	{
		_auto = false;
	}


	IEnumerator ShotAuto()
	{
		if (_auto) yield break;

		_auto = true;

		while (_auto)
		{
			Line.SetPosition(0, Position.position + Offset);
			Line.SetPosition(1, Target + Vector3.up * 2);
			Line.enabled = true;

			yield return new WaitForSeconds(speedShot);

			Line.enabled = false;
			yield return new WaitForSeconds(speedWait);

		}
	}

	public void OneShot()
	{
		StartCoroutine(ShotOne());
	}

	IEnumerator ShotOne()
	{
		_auto = false;
		yield return null;

		Line.SetPosition(0, Position.position + Offset);
		Line.SetPosition(1, Target);
		Line.enabled = true;

		yield return new WaitForSeconds(speedShot * 2);
		Line.enabled = false;

	}
}
