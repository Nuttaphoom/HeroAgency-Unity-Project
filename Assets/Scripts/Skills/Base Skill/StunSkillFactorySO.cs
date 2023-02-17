using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/BaseSkills/Stun")]
public class StunSkillFactorySO : EffectProviderFactorySO<Skill_StunSkill>
{
    public float LifeSpand;
}


public class Skill_StunSkill : Skill
{

    private float _timer = 0;
    private float _lifeSpand = 0;

    VisualEffectSO _visualEffectSO;

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is StunSkillFactorySO)
        {
            StunSkillFactorySO skillSO = s as StunSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._lifeSpand = skillSO.LifeSpand;
            this._sourceName = skillSO.SourceName;

            _producerPtr = _producer;

            EventManager.instance.OnCombatLeave += OnCombatLeave_RemoveThisSkillInstance;
            _producer.GetComponent<Entity>().AddSkill(this);
        }


    }

    public override void FreeSkill()
    {
        EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance;
        _producerPtr.GetComponent<Entity>().RemoveSkill(this);
    }

    private void OnCombatLeave_RemoveThisSkillInstance(int winner)
    {
        FreeSkill(); 
    }

    public override void RunTimeSkill()
    {
 

        _timer += 1 * Time.deltaTime;
          
        if (_timer >= _lifeSpand)
        {
            _producerPtr.GetComponent<Entity>().ChangeState(Entity.StateMachine.Idle);
            OnCombatLeave_RemoveThisSkillInstance(0);
        }
        else
        {
            _producerPtr.GetComponent<Entity>().ChangeState(Entity.StateMachine.Stun);
        }
    }



}