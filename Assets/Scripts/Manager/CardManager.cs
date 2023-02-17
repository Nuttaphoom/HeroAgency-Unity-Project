using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : SavableObject
{
    #region Singleton 
    public static CardManager instance;
    [SerializeField]
    private List<GameObject> _cardForThisLevel; //Use in Deck.cs 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject); 
        }
    }
    #endregion

    

    public void SetCardForThisLevel(List<GameObject> cards)
    {
        Debug.Log("setting card for level cards count : " + cards.Count);
        if (_cardForThisLevel == null)
            _cardForThisLevel = new List<GameObject>();

        _cardForThisLevel.Clear();

        for (int i = 0; i < cards.Count; i++)
        _cardForThisLevel.Add(cards[i]);
        
    }

    public List<GameObject> GetCardForThisLevel()
    {
        return _cardForThisLevel;
    }

    public override void RestoreState(object state)
    {
        List<object> s = (List<object>)state;
        int i = 0;
        foreach (GameObject card in FindObjectOfType<Database>().itemAsset.cardsAssets)
        {
            card.GetComponent<Card>().RestoreState(s[i]);
            i++;
        }
    }

    public override object CaptureState()
    {
        List<object> s = new List<object>();
        foreach (GameObject card in FindObjectOfType<Database>().itemAsset.cardsAssets)
        {
            s.Add(card.GetComponent<Card>().CaptureState());
        }

        return s;
    }
}
