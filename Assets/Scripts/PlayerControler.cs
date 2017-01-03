using System;
using UnityEngine;using System.Collections;

public class PlayerControler : MonoBehaviour
{
	private enum WeaponType
	{
		Auto,
		Sniper,
		Rpg
	}

	public static float Life = 100;

	public BlockControl Main;
	public Transform ground;

	[Range(0, 300)]
	public float jumpSpeed;

	public Animator Animator;

	public CameraManager MainCamera;

	[Header("Weapons")]
	public GameObject weaponAuto;
	public GameObject weaponSniper;
	public GameObject weaponRpg;
	public Grenade grenade;

	[Header("Ammo")]
	public AmmoCounter autoAmmo;
	public AmmoCounter sniperAmmo;
	public AmmoCounter rpgAmmo;
	public AmmoCounter grenadeAmmo;

	[Header("GUI")]
	public SniperScope sniperScope;
	public RpgScope rpgScope;
	public AutoScope autoScope;

	[Header("Effects")]
	public ShotLine shotLine;
	public ParticleSystem ShotingParticle;
	public RpgBomb rpgBomb;

	[Header("Sounds")]
	public AudioSource soundSniper;
	public AudioSource soundAuto;

	internal bool ded;

	private WeaponType weaponType = WeaponType.Auto;

	private float targetX;
	private Collider bodyCollider;
	private bool crawling;
	private Rigidbody Rigidbody;

	public static PlayerControler Instance;

