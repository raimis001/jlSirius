using UnityEngine;
using System.Collections;

public class ActorRow : ActorAbstract
{

	public override void EnterZone()
	{
		PlayerControler.Life = 0;
	}

	public override void ActivateZone()
	{
		
	}

	public override void Restart()
	{
		
	}
}
