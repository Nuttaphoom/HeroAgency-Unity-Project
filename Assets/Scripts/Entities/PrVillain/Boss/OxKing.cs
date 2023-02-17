using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxKing : Boss
{
    [SerializeField]
    private OxKingSkillFactoySO _skillSO;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _skillSO.CreateEffect(gameObject);
    }

    new void PrepareForCombat()
    {
        base.PrepareForCombat();
        
    }


}
