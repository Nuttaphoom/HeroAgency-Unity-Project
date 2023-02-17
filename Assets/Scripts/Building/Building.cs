using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour , IEffectProvider 
{
	public Sprite buildingSprite;
	
	[HideInInspector]
	public Tile tile_subject;  
	public Vector3 offset;
	public Vector3 offsetSize;
	public Vector3 offsetRot;

	public void SetTileSubject(Tile t)
	{
		this.tile_subject = t;
		transform.position = t.transform.position; 

		transform.position += offset;
		transform.localScale += offsetSize;

		transform.Rotate(offsetRot) ;  
	}

    public virtual bool InArea(Tile tile)
    {
        Debug.Log("InArea of " + this + "never been implemented");
        return false;
    }

	public void CombatEffect() { }

	public virtual void TakeCombatEffect(GameObject effectTile)
	{

    }

	public virtual void TakeAreaEffect(GameObject effectTile)
	{

    }

	public virtual void TakeEffect_BeforeSpawn(GameObject effectTile)
	{

    }

	public virtual void TakeEffect_OnHeadQuarterEnter(GameObject effectTile)
	{

	}

	public virtual void RemoveEffect(GameObject effectTile)
	{

    }
}
