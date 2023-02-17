using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Card")]
public class CardData : ScriptableObject
{
	public string cardName;
	public int cardCost; 
	[TextArea(3, 10)]
	public string cardDescription;

    public Sprite FullCardSprite;
    public Sprite BannerCardSprite ; 
}
