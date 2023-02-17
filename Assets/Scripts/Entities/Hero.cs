using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hero : Entity
{
    public string _heroName;
    public int _cost = 0;

    [SerializeField]
    private Sprite _heroFaceImage;
    [SerializeField]
    private Sprite _heroFullSpriteImage;
    [SerializeField]
    private Sprite _heroBannerImage; 
     
    [TextArea(10, 10)]
    public string HeroSkillDetail;
    [SerializeField]
    private bool _isUnlocked = false;

    [SerializeField]
    private HeroInventoryManager _heroInventoryManager   ; 

    [Serializable]
    struct HeroSaveData
    {
        public bool IsUnlocked;
    }

  
    public override void Init()
    {
        if (init)
            return;

        base.Init(); 

         _heroInventoryManager = new HeroInventoryManager(this);

    }

    public bool PaySalary()
    {
        if (FindObjectOfType<HQ>().money <= _cost)
        {
            FindObjectOfType<HQ>().IncreaseResource(_cost * -1, 0);
            return false;
        } else
        {
            FindObjectOfType<HQ>().IncreaseResource(_cost * -1, 0);
            return true;
        }


     }

    public bool IsUnlocked()
    {
        return _isUnlocked; 
    }

    public override void RestoreState(object state)
    {
        HeroSaveData hs = (HeroSaveData) state ;
        _isUnlocked = hs.IsUnlocked ;  
    }

    public override object CaptureState()
    {
        return new HeroSaveData
        {
            IsUnlocked = _isUnlocked
        };
    }


    public HeroInventoryManager GetInventoryManager()
    {

        return _heroInventoryManager;
    }

    public (Sprite, Sprite, Sprite) GetImages()
    {
       return (_heroFullSpriteImage, _heroFaceImage, _heroBannerImage);
    }

    public int GetCost()
    {
        return _cost;
    }

    public string GetHeroName()
    {
        return _heroName;
    }

}
