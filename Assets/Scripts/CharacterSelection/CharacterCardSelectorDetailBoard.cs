using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterCardSelectorDetailBoard : MonoBehaviour
{
    public enum Board
    {
        HeroBoard = 0,
        CardBoard = 1
    };

    [SerializeField]
    private List<GameObject> _boardList ;
    [SerializeField]
    private Banner _heroBannerTemplate;
    [SerializeField]
    private Banner _cardBannerTemplate;
    [SerializeField]
    private Transform _heroBannerPanelTF;
    [SerializeField]
    private Transform _cardBannerPanelTF;

    private object _openedItem = null ;
    private SelectedTap _openedTap = null;

    List<List<Banner>> _bannersLists; //0 is for Hero 1 is for Building
    private void Start()
    {
        FindObjectOfType<CharacterCardSelector>().DeSelectTapEvent += OnDeSelectTap_RemoveBanner;
    }

    public void HideBoard(Board b)
    {
        _boardList[(int)b].SetActive(false);
    }

    public void DisplayBoard(SelectedTap item)
    {
        Board b = item.GetRepresented() is Hero ? Board.HeroBoard : Board.CardBoard ;
        _boardList[(int) b].SetActive(true) ; 
        
        if (b == Board.HeroBoard)
        {
            AssignHeroBoard(item) ;
        }else if (b == Board.CardBoard)
        {
            AssignCardBoard(item);
        }
    }

    private void AssignHeroBoard(SelectedTap item)
    {
        Board b = Board.HeroBoard ;
        Hero hero = Database.instance.GetHero(item.GetRepresented() as Hero);

        _openedItem = hero;
        _openedTap = item;

        Transform detailBoardTF = _boardList[(int) b].transform.Find("DetailBoard").transform;
        detailBoardTF.Find("HeroFull Image").GetComponent<Image>().sprite = hero.GetImages().Item1 ;
        detailBoardTF.Find("SkillDetail Text").GetComponent<TextMeshProUGUI>().text = hero.HeroSkillDetail ;
        detailBoardTF.Find("StateValue Text").GetComponent<TextMeshProUGUI>().text = hero.ATK + "\n" + hero.HP + "\n" + hero.SPEED + "\n" + hero.SHIELD + "\n" + hero.ACCU + "%" + "\n\n" + hero.GetCost() ;
        detailBoardTF.Find("HeroNameText").GetComponent<TextMeshProUGUI>().text = hero.GetHeroName(); 
    }

    private void AssignCardBoard(SelectedTap item)
    {
        Board b = Board.CardBoard;
        Card c = item.GetRepresented() as Card ;

        _openedItem = c;
        _openedTap = item;

        Transform detailBoardTF = _boardList[(int)b].transform.Find("DetailBoard").transform;
        detailBoardTF.Find("Card Image").GetComponent<Image>().sprite = c.cardData.FullCardSprite;
        detailBoardTF.Find("BG Board Image").Find("Card Name Text").GetComponent<TextMeshProUGUI>().text = c.cardData.cardName;
        detailBoardTF.Find("BG Board Image").Find("Cost Text").GetComponent<TextMeshProUGUI>().text = "Card Cost : " + c.cardData.cardCost.ToString() ;
        detailBoardTF.Find("BG Board Image").Find("Detail Text").GetComponent<TextMeshProUGUI>().text = c.cardData.cardDescription ;



    }



    public void AcceptButton()
    {
        Board b = _openedItem as Hero ? Board.HeroBoard : Board.CardBoard;
        _boardList[(int)b].SetActive(false);

        if (_bannersLists == null)
        {
            _bannersLists = new List<List<Banner>>();
            _bannersLists.Add(new List<Banner>());
            _bannersLists.Add(new List<Banner>());
        }

        if (b == Board.HeroBoard)
        {
            if (_bannersLists[0].Count >= 6)
                return; 

           Banner newBanner = Instantiate(_heroBannerTemplate, _heroBannerPanelTF);
 
           newBanner.Init(_openedTap);
           newBanner.gameObject.SetActive(true);

           GetComponent<CharacterCardSelector>().PlayAddSelectItem(_openedTap) ;

            if (_bannersLists[0] == null)
                _bannersLists[0] = new List<Banner>();

            _bannersLists[0].Add(newBanner); 
            RearrangeOrder(0); 
        }

        else if (b == Board.CardBoard)
        {
            Banner newBanner = Instantiate(_cardBannerTemplate, _cardBannerPanelTF);
            newBanner.Init(_openedTap);
            newBanner.gameObject.SetActive(true);

            GetComponent<CharacterCardSelector>().PlayAddSelectItem(_openedTap);

            if (_bannersLists[1] == null)
                _bannersLists[1] = new List<Banner>();

            _bannersLists[1].Add(newBanner);
            RearrangeOrder(1); 
        }

    }

    public void CloseOpenedBoard()
    {

        Board b = _openedItem as Hero ? Board.HeroBoard : Board.CardBoard;
        _boardList[(int)b].SetActive(false);

        if (b == Board.HeroBoard)
        {
            
        }else
        {

        }
    }

    void RearrangeOrder(int index)
    { 
        for (int i = 0; i < _bannersLists[index].Count; i++)
        {
            _bannersLists[index][i].gameObject.transform.SetParent(transform);
        }

        for (int cost = 1; cost <= 100 ; cost++)
        {
            for (int i = 0; i < _bannersLists[index].Count; i++)
            {
                float itemCost = 0 ;
                Transform parentTf = transform ;
                if (index == 0) {
                    itemCost = (_bannersLists[index][i].GetRepresentedItem() as Hero).GetCost() ;
                    parentTf = _heroBannerPanelTF;
                } else
                {
                    itemCost = (_bannersLists[index][i].GetRepresentedItem() as Card).cardData.cardCost;
                    parentTf = _cardBannerPanelTF;
                }

                if (itemCost == cost) _bannersLists[index][i].gameObject.transform.SetParent(parentTf);
            }
        }
    }

    private void OnDeSelectTap_RemoveBanner(SelectedTap tap)
    {
        foreach (Banner t in _bannersLists[0])
        {
            if (t.GetRepresentedItem() == tap.GetRepresented())
            {
                _bannersLists[0].Remove(t);
                return;
            }
                
        }

        foreach (Banner t in _bannersLists[1])
        {
            if (t.GetRepresentedItem() == tap.GetRepresented())
            {
                _bannersLists[1].Remove(t);
                return;
            }

        }
    }

    public void DeselectEveryBoard()
    {
        for (int i = _bannersLists[0].Count - 1; i >= 0; i--)
        {
            FindObjectOfType<CharacterCardSelector>().PlayDeSelectTapEvent(_bannersLists[0][i].GetItem());   
        }

        for (int i = _bannersLists[1].Count - 1; i >= 0; i--)
        {
            FindObjectOfType<CharacterCardSelector>().PlayDeSelectTapEvent(_bannersLists[1][i].GetItem());
        }
    }

}
