using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStore : Building
{
    [SerializeField]
    private GameObject _cleaningRobot;

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

    public override void TakeEffect_OnHeadQuarterEnter(GameObject effectTile)
    {
        int[] placedTileIndex = effectTile.GetComponent<Tile>().indexInMap;
        if (effectTile.GetComponent<Tile>().tileType != ItemAsset.TileType.Road || Random.Range(0, 3) != 0 /* 33% */ )
            return;

        GameObject curTile = FindObjectOfType<MapGenerator>().tileMap[placedTileIndex[0], placedTileIndex[1]];

        for (int i = 0; i < 2; i++) 
        FindObjectOfType<Spawner>().SpawnEnemyOnRoad(curTile.GetComponent<Tile>(), _cleaningRobot);

    }



}
