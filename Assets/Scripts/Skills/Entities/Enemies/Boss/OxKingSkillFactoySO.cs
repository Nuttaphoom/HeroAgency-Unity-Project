using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/OxKingSkillFactorySO")]
public class OxKingSkillFactoySO : EffectProviderFactorySO<Skill_OxKingSkill>
{

}


public class Skill_OxKingSkill : Skill
{
    VisualEffectSO _visualEffectSO;

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is OxKingSkillFactoySO)
        {
            OxKingSkillFactoySO skillSO = s as OxKingSkillFactoySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName;

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
        if (_producerPtr)
            _producerPtr.GetComponent<Entity>().RemoveSkill(this);
    }

    public override void OnAttackSkill(Entity attacker, Entity target)
    {
         
    }




}

