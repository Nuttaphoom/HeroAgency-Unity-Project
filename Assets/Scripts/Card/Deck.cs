using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
	private int MaxCard = 5;
	public int ChanceOfDraw = 50;
	public List<Card> cardForThisLevel = new List<Card>(); 

	private List<Card> cardList = new List<Card>();

	public Transform firstCard_transform;
	public Vector2 offsetBetweenCard;

    public void Init()
    {
        SetCardForThisLevel(CardManager.instance.GetCardForThisLevel());

        EventManager.instance.OnCombatLeave += OnCombatLeave_InitNewCard;
 
    }

    private void OnDestroy()
	{
		EventManager.instance.OnCombatLeave -= OnCombatLeave_InitNewCard;
	}

	#region EventFunction
	private void OnCombatLeave_InitNewCard(int winner)
	{
		if (winner == 0)
		{
			if (Random.Range(0,101) <= ChanceOfDraw)  DrawNewCardIntoDeck();
		}
	}
	#endregion
	public void DrawNewCardIntoDeck()
	{
		int randomNum = Random.Range(0, cardForThisLevel.Count);
		var randomCard = cardForThisLevel[randomNum] ;

  		Card newCard = Instantiate(randomCard, Vector3.zero, transform.rotation);

		InsertCard(newCard);
		AdjustTransform(); 
	}

	public void InsertCard(Card c)
	{
        if (cardList.Count == MaxCard)
        {
            Destroy(cardList[0].gameObject);
            cardList.RemoveAt(0);
        }
		cardList.Add(c);
		AdjustTransform(); 
	}

	public void RmCard(Card c)
	{
		cardList.Remove(c);
		Destroy(c.gameObject); 
		AdjustTransform(); 
	}

	private void AdjustTransform()
	{
		for (int i = 0; i < cardList.Count; i++)
		{
			cardList[i].transform.position = new Vector3(firstCard_transform.position.x + offsetBetweenCard.x * i, firstCard_transform.position.y + offsetBetweenCard.y * i
														, firstCard_transform.position.z - 0.0001f * i); 
		}

	}

	public void SetCardForThisLevel(List<GameObject> cardForThisLevel_ToAdd)
	{ 
        if (cardForThisLevel == null)
        {
            cardForThisLevel = new List<Card>();

        }
        cardForThisLevel.Clear(); 

		for (int i = 0; i < cardForThisLevel_ToAdd.Count; i++)
		{
			cardForThisLevel.Add(cardForThisLevel_ToAdd[i].GetComponent<Card>());  
		}
	}
}
