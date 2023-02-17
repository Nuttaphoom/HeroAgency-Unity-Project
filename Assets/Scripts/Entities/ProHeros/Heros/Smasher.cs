using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smasher : ProHero
{
    [SerializeField]
    private SmasherSkillFactorySO _skillSO;
    private CombatManager _cm;


    public override void PrepareForCombat()
    {
        base.PrepareForCombat();
        _skillSO.CreateEffect(gameObject) ; 
    }


    public void Smash(float bootPercent)
    {
        if (_cm == null)
            _cm = FindObjectOfType<CombatManager>();  

        combatObj.GetComponent<Animator>().SetTrigger("Smash") ;
        StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.25f, 0.2f));

        float damage = ATK + (ATK / 100 * bootPercent);
        foreach (Entity entity in _cm.enemyEntities)
        {
            entity.Damaged(damage, this); 
        }

        foreach (GameObject hero in _cm.heroEntities)
        {
            if (! hero.GetComponent<Smasher>()) hero.GetComponent<Entity>().Damaged(damage, this);
        }

    }
}
