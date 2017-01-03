using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Helper 
{

	public static T FindClosestTarget<T>(Vector3 position, float distance = Mathf.Infinity) where T : MonoBehaviour
	{
		T[] objs = Object.FindObjectsOfType<T>();
		T closest = null;
		float dist = Mathf.Infinity;

		foreach (var obj in objs)
		{
			float delta = Vector3.Distance(position, obj.transform.position);

			if (delta <= distance && delta < dist)
			{
				closest = obj;
				dist = delta;
			}
		}

		return closest;
	}

	public static EnemyControler FindClosestEnemy(Vector3 position, bool isLive, float distance = Mathf.Infinity)
	{
		EnemyControler[] objs = Object.FindObjectsOfType<EnemyControler>();
		EnemyControler closest = null;
		float dist = Mathf.Infinity;

		foreach (var obj in objs)
		{
			if (isLive && obj.Ded) continue;

			float delta = Vector3.Distance(position, obj.transform.position);
			if (delta <= distance && delta < dist)
			{
				closest = obj;
				dist = delta;
			}
		}

		return closest;
	}


	public static IEnumerable GetEnemyInRange(Vector3 position, float range)
	{
		EnemyControler[] objs = Object.FindObjectsOfType<EnemyControler>();
		foreach (EnemyControler enemy in objs)
		{
			if (Vector3.Distance(position, enemy.transform.position) < range)
			{
				yield return enemy;
			}
		}

	}

	public static void PlaySound(AudioSource clip)
	{
		if (clip && !clip.isPlaying)
		{
			clip.Play();
		}
	}
	public static void StopSound(AudioSource clip)
	{
		if (clip && clip.isPlaying)
		{
			clip.Stop();
		}
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = Random.Range(0,list.Count);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

}
