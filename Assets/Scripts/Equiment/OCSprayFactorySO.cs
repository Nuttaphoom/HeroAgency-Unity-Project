using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Equiments/OCSpray")]
public class OCSprayFactorySO : EquimentFactorySO<Equiment_OCSpray>
{
   public StunSkillFactorySO StunSkillSO ;
    public float ChnaceToinflict; 
}

public class Equiment_OCSpray : Equipment
{
    VisualEffectSO _visualEffectSO;
    StunSkillFactorySO _stunSkillSO;
    private float _chnaceToinflict; 

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is OCSprayFactorySO)
        {
            OCSprayFactorySO skillSO = s as OCSprayFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._stunSkillSO = skillSO.StunSkillSO;
            this._chnaceToinflict = skillSO.ChnaceToinflict; 
            _producerPtr = _producer;
        }
    }

    public override void OnAttackSkill(Entity attacker, Entity target)
    {
        if (Random.Range(0,100) < _chnaceToinflict) 
            _stunSkillSO.CreateEffect(target.gameObject);
    }
}
