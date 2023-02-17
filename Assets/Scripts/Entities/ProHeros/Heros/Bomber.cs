using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : ProHero
{
    [SerializeField]
    private BomberSkillFactorySO bomberSkilLFactorySO;

    public override void PrepareForCombat()
    {
        base.PrepareForCombat();

        bomberSkilLFactorySO.CreateEffect(gameObject); 
        
    }

}
