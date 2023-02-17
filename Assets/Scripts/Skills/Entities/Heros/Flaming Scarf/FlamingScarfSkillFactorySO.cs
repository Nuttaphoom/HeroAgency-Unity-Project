using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/FlamingScarfSkill")]
public class FlamingScarfSkillFactorySO : EffectProviderFactorySO<Skill_FlamingScarfSkill>
{
    public BurningSkillFactorySO BurningSkilLFactory ;
    public float InflictChange; 
}


public class Skill_FlamingScarfSkill : Skill
{
    private float _timer;

    VisualEffectSO _visualEffectSO;

    BurningSkillFactorySO _burningSkilLFactory;
    float _inflictChange = 0;


    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is FlamingScarfSkillFactorySO)
        {
            FlamingScarfSkillFactorySO skillSO = s as FlamingScarfSkillFactorySO;
            this._name = skillSO.Name ;
            this._detail = skillSO.Detail ;
            this._sprite = skillSO.Sprite ;
            this._visualEffectSO = skillSO.visualEffectSO ;
            this._burningSkilLFactory = skillSO.BurningSkilLFactory ;
            this._sourceName = skillSO.SourceName ;
            this._inflictChange = skillSO.InflictChange ; 

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
        EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance ;
         _producerPtr.GetComponent<Hero>().RemoveSkill(this);
    }

    public override void OnAttackSkill(Entity attacker, Entity target)
    {
        if (Random.Range(0, 100) < _inflictChange)
        _burningSkilLFactory.CreateEffect(target.gameObject);
    }

  


}
