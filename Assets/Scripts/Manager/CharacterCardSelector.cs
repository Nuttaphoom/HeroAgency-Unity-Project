using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterCardSelector : MonoBehaviour
{
    [SerializeField]
    private List<SelectedTap> _selectingItems ;

    [SerializeField]
    private List<SelectedTap> _selectedItemList = new List<SelectedTap>() ;

    public event SelectabDel SelectTapEvent; // CharacterCardSelector,SelectedTap;
    public event SelectabDel DeSelectTapEvent; //CharacterCardSelector,SelectedTap , Banner;

    public delegate void SelectabDel(SelectedTap item);
    [SerializeField]
    private SelectedTap _selectTapTemplate = null ;

    [SerializeField]
    private GameObject[] _panelList ;
    private int _curPanel = 0;

    [SerializeField]
    private GameObject _componentHolder;

    private void Start()
    {
        EventManager.instance.OnEnterLevel += OnEnterLevel_LoadSelectedItemIntoLevel;
        DeSelectTapEvent += RemoveSelectedItem;
    }

    private void OnDestroy()
    {
        EventManager.instance.OnEnterLevel -= OnEnterLevel_LoadSelectedItemIntoLevel;
        DeSelectTapEvent -= RemoveSelectedItem;
    }

    public void EnterStage()
    {
        _componentHolder.SetActive(true);

        SelectTapEvent += AddSelectedItem ; 

        foreach (SelectedTap obj in _selectingItems)
        {
            Destroy(obj.gameObject);    
        }
        _selectingItems = new List<SelectedTap>();
 
        //Insert heros and cards in here 
        foreach (Hero hero in Database.instance.itemAsset.heros)
        {
            if (hero.IsUnlocked())
            {
                SelectedTap s = InitSelectedTap(hero);
                _selectingItems.Add(s);
            }
        }
        foreach (GameObject card in Database.instance.itemAsset.cardsAssets)
        {
            Card c = card.GetComponent<Card>();
            if (c.IsUnlock)
            {
                SelectedTap s = InitSelectedTap(c);
                _selectingItems.Add(s);
            }
        }

        foreach (SelectedTap tap in _selectingItems)
        {
            tap.SelectTapButton.GetComponent<Button>().onClick.AddListener(() => DisplayItemDetail(tap));
        }


    }

    private SelectedTap InitSelectedTap(object obj)
    {
        SelectedTap s = Instantiate(_selectTapTemplate, _selectTapTemplate.transform);
         if (obj as Card)
        {
            Card c = (Card)obj; 
            s.Init(Database.instance.GetCard(c.buildingType).GetComponent<Card>());
            s.transform.SetParent(_panelList[1].transform.Find("CardPanel" + c.cardData.cardCost).transform);
        }

        if (obj as Hero)
        {
            Hero hero = (Hero)obj; 
            s.Init(Database.instance.GetHero((Hero)obj)) ;
            s.transform.SetParent(_panelList[0].transform.Find("HeroPanel" + hero.GetCost() ).transform);
        }

        return s; 
    }



    #region Buttons
    public void ShowHeroPanel()
    {
        _curPanel = 0;
        _panelList[_curPanel].SetActive(true);
        _panelList[1].SetActive(false);
    }

    public void ShowCardPanel()
    {
        _curPanel = 1;
        _panelList[_curPanel].SetActive(true);
        _panelList[0].SetActive(false);
    }

    public void OnEnterLevel_LoadSelectedItemIntoLevel(Level lvl)
    {
        List<Entity> enL = new List<Entity>();
        List<GameObject> cardL = new List<GameObject>();

  
        foreach (SelectedTap obj in _selectedItemList)
        {
            object item = obj.GetRepresented();
            if (item as Hero)
            {
                if(! enL.Contains(item as Hero))
                    enL.Add(item as Hero) ;
            }else
            {
                cardL.Add(Database.instance.GetCard((item as Card).buildingType));  
            }
        }

        FindObjectOfType<CharacterManager>().SetHeroForThisLevel(enL) ;   
        FindObjectOfType<CardManager>().SetCardForThisLevel(cardL);

         
    }

    #endregion

    private void DisplayItemDetail(SelectedTap selectedTap) 
    {
        if (!selectedTap.CanBeSelected())
            return; 

        FindObjectOfType<CharacterCardSelectorDetailBoard>().DisplayBoard(selectedTap);
    }

    

    #region EventRegion
    public void PlayAddSelectItem(SelectedTap item)
    {
        SelectTapEvent?.Invoke(item); 
    }

    public void PlayDeSelectTapEvent(SelectedTap item)
    {
        DeSelectTapEvent?.Invoke(item); 
    }

    private void AddSelectedItem(SelectedTap item)
    {
        _selectedItemList.Add(item);
    }

    private void RemoveSelectedItem(SelectedTap item)
    {
         foreach (SelectedTap tap in _selectedItemList)
        {
             if (tap.GetRepresented() as Hero && tap.GetRepresented() as Hero == item.GetRepresented() as Hero)
            {
                _selectedItemList.Remove(tap);  

                break ;
            }
            else if (tap.GetRepresented() as Card && tap.GetRepresented() as Card == item.GetRepresented() as Card)
            {
                _selectedItemList.Remove(tap);  

                break;
            }
        }
    }

    #endregion

    public void PlayButton()
    {
        LevelManager.instance.LoadNextScene();
    }




}
