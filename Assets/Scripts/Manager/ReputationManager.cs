using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationManager : MonoBehaviour
{
    public enum BadgeLevel
    {
        OneStar = 0,
        TwoStar = 1,
        ThreeStar = 2,
        FourStar = 3,
        FiveStar = 4,

        ZeroStar = -1 //ERROR 
    }

    [SerializeField]
    private List<Sprite> _repBadgesImages;
    [SerializeField]
    private GameObject _badgeIconObj;


    private int expForNextBadge = 0 ; 
    private HQ _hq;

    private void Start()
    {
        _hq = FindObjectOfType<HQ>();
     }

    private void Update()
    {
        if (FindObjectOfType<HQ>().reputation   >= expForNextBadge)
            BadgeLevelUp();
        

        if (FindObjectOfType<HQ>().reputation > 100)
            FindObjectOfType<HQ>().reputation = 100; 
        
    }

    public BadgeLevel GetBadgelevel()
    {
        BadgeLevel bl = (BadgeLevel) ( (int) (FindObjectOfType<HQ>().reputation / 25)   ) ;
        return bl ;  
    }

    public Sprite GetBadgeSprite(BadgeLevel bl = BadgeLevel.ZeroStar)
    {
        if (bl == BadgeLevel.ZeroStar)
            bl = GetBadgelevel();
 

        return _repBadgesImages[(int)bl] ;
    }



    private void BadgeLevelUp()
    {
        FindObjectOfType<EventManager>().PlayOnReputationLevelUp(gameObject);
        expForNextBadge += 25;

    }








}
