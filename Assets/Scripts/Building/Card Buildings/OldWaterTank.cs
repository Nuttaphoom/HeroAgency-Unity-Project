using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldWaterTank : Building
{

    CombatManager _cm;
    [SerializeField]
    WaterTankSkillFactorySO _waterTankSkillSO;

    [SerializeField]
    VisualEffectSO _fireVisualeffectSO;
    [SerializeField]
    VisualEffectSO _healVisualeffectSO;

    public override bool InArea(Tile t)
    {
        int[] placedTileIndex = tile_subject.indexInMap;

        int yRange = Mathf.Abs(placedTileIndex[0] - t.indexInMap[0]);
        int xRange = Mathf.Abs(placedTileIndex[1] - t.indexInMap[1]);

        /*
				*   *
				  D 
				*   *
		 */

        if (yRange - xRange == 0 && Mathf.Abs(yRange) == 1 && Mathf.Abs(xRange) == 1)
        {
            return true; 
        }

        return false;
    }

    public override void TakeCombatEffect(GameObject effectTile)
    {
        _cm = FindObjectOfType<CombatManager>();
        _waterTankSkillSO.CreateEffect(gameObject); 
    }

    public void PerfromWaterTankHeal(int healPercent)
    {
        int side = 0;
        int who;

        bool fire = false;
        foreach (GameObject hero in FindObjectOfType<CombatManager>().heroEntities)
        {
            if (hero.GetComponent<ProHero>().weapon == ProHero.WeaponType.Fire)
            {
                fire = true; 
                break;
            }
        }

        do
        {
            side = Random.Range(-1, 2);
        } while (side == 0);

        Entity en  ;

        if (side == -1)
        {
            who = Random.Range(0, _cm.enemyEntities.Count);
            en = _cm.enemyEntities[who].GetComponent<Entity>();
            if (fire)
                en.Damaged(en.MAXHP / 100 * healPercent, null);
            else
                en.GetHeal(0, healPercent);
        }
        else  
        {
            who = Random.Range(0, _cm.heroEntities.Count) ;
            en = _cm.heroEntities[who].GetComponent<Entity>() ;
            if (fire)
                en.Damaged(en.MAXHP / 100 * healPercent, null);
            else
                en.GetHeal(0, healPercent);
         }

        if (fire)
            new VisualEffectFactorySO().CreateEffect(_fireVisualeffectSO, null, en);
        else
            new VisualEffectFactorySO().CreateEffect(_healVisualeffectSO, null, en);

        return; 
    }

    


}
