
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	 
	public List<Enemy> EnemyToSpawnList ;
	public Enemy Boss;

	[SerializeField]
	private int MaxSpawn = 4 ;
    [SerializeField]
	private int MinSpawn = 1 ;

	private List<Enemy> EnemyList = new List<Enemy>();


	void Start()
	{
		EventManager.instance.OnHeadQuarterEnter += OnHeadQuarterEnter_SpawnEnemy;
		EventManager.instance.OnHeadQuarterEnter += OnHeadQuarterEnter_SpawnBoos; 
	}

	private void OnDestroy()
	{
		EventManager.instance.OnHeadQuarterEnter -= OnHeadQuarterEnter_SpawnEnemy;
		EventManager.instance.OnHeadQuarterEnter -= OnHeadQuarterEnter_SpawnBoos;

	}

	private void Update()
	{ 

	}

	public void OnHeadQuarterEnter_SpawnEnemy(GameObject sender)
	{
		int num = 0;
        int attemp = 0;
 		do
		{
            if (EnemyToSpawnList.Count > 0)
            {
                foreach (Tile tile in FindObjectOfType<MapGenerator>().roadMap)
                {
                    if (num == MaxSpawn)
                        break;

                    if (Random.Range(0, 7) == 0) // (0 - 6) = 14.28 % for each road
                    {
                        int spawnThis = Random.Range(0, EnemyToSpawnList.Count);
                        SpawnEnemyOnRoad(tile, spawnThis);
                        num++;
                    }
                }
            }else
            {
                break;
            }

            attemp += 1; 
		} while (num < MinSpawn && attemp < 1000);
	}

	public void SpawnEnemyOnRoad(Tile tile,int spawnThis)
	{
        if (tile.enemyOnThisTile.Count >= 5)
            return;

        Enemy newEnemy = Instantiate(EnemyToSpawnList[spawnThis], new Vector3(0, 0, 0), EnemyToSpawnList[spawnThis].transform.rotation);
        newEnemy.Init();
        newEnemy.patrollingObj.transform.position = tile.transform.position;

        newEnemy.patrollingObj.transform.Find("PatrolGFX").GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2;

        EnemyList.Add(newEnemy);
        tile.AddEnemyOnThisTile(newEnemy);
    }                                                                                                                                          

	public void SpawnEnemyOnRoad(Tile tile, GameObject spawnThisObj)
	{
        if (tile.enemyOnThisTile.Count >= 5)
            return;

        Enemy newEnemy = Instantiate(spawnThisObj.GetComponent<Enemy>(), new Vector3(0, 0, 0), spawnThisObj.transform.rotation) as Enemy;
        newEnemy.Init();

        newEnemy.patrollingObj.transform.position = tile.transform.position;
        newEnemy.patrollingObj.transform.Find("PatrolGFX").GetComponent<SpriteRenderer>().sortingOrder = tile._buildingSortingOrder; 

        EnemyList.Add(newEnemy);
		tile.AddEnemyOnThisTile(newEnemy);

   
    }

	public void OnHeadQuarterEnter_SpawnBoos(GameObject sender)
	{
        Debug.Log(FindObjectOfType<HQ>().reputation + " >= " + FindObjectOfType<HQ>().maxpreputation);
		if (FindObjectOfType<HQ>().reputation >= FindObjectOfType<HQ>().maxpreputation)
		{
            Tile tile = null;
			foreach(GameObject t in FindObjectOfType<MapGenerator>().tileMap)
			{
				if (t.GetComponent<Tile>().tileType == ItemAsset.TileType.BossHeadQuarter)
				{
					tile = t.GetComponent<Tile>() ;
				}
			}

            SpawnEnemyOnRoad(tile,Boss.gameObject);


		}
	}
}
