 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 


[CreateAssetMenu(menuName = "ScriptableObject/Supports/HeroInsurance")]
public class HeroInsuranceSupportFactorySO : SupportFactorySO<HeroInsurance_Support>
{
    public HeroInsuranceSupportSkillFactorySO[] SkillSOs; 
}

public class HeroInsurance_Support : Support
{
    private HeroInsuranceSupportSkillFactorySO _skillSOs;

    public override void Init(ScriptableObject s, GameObject _producer, int supportLevel)
    {
        if (s is HeroInsuranceSupportFactorySO)
        {
            HeroInsuranceSupportFactorySO skillSO = s as HeroInsuranceSupportFactorySO;
            this._name = skillSO.Name[supportLevel];
            this._detail = skillSO.Detail[supportLevel];
            this._sprite = skillSO.Sprite[supportLevel];
            this.available = skillSO.AvailableLevel[supportLevel];
            this._skillSOs = skillSO.SkillSOs[supportLevel] ; 

            curVersion = new HeroInsuranceL2(_skillSOs);

        }
    }


}



public class HeroInsuranceL2 : EmergencyCall_Support
{
    private HeroInsuranceSupportSkillFactorySO _skillSO; 

    public HeroInsuranceL2(HeroInsuranceSupportSkillFactorySO skillSO)
    {
        _skillSO = skillSO;
    }

    public override void OnCombatEnterSkill(Entity me)
    {
        _skillSO.CreateEffect(null);
    }
}
