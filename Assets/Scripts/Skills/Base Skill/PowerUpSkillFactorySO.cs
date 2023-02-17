using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/BaseSkills/PowerUp")]
public class PowerUpSkillFactorySO : EffectProviderFactorySO<Skill_PowerUp>
{
    public float PercentIncreasedAttack;
    public float PercentIncreasedMAXHP ;
    public float PercentIncreasedSPEED ;
    public float PercentIncreasedSHIELD ;
    public float PercentIncreasedACCU ;

    public float Duration;
}


public class Skill_PowerUp : Skill
{
    VisualEffectSO _visualEffectSO;
    private float _percentIncreasedATK;
    private float _percentIncreasedMAXHP;
    private float _percentIncreasedSPEED;
    private float _percentIncreasedSHIELD;
    private float _percentIncreasedACCU;
    private float _duration;

    private bool _isPowered = false; 
    private float _timer;

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is PowerUpSkillFactorySO)
        {
            PowerUpSkillFactorySO skillSO = s as PowerUpSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName;

            this._percentIncreasedATK = skillSO.PercentIncreasedAttack;
            this._percentIncreasedMAXHP = skillSO.PercentIncreasedMAXHP;
            this._percentIncreasedSPEED = skillSO.PercentIncreasedSPEED;
            this._percentIncreasedSHIELD = skillSO.PercentIncreasedSHIELD;
            this._percentIncreasedACCU = skillSO.PercentIncreasedACCU ;

            this._duration = skillSO.Duration;

            this._isPowered = false;
            _producerPtr = _producer;

            _producer.GetComponent<Entity>().AddSkill(this);
            EventManager.instance.OnCombatLeave += OnCombatLeave_FreeSkill; 
        }
    }

    private void OnCombatLeave_FreeSkill(int winner)
    {
        FreeSkill(); 
    }

    public override void FreeSkill()
    {
        if (_producerPtr != null)
        {
            Entity en = _producerPtr.GetComponent<Entity>();

            en.ATK -= increasedATK;
            en.MAXHP -= increasedMAXHP;
            en.SPEED -= increasedSPEED;
            en.SHIELD -= increasedSHIELD;
            en.ACCU -= increasedACCU;
            EventManager.instance.OnCombatLeave -= OnCombatLeave_FreeSkill;
            _producerPtr.GetComponent<Entity>().RemoveSkill(this);
        }
    }

    float increasedATK = 0 ;
    float increasedMAXHP = 0;
    float increasedSPEED = 0;
    float increasedSHIELD = 0;
    float increasedACCU = 0;

    public override void RunTimeSkill()
    {
        _timer += 1 * Time.deltaTime; 
        if (_timer >= _duration)
        {
            _timer = 0;
            FreeSkill();
        }else if (! _isPowered)
        {
            _isPowered = true;
            Entity en = _producerPtr.GetComponent<Entity>();
            increasedATK += en.ATK / 100 * _percentIncreasedATK  ;
            increasedMAXHP += en.MAXHP / 100 * _percentIncreasedMAXHP ;
            increasedSPEED += en.SPEED / 100 * _percentIncreasedSPEED ;
            increasedSHIELD  += en.SHIELD / 100 * _percentIncreasedSHIELD ;
            increasedACCU += en.ACCU / 100 * _percentIncreasedACCU;

            en.ATK += increasedATK;
            en.MAXHP += increasedMAXHP ;
            en.SPEED += increasedSPEED ;
            en.SHIELD += increasedSHIELD ;
            en.ACCU += increasedACCU ;

        }

 
}






}

