using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_SkillExplain : MonoBehaviour
{
	private Skill _skill ;
    
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _detailText;
    [SerializeField]
    private Image _logoImage ;
    [SerializeField]
    private Transform _detailBoard;
    [SerializeField]
    private TextMeshProUGUI _sourceText; 

	public Skill GetSkill()
	{
		return _skill;
	}

	public void SetSkill(Skill skill)
	{
		_skill = skill;
        _nameText.text = skill.GetName();
        _detailText.text = skill.GetDetail();
        string s = skill.GetSourceName().ToString();
        _sourceText.text =  "Caused by " + s   ; 
        _logoImage.sprite = skill.GetSprite();
        _detailBoard.gameObject.SetActive(false);


    }

    private void OnMouseEnter()
	{
        _detailBoard.gameObject.SetActive(true) ;
	}

	private void OnMouseExit()
	{
        _detailBoard.gameObject.SetActive(false);
    }

}
