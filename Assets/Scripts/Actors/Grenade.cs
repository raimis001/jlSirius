using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour
{

	public Vector3 force = new Vector3(0, 350, 600);
	public Transform playerTransform;
	public GameObject picture;

	private Vector3 startPosition;

	public GameObject explosion;

	private Rigidbody rig;

	private bool inUse;

	// Use this for initialization
	void Awake()
	{
		startPosition = transform.localPosition;
		gameObject.SetActive(false);
	}
	void Start()
	{
		rig = GetComponent<Rigidbody>();
	}

	public void Throw()
	{
		if (inUse) return;

		inUse = true;

		explosion.SetActive(false);
		gameObject.SetActive(true);

		Invoke("Throwing", 0.01f);
	}

	void Throwing()
	{
		transform.SetParent(PlayerControler.Instance.transform);
		transform.localPosition = startPosition;

		picture.SetActive(true);

		rig.AddForce(force);
		rig.AddTorque(Vector3.right * 5);
		rig.AddTorque(Vector3.up * 1);


		transform.SetParent(null);

		StartCoroutine(TimeOut());

	}

	public void OnCollisionEnter(Collision collision)
	{
		//Debug.Log(collision.gameObject.layer);
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{

			rig.velocity = Vector3.zero;
			rig.angularVelocity = Vector3.zero;

			picture.SetActive(false);

			transform.SetParent(collision.transform);
			transform.localEulerAngles = Vector3.zero;


			StartCoroutine(WaitForEnd());
		}
	}

	IEnumerator WaitForEnd()
	{

		explosion.SetActive(true);
		DoExplosion();

		yield return new WaitForSeconds(2f);

		EndGranade();
	}

	IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(5.0f);
		if (!inUse) yield break;
		EndGranade();
	}

	private void EndGranade()
	{
		explosion.SetActive(false);
		picture.SetActive(false);
		gameObject.SetActive(false);

		transform.SetParent(playerTransform);
		transform.localPosition = startPosition;

		inUse = false;

	}

	public void OnTriggerEnter(Collider other)
	{
		ActorAbstract actor = other.GetComponent<ActorAbstract>();
		if (actor) actor.ActivateZone();
	}

	private void DoExplosion()
	{
		foreach (EnemyControler enemy in Helper.GetEnemyInRange(transform.position, 30))
		{
			enemy.Hit(1);
		}
	}
}
