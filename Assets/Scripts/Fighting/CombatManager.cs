using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CombatManager : MonoBehaviour
{
    #region Singleton 
    public static CombatManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    public List<GameObject> dummies = new List<GameObject>();
    public GameObject combat_component_holder;

    private SupportManager _supportManager; 

    [HideInInspector]
    public Tile curTile;
    [HideInInspector]
    public List<Entity> enemyEntities = new List<Entity>();
    [HideInInspector]
    public List<GameObject> heroEntities = new List<GameObject>();


    [SerializeField]
    private PowerUpSkillFactorySO _showOffSkillSO;
    [SerializeField]
    private PowerUpSkillFactorySO _tensefSkillSO;

    [SerializeField]
    private List<Transform> _dieForwardToTransforms;

    private StateMachine _stateMachine = StateMachine.Idle;

    public enum StateMachine
	{
		Idle,
		Begin,
		Combat,
        CombatWithBoss,
 		End,
	}

	enum Tactice
	{
		AGGRESSIVE,
		NORMAL,
		CHILL,
    }

    private Tactice tactice;


	//Skill Manager 
    [HideInInspector]
	public SkillManager SkillManager ;
    [SerializeField]
	private GameObject skillTemplate;
    [SerializeField]
    private Transform skillTemplateTransform; 

	private void Start()
    {
        SkillManager = new SkillManager();
        _supportManager = FindObjectOfType<SupportManager>(); 
        EventManager.instance.OnCombatEnter += OnCombatEnter_BeginCombat;
        EventManager.instance.OnBossEnounter += OnBossEncounter_BeginCombatWithBoss; 
		EventManager.instance.OnCombatLeave += OnCombatLeave_LeaveCombat;
      }

	void OnDestroy()
	{
		EventManager.instance.OnCombatEnter -= OnCombatEnter_BeginCombat;
		EventManager.instance.OnCombatLeave -= OnCombatLeave_LeaveCombat;
	}

	public void OnCombatLeave_LeaveCombat(int winner)
	{
		combat_component_holder.SetActive(false);


		for (int i = 0;  i < enemyEntities.Count; i++)
		{
            enemyEntities[i].LeaveCombat();
   		}

		for (int i  = 0; i < heroEntities.Count; i++)
		{
            heroEntities[i].GetComponent<Entity>().LeaveCombat();
  		}

 

		for (int i = 0; i < curTile.effectOnThisType.Count; i++)
		{
			curTile.effectOnThisType[i].RemoveEffect(curTile.gameObject);
		}
        SkillManager.ClearSkill();
	}


	public void OnCombatEnter_BeginCombat(Tile tile)
	{
		ChangeState(StateMachine.Begin);

		int randomHeroIndex = UnityEngine.Random.Range(0, FindObjectOfType<HeroStock>().heroLists.Count);
		int attemp = 0;

		do
		{
			attemp++;
			randomHeroIndex = UnityEngine.Random.Range(0, FindObjectOfType<HeroStock>().heroLists.Count);
			if (FindObjectOfType<HeroStock>().heroLists[randomHeroIndex].GetComponent<Entity>().HP > 0)
				break;

            if (attemp == 1000){
                FindObjectOfType<HeroStock>().heroLists[randomHeroIndex].GetComponent<Entity>().GetHeal(0, 40.0f);
            }
		} while (attemp < 1000) ;
 
		heroEntities.Clear();
		 
		heroEntities.Add(FindObjectOfType<HeroStock>().heroLists[randomHeroIndex]);

		curTile = tile;

 		EventManager.instance.PlayOnPause(this.gameObject);

        //Tkae building effect before spawn 
		for (int i = 0; i < curTile.effectOnThisType.Count; i++)
		{
			curTile.effectOnThisType[i].TakeEffect_BeforeSpawn(curTile.gameObject) ;
		}

		combat_component_holder.SetActive(true);

 		enemyEntities = tile.enemyOnThisTile ;

        //Spawning characters
        List<int> spawnOrderList = SetSpawnPoints("Enemy");

        for (int i =0; i< enemyEntities.Count; i++)
		{
 			enemyEntities[i].combatObj.transform.position = dummies[spawnOrderList[i]].transform.position;

            enemyEntities[i].PrepareForCombat();
            spawnOrderList = SetSpawnPoints("Enemy");
        }

        spawnOrderList = SetSpawnPoints("Player");

        for (int i = 0; i < heroEntities.Count; i++)
		{
            InitHeroToBattle(heroEntities[i],i);
        }
    
        for (int i = 0; i < _supportManager.GetUnlockedSupport().Count ; i++)
        {
            _supportManager.GetUnlockedSupport()[i].OnCombatEnterSkill(null); 
        }

        //Take building effect
        for (int i = 0; i < curTile.effectOnThisType.Count; i++)
		{
			curTile.effectOnThisType[i].TakeCombatEffect(curTile.gameObject);
		}



 	}

    public void OnBossEncounter_BeginCombatWithBoss(Tile tile)
    {
        ChangeState(StateMachine.CombatWithBoss);

        int randomHeroIndex = UnityEngine.Random.Range(0, FindObjectOfType<HeroStock>().heroLists.Count);
        int attemp = 0;
        List<int> usedNum = new List<int>();
        do
        {
            attemp++;
            randomHeroIndex = UnityEngine.Random.Range(0, FindObjectOfType<HeroStock>().heroLists.Count);
            if (FindObjectOfType<HeroStock>().heroLists[randomHeroIndex].GetComponent<Entity>().HP > 0 && !usedNum.Contains(randomHeroIndex))
            {
                usedNum.Add(randomHeroIndex);
                if (usedNum.Count >= 5)
                    break;
            }

            if (attemp == 1000)
            {
                if (usedNum.Count == 0)
                {
                    usedNum.Add(randomHeroIndex);
                    FindObjectOfType<HeroStock>().heroLists[randomHeroIndex].GetComponent<Entity>().GetHeal(0, 40.0f);
                }
            }
        } while (attemp < 1000);

        heroEntities.Clear();

        foreach (int index in usedNum)
            heroEntities.Add(FindObjectOfType<HeroStock>().heroLists[index]);

        curTile = tile;

        EventManager.instance.PlayOnPause(this.gameObject);

        for (int i = 0; i < curTile.effectOnThisType.Count; i++)
        {
            curTile.effectOnThisType[i].TakeEffect_BeforeSpawn(curTile.gameObject);
        }

        combat_component_holder.SetActive(true);

        enemyEntities = tile.enemyOnThisTile;

        while (enemyEntities.Count > 5)
        {
            enemyEntities.RemoveAt(enemyEntities.Count - 1) ;
        }

        List<int> spawnOrderList = SetSpawnPoints("Enemy");

        for (int i = 0; i < enemyEntities.Count; i++)
        {
            enemyEntities[i].combatObj.transform.position = dummies[spawnOrderList[i]].transform.position;

            enemyEntities[i].PrepareForCombat();
            spawnOrderList = SetSpawnPoints("Enemy");
        }

        spawnOrderList = SetSpawnPoints("Player");

        for (int i = 0; i < heroEntities.Count; i++)
        {
            heroEntities[i].GetComponent<Hero>().combatObj.transform.position = dummies[spawnOrderList[i]].transform.position;

            heroEntities[i].GetComponent<Hero>().PrepareForCombat();
            spawnOrderList = SetSpawnPoints("Player");


        }

        for (int i = 0; i < curTile.effectOnThisType.Count; i++)
        {
            curTile.effectOnThisType[i].TakeCombatEffect(curTile.gameObject);
        }
    }


    private void ChangeState(StateMachine stateToChange)
	{
		this._stateMachine = stateToChange; 
	}


	private void Update()
	{
		UpdateState();
 	}

	#region UpdateAccordingtoStateMachine

 	private void UpdateState()
	{

		if (_stateMachine == StateMachine.Idle)
			return;

		if (_stateMachine == StateMachine.Combat || _stateMachine == StateMachine.CombatWithBoss)
		{
			Combating();
		}

		if (_stateMachine == StateMachine.End)
		{
			ChangeState(StateMachine.Idle); 
		}
 
	}

	private void Combating()
	{
		//Check if there is a winner
		for (int i = 0; i < enemyEntities.Count; i++)
		{
			Entity entity = enemyEntities[i];
			if (entity.HP > 0)
				break;
			else if (i == enemyEntities.Count - 1)
			{
				StartCoroutine(CalculatingResourceGain(0)) ; //Player win 
                return; 
			}
		}

		for (int i = 0; i < heroEntities.Count; i++)
		{
 
			if (heroEntities[i].GetComponent<Entity>().HP > 0)
				break;
			else if (i == heroEntities.Count - 1)
			{
                StartCoroutine(CalculatingResourceGain(1)); //Enemy win 
                return;
			}
		}
		//////////////////////////////////////

 		//Attack each other
		for (int i = 0; i < enemyEntities.Count; i++)
		{
			Entity entity = enemyEntities[i];
			if (entity.HP <= 0)
				continue;
			int index = UnityEngine.Random.Range(0, heroEntities.Count);
			int time = 0;
			while (heroEntities[index].GetComponent<Entity>().state == Entity.StateMachine.Die)
			{
				index = UnityEngine.Random.Range(0, heroEntities.Count);
				if (time > 1000)
					break;
				time++;
			}
			entity.Attack(heroEntities[index].GetComponent<Hero>());
				
		}

		for (int i = 0; i < heroEntities.Count; i++)
		{
			if (heroEntities[i].GetComponent<Entity>().HP <= 0)
				continue;

			int index = UnityEngine.Random.Range(0, enemyEntities.Count);
			int time = 0; 
			while (enemyEntities[index].HP <= 0)
			{
				index = UnityEngine.Random.Range(0, enemyEntities.Count);
				if (time > 1000)
					break;
				time++; 
			}
			heroEntities[i].GetComponent<Entity>().Attack(enemyEntities[index]);
		}

        //////////////////////////
        /// Use Runtime skill
        SkillManager.PlayRunTimeSkill(); 



	}
	#endregion
 

	private IEnumerator  CalculatingResourceGain(int winner)
    {

        if (_stateMachine == StateMachine.CombatWithBoss)
        {
            if (winner == 0)
            {
                EventManager.instance.PlayOnGameWin(gameObject);
            }
            else
            {
                EventManager.instance.PlayOnGameOver(gameObject);
            }
        }
        else
        {
            ChangeState(StateMachine.End);
            yield return new WaitForSeconds(1.25f);

            StartCoroutine(FindObjectOfType<RewardManager>().CalculateReward(0, winner, enemyEntities, heroEntities));
        }


        
    }

    public void AddSkill(Skill skill)
    {
        SkillManager.AddSkill(skill, skillTemplate,  skillTemplateTransform) ;
    }

    public void RemoveSkill(Skill skill)
    {
        SkillManager.RemoveSkill(skill);
    }
	 

	public void Button_Aggresive()
	{
		if (_stateMachine != StateMachine.Begin)
			return;

		tactice = Tactice.AGGRESSIVE;

        foreach (GameObject hero in heroEntities)
        {
            _tensefSkillSO.CreateEffect(hero); 
        }

        RewardManager.instance.IncreaseMultipler(-0.1f, -0.1f);

 		_stateMachine = StateMachine.Combat; 
	}

	public void Button_Normal()
	{
		if (_stateMachine != StateMachine.Begin)
			return;
		tactice = Tactice.NORMAL;

        _stateMachine = StateMachine.Combat;

	}

	public void Button_Chill()
	{
		if (_stateMachine != StateMachine.Begin)
			return;
		tactice = Tactice.CHILL;

        foreach (GameObject hero in heroEntities)
        {
            _showOffSkillSO.CreateEffect(hero);
        }

        RewardManager.instance.IncreaseMultipler(0.1f, 0.1f);

        _stateMachine = StateMachine.Combat;

	}

 
	private List<int> SetSpawnPoints(string forWho) //offset = 0 : enemy , 1 : player
	{
		List<int> spawnOrderList = new List<int>();

		if (forWho == "Enemy")
		{
			if (enemyEntities.Count == 1)
			{
				spawnOrderList.Add(0);
			}
			else if (enemyEntities.Count == 2)
			{
				spawnOrderList.Add(0);
				spawnOrderList.Add(1);
			}
			else if (enemyEntities.Count == 3)
			{
				spawnOrderList.Add(0);
				spawnOrderList.Add(1);
				spawnOrderList.Add(2);
			}
			else if (enemyEntities.Count == 4)
			{
				spawnOrderList.Add(0);
				spawnOrderList.Add(1);
				spawnOrderList.Add(2);
				spawnOrderList.Add(3);
			}
			else if (enemyEntities.Count == 5)
			{
				spawnOrderList.Add(0);
				spawnOrderList.Add(1);
				spawnOrderList.Add(2);
				spawnOrderList.Add(3);
				spawnOrderList.Add(4);
			}
		}else if (forWho == "Player")
		{
			if (heroEntities.Count == 1)
			{
				spawnOrderList.Add(0+5);
			}
			else if (heroEntities.Count == 2)
			{
				spawnOrderList.Add(0+5);
				spawnOrderList.Add(1+5);
			}
			else if (heroEntities.Count == 3)
			{
				spawnOrderList.Add(0+5);
				spawnOrderList.Add(1+5);
				spawnOrderList.Add(2+5);
			}
			else if (heroEntities.Count == 4)
			{
				spawnOrderList.Add(0+5);
				spawnOrderList.Add(1+5);
				spawnOrderList.Add(2+5);
				spawnOrderList.Add(3+5);
			}
			else if (heroEntities.Count == 5)
			{
				spawnOrderList.Add(0+5);
				spawnOrderList.Add(1+5);
				spawnOrderList.Add(2+5);
				spawnOrderList.Add(3+5);
				spawnOrderList.Add(4+5);
			}
		}
		return spawnOrderList;


	}

	public StateMachine GetState()
	{
		return _stateMachine  ;
	}

    public (Transform , Transform) GetDiePos()
    {
        return (_dieForwardToTransforms[0], _dieForwardToTransforms[1]);
    }

    public void InitHeroToBattle(GameObject hero,int i,bool needToInit = false )
    {
        if (needToInit)
        {
            heroEntities[i] = Instantiate(hero, FindObjectOfType<HeroStock>().transform);
        }

        List<int> spawnOrderList = SetSpawnPoints("Player");
        heroEntities[i].GetComponent<Hero>().Init();

        heroEntities[i].GetComponent<Hero>().combatObj.transform.position = dummies[spawnOrderList[i]].transform.position;

        heroEntities[i].GetComponent<Hero>().PrepareForCombat();
        heroEntities[i].GetComponent<Hero>().combatObj.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 5 - i;
     }

    public bool CanAddMoreHero()
    {
        return heroEntities.Count >= 5 ? false : true; 
    }
 
}
