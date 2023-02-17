using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
 

[CreateAssetMenu(menuName = "ScriptableObject/Supports/EmergencyCall")]
public class EmergencySupportFactorySO : SupportFactorySO<EmergencyCall_Support>
{

}

public class EmergencyCall_Support : Support
{
    public override void Init(ScriptableObject s, GameObject _producer, int supportLevel)
    {
        if (s is EmergencySupportFactorySO)
        {
            EmergencySupportFactorySO skillSO = s as EmergencySupportFactorySO;
            this._name = skillSO.Name[supportLevel];
            this._detail = skillSO.Detail[supportLevel];
            this._sprite = skillSO.Sprite[supportLevel];
            this.available = skillSO.AvailableLevel[supportLevel];
            curVersion = new EmergencyCallL4(); 
        }
    }
}



public class EmergencyCallL4 : EmergencyCall_Support
{
    public override void OnCombatEnterSkill(Entity me)
    {
        if (CombatManager.instance.CanAddMoreHero())
        {
            HeroStock hs = HeroStock.instance;
            for (int i = 0; i < hs.heroLists.Count; i++)
            {
                Debug.Log("finding");

                bool _containH = false;

                GameObject h = hs.heroLists[i]; // ;
                for (int j = 0; j < CombatManager.instance.heroEntities.Count; j++)
                {
                    if (CombatManager.instance.heroEntities.Contains(h))
                        _containH = true;
                }


                if (hs.heroLists[i].GetComponent<Entity>().HP <= 0)
                    continue;

                if (!_containH)
                {
                    Debug.Log("called");

                    CombatManager.instance.heroEntities.Add(h);
                    CombatManager.instance.InitHeroToBattle(h, CombatManager.instance.heroEntities.Count - 1);
                    break;
                }
            }
        }
    }
}

 


