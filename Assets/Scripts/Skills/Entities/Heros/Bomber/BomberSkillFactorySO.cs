using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/BomberSkill")]
public class BomberSkillFactorySO :   EffectProviderFactorySO<Skill_BomberSkill>
{
 
}


 

public class Skill_BomberSkill : Skill
{
    private float _timer  ;

    VisualEffectSO _visualEffectSO ;

	public override void Init(ScriptableObject s, GameObject _producer)
    {
      if (s is BomberSkillFactorySO)
	    {
            BomberSkillFactorySO skillSO = s as BomberSkillFactorySO;
			this._name = skillSO.Name;
			this._detail = skillSO.Detail;
			this._sprite = skillSO.Sprite;
			this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName; 

            _producerPtr = _producer;

            _producerPtr.GetComponent<Entity>().AddSkill(this);
            EventManager.instance.OnCombatLeave += CallFreeSkill ;
		}
	}

    private void CallFreeSkill(int winner)
    {
        FreeSkill(); 
    }

	public override void FreeSkill( )
	{
        _producerPtr.GetComponent<Entity>().RemoveSkill(this);
    }

    public override void OnAttackSkill(Entity attacker, Entity target)
    {
        attacker.Damaged(attacker.ATK / 2.0f, target);
    }

} 