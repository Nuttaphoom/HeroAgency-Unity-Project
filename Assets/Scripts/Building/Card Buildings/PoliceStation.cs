using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceStation : Building
{
    [SerializeField]
    private PoliceStationSkillFactorySO _skillSO;

    private CombatManager _cm;
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

    public override void TakeCombatEffect(GameObject effectTile)
    {
        _cm = FindObjectOfType<CombatManager>();
        _skillSO.CreateEffect(gameObject);    
    }

    public void ArrestEnttities(float percent, VisualEffectSO visualEffectSO)
    {
        foreach (Entity en in _cm.enemyEntities)
        { 
            if (en.HP <= en.MAXHP / 100 * percent && en.HP > 0)
            {
                Debug.Log("got elim");
                en.Damaged(99999, null);
                new VisualEffectFactorySO().CreateEffect(visualEffectSO, en, en);
            } else
            {
                Debug.Log("surview"); 
            }
        }
    }

 
}
