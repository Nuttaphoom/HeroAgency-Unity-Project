using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupportManager : MonoBehaviour
{

    private int _supportSelectReamin = 3 ;

    private List<Support> _supportsForThisLevel;
    private List<Support> _unlockedSupport ;

    private int _level; 
    private HQ _hq;

    [SerializeField]
    private SupportUnlockTap _supportUnlockTapTemplate;

    [SerializeField]
    private SupportUnlockTap _activeSupportUnlockTap; 

    [SerializeField]
    private Transform _supportUnlockParentTransform;

    bool init = false ;

    public void Init()
    {
        if (init)
            return ;

        init = true;

        _unlockedSupport =  new List<Support>() ;

        _hq = FindObjectOfType<HQ>();
        EventManager.instance.OnReputationLevelUp += OnReputationLevelUp_CreateNewSupports;
        EventManager.instance.OnBuySupport += OnBuySupport_AddNewSupport;
    }

 

    private void OnDestroy()
    {
        EventManager.instance.OnReputationLevelUp -= OnReputationLevelUp_CreateNewSupports;
        EventManager.instance.OnBuySupport -= OnBuySupport_AddNewSupport;
    }

    private void Update()
    {
        Init();

        _level = ((int) _hq.reputation / 25) + 1 ;

        foreach (Support s in _unlockedSupport)
        {
            s.RunTimeSkill();
        }
    }



    public void CreateNewSupports(ReputationManager.BadgeLevel lvl = ReputationManager.BadgeLevel.ZeroStar)
    {
        if (_supportSelectReamin <= 0)
            return;


        if (_activeSupportUnlockTap != null)
            Destroy(_activeSupportUnlockTap.gameObject); 

        if (lvl == ReputationManager.BadgeLevel.ZeroStar)
            lvl = FindObjectOfType<ReputationManager>().GetBadgelevel();

        SupportUnlockTap newSupportTaps = Instantiate(_supportUnlockTapTemplate, _supportUnlockParentTransform.transform);
        newSupportTaps.transform.localScale = _supportUnlockTapTemplate.transform.localScale;

        List<Support> s = new List<Support>() ;
        int used = -1;
        for (int j = 0; j < 2; j++)
        {
            while (true)
            {
                int i;
                do
                {
                    i = Random.Range(0, Database.instance.itemAsset.SupportFactorySOs.Count);
                } while (used == i);

                if (i == 0)
                {
                    if ((Database.instance.GetSupportFactorySO<CombatAndroidFactorySO>() as CombatAndroidFactorySO).AvailableLevel[(int)lvl] == false)
                        continue;
                    s.Add((Database.instance.GetSupportFactorySO<CombatAndroidFactorySO>() as CombatAndroidFactorySO).CreateEffect((int)lvl));
                }else if (i == 1)
                {
                    if ((Database.instance.GetSupportFactorySO<EmergencySupportFactorySO>() as EmergencySupportFactorySO).AvailableLevel[(int)lvl] == false)
                        continue;
                    s.Add((Database.instance.GetSupportFactorySO<EmergencySupportFactorySO>() as EmergencySupportFactorySO).CreateEffect((int)lvl));
                } else if (i == 2)
                {
                    if ((Database.instance.GetSupportFactorySO<GoldTruckFactorySO>() as GoldTruckFactorySO).AvailableLevel[(int)lvl] == false)
                        continue;
                    s.Add((Database.instance.GetSupportFactorySO<GoldTruckFactorySO>() as GoldTruckFactorySO).CreateEffect((int)lvl));
                }
                else if (i == 3)
                {
                    if ((Database.instance.GetSupportFactorySO<HealPotionFactorySO>() as HealPotionFactorySO).AvailableLevel[(int)lvl] == false)
                        continue;
                    s.Add((Database.instance.GetSupportFactorySO<HealPotionFactorySO>() as HealPotionFactorySO).CreateEffect((int)lvl));
                }
                else if (i == 4)
                {
                    if ((Database.instance.GetSupportFactorySO<HeroInsuranceSupportFactorySO>() as HeroInsuranceSupportFactorySO).AvailableLevel[(int)lvl] == false)
                        continue;
                    s.Add((Database.instance.GetSupportFactorySO<HeroInsuranceSupportFactorySO>() as HeroInsuranceSupportFactorySO).CreateEffect((int)lvl));
                }
                used = i; 

                break;
            }
        }

        newSupportTaps.Init(s[0],s[1]);

        _activeSupportUnlockTap = newSupportTaps;

        newSupportTaps.gameObject.SetActive(true);
    }

    private void OnReputationLevelUp_CreateNewSupports(GameObject sender)
    {
        CreateNewSupports();
    }

    private void OnBuySupport_AddNewSupport(Support sup)
    {
        if ( ContainSup(sup))
        {
            for (int i = _unlockedSupport.Count - 1; i >=0 ; i--)
            {
                _unlockedSupport.RemoveAt(i);
                _supportSelectReamin += 1;
            }
        }

        _supportSelectReamin -= 1;

        _unlockedSupport.Add(sup);  
    }

    public void PlayCombatEnterSkill_Support()
    {
        foreach (Support s in _unlockedSupport)
        {
            s.OnCombatEnterSkill(null); 
        }
    }

    public bool ContainSup(Support sup)
    {
        foreach (Support s in _unlockedSupport)
        {
            if (s.GetType() == sup.GetType())
                return true;
        }
        return false; 
    }

    #region Getter
    public void SetSupportForThisLevel(List<Support> sups)
    {
        _supportsForThisLevel = sups;
    }

    public int GetSupportLevel()
    {
        return _level ;
    }

    public  List<Support> GetUnlockedSupport()
    {
        return _unlockedSupport; 
    }
    #endregion


}
