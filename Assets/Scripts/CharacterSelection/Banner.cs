using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Banner : MonoBehaviour
{

    [SerializeField]
    private object _representedItem ;
    [SerializeField]
    private Image _bannerImageGFX;
    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private SelectedTap _item ;

    private void OnDestroy()
    {
        FindObjectOfType<CharacterCardSelector>().DeSelectTapEvent -= DestroyBanner;
    }

    public void Init(SelectedTap item)
    {
        FindObjectOfType<CharacterCardSelector>().DeSelectTapEvent += DestroyBanner;
        this._item = item;

        _representedItem = item.GetRepresented();
        if (_representedItem as Hero)
        {
            _representedItem = _representedItem as Hero ;
            _bannerImageGFX.sprite = (_representedItem as Hero).GetComponent<Hero>().GetImages().Item3 ;
            _costText.text = (_representedItem as Hero).GetComponent<Hero>().GetCost().ToString() ; 
        }
        else if (_representedItem as Card)
        {
            _representedItem = _representedItem as Card ;
            _bannerImageGFX.sprite = (_representedItem as Card).GetComponent<Card>().GetCardData().BannerCardSprite ;
            _costText.text = (_representedItem as Card).GetComponent<Card>().GetCardData().cardCost.ToString();

        }

        GetComponent<Button>().onClick.AddListener(() =>  RemoveFromSelectedList(item)   );
    }

    public void RemoveFromSelectedList(SelectedTap item)
    {
        FindObjectOfType<CharacterCardSelector>().PlayDeSelectTapEvent(item);
    }

    public void DestroyBanner(SelectedTap item)
    {
        if (item == _item)
        {
            Destroy(gameObject);
        }
         
    }
    public object GetRepresentedItem()
    {
        return _representedItem; 
    }

    public SelectedTap GetItem()
    {
        return _item; 
    }
}
