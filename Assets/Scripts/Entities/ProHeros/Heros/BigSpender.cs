using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSpender : ProHero
{
    [SerializeField]
    private BigSpenderSkillFactorySO _skillSO;

    [SerializeField]
    private GameObject _moenySpreadUIEffect ;
   
    private new void Update()
    {
        base.Update();
    }

    private new void Start()
    {
        base.Start();
        _skillSO.CreateEffect(gameObject);
      }

    public void IncreaseMoney()
    {
        Transform parentUIVisual =  GameObject.FindGameObjectWithTag("UIVisualEffectHolder").transform ;
        Instantiate(_moenySpreadUIEffect, parentUIVisual) ;  
        float increasedAmount = FindObjectOfType<PlayerController>().LoopCount / 2 * 10;
        FindObjectOfType<HQ>().IncreaseResource(increasedAmount, 0);
    }







}
