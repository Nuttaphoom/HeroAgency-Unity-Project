using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Skill : FactoryObjectRuntime
{
    protected string _name;
    protected string _detail;
    protected Sprite _sprite;
    protected bool available ;
    protected string _sourceName = "unknow";

    public GameObject _producerPtr;

    public abstract void FreeSkill();

    public override void Init(ScriptableObject s, GameObject _producer)
    {

    }

    public override void Init(ScriptableObject s, GameObject _producer, int supportLevel)
    {

    }

    public virtual void RunTimeSkill()
	{

    }

	public virtual void OnAttackSkill(Entity attacker ,Entity target) 
	{
		
	}

	public virtual void OnDamagedSkill(Entity attacker, Entity target)
	{

	}

    public virtual void OnCombatEnterSkill(Entity meEntity)
    {

    }

	public Sprite GetSprite()
	{
		return _sprite;
	}

	public string GetName()
	{
		return _name; 
	}

	public string GetDetail()
	{
		return _detail; 
	}

    public string GetSourceName()
    {
        return _sourceName; 
    }

 
}
