using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System ;

public abstract class SavableObject : MonoBehaviour
{
    public string SaveID = null ;

    abstract public void RestoreState(object state);
    abstract public object CaptureState();

    [ContextMenu("GenID")]
    public void GenerateID() => SaveID = Guid.NewGuid().ToString();

 }
