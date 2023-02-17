using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/SmasherSkillSO")]
public class SmasherSkillFactorySO : EffectProviderFactorySO<Skill_SmasherSkill>
{
    public float BootPercent;
    public float ChanceToInflictDamage; 
}


public class Skill_SmasherSkill : Skill
{
    private float _timer;

    private VisualEffectSO _visualEffectSO;

    private float _bootPercent = 0;
    private float _chanceToInflictDamage = 0;

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is SmasherSkillFactorySO)
        {
            SmasherSkillFactorySO skillSO = s as SmasherSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName;
            this._bootPercent = skillSO.BootPercent;
            this._chanceToInflictDamage = skillSO.ChanceToInflictDamage ;  

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

    public override void OnAttackSkill(Entity attacker, Entity target)
    {
        int i = Random.Range(0, 100); 
        if (i < _chanceToInflictDamage) 
        _producerPtr.GetComponent<Smasher>().Smash(_bootPercent); 
    }




}

