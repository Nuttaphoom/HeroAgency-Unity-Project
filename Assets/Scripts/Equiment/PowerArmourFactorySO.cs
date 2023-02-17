using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Equiments/PowerArmour")]
public class PowerArmourFactorySO : EquimentFactorySO<Equiment_PowerArmour>
{
    public float InflictChance;
    public float Cooldown;
    public PowerUpSkillFactorySO PowerUpSkillFactorySO;
}

public class Equiment_PowerArmour : Equipment
{
    VisualEffectSO _visualEffectSO;
    PowerUpSkillFactorySO _skillSO;
 
    Entity equiper;
    private float _inflictChance;
    private float _cooldown;
    private float _timer;

    public override void Init(ScriptableObject s, GameObject _producer)
    {
        if (s is PowerArmourFactorySO)
        {
            PowerArmourFactorySO skillSO = s as PowerArmourFactorySO;
            this._name = skillSO.Name;
            this._detail = skillSO.Detail;
            this._sprite = skillSO.Sprite;
            this._visualEffectSO = skillSO.visualEffectSO;
            this._skillSO = skillSO.PowerUpSkillFactorySO;
            this._inflictChance = skillSO.InflictChance;
            this._cooldown = skillSO.Cooldown;
            this._producerPtr = _producer; 

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
        _timer += 1 * Time.deltaTime; 
    }

    public override void OnDamagedSkill(Entity attacker, Entity target)
    {
        int i = Random.Range(0, 100);

        Debug.Log("power arm i : " + i + "_inflictChance : " + _inflictChance);

        if (i <= _inflictChance && _timer >= _cooldown)
        {
            _skillSO.CreateEffect(equiper.gameObject);
            _timer = 0; 
        }
    }
 

}
