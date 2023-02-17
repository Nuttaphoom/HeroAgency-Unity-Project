using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTowner : Building
{

    CombatManager _cm;

    [SerializeField]
    PowerUpSkillFactorySO _skillSO ;

    [SerializeField]
    VisualEffectSO _buff;
     

    public override bool InArea(Tile t)
    {
        int[] placedTileIndex = tile_subject.indexInMap;

        int yRange = Mathf.Abs(placedTileIndex[0] - t.indexInMap[0]);
        int xRange = Mathf.Abs(placedTileIndex[1] - t.indexInMap[1]);

        /*
                    *
				    *
				* * D * * 
				    *
                    * 
		 */

        if (Mathf.Abs(yRange) - Mathf.Abs(xRange) == 0 && Mathf.Abs(yRange) == 1 && Mathf.Abs(xRange) == 1)
        {
            return true; 
        }

        return false;
    }

    public override void TakeCombatEffect(GameObject effectTile)
    {
        _cm = FindObjectOfType<CombatManager>();
        foreach(GameObject hero in _cm.heroEntities)
        {
            _skillSO.CreateEffect(hero); 
        }
    }

     

    


}
