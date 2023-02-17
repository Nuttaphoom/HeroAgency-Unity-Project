using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	private Sprite landSprite;
	private GameObject GUISpriteObj;
	public ItemAsset.TileType tileType;

	public List<Entity> enemyOnThisTile  ;
	public Building buildingOnThisTile  ;

	public List<IEffectProvider> effectOnThisType;
	public int[] indexInMap  ;

	public int _buildingSortingOrder ; //Will be set in the MapGenerator 

	public void addEffect(IEffectProvider e)
	{
		if (effectOnThisType == null)
			effectOnThisType = new List<IEffectProvider>();

 		effectOnThisType.Add(e);
	}

	public void removeEffect(IEffectProvider e)
	{
		if (effectOnThisType == null)
			return;
		effectOnThisType.Remove(e);

	}

	public void AddEnemyOnThisTile(Entity e)
	{
		if (enemyOnThisTile == null)
			enemyOnThisTile = new List<Entity>();

		enemyOnThisTile.Add(e); 
	}
	public void DeleteEnemyOnThisTile(Entity e)
	{
		if (enemyOnThisTile == null)
			return;

		enemyOnThisTile.Remove(e);
        e.DestroyEntity();
   	}
	public bool CanBePlaced()
	{
		if (buildingOnThisTile || tileType != ItemAsset.TileType.Placable)
			return false;
	 

		return true;
	}
	public void CreateBuilding(Building b)
	{
		buildingOnThisTile = Instantiate(b, gameObject.transform) as Building;
		buildingOnThisTile.SetTileSubject(GetComponent<Tile>());

		for (int i = 0; i < Database.sMapHeight; i++)
		{
			for (int j = 0; j < Database.sMapWidth; j++)
			{
				if (buildingOnThisTile.InArea(FindObjectOfType<MapGenerator>().tileMap[i, j].GetComponent<Tile>()))
				{
 					FindObjectOfType<MapGenerator>().tileMap[i, j].GetComponent<Tile>().addEffect(buildingOnThisTile);
				}
			}
		}

		buildingOnThisTile.GetComponent<SpriteRenderer>().sortingOrder = _buildingSortingOrder;
	}

	private void Update()
	{
		 
	}

 	private void Start()
	{
		if (effectOnThisType == null) 
			effectOnThisType = new List<IEffectProvider>();

		EventManager.instance.OnCombatLeave += OnCombatLeave_ClearEnemyOnThisTile;
 		EventManager.instance.OnCardSelectCancel += OnCardSelectCancel_UnHighlightedTile;
		EventManager.instance.OnCardPlay += OnCardPlay_UnHighlightedTile; 
	}

	private void OnDestroy()
	{
		EventManager.instance.OnCombatLeave -= OnCombatLeave_ClearEnemyOnThisTile;
 		EventManager.instance.OnCardSelectCancel -= OnCardSelectCancel_UnHighlightedTile;
		EventManager.instance.OnCardPlay -= OnCardPlay_UnHighlightedTile;


	}

	

	////////////////////////////////
	#region GETTER
	public ItemAsset.TileType getType() { return tileType; }

	#endregion

	#region EventFunction
	private void OnCardPlay_UnHighlightedTile(Card c)
	{
		Higlighted(Color.white); 
 	}

	private void OnCardSelectCancel_UnHighlightedTile(GameObject sender)
	{
		Higlighted(Color.white);
	}

	private void OnCombatLeave_ClearEnemyOnThisTile(int winner)
	{
		for (int i = 0; i < enemyOnThisTile.Count; i++)
		{
			if (enemyOnThisTile[i].HP <= 0 )
			{
				DeleteEnemyOnThisTile(enemyOnThisTile[i]);
				i--;   
			}
		}
	}
	#endregion

	public void SetType(ItemAsset.TileType tag)
	{
		tileType = tag;  
	}

	public void SetGUISprite(Sprite s)
	{
		GUISpriteObj.GetComponent<SpriteRenderer>().sprite = s ; 
	}

	public void SetLandSprite(Sprite s)
	{
		GetComponent<SpriteRenderer>().sprite = s; 
	}

	public void SetBuildingLayer(int buildingLayer)
	{
		_buildingSortingOrder = buildingLayer; 
	}

	private void OnMouseEnter()
	{
		FindObjectOfType<Player>().GetComponent<Player>().MouseEnter_TilePTR = gameObject;

	}

	private void OnMouseExit()
	{
		if (FindObjectOfType<Player>().GetComponent<Player>().MouseEnter_TilePTR == gameObject)
			FindObjectOfType<Player>().GetComponent<Player>().MouseEnter_TilePTR = null;
	}

	public void Higlighted(Color c)
	{
		GetComponent<SpriteRenderer>().color = c ;
	}
}
