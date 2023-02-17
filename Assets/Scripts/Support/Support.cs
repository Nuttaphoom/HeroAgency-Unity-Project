using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 



public abstract class SupportFactorySO : ScriptableObject
{
    public string[] Name;
    [TextArea(3, 10)]
    public string[] Detail;
    public Sprite[] Sprite;
    public string[] SourceName  ;
    public bool[] AvailableLevel;
    public VisualEffectSO[] visualEffectSO;
    


 
}

public class SupportFactorySO<effectType> : SupportFactorySO where effectType : FactoryObjectRuntime, new()
{

    public effectType CreateEffect(int levelOfSupport)
    {
        effectType support = new effectType();
        support.Init(this, new GameObject(), levelOfSupport);
        return support;
    }
}

public abstract class Support : Skill
{
    protected Support curVersion ;

    public override void FreeSkill()
    {

    }

    public override void RunTimeSkill()
    {

        if (curVersion != null)
        {
            curVersion.RunTimeSkill();
        }
    }

    public override void OnAttackSkill(Entity attacker, Entity target)
    {
        if (curVersion != null) curVersion.OnAttackSkill(attacker, target);
    }

    public override void OnDamagedSkill(Entity attacker, Entity target)
    {
        if (curVersion != null) curVersion.OnDamagedSkill(attacker, target);
    }

    public override void OnCombatEnterSkill(Entity meEntity)
    {
        if (curVersion != null) curVersion.OnCombatEnterSkill(meEntity);
    }

    public virtual void FreeSupport()
    {

    }
}

