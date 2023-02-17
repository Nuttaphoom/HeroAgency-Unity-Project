using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroCat : ProHero
{
    [SerializeField]
    ElectricCatSkillFactorySO _skillSO;

    public override void PrepareForCombat()
    {
        base.PrepareForCombat();

        _skillSO.CreateEffect(gameObject); 

    }

}
