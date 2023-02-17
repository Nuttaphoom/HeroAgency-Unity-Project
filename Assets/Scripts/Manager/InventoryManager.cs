using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    enum PanelEnum
    {
        Inventory = 0,
        Support = 1,
        EquipedItem = 2,
        HiredHeroList = 3
    };

    [SerializeField]
    private Transform[] _panelTransformList;

    [SerializeField]
    private GameObject[] _itemTemplate;

    [SerializeField]
    private List<GameObject> _equipments; //Can't check what represented equipment is.

    [SerializeField]
    private TextMeshProUGUI _heroStatText;
    [SerializeField]
    private TextMeshProUGUI _heroDetailText;
    [SerializeField] 
    private Image _heroFullImage ;
    [SerializeField]
    private TextMeshProUGUI _skillDetailText;
    [SerializeField]
    private TextMeshProUGUI _heorLevel; 

    private Hero _selectedHero ;
    private HeroStock _heroStock;

    private List<GameObject> equipedItemList;


    public void Init()
    {
        equipedItemList = new List<GameObject>();
        EventManager.instance.OnHireHero += OnHireHero_AddNewHeroInInventory;
        EventManager.instance.OnBuySupport += OnBuySupport_AddSupportToInventory; 
        _heroStock = FindObjectOfType<HeroStock>();
    }

    private void OnDestroy()
    {
        EventManager.instance.OnHireHero -= OnHireHero_AddNewHeroInInventory;
        EventManager.instance.OnBuySupport -= OnBuySupport_AddSupportToInventory;
    }

    public void AddNewItemInInventory(Equipment s)
    {
        int t = 0;
        while  (_equipments.Count >= 10)
        {
            t++;
            if (t >= 1000) break;

            Destroy(_equipments[0].gameObject);
            _equipments.Remove(_equipments[0]);
        }

        GameObject itemObjIcon = Instantiate(_itemTemplate[(int)PanelEnum.Inventory].gameObject, _panelTransformList[(int)PanelEnum.Inventory]);
        itemObjIcon.transform.localPosition = _panelTransformList[(int)PanelEnum.Inventory].transform.localPosition;
        itemObjIcon.transform.Find("Item GFX").GetComponent<Image>().sprite = s.GetSprite() ;
        itemObjIcon.transform.Find("Item Detail Board").transform.Find("Item Name Text").GetComponent<TextMeshProUGUI>().text = s.GetName();
        itemObjIcon.transform.Find("Item Detail Board").transform.Find("Item Detail Text").GetComponent<TextMeshProUGUI>().text = s.GetDetail();

        itemObjIcon.GetComponent<Button>().onClick.AddListener(() => { EquipItemToHero(itemObjIcon.GetComponent<Button>(),s,_selectedHero); });

        

        _equipments.Add(itemObjIcon);

        itemObjIcon.SetActive(true);
    }

 
    public void OnHireHero_AddNewHeroInInventory(Entity hero)
    {
        GameObject heroButton = Instantiate(_itemTemplate[(int)PanelEnum.HiredHeroList].gameObject, _panelTransformList[(int)PanelEnum.HiredHeroList]);
        heroButton.transform.localPosition = _itemTemplate[(int)PanelEnum.HiredHeroList].transform.localPosition;
        heroButton.SetActive(true);

        heroButton.transform.GetComponent<Image>().sprite = (hero as Hero).GetImages().Item2;
 
        Hero s = _heroStock.GetHeroFromStock(hero as Hero);
        heroButton.GetComponent<Button>().onClick.AddListener( () => { SelectHero(s);  }) ;
    }

    public void EquipItemToHero(Button button,Equipment item, Hero hero)
    {
        if (hero.GetInventoryManager().GetItemCount() >= 3)
        {
            StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.25f, 0.2f));
            return; 
        }

        for (int i =0;i < hero.GetInventoryManager().GetItemCount(); i++) {
            if (hero.GetInventoryManager().GetItemInInventory()[i].GetType() == item.GetType())
                return; 
        }
       
        hero.GetInventoryManager().EquipItem(item);
        button.gameObject.SetActive(false);
    }
     
    private void SelectHero(Hero hero)
    {


        _heroDetailText.text = hero.HeroSkillDetail;
        _heroStatText.text = hero.ATK + "\n" + hero.HP + "\n" + hero.SPEED + "\n" + hero.SHIELD + "\n" + hero.ACCU + "%" + "\n\n" + hero.GetCost();
        _skillDetailText.text = hero.HeroSkillDetail ;
        _heorLevel.text = "LVL  " + hero.LEVEL.ToString() ;  
        _heroFullImage.sprite = hero.GetImages().Item2;


        if (_selectedHero != hero || equipedItemList.Count != hero.GetInventoryManager().GetItemCount())
        {
            for (int i = equipedItemList.Count - 1; i >= 0; i--)
            {
                Destroy(equipedItemList[i]);
            }

            equipedItemList.Clear();

            for (int i = 0; i < hero.GetInventoryManager().GetItemCount() ; i++)
            {
                GameObject newObj = Instantiate(_itemTemplate[(int)PanelEnum.EquipedItem].gameObject, _panelTransformList[(int)PanelEnum.EquipedItem]);

                equipedItemList.Add(newObj);
                equipedItemList[i].transform.Find("Item GFX").GetComponent<Image>().sprite = hero.GetInventoryManager().GetItemInInventory()[i].GetSprite();
                equipedItemList[i].transform.Find("Item Detail Board").Find("Item Name Text").GetComponent<TextMeshProUGUI>().text = hero.GetInventoryManager().GetItemInInventory()[i].GetName() ;
                equipedItemList[i].transform.Find("Item Detail Board").Find("Item Detail Text").GetComponent<TextMeshProUGUI>().text = hero.GetInventoryManager().GetItemInInventory()[i].GetDetail() ;

                equipedItemList[i].SetActive(true);
            }

            _selectedHero = hero; 
        }

    }

    private void OnBuySupport_AddSupportToInventory(Support s)
    {
        GameObject supportIcon = Instantiate(_itemTemplate[(int)PanelEnum.Support].gameObject, _panelTransformList[(int)PanelEnum.Support]);
        supportIcon.transform.localPosition = _panelTransformList[(int)PanelEnum.Support].transform.localPosition;
        supportIcon.transform.Find("Item GFX").GetComponent<Image>().sprite = s.GetSprite();
        supportIcon.transform.Find("Item Detail Board").transform.Find("Item Name Text").GetComponent<TextMeshProUGUI>().text = s.GetName();
        supportIcon.transform.Find("Item Detail Board").transform.Find("Item Detail Text").GetComponent<TextMeshProUGUI>().text = s.GetDetail(); 

        supportIcon.SetActive(true);
    }

    private void Update()
    {
        if (_selectedHero != null) SelectHero(_selectedHero);
    }
}

