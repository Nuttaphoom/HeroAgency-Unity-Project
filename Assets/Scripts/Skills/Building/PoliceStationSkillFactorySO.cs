using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 


[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/Buildings/PoliceStationSkill")]
public class PoliceStationSkillFactorySO : EffectProviderFactorySO<Skill_PoliceStation>
{
    public float Stenght = 0; // In percentage
}


public class Skill_PoliceStation : Skill
{
    VisualEffectSO _visualEffectSO;
    private float _counter;
    private float _stenght = 0; 
    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is PoliceStationSkillFactorySO)
        {
            PoliceStationSkillFactorySO skillSO = s as PoliceStationSkillFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName;
            this._stenght = skillSO.Stenght; 
            _producerPtr = _producer;

            EventManager.instance.OnCombatLeave += OnCombatLeave_RemoveThisSkillInstance;
            RewardManager.instance.OnBeforeCalulateResource += SetRepMultiplerZero;

            CombatManager.instance.AddSkill(this);
        }
    }

    public override void FreeSkill()
    {
        EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance;
        RewardManager.instance.OnBeforeCalulateResource -= SetRepMultiplerZero;
        CombatManager.instance.RemoveSkill(this);
    }

    private void OnCombatLeave_RemoveThisSkillInstance(int winner)
    {
        FreeSkill(); 
    }


    public override void RunTimeSkill()
    {
        _producerPtr.GetComponent<PoliceStation>().ArrestEnttities(_stenght,_visualEffectSO) ;
    }

    private void SetRepMultiplerZero()
    {
        Debug.Log("set multipler");
        RewardManager.instance.SetMultipler(-1, 0);
    }




}

