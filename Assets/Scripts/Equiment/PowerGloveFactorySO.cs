using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Equiments/PowerGlove")]
public class PowerGloveFactorySO : EquimentFactorySO<Equiment_PowerGlove>
{
    public float Cooldown;
}

public class Equiment_PowerGlove : Equipment
{
    VisualEffectSO _visualEffectSO;
 
    Entity equiper;
    private float _cooldown;
    private float _timer;
    private bool _readyToHit; 

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is PowerGloveFactorySO)
        {
            PowerGloveFactorySO skillSO = s as PowerGloveFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._cooldown = skillSO.Cooldown;
            this._producerPtr = _producer;
            this._timer = 0;

        }
    }


    public override void OnEquip(Entity equipTo)
    {
        equiper = equipTo;
    }

    public override void FreeSkill()
    {
        _producerPtr.GetComponent<Entity>().RemoveSkill(this);
    }

    public override void RunTimeSkill()
    {
        if (! _readyToHit)_timer += 1 * Time.deltaTime ;
        if (_timer >= _cooldown)
        {
            _timer = 0;
            _readyToHit = true; 
        }
    }

    public override void OnAttackSkill(Entity attacker, Entity target)
    {
        if (_readyToHit)
        {
            attacker.StartAttacckAgain(attacker, target);
            _readyToHit = false;
        }
    }




}
