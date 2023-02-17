using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SelectedTap  : MonoBehaviour
{
    private Entity RepresentedHero = null;
    private Card RepresentedCard = null ;

    [SerializeField]
    private GameObject _heroTapTemplate ; 
    [SerializeField]
    private GameObject _cardTapTemplate ;
    
    public GameObject SelectTapButton ; 

    bool _canBeSelected = true; 

    public void Init(object obj)
    {
        FindObjectOfType<CharacterCardSelector>().SelectTapEvent += SelectTap_SelectThisTap;
        FindObjectOfType<CharacterCardSelector>().DeSelectTapEvent += DeSelectTap_DeSelectThisTap ;

        bool isEntity = (obj is Entity);

        gameObject.SetActive(true);

        if (isEntity)
        {
            RepresentedHero = (Hero)obj;
            _heroTapTemplate.SetActive(true);
            SelectTapButton.GetComponent<Image>().sprite = RepresentedHero.GetComponent<Hero>().GetImages().Item1;
        }

        else
        {
            RepresentedCard = (Card) obj;
 
            _cardTapTemplate.SetActive(true);
            SelectTapButton.GetComponent<Image>().sprite = RepresentedCard.GetComponent<Card>().GetCardData().FullCardSprite ;
        }

    }

    public object GetRepresented()
    {
        if (RepresentedCard != null)
            return RepresentedCard;
        else
            return RepresentedHero;
    }

    private void SelectTap_SelectThisTap(SelectedTap selectedTap)
    {
        if (selectedTap != this)
            return ;

        _canBeSelected = false;

        if (RepresentedHero)
        {
            SelectTapButton.GetComponent<Image>().color = Color.gray ;
        }
        else if (RepresentedCard)
        {
            SelectTapButton.GetComponent<Image>().color = Color.gray;
        }
    }

    private void DeSelectTap_DeSelectThisTap(SelectedTap selectedTap)
    {
 
        if (selectedTap != this)
            return;

         _canBeSelected = true ;

        if (RepresentedHero)
        {
            SelectTapButton.GetComponent<Image>().color = Color.white ;
        }
        else if (RepresentedCard)
        {
            SelectTapButton.GetComponent<Image>().color = Color.white;
        }
    }

    public bool CanBeSelected()
    {
        return _canBeSelected;
    }
}
