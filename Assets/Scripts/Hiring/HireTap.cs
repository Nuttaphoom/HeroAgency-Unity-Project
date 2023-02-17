using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class HireTap : MonoBehaviour
{
	[SerializeField]
	private Image _fullSpriteRenderer;
	[SerializeField]
	private Image _faceSpriteRenderer ;
	[SerializeField]
	private TextMeshProUGUI _nameText;
	[SerializeField]
	private TextMeshProUGUI _statValueText;
    [SerializeField]
    private TextMeshProUGUI _skillExplainerText;

    [HideInInspector]
	public Hero HeroData;

    private void Start()
    {

    }

    private void OnDestroy()
    {

    }

    public void InitHireTap<heroType>(heroType heroT) where heroType : Hero ,new()
	{
        HeroData = Database.instance.GetHero<heroType>(heroT);
        _faceSpriteRenderer.sprite = HeroData.GetImages().Item2;

        gameObject.SetActive(true);



    }

    public void AdjustFaceSpriteTransform(Vector3 moveTo)
    {
         _faceSpriteRenderer.gameObject.transform.position = moveTo;
    }

    public void OnMouseClickOnFaceSprite()
    {
        _nameText.text = HeroData.GetHeroName();
        _statValueText.text = HeroData.ATK + "\n" + HeroData.HP + "\n" + HeroData.SPEED + "\n" + HeroData.SHIELD + "\n" + HeroData.ACCU + "%" + "\n\n" + HeroData.GetCost(); 

        _fullSpriteRenderer.sprite = HeroData.GetImages().Item1;
        _skillExplainerText.text = HeroData.HeroSkillDetail;
        FindObjectOfType<HireManager>().SetHeroData(HeroData);

     } 



 


    
}
