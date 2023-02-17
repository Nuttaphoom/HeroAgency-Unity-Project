using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningRobot : Enemy
{
    private bool _isSmoked = false;
    [SerializeField]
    private float _smokeDuration;
    private float _timer = 0 ;

    [SerializeField]
    private CleaningSmokeSkillFactorySO _skillSO;

    
    
    public bool IsSmoked()
    {
        return _isSmoked; 
    }

    public void StartHiding()
    {
        if (IsSmoked())
            return;

        _isSmoked = true;
        combatObj.GetComponent<Animator>().SetBool("IsSmoked", _isSmoked) ;
 
    }

    public override void Damaged(float atk, Entity attacker)
    {
        if (_isSmoked && atk < 1000)
            base.Damaged(0, attacker);
        else
            base.Damaged(atk, attacker);
    }

    public void StopHiding()
    {
        if (! IsSmoked())
            return;

        _isSmoked = false ; 
        combatObj.GetComponent<Animator>().SetBool("IsSmoked", _isSmoked);
    }

    new private void Update()
    {
        base.Update() ;

        if ( IsSmoked() ) _timer += 1 * Time.deltaTime;
        if (_timer >= _smokeDuration)
            StopHiding();
    }

    public override void PrepareForCombat() {
        base.PrepareForCombat();
        _skillSO.CreateEffect(gameObject);
    }


}
