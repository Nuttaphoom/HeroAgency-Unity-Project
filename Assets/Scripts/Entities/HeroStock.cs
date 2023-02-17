using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStock : MonoBehaviour
{
    public static HeroStock instance;
    private void Awake()
    {
        instance = this; 
    }

    public List<Entity> heroForThisLevel = new List<Entity>(); 
	public List<GameObject> heroLists = new List<GameObject>(); //Hero in stock  

	public void Init()
	{
        SetHEROForThisLevel(CharacterManager.instance.GetHeroForThisLevel());
 
		for (int i = 0; i < heroForThisLevel.Count; i++)
		{
			FindObjectOfType<HireManager>().AddHireableHero(heroForThisLevel[i].GetComponent<Hero>()) ;
		}

        EventManager.instance.OnHeadQuarterEnter += OnHeadQuarterEnter_PaySalary;
        EventManager.instance.OnHireHero += OnHireHero_AddHero;

    }

	private void OnDestroy()
	{
		EventManager.instance.OnHeadQuarterEnter -= OnHeadQuarterEnter_PaySalary;
        EventManager.instance.OnHireHero -= OnHireHero_AddHero;
    }

    public void AddHero<HeroType>(HeroType h) where HeroType : Hero , new()
	{
		GameObject newHero = Instantiate(Database.instance.GetHero(h).gameObject, transform);
 		heroLists.Add(newHero);
	}

	public void RemoveHero(GameObject rmHero)
	{
		heroLists.Remove(rmHero);
	}

	public void RemoveHEROForThisLevel(Entity e)
	{
		heroForThisLevel.Remove(e);
	}

	public void SetHEROForThisLevel(List<Entity> heroForThisLevel_Toadd)
	{ 
		heroForThisLevel.Clear();
		for (int i = 0; i < heroForThisLevel_Toadd.Count; i++)
		{
			heroForThisLevel.Add(heroForThisLevel_Toadd[i] ); 
		}
	}

    public HeroType GetHeroFromStock<HeroType>(HeroType heroT = null) where HeroType : Hero, new()
    {
         if (heroT == null)
            heroT = new HeroType();

        for (int i = 0; i < heroLists.Count; i++)
        {
            if (heroLists[i].GetComponent<HeroType>().GetType() == heroT.GetType())
            {
                return heroLists[i].GetComponent<HeroType>() ;
            }
        }

        return null;
    }

    #region EventAdder
    private void OnHireHero_AddHero(Entity nh)
    {
        AddHero(nh as Hero) ; 
    }

	private void OnHeadQuarterEnter_PaySalary(GameObject sender)
	{
		for (int i = 0; i  < heroLists.Count; i++)
		{
			heroLists[i].GetComponent<Hero>().PaySalary(); 
		} 
	}
    #endregion
}
