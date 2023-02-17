using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class UI_Resource_Detail : MonoBehaviour
{
	public TextMeshProUGUI balanceText;
	public GameObject reputationObj;
    public GameObject _balancePlusText;

    [SerializeField]
    private Image _badgeGFX;

    [SerializeField]
    private Slider _slider; 

    private HQ _hq; 

    private void Start()
	{
        _slider.maxValue = 25;
        _hq = FindObjectOfType<HQ>(); 

        EventManager.instance.OnReputationLevelUp += OnReputationLevelUp_UpdateBadge;
    }

    private void OnDestroy()
    {
        EventManager.instance.OnReputationLevelUp -= OnReputationLevelUp_UpdateBadge;
    }

    private void Update()
	{
        _slider.value = ((int)_hq.reputation) % 25;

        balanceText.text =  "x " + (int) FindObjectOfType<HQ>().money ;
	}

    private void OnReputationLevelUp_UpdateBadge(GameObject sender)
    {
        _badgeGFX.sprite = FindObjectOfType<ReputationManager>().GetBadgeSprite() ;
    }



}
