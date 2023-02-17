using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(menuName = "ScriptableObject/Supports/GoldTruck")]
public class GoldTruckFactorySO : SupportFactorySO<GoldTruck_Support>
{
    public float[] IncreasedAmount ;
}

public class GoldTruck_Support : Support
{

    private float _increasedAmount = 0;

    public override void Init(ScriptableObject s, GameObject _producer, int supportLevel)
    {
        if (s is GoldTruckFactorySO)
        {
            GoldTruckFactorySO skillSO = s as GoldTruckFactorySO;
            this._name = skillSO.Name[supportLevel];
            this._detail = skillSO.Detail[supportLevel];
            this._sprite = skillSO.Sprite[supportLevel];
            this._increasedAmount = skillSO.IncreasedAmount[supportLevel] ;


            if (supportLevel == 2)
            {
                curVersion = new GoldTruckL2(_increasedAmount);
            }
            else if (supportLevel == 3)
            {
                curVersion = new GoldTruckL2(_increasedAmount);
            }
            else if (supportLevel == 4)
            {
                curVersion = new GoldTruckL2(_increasedAmount);
            }
            else if (supportLevel == 5)
            {
                curVersion = new GoldTruckL2(_increasedAmount);
            }
        }
    }


}



public class GoldTruckL2 : CombatAndroid_Support
{
    float _increasedAmount = 0;
    
    public GoldTruckL2(float inc)
    {
        _increasedAmount = inc;
        EventManager.instance.OnHeadQuarterEnter += OnHeadQuarterEnter_GoldFromTruck; 
    }

    public override void FreeSupport()
    {
        EventManager.instance.OnHeadQuarterEnter -= OnHeadQuarterEnter_GoldFromTruck;
    }

    private void OnHeadQuarterEnter_GoldFromTruck(GameObject sender)
    {
        HQ.instance.IncreaseResource(_increasedAmount, 0);
    }
}

 

