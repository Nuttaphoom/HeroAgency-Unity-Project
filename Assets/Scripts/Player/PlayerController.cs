using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	int TopY = Database.sMapHeight - 1, BotY = 0, LeftX = 0, RightX = Database.sMapWidth - 1 ;

	public int LoopCount = 0; 
	public bool Pause = false ;

	[SerializeField]
	private float speed = 1; 

	private MapGenerator mapGenerator;
	private GameObject headQuarterTile;
 

	public int[] currentPosition = { -1, -1 } ;
	private int[] previousPosition = { -1,-1} ;
	private int[] targetPosition = { -1,-1};

    [SerializeField]
    private GameObject _playerGFX ;

	List<Support> supports = new List<Support>() ;


    private SpeedManager _speedManager;

    public void Init()
	{
		mapGenerator = FindObjectOfType<MapGenerator>();

        _speedManager = FindObjectOfType<SpeedManager>();

        for (int i =0; i < Database.sMapHeight ; i++)
		{
			for (int j = 0; j < Database.sMapWidth ; j++)
			{
				GameObject t = FindObjectOfType<MapGenerator>().tileMap[i, j];
				if (t.GetComponent<Tile>().getType() == ItemAsset.TileType.HeadQuarter)
				{
					headQuarterTile = t;
					transform.position = headQuarterTile.transform.position;
					currentPosition[0] = i; currentPosition[1] = j;
			}
			}
		}

		EventManager.instance.OnPause += OnPause_StopPatrolling;
		EventManager.instance.OnStopPause += OnPause_StopPausing;		 
  	}

	#region Event
	public void OnPause_StopPatrolling(GameObject sender)
    {
        Pause = true ;
	}

	void OnPause_StopPausing(GameObject sender)
	{
        if (CombatManager.instance.GetState() == CombatManager.StateMachine.Combat)
            return; 

		Pause = false;
	}
	#endregion

	// Update is called once per frame
	void Update()
    {
		if (Pause)
			return;

        
		if (FindObjectOfType<HeroStock>().heroLists.Count > 0)UpdatePath();
	}

	void UpdatePath()
	{
		if (targetPosition[0] == -1 || targetPosition[1] == -1)
			FindTarget();
		else
		{
			GameObject targetObj = FindObjectOfType<MapGenerator>().tileMap[targetPosition[0], targetPosition[1]];

			transform.position = Vector3.MoveTowards(transform.position, targetObj.transform.position, speed * _speedManager.GetSpeedMultipler() * Time.deltaTime);

			if (targetObj.GetComponent<SpriteRenderer>().sortingOrder > _playerGFX.GetComponent<SpriteRenderer>().sortingOrder - 2)
                _playerGFX.GetComponent<SpriteRenderer>().sortingOrder = targetObj.GetComponent<SpriteRenderer>().sortingOrder + 2;
			
			if (Vector3.Distance(targetObj.transform.position, transform.position) < 0.05f)
			{
                _playerGFX.GetComponent<SpriteRenderer>().sortingOrder = targetObj.GetComponent<SpriteRenderer>().sortingOrder + 2;
				ReachNewTile();
			}
		}
	}

	void ReachNewTile()
	{
		previousPosition[0] = currentPosition[0];
		previousPosition[1] = currentPosition[1];

		currentPosition[0] = targetPosition[0];
		currentPosition[1] = targetPosition[1];

		targetPosition[0] = -1;
		targetPosition[1] = -1;

		Tile curTile = FindObjectOfType<MapGenerator>().tileMap[currentPosition[0], currentPosition[1]].GetComponent<Tile>() ;

		//Taking effect area 
		for (int i = 0; i < Database.sMapHeight; i++)
		{
			for (int j = 0; j < Database.sMapWidth; j++)
			{
				Tile targetTile = FindObjectOfType<MapGenerator>().tileMap[i, j].GetComponent<Tile>();
				if (targetTile.buildingOnThisTile)
				{
					if (targetTile.buildingOnThisTile.InArea(curTile))
					{
						targetTile.buildingOnThisTile.TakeAreaEffect(targetTile.gameObject);
					}
				}
			}
		}

		if (curTile.tileType == ItemAsset.TileType.HeadQuarter)
		{
			LoopCount++; 
			EventManager.instance.PlayOnHeadQuaterEnter(this.gameObject);
			//Use TakeEffect_OnHeadQuarterEnter in every tiles
			
			for (int i = 0; i < Database.sMapHeight; i++)
			{
				for (int j = 0; j < Database.sMapWidth; j++)
				{
					GameObject tile = FindObjectOfType<MapGenerator>().tileMap[i, j];
					foreach (IEffectProvider eff in tile.GetComponent<Tile>().effectOnThisType) { 
						eff.TakeEffect_OnHeadQuarterEnter(tile);
					}
				}
			}
		}

		if (curTile.enemyOnThisTile.Count > 0 )
        {
            foreach (Enemy en in curTile.enemyOnThisTile)
            {
                if (en.GetComponent<Boss>())
                {
                    Debug.Log("boss encounter");
                    EventManager.instance.PlayOnBossEncounter(curTile);
                    return;
                }
            }

 			EventManager.instance.PlayOnCombatEnter(curTile);
		}
 


	}

	void FindTarget()
	{
		/*Find how we walk last time*/
		int lastTimeWalk = -1; //Top,Bottom,Left , Right

		if (previousPosition[0] < currentPosition[0]) // Walk Top last time
			lastTimeWalk = 0; 
		
		else if (previousPosition[0] > currentPosition[0]) // Walk Bottom last time
			lastTimeWalk = 1; 
		
		else if (previousPosition[1] < currentPosition[1]) //Walk Right last time 
			lastTimeWalk = 2; 
		
		else if (previousPosition[1] > currentPosition[1]) // Walk left last time 
			lastTimeWalk = 3;

		if (currentPosition[0] + 1 <= TopY && lastTimeWalk != 1) //Go Up 
		{
			if (FindObjectOfType<MapGenerator>().IsWalkableTile(currentPosition[1], currentPosition[0] + 1))
			{
				targetPosition[1] = currentPosition[1];
				targetPosition[0] = currentPosition[0] + 1;
			}
		}

		if (currentPosition[1] - 1 >= LeftX && lastTimeWalk != 2) // Go Left
		{
			if (FindObjectOfType<MapGenerator>().IsWalkableTile(currentPosition[1] - 1, currentPosition[0]))
			{
				targetPosition[1] = currentPosition[1] - 1;
				targetPosition[0] = currentPosition[0];
			}
		}

		if (currentPosition[0] - 1 >= BotY && lastTimeWalk != 0) // Go Down 
		{
			if (FindObjectOfType<MapGenerator>().IsWalkableTile(currentPosition[1], currentPosition[0] - 1))
			{
				targetPosition[1] = currentPosition[1];
				targetPosition[0] = currentPosition[0] - 1;
			}
		}

		if (currentPosition[1] + 1 <= RightX && lastTimeWalk != 3) // Go Right 
		{
			if (FindObjectOfType<MapGenerator>().IsWalkableTile(currentPosition[1] + 1, currentPosition[0]))
			{
				targetPosition[1] = currentPosition[1] + 1;
				targetPosition[0] = currentPosition[0];
			}
		}

		if (previousPosition[0] == -1)
        {

            if (currentPosition[1] - 1 >= BotY)
			{
				if (FindObjectOfType<MapGenerator>().IsWalkableTile(currentPosition[0], currentPosition[1] - 1))
                {
                    targetPosition[0] = currentPosition[0];
					targetPosition[1] = currentPosition[1] - 1;
				}
			}
			if (currentPosition[0] + 1 <= RightX)
			{

                if (FindObjectOfType<MapGenerator>().IsWalkableTile(currentPosition[0] + 1, currentPosition[1]  ))
                {
                    Debug.Log("2");

                    targetPosition[0] = currentPosition[0] + 1;
					targetPosition[1] = currentPosition[1];
				}
			}

			if (currentPosition[0] - 1 >= LeftX)
			{

                if (FindObjectOfType<MapGenerator>().IsWalkableTile(currentPosition[0] - 1, currentPosition[1]))
                {
                    Debug.Log("3");

                    targetPosition[0] = currentPosition[0] - 1;
					targetPosition[1] = currentPosition[1];
				}
			}


		}
	}
}
