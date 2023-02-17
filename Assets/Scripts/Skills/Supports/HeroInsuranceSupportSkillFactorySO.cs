 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/Supports/HeroInsurance")]
public class HeroInsuranceSupportSkillFactorySO : EffectProviderFactorySO<Skill_HeroInsurance>
{
    public float CoinGet = 0 ;
    public float HPMin = 0;
}


public class Skill_HeroInsurance : Skill
{
    float _cointGet = 0;
    float _hpMin = 0;

 
    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is HeroInsuranceSupportSkillFactorySO)
        {
            HeroInsuranceSupportSkillFactorySO skillSO = s as HeroInsuranceSupportSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._sourceName = skillSO.SourceName;
            this._cointGet = skillSO.CoinGet;
            this._hpMin = skillSO.HPMin;

            _producerPtr = _producer;

            EventManager.instance.OnCombatLeave += OnCombatLeave_RemoveThisSkillInstance;
            CombatManager.instance.AddSkill(this);
        }
    }

    public override void FreeSkill()
    {
        OnCombatLeave_RemoveThisSkillInstance(0);
    }

    private void OnCombatLeave_RemoveThisSkillInstance(int winner)
    {
        EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance;
        CombatManager.instance.RemoveSkill(this);
    }

    List<Hero> paidHero = new List<Hero>() ; 

    public override void OnDamagedSkill(Entity attacker, Entity target)
    {
        if (paidHero == null)
            paidHero = new List<Hero>();
        if (target.GetComponent<Hero>())
        {
            if (attacker.GetComponent<Hero>())
            {
                HQ.instance.IncreaseResource(_cointGet, 0);
            }else if (target.HP < target.MAXHP / 100 * _hpMin  )
            {
                foreach (Hero hr in paidHero)
                {
                    if (hr.GetType() == target.GetType())
                        return; 
                }
                paidHero.Add(target.GetComponent<Hero>());
                HQ.instance.IncreaseResource(_cointGet, 0);
            }
        }
    }




}
