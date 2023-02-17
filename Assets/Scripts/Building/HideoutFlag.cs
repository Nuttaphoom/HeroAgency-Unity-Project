using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideoutFlag : Building
{
    public override bool InArea(Tile tile)
    {
        return false; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
