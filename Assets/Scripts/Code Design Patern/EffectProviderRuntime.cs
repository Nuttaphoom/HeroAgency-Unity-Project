using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryObjectRuntime  
{
	public abstract void Init(ScriptableObject s, GameObject _producer);
    public abstract void Init(ScriptableObject s, GameObject _producer,int supportLevel);


}
