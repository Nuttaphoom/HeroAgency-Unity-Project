using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingScarf : ProHero
{
    [SerializeField]
    private FlamingScarfSkillFactorySO _skillSO;

    public override void PrepareForCombat()
    {

        base.PrepareForCombat();
        _skillSO.CreateEffect(gameObject);
    }
}
