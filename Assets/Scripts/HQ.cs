using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour
{


    public static HQ instance ;
    private void Awake()
    {
        instance = this; 
    }

    public float money = 0;
	public float reputation = 0 ;
	public int maxpreputation = 100;

	private float HQExp = 0; 

	private int failMoneyStack = 0; // To Check game over condition 

	private void Start()
	{
		EventManager.instance.OnHeadQuarterEnter += OnHeadQuarterEnter_GameOverConditionCheck;
        FindObjectOfType<ReputationManager>().GetBadgelevel();

    }

	private void OnDestroy()
	{
		EventManager.instance.OnHeadQuarterEnter -= OnHeadQuarterEnter_GameOverConditionCheck;
	}

	private void OnHeadQuarterEnter_GameOverConditionCheck(GameObject sender)
	{
		if (money < 0)
		{
			failMoneyStack++;
		}
		else
		{
			failMoneyStack = 0;
		}

		if (failMoneyStack >= 3)
		{
			EventManager.instance.PlayOnGameOver(gameObject);
		}
	}

    public void HireThisHero(Hero hero)
    {
        if (hero.GetCost() > money){
            return;
        }

        IncreaseResource(-hero.GetCost(),0) ;
        EventManager.instance.PlayOnHireHero(hero) ; 

    }
	public void IncreaseResource(float increasedMoney, float increasedRep)
	{
		money += increasedMoney;
		reputation += increasedRep;
	}
}
