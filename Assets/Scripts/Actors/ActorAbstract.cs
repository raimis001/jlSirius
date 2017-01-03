using UnityEngine;
using System.Collections;

public abstract class ActorAbstract : MonoBehaviour
{

	internal bool active = true;

	public abstract void EnterZone();
	public abstract void ActivateZone();
	public abstract void Restart();

}