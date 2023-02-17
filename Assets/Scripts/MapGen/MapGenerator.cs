using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

	public GameObject[,] tileMap = new GameObject[Database.sMapHeight, Database.sMapWidth]; //level map   
    [HideInInspector]
    public List<Tile> roadMap = new List<Tile>() ;  //all road tiles in the level
	public ItemAsset.LandType levelLandType; //land type in this level , need to be set in different scene 
	[SerializeField]
	private GameObject tile ;

	private int _buildingLayerOffset = 3; 
	public void Init()
	{
		CreateMap();
	}

	void CreateMap()
	{
        levelLandType = LevelManager.instance.selectedLevel.GetLandType();

        Vector3 startPos = tile.transform.position ;
		Quaternion Rot = tile.transform.rotation ;   
		Database.TileMapData [,] s = Database.GetMapData() ;

 
		//Init every tiles  
		for (int i = 0; i < Database.sMapHeight ; i++)
		{
			for (int j = 0; j < Database.sMapWidth ; j++)
			{
				Vector3 pos = startPos;
				pos.x += j;
				GameObject newTile = Instantiate(tile, pos , Rot )  ;
				newTile.transform.position = new Vector3(pos.x, pos.y, pos.z  );

				string TileTypeString = s[i, j].TileType[0].ToString() + s[i, j].TileType[1].ToString();
				newTile.GetComponent<Tile>().SetType((ItemAsset.TileType)int.Parse(TileTypeString));

				//Remember the index (j,i) = (x,y) 
				newTile.GetComponent<Tile>().indexInMap[0] = i  ;
				newTile.GetComponent<Tile>().indexInMap[1] = j   ;

				//Setting Sorting Layers
				newTile.GetComponent<SpriteRenderer>().sortingOrder = (Database.sMapHeight + Database.sMapWidth - 2 ) -  (i + j + 1)  ;
				newTile.GetComponent<Tile>().SetBuildingLayer( newTile.GetComponent<SpriteRenderer>().sortingOrder + _buildingLayerOffset);

				tileMap[i, j] = newTile;



			}
			startPos.y += 1; 
		}

		//SetLandSprite
		for (int i =0; i < Database.sMapHeight ; i++)
		{
			for (int j = 0; j < Database.sMapWidth; j++)
			{
				string TileTypeString = s[i, j].TileType[0].ToString() + s[i,j].TileType[1].ToString() ;
 
				if (IsWalkableTile(j,i ) )
				{
					tileMap[i, j].GetComponent<Tile>().SetLandSprite(SetRoadSprite(i, j));
                    if (tileMap[i, j].GetComponent<Tile>().getType() == ItemAsset.TileType.Road)  roadMap.Add(tileMap[i, j].GetComponent<Tile>());
				}

				else if (tileMap[i, j].GetComponent<Tile>().getType() == ItemAsset.TileType.Placable)
				{
					tileMap[i, j].GetComponent<Tile>().SetLandSprite(Database.instance.itemAsset.GetItem(ItemAsset.TileType.Placable).getSprite());
 				}

				else if (tileMap[i, j].GetComponent<Tile>().getType() == ItemAsset.TileType.NonInteracble)
				{
					tileMap[i, j].GetComponent<Tile>().SetLandSprite(Database.instance.itemAsset.GetItem(ItemAsset.TileType.NonInteracble).getSprite());
				}
			}
		}

		//CreateBuilding  
		for (int i = 0; i < Database.sMapHeight;i++)
		{
			for (int j = 0; j < Database.sMapWidth; j++)
			{
				Building b;
				string buildingTypeString = s[i,j].BuildingType[0].ToString() + s[i,j].BuildingType[1].ToString() ; 
				if ((b = Database.instance.itemAsset.GetBuilding((ItemAsset.BuildingType)int.Parse(buildingTypeString))) != null)
					tileMap[i, j].GetComponent<Tile>().CreateBuilding(b); 
			}
		}

	}

	public bool IsWalkableTile(int j /*y*/, int i /*x*/)
	{
 		if (tileMap[i, j].GetComponent<Tile>().getType() == ItemAsset.TileType.HeadQuarter)
			return true;
		if (tileMap[i, j].GetComponent<Tile>().getType() == ItemAsset.TileType.Road)
			return true;

		if (tileMap[i, j].GetComponent<Tile>().getType() == ItemAsset.TileType.BossHeadQuarter)
			return true;
		return false; 
	}

	public void HightlightEveryTile(Color c)
	{
		for (int i = 0; i < Database.sMapHeight; i++)
		{
			for (int j = 0; j < Database.sMapWidth; j++)
			{
				tileMap[i, j].GetComponent<Tile>().Higlighted(c);
			}
		}
	}


	public void HightlightEffectAreaOfThisBuilding(Building bd)
	{
		for (int i = 0; i < Database.sMapHeight; i++)
		{
			for (int j = 0; j < Database.sMapWidth; j++)
			{
 				if (FindObjectOfType<Player>().MouseEnter_TilePTR.GetComponent<Tile>().buildingOnThisTile.InArea(FindObjectOfType<MapGenerator>().tileMap[i, j].GetComponent<Tile>()))
				{
					tileMap[i, j].GetComponent<Tile>().Higlighted(Color.yellow);
				}
			}
		}
	}

	public void UnHightlightEffectAreaOfThisBuilding()
	{
		for (int i = 0; i < Database.sMapHeight; i++)
		{
			for (int j = 0; j < Database.sMapWidth; j++)
			{
				tileMap[i, j].GetComponent<Tile>().Higlighted(Color.white);
			}
		}
	}

	private Sprite SetRoadSprite(int y, int x)
	{
		int TopY = Database.sMapHeight - 1, BotY = 0, LeftX = 0, RightX = Database.sMapWidth - 1;

		bool[] side = { false, false, false, false }; //TOP BOTTOM LEFT RIGHT;
		//Check TOP
		if (y + 1 <= TopY)
		{
			if (IsWalkableTile(x, y + 1))
			{
				side[0] = true; 
			} 
		}
		//Check Bottom
		if (y - 1 >= BotY)
		{
			if (IsWalkableTile(x, y - 1))
			{
				side[1] = true; 
			}
		}


		//Check Left
		if (x - 1 >= LeftX)
		{
			if (IsWalkableTile(x - 1, y))
			{
				side[2] = true; 
			}
		}
		//Check Right
		if (x + 1 <= RightX)
		{
			if (IsWalkableTile(x + 1, y))
			{
				side[3] = true; 
			}
		}

		int selectedSprite = 0;

		if (side[0]) //0 1 6 7
		{
			if (side[3])
				selectedSprite = 0;
			else if (side[2])
				selectedSprite = 1;
			else
				selectedSprite = 7;
		}

		else if (side[1]) //4 5
		{
			if (side[3])
				selectedSprite = 4;
			else if (side[2])
				selectedSprite = 5; 
		}

		else if (side[2]) // 3
		{
			selectedSprite = 3;
		}

 

		return Database.instance.itemAsset.roadSprites[selectedSprite]; 
	}

	

}
