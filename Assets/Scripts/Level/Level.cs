using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level : SavableObject 
{
    [SerializeField]
    private int _levelNumber = 0; 
    [SerializeField]
    private string _levelFileDataPath = "Datapath haven't been assigned" ;
    [SerializeField]
    private ItemAsset.LandType _landType = ItemAsset.LandType.Glass ;
    [SerializeField]
    private bool _unlocked = false ; 

    [Serializable]
    private struct LevelSaveData
    {
        public bool Unlocked;
    }

    public void UnLocked()
    {
        _unlocked = true;
    }

    public bool IsUnlocked()
    {
        return _unlocked;
    }

    public (int,string,ItemAsset.LandType) GetLevelData()
    {
        return (_levelNumber, _levelFileDataPath, _landType) ;
    }

    public override void RestoreState(object state)
    {
        LevelSaveData LevelSaveData = (LevelSaveData)state ;

        this._unlocked = LevelSaveData.Unlocked; 
    }

    public override object CaptureState()
    {
        return new LevelSaveData
        {
            Unlocked = _unlocked
        };
    }

    public string GetDataPath()
    {
        return _levelFileDataPath ;
    }

    public ItemAsset.LandType GetLandType()
    {
        return _landType; 
    }

}
