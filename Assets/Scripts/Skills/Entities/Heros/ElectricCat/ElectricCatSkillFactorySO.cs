using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/ElectricCatSkill")]
public class ElectricCatSkillFactorySO : EffectProviderFactorySO<Skill_ElectricCatSkill>
{
    public ShockSkillFactorySO CatShockSkillFactorySO;
    public float InflictChance ;
}


public class Skill_ElectricCatSkill : Skill
{
    private float _timer;

    VisualEffectSO _visualEffectSO;

    ShockSkillFactorySO _shockSkilLFactory;
    private float _inflictChance;


    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is ElectricCatSkillFactorySO)
        {
            ElectricCatSkillFactorySO skillSO = s as ElectricCatSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._shockSkilLFactory = skillSO.CatShockSkillFactorySO;
            this._sourceName = skillSO.SourceName;
            this._inflictChance = skillSO.InflictChance; 

            _producerPtr = _producer;

            EventManager.instance.OnCombatLeave += OnCombatLeave_RemoveThisSkillInstance;
            _producer.GetComponent<Entity>().AddSkill(this);
        }
    }

    public override void FreeSkill()
    {
        OnCombatLeave_RemoveThisSkillInstance(0);
    }

    private void OnCombatLeave_RemoveThisSkillInstance(int winner)
    {
        EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance;
        _producerPtr.GetComponent<Hero>().RemoveSkill(this);
    }

    public override void OnAttackSkill(Entity attacker, Entity target)
    {
        if (Random.Range(0,100) < _inflictChance)
            _shockSkilLFactory.CreateEffect(target.gameObject);
    }




}

