using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/BigSpender")]
public class BigSpenderSkillFactorySO : EffectProviderFactorySO<BigSpender_IncreaseMoneyBigSpender>
{
 
}


public class BigSpender_IncreaseMoneyBigSpender : Skill
{
    private float _timer  ;

    VisualEffectSO _visualEffectSO ;



    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is BigSpenderSkillFactorySO)
	    {
            BigSpenderSkillFactorySO skillSO = s as BigSpenderSkillFactorySO;
			this._name = skillSO.Name;
			this._detail = skillSO.Detail;
			this._sprite = skillSO.Sprite;
			this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName;

            _producerPtr = _producer;

			EventManager.instance.OnHeadQuarterEnter += OnHeadQuarterEnter_IncreaseMoney; 

        }
	}

    public override void FreeSkill()
    {
        EventManager.instance.OnHeadQuarterEnter -= OnHeadQuarterEnter_IncreaseMoney;
    }



    public void OnHeadQuarterEnter_IncreaseMoney(GameObject sender) 
	{
        _producerPtr.GetComponent<BigSpender>().IncreaseMoney(); 
	}
} 