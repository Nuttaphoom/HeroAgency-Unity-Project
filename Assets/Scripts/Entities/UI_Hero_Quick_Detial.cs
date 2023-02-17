using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class UI_Hero_Quick_Detial : MonoBehaviour
{
	public class Tap
	{
		public TextMeshProUGUI heroName ;
        public TextMeshProUGUI heroHPText;
        public TextMeshProUGUI heroLevelText;
        public Slider hpSlider;
		public Image heroFaceImage ;
		public GameObject trapObj ;
        public Hero entity; 
        

		public Tap(GameObject trapObj)
		{
			this.trapObj = trapObj;
		}
	}

	private List<Tap> _tapLists ;

	public GameObject tapTemplateObj ;

    private HeroStock _heroStock;

    public void Init()
	{
        if (_tapLists == null) 
        _tapLists = new List<Tap>();

        _heroStock = FindObjectOfType<HeroStock>();
		int i = 0;
		while (_tapLists.Count < FindObjectOfType<HeroStock>().heroLists.Count)
		{
			CreatNewTap(_heroStock.heroLists[i].GetComponent<Hero>() );
			i++; 
		}

        tapTemplateObj.SetActive(false);

        EventManager.instance.OnHireHero += OnHireHero_CreateNewTap;
 
    }

	private void OnDestroy()
	{
		EventManager.instance.OnHireHero -= OnHireHero_CreateNewTap;
    }


    private void Update()
    {
        UpdateEveryTap();
        
    }

    private void OnHireHero_CreateNewTap (Entity en)
	{
		CreatNewTap(en as Hero);
	}

    private void UpdateEveryTap()
    {
        foreach (var tap in _tapLists)
        {
            SetHPOfHerosInTap(tap);
            tap.heroLevelText.text = _heroStock.GetHeroFromStock(tap.entity).LEVEL.ToString();
        }
    }

    private void SetHPOfHerosInTap(Tap tap)
    {
        Hero h = _heroStock.GetHeroFromStock(tap.entity);

        tap.hpSlider.maxValue = h.MAXHP;
        tap.hpSlider.value = h.HP;

        tap.heroHPText.text = (int) h.HP + " / " + (int) h.MAXHP;
    }

    public void CreatNewTap(Hero entity)
	{
        if (_tapLists == null)
            _tapLists = new List<Tap>();


        GameObject _tapObj = Instantiate(tapTemplateObj,transform) as GameObject ;
		Tap _tap = new Tap(_tapObj) ;
        entity.Init();

		_tap.heroName = _tap.trapObj.transform.Find("HeroNameText").GetComponent<TextMeshProUGUI>();
		_tap.heroFaceImage = _tap.trapObj.transform.Find("HeroFaceImage").GetComponent<Image>();
        _tap.hpSlider = _tap.trapObj.transform.Find("HP BAR Holder").GetComponent<Slider>();
        _tap.heroHPText = _tap.trapObj.transform.Find("HP BAR Holder").transform.Find("HP Text").GetComponent<TextMeshProUGUI>();
        _tap.heroLevelText = _tap.trapObj.transform.Find("HeroLevelText").GetComponent<TextMeshProUGUI>();
        _tap.entity = entity; 

        SetHPOfHerosInTap(_tap);

		_tap.heroName.text = entity.GetHeroName() ;
		_tap.heroFaceImage.sprite = entity.GetImages().Item2;
        _tap.heroLevelText.text = entity.LEVEL.ToString() ;

		_tap.trapObj.SetActive(true);

        _tapLists.Add(_tap);

    }




}
