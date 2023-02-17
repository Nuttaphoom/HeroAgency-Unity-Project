using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/BaseSkills/Burning")]
public class BurningSkillFactorySO : EffectProviderFactorySO<Skill_BurningSkill>
{
    public int DamagePerHit = 0 ;
    public float TimerPerHit = 0;
    public float LifeSpand = 0;  
}


public class Skill_BurningSkill : Skill
{

    private float _timer = 0 ;
    private int _damagePerHit = 0;
    private float _timerPerHit = 0;
    private float _lifeSpand = 0;

    VisualEffectSO _visualEffectSO;

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is BurningSkillFactorySO)
        {
            BurningSkillFactorySO skillSO = s as BurningSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._damagePerHit = skillSO.DamagePerHit ;
            this._timerPerHit = skillSO.TimerPerHit ;
            this._lifeSpand = skillSO.LifeSpand ;
            this._sourceName = skillSO.SourceName;

            _producerPtr = _producer;

            EventManager.instance.OnCombatLeave += OnCombatLeave_RemoveThisSkillInstance;
            _producer.GetComponent<Entity>().AddSkill(this);
        }

 
    }

    private void OnCombatLeave_RemoveThisSkillInstance(int winner)
    {
        EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance;
        _producerPtr.GetComponent<Entity>().RemoveSkill(this);
    }

    public override void RunTimeSkill()
    {
        _timer += 1 * Time.deltaTime;
        _lifeSpand -= 1 * Time.deltaTime;
        if (_timer >= _timerPerHit)
        {
            new VisualEffectFactorySO().CreateEffect(_visualEffectSO, _producerPtr.GetComponent<Entity>(), _producerPtr.GetComponent<Entity>());
            _producerPtr.GetComponent<Entity>().Damaged(_damagePerHit, null);
            _timer = 0; 
        }
        if (_lifeSpand <= 0)
        {
            OnCombatLeave_RemoveThisSkillInstance(0);
        }
    }

    public override void FreeSkill()
    {
        OnCombatLeave_RemoveThisSkillInstance(1);
    }
}
