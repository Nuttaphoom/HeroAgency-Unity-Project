using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    #region Singleton
    static public EventManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this);
    }
    #endregion

    public event NormalDelegate OnHeadQuarterEnter; // Spawner, HeroStock , HQ

    public event TileDelegate OnCombatEnter; //CombatManager, Player
    public event IntDelegate OnCombatLeave; // CombatManager, Tile , Deck , Player
    public event TileDelegate OnBossEnounter ; //CombatManager

    public event EntityDelegate OnHireHero;   //  HireManager , UI_Heor_Quick_Detail
    public event SupportDelegate OnBuySupport; // SupportUnlockTap ,  SupportManager , InventoryManager
    public event NormalDelegate OnReputationLevelUp;  //ReputationManager , SupportManager, UI_Resource_Detail
    public event LevelDelegate OnEnterLevel; //CharacterCardSelector , LevelManager , Database 

    public event NormalDelegate OnPause; // PlayerController , SpeedManager
    public event NormalDelegate OnStopPause; // PlayerController , SpeedManager

    public event NormalDelegate OnCardSelect; //Card 
    public event NormalDelegate OnCardSelectCancel; //Card , Tile
    public event CardDelegae OnCardPlay; //Player , Tile , Card

    public event NormalDelegate OnGameOver; // UI_GameOverAndWin
    public event NormalDelegate OnGameWin; // UI_GameOverAndWin

    public delegate void NormalDelegate(GameObject sender);
    public delegate void TileDelegate(Tile tile);
    public delegate void IntDelegate(int i);
    public delegate void CardDelegae(Card c);
    public delegate void EntityDelegate(Entity entity);
    public delegate void SupportDelegate(Support support);
    public delegate void LevelDelegate(Level lvl);

    public void PlayOnEnterLevel(Level sender)
    {
        OnEnterLevel?.Invoke(sender);
    }

    public void PlayOnBossEncounter(Tile sender)
    {
        OnBossEnounter?.Invoke(sender);
    }

    public void PlayOnReputationLevelUp(GameObject sender)
    {
        OnReputationLevelUp?.Invoke(sender); 
    }

    public void PlayOnBuySupport(Support sup)
    {
        OnBuySupport?.Invoke(sup);
    }

    public void PlayOnGameWin(GameObject sender)
	{
 		OnGameWin?.Invoke(sender); 
	}

	public void PlayOnHireHero(Entity entity)
	{
		OnHireHero?.Invoke(entity); 
	}

	public void PlayOnGameOver(GameObject sender)
	{
		OnGameOver?.Invoke(sender); 
	}

	public void PlayOnCardPlay(Card c)
	{
		OnCardPlay?.Invoke(c); 
	}

	public void PlayOnCardSelectCancel(GameObject sender)
	{
		OnCardSelectCancel?.Invoke(sender); 
	}

	public void PlayOnCardSelect(GameObject sender)
	{
		OnCardSelect?.Invoke(sender); 
	}

	public void PlayOnCombatLeave(int num)
    {
        OnCombatLeave?.Invoke(num);   
	}

	public void PlayOnHeadQuaterEnter(GameObject sender)
	{

		OnHeadQuarterEnter?.Invoke(sender);
	}

	public void PlayOnCombatEnter(Tile tile)
	{
		OnCombatEnter?.Invoke(tile); 
	}

	public void PlayOnPause(GameObject sender)
	{
		OnPause?.Invoke(sender);
	}

	public void PlayOnStopPause(GameObject sender)
	{
		OnStopPause?.Invoke(sender);
	}
}
