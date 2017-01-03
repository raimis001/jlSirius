using UnityEngine;
using System.Collections;

public class EnemyControler : ActorAbstract
{
	public enum NpcType
	{
		Cover = 0,
		Laying = 1,
		Standing = 2
	}


	[Header("Shot")]
	[Range(0, 5)]
	public float shotSpeed;
	public float shotHp;
	public float shotDistance;

	[Header("Start animation")]
	public Animator animator;
	public NpcType type;

	public GameObject particles;

	internal float Lives = 1;
	internal bool Ded;

	private float shot;

	void Start()
	{
		shot = shotSpeed;
		animator.SetInteger("Type",(int)type);
	}

	public void LateUpdate()
	{
		if (Ded) return;

		if (Lives <= 0)
		{
			DoDed();
			return;
		}

		if (PlayerControler.Distance(transform.position) > shotDistance)
		{
			particles.SetActive(false);
			animator.SetFloat("Shot",0);
			return;
		}

		if (shot > 0)
		{
			animator.SetFloat("Shot", 1);
			particles.SetActive(true);
			shot -= Time.deltaTime;
			if (shot <= 0)
			{
				//TODO: shot player
				ParticleSystem ps = particles.GetComponentInChildren<ParticleSystem>();
				if (ps && ps.isStopped) ps.Play();
				PlayerControler.Life -= shotHp;
				shot = shotSpeed;
			}
		}

	}

	public void Hit(float hitDamage)
	{
		if (Ded) return;
		Lives -= hitDamage;
	}

	public void DoDed()
	{
		Ded = true;
		animator.SetInteger("Ded",Random.Range(1,3));
		animator.SetTrigger("Doded");
		Helper.StopSound(GetComponent<AudioSource>());
		particles.SetActive(false);
	}

	public override void EnterZone()
	{
		
	}

	public override void ActivateZone()
	{
		
	}

	public override void Restart()
	{
		Ded = false;
		Lives = 1;
		animator.SetTrigger("Restart");
	}
}
