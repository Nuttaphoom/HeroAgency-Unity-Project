using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPowerPlant : Building
{
	[SerializeField]
	private OldPowerPlantSkillFactorySO _oldPowerPlantFactory ; 
	
	public override bool InArea(Tile t)
	{
		int[] placedTileIndex = tile_subject.indexInMap;

		int yRange = Mathf.Abs(placedTileIndex[0] - t.indexInMap[0]);
		int xRange = Mathf.Abs(placedTileIndex[1] - t.indexInMap[1]);

		/*
				* * *
				* D *
				* * *
		 */

		if (yRange <= 1 && xRange <= 1 && !(yRange == 1 && xRange == 1))
			return true;
		
		if (Mathf.Abs(yRange) == Mathf.Abs(xRange) && yRange == 1)
			return true;
		
		return false;
	}

	protected void TakeEffect(GameObject effectTile)
	{
 
	}

	public override void TakeCombatEffect(GameObject effectTile)
	{
 
		foreach (GameObject en in FindObjectOfType<CombatManager>().heroEntities)
		{
			_oldPowerPlantFactory.CreateEffect(en);
 		}
	}

	public override void TakeAreaEffect(GameObject effectTile)
	{

	}

	public override void TakeEffect_BeforeSpawn(GameObject effectTile)
	{

	}

	public override void TakeEffect_OnHeadQuarterEnter(GameObject effectTile)
	{
		 
	}

	public override void RemoveEffect(GameObject effectTile)
	{
 
	}

 
}
