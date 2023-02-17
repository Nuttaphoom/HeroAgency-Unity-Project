using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Mostly used for Transfering and save/load data to each levels 
//If you want to see more about "in level" hero manager. See in HeroStock.cs
public class CharacterManager : SavableObject 
{
    #region Singleton
    public static CharacterManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);  
    }
    #endregion

    [SerializeField]
    private List<Entity> _heroForThisLevel; //Use in HeroStock.cs
    [SerializeField]
    private List<Entity> _enemyForThisLevel;  //Use in ..

    public void SetHeroForThisLevel(List<Entity> entities)
    {

        if (_heroForThisLevel == null)
            _heroForThisLevel = new List<Entity>();

        _heroForThisLevel.Clear();

        _heroForThisLevel = entities;
        Debug.Log("_heroForThisLevel : " + _heroForThisLevel.Count);

    }

    public void SetEnemyForThisLevel(List<Entity> entities)
    {
        if (_enemyForThisLevel == null)
            _enemyForThisLevel = new List<Entity>();

        _enemyForThisLevel.Clear();

        _enemyForThisLevel = entities;
    }

    public List<Entity> GetHeroForThisLevel()
    {
        return _heroForThisLevel;
    }

    public List<Entity> GetEnemyForThisLevel()
    {
        return _enemyForThisLevel;
    }

    public override void RestoreState(object state)
    {
        List<object> s = (List<object>)state;
        int i = 0;
        foreach (Hero hero in Database.instance.itemAsset.heros)
        {
            hero.RestoreState(s[i]);
            i++;
        }
    }

    public override object CaptureState()
    {
        List<object> s = new List<object>();
 
        foreach (Hero hero in FindObjectOfType<Database>().itemAsset.heros)
        {
            s.Add(hero.CaptureState());
        }

        return s;
    }
}
