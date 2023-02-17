using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/BaseSkills/Shock")]
public class ShockSkillFactorySO : EffectProviderFactorySO<Skill_ShockSkill>
{
    public float LifeSpand;
    public float Strength ;
}


public class Skill_ShockSkill : Skill
{

    private float _timer = 0;
    private float _lifeSpand = 0;
    private float _strength = 0; //In percentage (0 = 0% , 1 = 100% ) 

    VisualEffectSO _visualEffectSO;


    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is ShockSkillFactorySO)
        {
            ShockSkillFactorySO skillSO = s as ShockSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._lifeSpand = skillSO.LifeSpand;
            this._sourceName = skillSO.SourceName;
            this._strength = skillSO.Strength; 

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
        _producerPtr.GetComponent<Entity>().RemoveSkill(this);
    }

    public override void RunTimeSkill()
    {
        float attackDelay = _producerPtr.GetComponent<Entity>().attackDelay;
        _producerPtr.GetComponent<Entity>().attackDelay -= (attackDelay * _strength * Time.deltaTime);
    }



}