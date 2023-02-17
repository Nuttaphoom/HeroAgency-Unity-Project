using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SupportUnlockTap : MonoBehaviour
{
    [System.Serializable]
    public struct SupportTap
    {
        public Support s1;
        public Image ItemImage;
        public TextMeshProUGUI DetailText;
        public Transform StarPanel;
        public Button Button;
    }

    [SerializeField]
    private List<SupportTap> _supportTapList;

    [SerializeField]
    private GameObject _starTemplate; 
    public void Init(Support s1,Support s2)
    {
        ReputationManager reputationManager = FindObjectOfType<ReputationManager>();
        int level = (int) reputationManager.GetBadgelevel();

        _supportTapList[0].ItemImage.sprite = s1.GetSprite();
        _supportTapList[0].DetailText.text = s1.GetDetail() ;
        _supportTapList[0].Button.onClick.AddListener(() => { BuyThisSupport(s1); });

        _supportTapList[1].ItemImage.sprite = s2.GetSprite();
        _supportTapList[1].DetailText.text = s2.GetDetail();
        _supportTapList[1].ItemImage.GetComponent<Button>().onClick.AddListener(() => { BuyThisSupport(s2); });
        _supportTapList[1].Button.onClick.AddListener(() => { BuyThisSupport(s2); });

        for (int i =0; i <= level; i++)
        {
            GameObject star1 = Instantiate(_starTemplate,_supportTapList[0].StarPanel.transform) ;
            star1.SetActive(true);
            GameObject star2 = Instantiate(_starTemplate, _supportTapList[1].StarPanel.transform);
            star2.SetActive(true);
        }
    }


    public void BuyThisSupport(Support s)
    {
        EventManager.instance.PlayOnBuySupport(s) ;
        Destroy(gameObject);
    }

     






}
