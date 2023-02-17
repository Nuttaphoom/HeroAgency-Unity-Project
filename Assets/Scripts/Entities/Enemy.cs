using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy :  Entity
{  	 
	private Tile onThisTile  ;

	private new void Update()
	{
 		while (LEVEL < FindObjectOfType<PlayerController>().LoopCount)
		{
			LevelUp();
		}
 
		base.Update(); 

		
	}

	

	public Tile  GetOnThisTile()
	{
		return onThisTile; 
	}

	public void SetOnThisTile(Tile onThis)
	{
		onThisTile = onThis;  
	}

    public override void RestoreState(object state)
    {
        throw new System.NotImplementedException();
    }

    public override object CaptureState()
    {
        throw new System.NotImplementedException();
    }
}
