using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
     public List<GameObject> _skillLists = new List<GameObject>() ;

    bool init = false;
    void Init()
    {
        if (init)
            return;

        init = true;
        _skillLists = new List<GameObject>();
    }

    private void Update()
    {
        Init();
    }

    public void PlayRunTimeSkill()
    {
 
        for (int i = 0; i < _skillLists.Count; i++)
        {
            Skill s = _skillLists[i].GetComponent<UI_SkillExplain>().GetSkill();
             s.RunTimeSkill();
        }
    }

    public void PlayOnDamagedSkill(Entity attacker, Entity target)
    { 
        for (int i = 0; i < _skillLists.Count; i++)
        {
            Skill s = _skillLists[i].GetComponent<UI_SkillExplain>().GetSkill();
            s.OnDamagedSkill(attacker, target);
        }
    }

    public void PlayOnAttackSkill(Entity attacker, Entity target)
    {
        for (int i = 0; i < _skillLists.Count; i++)
        {
            Skill s = _skillLists[i].GetComponent<UI_SkillExplain>().GetSkill();
            s.OnAttackSkill(attacker, target);
        }
    }

    public void PlayOnCombatEnterSkill(Entity meEntity)
    {
 
        for (int i = 0; i < _skillLists.Count; i++)
        {
            Skill s = _skillLists[i].GetComponent<UI_SkillExplain>().GetSkill();
            s.OnCombatEnterSkill(meEntity) ;
        }
    }


    public void AddSkill(Skill _skill, GameObject template,Transform parent )
    {
        GameObject _newObj = Instantiate(template,  new Vector3(0,0,0), template.transform.rotation );
        _newObj.transform.SetParent(parent) ;

        _newObj.transform.localScale = template.transform.localScale;

        _newObj.GetComponent<UI_SkillExplain>().SetSkill(_skill);

        _newObj.SetActive(true);

		_skillLists.Add(_newObj);

    }

    public void RemoveSkill(Skill _skill)
	{
 		Skill s = _skill; 
		for (int i = 0; i < _skillLists.Count; i++)
		{
			if (_skillLists[i].GetComponent<UI_SkillExplain>().GetSkill().GetType() == s.GetType() )
			{
				Destroy(_skillLists[i]);
				_skillLists.Remove(_skillLists[i]); 
 			}
		}
 
	}

    public void ClearSkill()
    {
         while (_skillLists.Count > 0)
        {
            for (int i = _skillLists.Count - 1; i >= 0; i--)
            {
                if (i < _skillLists.Count)
                    _skillLists[i].GetComponent<UI_SkillExplain>().GetSkill().FreeSkill();
            }
        }
        
    }

    public List<GameObject> GetSkillList()
    {
        return _skillLists;
    }
	 
}
