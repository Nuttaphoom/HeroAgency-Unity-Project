using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public abstract class Entity : SavableObject
{
    [HideInInspector]
	public float EntityEXP = 0;

	public enum StateMachine
	{
		Attacking,
		Idle,
 		Die,
        Stun,
 	}

	public StateMachine state;
 
	public float HP = 0 ;
	public float ATK = 0; 
	public float SPEED = 0 ;
	public float ACCU = 0 ;
	public float SHIELD = 0;

    [HideInInspector]
	public float MAXHP = 100;
    [HideInInspector]
    public int LEVEL = 1;


 	public float HPPerLevel = 0, ATKPerLevel = 0,
		SPEEDPerLevel = 0, SHIELDPerLevel = 0,
		ACCUPerLevel = 0;

	[HideInInspector]
 	public GameObject combatObj;
	[HideInInspector]
	public GameObject patrollingObj;


	/*Sliders*/
 	public Slider HpSlider ; //Green bar
	public Slider attackDelaySlider; //Yellow bar

	/*For Combating*/
    [HideInInspector]
	public float attackDelay = 0;
	public float delayBetweenAttack = 0;
	protected float delayBetweenAttackCounter = 0;

	/*VisaulEffect SO*/
	[SerializeField]
	private VisualEffectSO visualEffectSO = null;


    /*Skills applied to this entity*/
    protected SkillManager _skillManager;
    [SerializeField]
    private Transform _skillTransform;

    protected bool init = false;
    public virtual void Init()
    {
        if (init)
            return;

        init = true;

        LEVEL = 1;

        combatObj = transform.Find("CombatObj").gameObject;
        patrollingObj = transform.Find("PatrolingObj").gameObject;

        combatObj.SetActive(false);

        _skillManager = new SkillManager();

        ChangeState(StateMachine.Idle);
        SetSlider(HP);

    }

    protected void Start()
	{
        Init();
 	}

    protected void Update()
    {
        Init();

        if (SPEED < 0)
            SPEED = 0;

        if (EntityEXP > LEVEL * LEVEL)
            LevelUp();

        if (state == StateMachine.Die)
        {
            if (HP > 0)
                ChangeState(StateMachine.Idle);
        }
        else if (HP <= 0)
        {
            ChangeState(StateMachine.Die);
        }
        else 
        {
            _skillManager.PlayRunTimeSkill();
        }
    }

	public void LevelUp()
	{
		LEVEL ++;

        EntityEXP = 0; 

 		MAXHP += HPPerLevel; 
		ATK += ATKPerLevel;
		SPEED += SPEEDPerLevel;
		ACCU += ACCUPerLevel;
		SHIELD += SHIELDPerLevel;

        GetHeal(0, 100);
	}

    public virtual void GetHeal(int _healAmount = 0,float _healPercent = 0)
    {
        if (this.HP < 0) this.HP = 0; 

        this.HP += _healAmount;
        this.HP += MAXHP / 100 * _healPercent;

        if (this.HP > MAXHP)
            this.HP = MAXHP; 

        SetHealth(this.HP);
        return;
    }

	public virtual void Attack(Entity target)
	{
		if (state == StateMachine.Attacking) { //In delay time 
			delayBetweenAttackCounter += 1 * Time.deltaTime;
			if (delayBetweenAttackCounter >= delayBetweenAttack)
			{
				ChangeState(StateMachine.Idle);
				delayBetweenAttackCounter = 0; 
			}
		}
		else if (state != StateMachine.Die && state != StateMachine.Stun) //this entity is alive 
		{
			attackDelay +=  SPEED * Time.deltaTime; //Base is SPEED in second (if SPEED = 1, character will hit every 1 sec
			attackDelaySlider.value = attackDelay;

			if (attackDelay >= 0.6)
			{
 				DoDamage(target);  
				
				attackDelay = 0;
			}
		}
	}

	public virtual void DoDamage(Entity target)
	{
		if (visualEffectSO != null)
			new VisualEffectFactorySO().CreateEffect(visualEffectSO, this, target);

		ChangeState(StateMachine.Attacking);
		float miss = Random.Range(0, 101 + target.SPEED); // default is 1 + ACCU in 100 + target.SPEED hit, 
		if (miss >= ACCU) // Miss
			target.Damaged(0,this);
		else
		{
			float atk = ATK ;
			target.Damaged(atk,this);
		}

        _skillManager.PlayOnAttackSkill(this,target);
        CombatManager.instance.SkillManager.PlayOnAttackSkill(this, target);
    }

	public virtual void Damaged(float atk , Entity attacker)
	{
        if (HP <= 0)
            return; 

 		if (combatObj.GetComponent<Animator>()) combatObj.GetComponent<Animator>().SetTrigger("Damaged");

		float damagedDone = atk == 0 ? 0 :   (atk - Random.Range(0, SHIELD)) ;

 		FindObjectOfType<DamageDoneTextFactory>().CreateDamageDoneText(this, damagedDone);

        if (damagedDone < 0)
			damagedDone = 0;

		this.HP -= damagedDone;

        if (this.HP <= 0)
		{
            this.HP = 0;
            StartCoroutine( Die()) ;
		}

        SetHealth(this.HP);

        _skillManager.PlayOnDamagedSkill(attacker, this);
        CombatManager.instance.SkillManager.PlayOnDamagedSkill(attacker, this);

    }


    public void ChangeState(StateMachine nextState)
	{
		delayBetweenAttackCounter = 0;

		if (nextState == StateMachine.Attacking)
		{
			if (combatObj.GetComponent<Animator>())
				combatObj.GetComponent<Animator>().SetTrigger("Attack");
		}


		if (nextState == StateMachine.Idle)
		{
			if (combatObj.activeSelf)
				combatObj.GetComponent<Animator>().SetTrigger("Idle");
			
		}

        if (nextState == StateMachine.Stun)
        {
            if (combatObj.activeSelf)
                combatObj.GetComponent<Animator>().SetTrigger("Idle");
        }

		this.state = nextState;
	}

    public virtual void PrepareForCombat()
    {
        ChangeState(StateMachine.Idle);
        patrollingObj.SetActive(false);
        combatObj.SetActive(true);

        combatObj.transform.Find("Bar Canvas").gameObject.SetActive(true) ;
        

        _skillManager.PlayOnCombatEnterSkill(this);

    }

    public virtual void LeaveCombat()
    {
        attackDelay = 0;
        delayBetweenAttackCounter = 0;
        patrollingObj.SetActive(true);
        combatObj.SetActive(false);
    }

    public void SetSlider(float health)
	{
		MAXHP = health; 
		HpSlider.maxValue = health;
		HpSlider.value = health;
		attackDelaySlider.maxValue = 0.6f; 
	}

	public void SetHealth(float health)
	{
		HpSlider.value = health;
	}

	public void AddSkill(Skill skill)
	{
        _skillManager.AddSkill(skill, Resources.Load("Prefabs/UI/Skill Template") as GameObject, _skillTransform);
    }

	public void RemoveSkill(Skill skill)
	{
        _skillManager.RemoveSkill(skill);
	}

    public void DestroyEntity()
    {
        _skillManager.ClearSkill();
        Destroy(gameObject);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(0.2f);

        Vector3 _diePos = GetComponent<Hero>() ? FindObjectOfType<CombatManager>().GetDiePos().Item1.position  :  FindObjectOfType<CombatManager>().GetDiePos().Item2.position;
        Vector3 _direction = _diePos - combatObj.transform.position; ;

 

        StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.25f, 0.3f));

        var barCanvas = combatObj.transform.Find("Bar Canvas").gameObject ;
        barCanvas.SetActive(false);
        while (Mathf.Abs(_diePos.x - combatObj.transform.position.x) >= 0.01f)
        {
            combatObj.transform.position = Vector3.MoveTowards(combatObj.transform.position,_diePos,45 * Time.deltaTime)  ;
            yield return new WaitForEndOfFrame() ;
        }

        combatObj.SetActive(false);

        var a = Instantiate(Resources.Load<GameObject>("To The Moon Animation"), (GetComponent<Hero>() ? FindObjectOfType<CombatManager>().GetDiePos().Item1 : FindObjectOfType<CombatManager>().GetDiePos().Item2).transform) ;
        a.transform.position = (GetComponent<Hero>() ? FindObjectOfType<CombatManager>().GetDiePos().Item1 : FindObjectOfType<CombatManager>().GetDiePos().Item2).transform.Find("ToTheMoonPos").transform.position ;
        a.transform.rotation = (GetComponent<Hero>() ? FindObjectOfType<CombatManager>().GetDiePos().Item1 : FindObjectOfType<CombatManager>().GetDiePos().Item2).transform.Find("ToTheMoonPos").transform.rotation;


        ChangeState(StateMachine.Die);
     }

    public void StartAttacckAgain(Entity attacker, Entity target)
    {
        StartCoroutine(attackAgain(attacker, target));
    }

    private IEnumerator attackAgain(Entity attacker, Entity target)
    {
        yield return new WaitForSeconds(attacker.delayBetweenAttack);
        attacker.DoDamage(target);

    }

}
