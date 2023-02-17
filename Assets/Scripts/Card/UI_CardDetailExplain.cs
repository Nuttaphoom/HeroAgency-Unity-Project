using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_CardDetailExplain : MonoBehaviour
{
	public TextMeshProUGUI cardName_Text;
	public TextMeshProUGUI cardCost_Text;
	public TextMeshProUGUI cardDescription_Text;

	public Card cardPtr ;

	public void UpdateCurrentCard(Card c)
	{
		if (c)
		{
			if (cardPtr != c)
			{
				cardPtr = c;
				SetCardDetailExplain(cardPtr.cardData.cardName, cardPtr.cardData.cardCost.ToString(), cardPtr.cardData.cardDescription);
			}
		}
		else
		{
			cardPtr = null;
			SetCardDetailExplain(" ", " ", " ") ;
		}
	}

	private void SetCardDetailExplain(string cardName, string cardCost, string cardDescription)
	{
		if (cardName != " ")
		{
			cardName_Text.text =  cardName;
			cardCost_Text.text = "Cost : " + cardCost;
			cardDescription_Text.text = cardDescription;
		}else
		{
			cardName_Text.text =  cardName;
			cardCost_Text.text =  cardCost;
			cardDescription_Text.text = cardDescription;
		}
	}

}
