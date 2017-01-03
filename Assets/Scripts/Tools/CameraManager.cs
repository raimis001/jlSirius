using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{

	public float minDistance;
	public float followDistance;
	public Transform target;
	public Vector3 offset;

	[Range(0, 1)]
	public float speed = 0.5f;

	Vector3 targetPos;
	float interpVelocity;

	internal bool Ded;

	// Use this for initialization
	void Start()
	{
		targetPos = transform.position;
	}
	
	void FixedUpdate()
	{
		if (Ded)
		{
			if (transform.localEulerAngles.x < 40)
			{
				transform.localEulerAngles += new Vector3(1f, 0, 0);
			}
			//return;		
		}

		if (target)
		{
			Vector3 posNo = transform.position;
			posNo.z = target.position.z;
			posNo.y = target.position.y;

			Vector3 targetDirection = (target.position - posNo);
			

			interpVelocity = targetDirection.magnitude * 5f;

			targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

			transform.position = Vector3.Lerp(transform.position, targetPos + offset, speed);
		}
	}

}
