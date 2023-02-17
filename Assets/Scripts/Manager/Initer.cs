using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initer : MonoBehaviour //Responsible to call Start function of every 
{
 

    // Start is called before the first frame update
    void Start ()
    {


        FindObjectOfType<MapGenerator>().Init();

        FindObjectOfType<PlayerController>().Init();

        FindObjectOfType<HeroStock>().Init();

        FindObjectOfType<Deck>().Init(); 

        FindObjectOfType<HireManager>().Init();

        FindObjectOfType<UI_Hero_Quick_Detial>().Init();

        FindObjectOfType<Spawner>().OnHeadQuarterEnter_SpawnEnemy(gameObject);

        FindObjectOfType<InventoryManager>().Init();

 



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
