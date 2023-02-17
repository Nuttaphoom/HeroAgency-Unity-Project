using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance ;
    private void Awake()
    {
 

        instance = this; 
    }

    public float RepMultipler = 1;
    public float CoinMultipler = 1;
    private float _previousRepMultipler;
    private float _previousCoinMultipler; 

    public event ResourceCallDelegate OnBeforeCalulateResource ;
    public delegate void ResourceCallDelegate();

    struct HeroRewardTap
    {
        public GameObject TapObj ;
        public TextMeshProUGUI HeroNameText ;
        public TextMeshProUGUI LevelText ;
        public Slider slider;
        public Image FaceImage; 
    }

    struct EquipmentRewardTap
    {
        public GameObject TapObj;
        public TextMeshProUGUI NameText;
        public Image EquipmentImage; 
    }

    [SerializeField]
    private GameObject _heroRewardTemplate;
    [SerializeField]
    private Transform _heroRewardPanel;

    [SerializeField]
    private GameObject _equipmentRewardTemplate;
    [SerializeField]
    private Transform _equipmentRewardPanel ; 

    [SerializeField]
    private GameObject _rewardManagerHolder;

    [SerializeField]
    private TextMeshProUGUI _rewardMoneyText;
    [SerializeField]
    private TextMeshProUGUI _rewardRepText;

 

    private List<HeroRewardTap> heroRewardTaps = new List<HeroRewardTap>() ;
    private List<EquipmentRewardTap> _equipmentRewardTaps = new List<EquipmentRewardTap>() ;

    int winner = -99;

    public IEnumerator CalculateReward(int mode, int winner, List<Entity> enemyEntities, List<GameObject> heroEntities) //0 = Normal -1 = Aggressive, 1 = Chill
    {
        OnBeforeCalulateResource?.Invoke();

        bool repeat = false; 
        do
        {
            repeat = false; 
            foreach (GameObject hero in heroEntities)
            {
                if (hero.GetComponent<SupportHero>())
                {
                    repeat = true; 
                    heroEntities.Remove(hero);
                    Destroy(hero);
                    break; 
                }
            }
        } while (repeat);

        Debug.Log("CoinMultipler : " + CoinMultipler);

        float timeUsedForCalculation = 3.0f ;
        float timeUsed = 0 ; 
        
        bool calculationEnd = false ;
        this.winner = winner;

        float enemyStrenght = CalculationEnemiesStenghtInt(enemyEntities,mode);


        float rewardedMoney = (enemyStrenght + Random.Range(-enemyStrenght / 3.0f , enemyStrenght / 3.0f) ) * 10  * 1.4f * ( winner == 0 ? 1:0) * CoinMultipler  ;
        float increasedMoney = 0;


        float rewardedRep = enemyStrenght * (3 + Random.Range(-enemyStrenght / 3.0f, enemyStrenght / 3.0f) * 10  * 1.5f * (winner == 0 ? 1 : -1)) * RepMultipler ;
        float increasedRep = 0;

        heroRewardTaps = CreateHeroRewardTap(heroEntities);

        _rewardManagerHolder.SetActive(true);

        float increaseEXPAmount = enemyStrenght / 2.0f * 1.25f ;
        float increasedEXP = 0 ;
 
        List<Equipment> rewardedEquipmentLists = new List<Equipment>() ;
        if (winner == 0)
        {
            for (int i = 0; i < enemyStrenght; i++)
            {
                if (Random.Range(0, 10) >= 9)
                { //1 in 10
                    int randomEQ = Random.Range(0, Database.instance.itemAsset.EquimentFactorySOs.Count);
                    if (randomEQ == 0)
                        rewardedEquipmentLists.Add((Database.instance.GetEquipmentFactorySO<OCSprayFactorySO>() as OCSprayFactorySO).CreateEffect(gameObject));
                    else if (randomEQ == 1)
                        rewardedEquipmentLists.Add((Database.instance.GetEquipmentFactorySO<PowerArmourFactorySO>() as PowerArmourFactorySO).CreateEffect(gameObject));
                    else if (randomEQ == 2)
                        rewardedEquipmentLists.Add((Database.instance.GetEquipmentFactorySO<PowerGloveFactorySO>() as PowerGloveFactorySO).CreateEffect(gameObject));
                }
                else
                    break;
            }
        }

        _equipmentRewardTaps = CreateEquipmentRewardTap(rewardedEquipmentLists);

        //Apply Reward
        FindObjectOfType<HQ>().IncreaseResource(rewardedMoney, rewardedRep);

        foreach (Equipment eq in rewardedEquipmentLists)
            FindObjectOfType<InventoryManager>().AddNewItemInInventory(eq);

        //Calculating Animation 
        do
        {
            if (increasedEXP < increaseEXPAmount)
            {
                for (int i = 0; i < heroRewardTaps.Count; i++)
                {
       
                    heroEntities[i].GetComponent<Entity>().EntityEXP += increaseEXPAmount * Time.deltaTime / timeUsedForCalculation;
                    heroRewardTaps[i].slider.value = heroEntities[i].GetComponent<Entity>().EntityEXP;
                    heroRewardTaps[i].slider.maxValue = heroEntities[i].GetComponent<Entity>().LEVEL * heroEntities[i].GetComponent<Entity>().LEVEL;
                    heroRewardTaps[i].LevelText.text = "Level " + heroEntities[i].GetComponent<Entity>().LEVEL; 
                }

                increasedEXP += increaseEXPAmount * Time.deltaTime / timeUsedForCalculation ;
            }

            if (winner == 0)
            {
                if (increasedMoney <= rewardedMoney)
                {
                    increasedMoney += rewardedMoney * Time.deltaTime / timeUsedForCalculation;
                    _rewardMoneyText.text = increasedMoney.ToString();
                }

                if (increasedRep <= rewardedRep)
                {
                    increasedRep += rewardedRep * Time.deltaTime / timeUsedForCalculation;
                    _rewardRepText.text =  increasedRep.ToString();
                }
            }
            else if (winner == 1)
            {
                if (increasedRep >= rewardedRep)
                {
                    increasedRep += -rewardedRep * Time.deltaTime / timeUsedForCalculation;
                    _rewardRepText.text = increasedRep.ToString();
                }
                increasedMoney = 0;
                _rewardMoneyText.text =   increasedMoney.ToString();
            }


            if ((timeUsed += Time.deltaTime) >= timeUsedForCalculation)
            {
                calculationEnd = true ; 
            }


            yield return new WaitForEndOfFrame(); 
        } while (! calculationEnd);

        RepMultipler = 1;
        CoinMultipler = 1;


}

    private List<HeroRewardTap> CreateHeroRewardTap(List<GameObject> heroEntities)
    {
        List<HeroRewardTap> heroRewardTap = new List<HeroRewardTap>() ;

        for (int i = 0; i < heroEntities.Count; i++)
        {
            HeroRewardTap tap = new HeroRewardTap() ; 
            tap.TapObj = Instantiate(_heroRewardTemplate, _heroRewardPanel) ;
            tap.HeroNameText = tap.TapObj.transform.Find("HeroNameText").GetComponent<TextMeshProUGUI>() ; 
            tap.LevelText = tap.TapObj.transform.Find("HeroLevel Text").GetComponent<TextMeshProUGUI>() ; 
            tap.slider = tap.TapObj.transform.Find("EXP Slider").GetComponent<Slider>() ;
            tap.FaceImage = tap.TapObj.transform.Find("Hero Face Image").GetComponent<Image>()  ;

            tap.TapObj.SetActive(true);
            tap.LevelText.text = "Level " + heroEntities[i].GetComponent<Hero>().LEVEL.ToString() ;
            tap.HeroNameText.text = heroEntities[i].GetComponent<Hero>().GetHeroName();
            tap.FaceImage.sprite = heroEntities[i].GetComponent<Hero>().GetImages().Item2;

            tap.slider.maxValue = heroEntities[i].GetComponent<Hero>().LEVEL * heroEntities[i].GetComponent<Hero>().LEVEL ;
            tap.slider.value = heroEntities[i].GetComponent<Hero>().EntityEXP ;

            heroRewardTap.Add(tap);
        }

        return heroRewardTap;
    }

    private List<EquipmentRewardTap> CreateEquipmentRewardTap(List<Equipment> rewardEquipment )
    {
        List<EquipmentRewardTap> taps = new List<EquipmentRewardTap>();
        foreach (Equipment equipment in rewardEquipment)
        {
            EquipmentRewardTap tap = new EquipmentRewardTap() ;
            tap.TapObj = Instantiate(_equipmentRewardTemplate, _equipmentRewardPanel);
            tap.EquipmentImage = tap.TapObj.transform.Find("Equipment Image").GetComponent<Image>();
            tap.NameText = tap.TapObj.transform.Find("Equipment Name Text").GetComponent<TextMeshProUGUI>();

            tap.EquipmentImage.sprite = equipment.GetSprite() ;
            tap.NameText.text = equipment.GetName();

            tap.TapObj.SetActive(true);

            taps.Add(tap);
        }

        return taps; 

    }

    private float CalculationEnemiesStenghtInt(List<Entity> enemyEntities, int mode)
    {
        float result = 0 ;

        foreach (Entity enemy in enemyEntities)
        {
            result += (( (float) enemy.ATK  / (float) enemy.SPEED ) + ((float) enemy.ACCU / 100.0f) + ((float) enemy.HP / 10) ) / 5;
        }

        return result; 
    }

    public void CloseMenuButton()
    {
        for (int i = heroRewardTaps.Count - 1; i >= 0 ; i--)
        {
            Destroy(heroRewardTaps[i].TapObj);
            heroRewardTaps.RemoveAt(i);
        }

        for (int i = _equipmentRewardTaps.Count - 1;  i >= 0; i--)
        {
            Destroy(_equipmentRewardTaps[i].TapObj);
            _equipmentRewardTaps.RemoveAt(i);
        }

        _rewardManagerHolder.SetActive(false);
        EventManager.instance.PlayOnCombatLeave(this.winner) ;
        this.winner = -99;
    }

    public void IncreaseMultipler(float coin,float rep)
    {
        if (CoinMultipler != 0 && CoinMultipler - Mathf.Abs(coin) > 0) CoinMultipler += coin;
        if (RepMultipler != 0 && RepMultipler - Mathf.Abs(rep) > 0) RepMultipler += rep;
    }

    public void SetMultipler(float coin, float rep)
    {
        if (coin >= 0) CoinMultipler = coin;
        if (rep >= 0)  RepMultipler = rep;
         
    }
}
