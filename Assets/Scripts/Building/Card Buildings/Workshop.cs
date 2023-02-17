using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : Building 
{
	public GameObject ironFist_GameObj;
	public GameObject greatIronFist_GameObj;

	public override bool InArea(Tile t)
	{
        int[] placedTileIndex = tile_subject.indexInMap;

        int yRange = Mathf.Abs(placedTileIndex[0] - t.indexInMap[0]);
        int xRange = Mathf.Abs(placedTileIndex[1] - t.indexInMap[1]);

        /*
				  *
				* D *
				  *
		 */

        if (yRange <= 1 && xRange <= 1 && !(yRange == 1 && xRange == 1))
            return true;

        return false;
    }

	protected void TakeEffect(GameObject effectTile)
	{

	}

	public override void TakeCombatEffect(GameObject effectTile)
	{
		
 	}

	public override void TakeAreaEffect(GameObject effectTile)
	{
		 
	}

	public override void TakeEffect_BeforeSpawn(GameObject effectTile)
	{
 
	}

	public override void TakeEffect_OnHeadQuarterEnter(GameObject effectTile)
	{
		int[] placedTileIndex = effectTile.GetComponent<Tile>().indexInMap;
		if (effectTile.GetComponent<Tile>().tileType != ItemAsset.TileType.Road || Random.Range(0, 3) != 0 /* 33% */ )
			return;

		GameObject curTile = FindObjectOfType<MapGenerator>().tileMap[placedTileIndex[0], placedTileIndex[1]];

		bool alreadyHaveIronFist = false;
		List<Entity> enemyList = curTile.GetComponent<Tile>().enemyOnThisTile;
		for (int p = 0; p < enemyList.Count; p++)
		{
			if (enemyList[p].GetComponent<IronFist>())
			{
				alreadyHaveIronFist = true;
				curTile.GetComponent<Tile>().DeleteEnemyOnThisTile(curTile.GetComponent<Tile>().enemyOnThisTile[p]);
				break;
			}
		}

		if (alreadyHaveIronFist)
			FindObjectOfType<Spawner>().SpawnEnemyOnRoad(curTile.GetComponent<Tile>(), greatIronFist_GameObj);
		else
		{
			FindObjectOfType<Spawner>().SpawnEnemyOnRoad(curTile.GetComponent<Tile>(), ironFist_GameObj);
		}
	}

	public override void RemoveEffect(GameObject effectTile)
	{
		 
	}

	private void OnCombatLeave_Effect_GainMoreResource(int winner)
	{
		EventManager.instance.OnCombatLeave -= OnCombatLeave_Effect_GainMoreResource;
		FindObjectOfType<HQ>().IncreaseResource(5, 0);
	}
}
