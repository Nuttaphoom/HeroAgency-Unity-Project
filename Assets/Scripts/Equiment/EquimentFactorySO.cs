using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 

public abstract class EquimentFactorySO : EffectProviderFactorySO  
{
    public Sprite inventorySprite;

}

public class EquimentFactorySO<effectType> : EquimentFactorySO where effectType : FactoryObjectRuntime, new() 
{

    public effectType CreateEffect(GameObject _producer)
    {
        effectType support = new effectType();
        support.Init(this, _producer);
        return support;
    }
}



public abstract class Equipment : Skill
{
 
    public virtual void OnEquip(Entity equipTo)
    {

    }

    public override void FreeSkill()
    {
         
    }
}
