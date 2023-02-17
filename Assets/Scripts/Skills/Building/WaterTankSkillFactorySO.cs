using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/Buildings/WaterTankSkillFactorySO")]
public class WaterTankSkillFactorySO : EffectProviderFactorySO<Skill_WaterTank>
{
    public int Stenght = 0; // In percentage
    public float TimeMax = 0;
}


public class Skill_WaterTank : Skill
{
    VisualEffectSO _visualEffectSO;
    private float _counter;
    private int _stenght = 0;
    private float _timeMax = 0; 

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is WaterTankSkillFactorySO)
        {
            WaterTankSkillFactorySO skillSO = s as WaterTankSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName;
            this._stenght = skillSO.Stenght;
            this._timeMax = skillSO.TimeMax; 

            _producerPtr = _producer;

            EventManager.instance.OnCombatLeave += OnCombatLeave_RemoveThisSkillInstance;

            CombatManager.instance.AddSkill(this);
        }
    }

    public override void FreeSkill()
    {
        EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance;
        CombatManager.instance.RemoveSkill(this);
    }

    private void OnCombatLeave_RemoveThisSkillInstance(int winner)
    {
        FreeSkill();
    }


    public override void RunTimeSkill()
    {
        _counter += 1 * Time.deltaTime; 
        if (_counter >= _timeMax )
        {
            _counter = 0;
            _producerPtr.GetComponent<OldWaterTank>().PerfromWaterTankHeal(_stenght) ; 
        } 

    }



}

