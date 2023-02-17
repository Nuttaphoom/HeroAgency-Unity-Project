using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/CleaningRobot/CleaningSmoke")]
public class CleaningSmokeSkillFactorySO : EffectProviderFactorySO<Skill_CleaningSmoke>
{
 
}

public class Skill_CleaningSmoke : Skill
{
    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is CleaningSmokeSkillFactorySO)
        {
            CleaningSmokeSkillFactorySO skillSO = s as CleaningSmokeSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._sourceName = skillSO.SourceName;

            _producerPtr = _producer; //Producer can only be CleaningRobot

            EventManager.instance.OnCombatLeave += OnCombatLeave_RemoveThisSkillInstance;
            _producer.GetComponent<Entity>().AddSkill(this);
        }
    }


    private void OnCombatLeave_RemoveThisSkillInstance(int winner)
    {
        EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance;
        _producerPtr.GetComponent<Entity>().RemoveSkill(this);
    }

    public override void OnDamagedSkill(Entity attacker, Entity target)
    {
        Debug.Log(100 * target.HP / target.MAXHP);
        if (100 * target.HP / target.MAXHP <= 20)
        _producerPtr.GetComponent<CleaningRobot>().StartHiding() ;
         
    }

    public override void FreeSkill()
    {
        OnCombatLeave_RemoveThisSkillInstance(0);
    }
}
