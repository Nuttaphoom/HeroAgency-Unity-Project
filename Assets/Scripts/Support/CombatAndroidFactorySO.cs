using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(menuName = "ScriptableObject/Supports/CombatAndroid")]
public class CombatAndroidFactorySO : SupportFactorySO<CombatAndroid_Support> 
{
    public CombatAndroid CombatAndroid ;
}

public class CombatAndroid_Support : Support
{

    public CombatAndroid _combatAndroid ; 

    public override void Init(ScriptableObject s, GameObject _producer,int supportLevel)
	{
        if (s is CombatAndroidFactorySO)
        {
            CombatAndroidFactorySO skillSO = s as CombatAndroidFactorySO;
            this._name = skillSO.Name[supportLevel];
            this._detail = skillSO.Detail[supportLevel];
            this._sprite = skillSO.Sprite[supportLevel];
            this._combatAndroid = skillSO.CombatAndroid ;

            curVersion = new CombatAndroidL4(s);

             
        }
    }


}

public class CombatAndroidL4 : CombatAndroid_Support
{
    CombatAndroid _combatAndroid;

    public CombatAndroidL4(ScriptableObject s)
    {
        CombatAndroidFactorySO skillSO = s as CombatAndroidFactorySO;
        _combatAndroid = skillSO.CombatAndroid;
    }

    public override void OnCombatEnterSkill(Entity meEntity)
    {
        _combatAndroid.Init();
        if(CombatManager.instance.CanAddMoreHero()) {
            CombatManager.instance.heroEntities.Add(_combatAndroid.gameObject);
            CombatManager.instance.InitHeroToBattle(CombatManager.instance.heroEntities[CombatManager.instance.heroEntities.Count - 1], CombatManager.instance.heroEntities.Count - 1, true);
        }
    }
}




