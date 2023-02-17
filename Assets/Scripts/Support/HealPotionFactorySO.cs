using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 


[CreateAssetMenu(menuName = "ScriptableObject/Supports/HealPotionFactory")]
public class HealPotionFactorySO : SupportFactorySO<HealPotion_Support>
{
    public float[] HealPercent;
    public float[] HealthMin; 
}

public class HealPotion_Support : Support
{
    private float _healPercent; 

    public override void Init(ScriptableObject s, GameObject _producer, int supportLevel)
    {
        if (s is HealPotionFactorySO)
        {
            HealPotionFactorySO skillSO = s as HealPotionFactorySO ;
            this._name = skillSO.Name[supportLevel];
            this._detail = skillSO.Detail[supportLevel];
            this._sprite = skillSO.Sprite[supportLevel];
            this.available = skillSO.AvailableLevel[supportLevel];
 
            curVersion = new HealPotionL2(skillSO.HealPercent[supportLevel],skillSO.HealthMin[supportLevel]);

        }
    }


}



public class HealPotionL2 : HealPotion_Support
{
    float _healPercent = 0;
    float _healMin = 0;

    public HealPotionL2(float healPercent,float healMin)
    {
        _healPercent = healPercent;
        _healMin = healMin;
    }

    public override void OnCombatEnterSkill(Entity me)
    {
        foreach (GameObject hero in CombatManager.instance.heroEntities)
        {
            if (hero.GetComponent<Hero>().HP < hero.GetComponent<Hero>().MAXHP / 100 * _healMin)
            {
                hero.GetComponent<Hero>().GetHeal(0, _healPercent); 
            }
        }
    }
}
