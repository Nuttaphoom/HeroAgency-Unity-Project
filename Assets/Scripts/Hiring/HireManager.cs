using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class HireManager : MonoBehaviour
{
	
	private List<Hero> _hireableHeros ;

	private List<HireTap> _hireHeroTap = new List<HireTap>() ;
	public HireTap hireHeroTapTemplate;

    private float offsetX = 61 ;

	private int _curHeroIndex = 0 ;

    private Hero _heroData; 




    public void Init()
	{
        EventManager.instance.OnHireHero += OnHireHero_RemoveHirableHero;
        //Load all hireableHeros
        for (int i = 0; i < _hireableHeros.Count; i++)
		{
			HireTap hireHeroTap = Instantiate(hireHeroTapTemplate, transform.Find("HireBoard").Find("Hero Hire Tap Panel").transform ) as HireTap;

              hireHeroTap.InitHireTap (_hireableHeros[i]) ;

			hireHeroTap.transform.localScale = hireHeroTapTemplate.transform.localScale ;
			hireHeroTap.transform.rotation = hireHeroTapTemplate.transform.rotation ;
			hireHeroTap.transform.position = hireHeroTapTemplate.transform.position ;

             _hireHeroTap.Add(hireHeroTap);

		}
         hireHeroTapTemplate.gameObject.SetActive(false);
 	}

    private void OnDestroy()
    {
        EventManager.instance.OnHireHero -= OnHireHero_RemoveHirableHero;
    }

    public void AddHireableHero(Hero hero)
	{
 		if (_hireableHeros == null) 
			_hireableHeros = new List<Hero>();

		_hireableHeros.Add(hero);
	}

	public void RemoveHireableHero(Hero hero) 
	{
		if (_hireableHeros.Contains(hero))
		{
			_hireableHeros.Remove(hero);
		}
	} 

 

    

    public void OnHireHeroButton()
    {
        FindObjectOfType<HQ>().HireThisHero(_heroData);
        _heroData = null; 
    }

    public void SetHeroData(Hero h)
    {
        _heroData = h; 
    }

    #region EventAdder
    public void OnHireHero_RemoveHirableHero(Entity en)
    {
        Hero h = en as Hero; 
        for (int i = 0;i < _hireHeroTap.Count; i++)
        {
            if (h.GetHeroName() == _hireHeroTap[i].HeroData.GetHeroName() )
            {
                Destroy(_hireHeroTap[i].gameObject);
                _hireableHeros.Remove(_hireHeroTap[i].HeroData);
                _hireHeroTap.Remove(_hireHeroTap[i]);
                break ;
            }
        }

 
     }
    #endregion







}
