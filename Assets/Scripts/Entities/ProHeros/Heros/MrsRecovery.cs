using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MrsRecovery : ProHero
{
	[SerializeField]
	private MrsRecoverySkillFactorySO _skillSO;

	private Skill_MrsRecoverySkill _skill = null;


	public void PerformHealing(VisualEffectSO visualEfectSO)
	{
		if (combatObj.GetComponent<Animator>())
		{
			new VisualEffectFactorySO().CreateEffect(visualEfectSO, this, this);
            GetHeal(0, 10.0f);
			combatObj.GetComponent<Animator>().SetTrigger("Heal");
		}
	}
 

	public override void PrepareForCombat()
    {
        base.PrepareForCombat();
        _skillSO.CreateEffect(gameObject);
     }

 
}
