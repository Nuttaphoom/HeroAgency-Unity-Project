using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 
public class EffectProviderFactorySO : ScriptableObject
{
    public string Name;
    [TextArea(3, 10)]
    public string Detail;
    public Sprite Sprite;
    public string SourceName;
    public VisualEffectSO visualEffectSO;
    public bool SkillStackable;
 
}


public class EffectProviderFactorySO<effectType> : EffectProviderFactorySO where effectType : FactoryObjectRuntime, new()
{
	public effectType CreateEffect(GameObject _producer)
    {
        effectType support = new effectType();
		support.Init(this,_producer);
		return support ; 
	}
} 
 