	public static Vector3 Position
	{
		get
		{
			return Instance.transform.position;
		}
	}

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		Rigidbody = GetComponent<Rigidbody>();
		bodyCollider = GetComponent<Collider>();
		sniperScope.SetActive(false);
	}

	// Update is called once per frame
	private void Update()
	{
		if (ded) return;

		if (Life <= 0)
		{
			DoDed();
			return;
		}

		bool grounded = IsGrounded();

		#region JUMP

		if (Input.GetKeyDown(KeyCode.W) && grounded)
		{
			Rigidbody.AddForce(new Vector3(0, jumpSpeed*3, 0));
			Animator.SetTrigger("Jump");
		}

		#endregion

		#region STRAFE

		if (grounded)
		{
			targetX += Input.GetKeyDown(KeyCode.D) ? 10 : Input.GetKeyDown(KeyCode.A) ? -10 : 0;
			targetX = Mathf.Clamp(targetX, -10, 10);
		}
		Animator.SetFloat("Grounded", grounded ? 0 : 1);

		float lx = transform.localPosition.x;

		if (Mathf.Abs(targetX - lx) > 1f)
		{
			float delta = Mathf.Sign(targetX - lx);
			transform.Translate(delta*Time.deltaTime*20, 0, 0);
		}
		else
		{
			transform.localPosition = new Vector3(targetX, transform.localPosition.y, transform.localPosition.z);
		}

		#endregion

		#region CRAWL

		if (Input.GetKeyDown(KeyCode.S))
		{
			Animator.SetTrigger("Crawl");
			((CapsuleCollider) bodyCollider).height = 1;
			((CapsuleCollider) bodyCollider).center = new Vector3(0, 0.5f, 0);
			crawling = true;
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			((CapsuleCollider) bodyCollider).height = 3;
			((CapsuleCollider) bodyCollider).center = new Vector3(0, 1.3f, 0);
			Animator.SetTrigger("Stand");
			crawling = false;
		}

		#endregion

		if (!grounded || crawling)
		{
			autoScope.SetActive(false);
			return;
		}

		#region SNIPER

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			sniperScope.SetActive(true);
			autoScope.SetActive(false);
		}
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			sniperScope.SetActive(false);
		}
		#region SNIPER SHOT

		if (sniperScope.active)
		{
			SwitchWeapon(WeaponType.Sniper);
			if (Input.GetMouseButtonDown(0) && sniperAmmo.CanShot())
			{
				shotLine.Target = shotLine.RandomTarget();

				Vector3 target;
				EnemyControler enemy = RayCastEnemy(20, sniperAmmo.shotDistance, out target);
				if (enemy)
				{
					enemy.Hit(sniperAmmo.shotHp);
					shotLine.Target = target + new Vector3(transform.position.x, 0, 0);
				}
				else
				{
					shotLine.Target = shotLine.RandomTarget();
				}

				sniperAmmo.Shot();
				shotLine.OneShot();
				Animator.SetFloat("Shoting", 1);
				Helper.PlaySound(soundSniper);
			}
			if (sniperAmmo.CanShot())
			{
				Animator.SetFloat("Shoting", 0);
			}
			return;
		}
		#endregion
		#endregion

		#region GRANADE
		if (Input.GetMouseButtonDown(1) && grenadeAmmo.CanShot())
		{
			grenade.Throw();
			grenadeAmmo.Shot();
			return;
		}
		#endregion

		#region RPG
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			rpgScope.SetActive(true);
			autoScope.SetActive(false);

			Animator.SetFloat("Shoting", 1);
			SwitchWeapon(WeaponType.Rpg);

		}
		if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			rpgScope.SetActive(false);

			SwitchWeapon(WeaponType.Auto);
		}
		#region RPG SHOT
		if (rpgScope.active)
		{
			rpgScope.target = null;
			Airplane plane = Helper.FindClosestTarget<Airplane>(transform.position, 600);
			if (plane && plane.transform.position.z > 20)
			{
				float dist = Vector3.Distance(transform.position, plane.transform.position);
				if (dist > 20)
				{
					rpgScope.target = plane.transform;
				}
			}

			if (Input.GetMouseButtonDown(0) && rpgAmmo.CanShot())
			{
				rpgBomb.Shot(plane ? plane.transform : null);
				rpgAmmo.Shot();
			}

			return;
		}
		#endregion
		#endregion

		#region AUTO SHOT
		autoScope.SetActive(true);
		SwitchWeapon(WeaponType.Auto);

		if (Input.GetMouseButton(0))
		{
			Animator.SetFloat("Shoting", 1);
			if (ShotingParticle.isStopped) ShotingParticle.Play();
			Helper.PlaySound(soundAuto);

			if (autoAmmo.CanShot())
			{
				Vector3 target;
				EnemyControler enemy = RayCastEnemy(5, autoAmmo.shotDistance, out target);
				if (enemy)
				{
					enemy.Hit(autoAmmo.shotHp);
					shotLine.Target = target + new Vector3(transform.position.x, 0, 0);
				}
				else
				{
					shotLine.Target = shotLine.RandomTarget();
				}
				autoAmmo.Shot();
				shotLine.StartAuto();
			}
		}
		else
		{
			Animator.SetFloat("Shoting", 0);
			if (ShotingParticle.isPlaying) ShotingParticle.Stop();
			shotLine.StopAuto();
			Helper.StopSound(soundAuto);
		}
		#endregion

	}


	bool IsGrounded()
	{
		Collider[] grounds = Physics.OverlapSphere(ground.position, 0.2f, LayerMask.GetMask("Ground"));
		return grounds.Length > 0;
	}

	public void OnTriggerEnter(Collider other)
	{
		//Debug.Log(other.name);
		ActorAbstract actor = other.GetComponentInParent<ActorAbstract>();
		if (actor)
		{
			actor.EnterZone();
		}
	}

	void DoDed()
	{
		if (ded) return;
		ded = true;

		shotLine.StopAuto();
		if (ShotingParticle.isPlaying) ShotingParticle.Stop();

		rpgScope.SetActive(false);
		sniperScope.SetActive(false);
		autoScope.SetActive(false);

		Animator.SetFloat("Shoting", 0);

		MainCamera.Ded = true;
		Main.speed = 0;
		Animator.SetTrigger("Ded");
	}

	void SwitchWeapon(WeaponType weapon)
	{
		if (weaponType == weapon) return;
		weaponType = weapon;

		if (weaponAuto) weaponAuto.SetActive(weapon == WeaponType.Auto);
		if (weaponSniper) weaponSniper.SetActive(weapon == WeaponType.Sniper);
		if (weaponRpg) weaponRpg.SetActive(weapon == WeaponType.Rpg);
	}

	EnemyControler RayCastEnemy(float minDistance, float maxDistance, out Vector3 position)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll(ray);

		EnemyControler result = null;
		position = Vector3.zero;

		foreach (RaycastHit hit in hits)
		{
			EnemyControler enemy = hit.collider.GetComponent<EnemyControler>();
			if (!enemy) continue;

			float dist = Vector3.Distance(transform.position, enemy.transform.position);
			if (dist > maxDistance || dist < minDistance) continue;

			position = (enemy.transform.position - transform.position).normalized * maxDistance;
			if (position.z < 10) continue;

			result = enemy;
		}
		return result;
	}

	public static float Distance(Vector3 position)
	{
		return Vector3.Distance(Position, position);
	}
}
