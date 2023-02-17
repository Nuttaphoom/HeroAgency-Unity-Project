using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/RunTimeSkills/OldPowerPlantSkill")]
public class OldPowerPlantSkillFactorySO : EffectProviderFactorySO<Skill_OldPowerPlant>
{

}

 
public class Skill_OldPowerPlant : Skill
{
	VisualEffectSO _visualEffectSO;
    private float _counter  ;

    public override void Init(ScriptableObject s, GameObject _producer)
	{
		if (s is OldPowerPlantSkillFactorySO)
		{
            OldPowerPlantSkillFactorySO skillSO = s as OldPowerPlantSkillFactorySO;
			this._name = skillSO.Name;
			this._detail = skillSO.Detail;
			this._sprite = skillSO.Sprite;
			this._visualEffectSO = skillSO.visualEffectSO;
            this._sourceName = skillSO.SourceName;

            _producerPtr = _producer;

			EventManager.instance.OnCombatLeave += OnCombatLeave_RemoveThisSkillInstance;

            CombatManager.instance.AddSkill(this);
		}
	}

    public override void FreeSkill()
    {
        OnCombatLeave_RemoveThisSkillInstance(0);
    }

    private void OnCombatLeave_RemoveThisSkillInstance(int winner)
	{
		EventManager.instance.OnCombatLeave -= OnCombatLeave_RemoveThisSkillInstance;
        CombatManager.instance.RemoveSkill(this);
	}


	public override void RunTimeSkill()
	{
		_counter += 1 * Time.deltaTime;
		if (_counter >= 5.00f)
		{
			_counter = 0;
			CombatManager CM = CombatManager.instance ;
			int _randomEn = Random.Range(0, CM.heroEntities.Count + CM.enemyEntities.Count);
			if (_randomEn < CM.enemyEntities.Count)
			{
				PowerPlantDoDamage(CM.enemyEntities[_randomEn]);
			}else
			{
				PowerPlantDoDamage(CM.heroEntities[_randomEn - CM.enemyEntities.Count].GetComponent<Entity>());
			}
		}
	}

	private void PowerPlantDoDamage(Entity target) 
	{
        VisualEffectFactorySO ss = new VisualEffectFactorySO() ; 

        ss.CreateEffect(_visualEffectSO, null, target);
		Debug.Log("Do Damage to " + target.GetType());
        target.Damaged(3, null);
	}

}
 
