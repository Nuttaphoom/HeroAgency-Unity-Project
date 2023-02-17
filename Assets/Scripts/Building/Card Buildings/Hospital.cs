using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : Building 
{
	public override bool InArea(Tile t)
	{
		int[] placedTileIndex = tile_subject.indexInMap;

		int yRange = Mathf.Abs(placedTileIndex[0] - t.indexInMap[0] ) ; 
		int xRange = Mathf.Abs(placedTileIndex[1] - t.indexInMap[1] ) ;

		/*
				  *
				* D *
				  *
		 */

		if (yRange <= 1 && xRange <= 1 && ! (yRange == 1 && xRange == 1))
			return true;

		return false; 
	}

	public override void TakeCombatEffect(GameObject effectTile)
	{

	}

	public override void TakeAreaEffect(GameObject effectTile)
	{
        List<GameObject> heroLists = FindObjectOfType<HeroStock>().heroLists;
        for (int i = 0; i < heroLists.Count; i++)
            heroLists[i].GetComponent<Entity>().GetHeal(0, 10);
        
		FindObjectOfType<HQ>().IncreaseResource(-10 * heroLists.Count , 0);
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
