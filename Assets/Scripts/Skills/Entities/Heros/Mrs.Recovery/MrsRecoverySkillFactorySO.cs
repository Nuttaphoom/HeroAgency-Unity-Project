using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/MrsRecoverySkill")]
public class MrsRecoverySkillFactorySO :   EffectProviderFactorySO<Skill_MrsRecoverySkill>
{
 
}


 

public class Skill_MrsRecoverySkill : Skill
{
    private float _timer  ;

    VisualEffectSO _visualEffectSO ;

	public override void Init(ScriptableObject s, GameObject _producer)
    {
      if (s is MrsRecoverySkillFactorySO)
	    {
            MrsRecoverySkillFactorySO skillSO = s as MrsRecoverySkillFactorySO;
			this._name = skillSO.Name;
			this._detail = skillSO.Detail;
			this._sprite = skillSO.Sprite;
			this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName; 

            _producerPtr = _producer;

             CombatManager.instance.AddSkill(this);
            EventManager.instance.OnCombatLeave += CallFreeSkill ;
 
		}
	}

    private void CallFreeSkill(int winner)
    {
        FreeSkill(); 
    }

	public override void FreeSkill( )
	{
 		CombatManager.instance.RemoveSkill(this);
 	}

	public override void RunTimeSkill()
	{
        Debug.Log("Mrs.remover runtime skill");
        if (CombatManager.instance.GetState() != CombatManager.StateMachine.Combat)
        {
            _timer = 0; 
            return;
        }

        _timer += 1 * Time.deltaTime;
		if (_timer >= 4)
		{
 			_producerPtr.GetComponent<MrsRecovery>().PerformHealing(_visualEffectSO); 
			_timer = 0;

		}
	}
} 