using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Card : SavableObject
{
	public Building buildingType; 

	public CardData cardData;
	public GameObject artSpriteObj;

    public bool IsUnlock = false;

    [SerializeField]
    private bool init = false; 
    
    [Serializable]
    public struct SaveData
    {
        public bool isUnlock  ;
		
    }

	public Card(Card c)
	{
 		this.buildingType = c.buildingType;
		this.cardData = c.cardData;
		this.artSpriteObj = c.artSpriteObj;
	}

    public void Init()
    {
        if (init)
            return; 

        init = false ;

        EventManager.instance.OnCardSelect += OnCardSelect_DisableThisCard;
        EventManager.instance.OnCardSelectCancel += OnCardSelectCancel_EnableThisCard;
        EventManager.instance.OnCardPlay += OnCardPlay_EnableThisCard;

        transform.Find("CardGFX").Find("Card GFX").GetComponent<SpriteRenderer>().sprite = cardData.FullCardSprite ;
        transform.Find("CardGFX").Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = cardData.cardName ; 
    }

	private void Start()
	{
        Init() ;
    }

	private void OnDestroy()
	{
		EventManager.instance.OnCardSelect -= OnCardSelect_DisableThisCard;
		EventManager.instance.OnCardSelectCancel -= OnCardSelectCancel_EnableThisCard;
		EventManager.instance.OnCardPlay -= OnCardPlay_EnableThisCard;
	}

    public CardData GetCardData()
    {
        return cardData;
    }

	public bool IsConiditonMet()
	{
		if (FindObjectOfType<HQ>().money >= cardData.cardCost) 
			return true;

		return false;
	}

	#region EventFunction
	void OnCardPlay_EnableThisCard(Card c)
	{
		if (c == this)
			FindObjectOfType<Deck>().RmCard(c); 
		else
		{
			if (!artSpriteObj.activeSelf)
				artSpriteObj.SetActive(true);
		}
	}

	void OnCardSelectCancel_EnableThisCard(GameObject sender)
	{
		if(! artSpriteObj.activeSelf) artSpriteObj.SetActive(true);
	}

	void OnCardSelect_DisableThisCard(GameObject sender)
	{
		artSpriteObj.SetActive(false);
	}

	#endregion

	private void OnMouseEnter()
	{
		FindObjectOfType<Player>().GetComponent<Player>().MouseEnter_CardPTR = this;
	}

	private void OnMouseExit()
	{
		if (FindObjectOfType<Player>().GetComponent<Player>().MouseEnter_CardPTR == this)
			FindObjectOfType<Player>().GetComponent<Player>().MouseEnter_CardPTR = null;
	}

    public override void RestoreState(object state)
    {
        SaveData data = (SaveData)state ; 

        IsUnlock = data.isUnlock; 
    }

    public override object CaptureState()
    {
 
        return new SaveData {
            isUnlock = IsUnlock 
        };
    }
}
