using UnityEngine;
using System.Collections;

public class RpgBomb : MonoBehaviour
{

	public GameObject explosion;
	public GameObject picture;

	internal Transform target;

	private Vector3 startPosition;

	void Awake()
	{
		startPosition = transform.localPosition;
		gameObject.SetActive(false);
	}

	public void Shot(Transform t)
	{
		target = t;

		transform.localPosition = startPosition;

		explosion.SetActive(false);
		picture.SetActive(true);

		gameObject.SetActive(true);

		StartCoroutine(Timeout());
	}

	void Update()
	{
		Vector3 targetPos = target ? target.position : new Vector3(0, 0, 1000);
		Vector3 t = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 150f);
		transform.position = t;
	}

	public void OnCollisionEnter(Collision collision)
	{
		Destroy(collision.gameObject);
		StartCoroutine(WaitForEnd());
	}

	IEnumerator Timeout()
	{
		yield return new WaitForSeconds(10);
		explosion.SetActive(false);
		picture.SetActive(false);
		gameObject.SetActive(false);
		target = null;
	}

	IEnumerator WaitForEnd()
	{
		target = null;

		explosion.SetActive(true);
		picture.SetActive(false);

		yield return new WaitForSeconds(2);

		explosion.SetActive(false);
		transform.localPosition = startPosition;

	}
}
