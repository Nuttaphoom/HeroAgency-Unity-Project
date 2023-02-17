using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;
using System;
using Random = UnityEngine.Random;
 
[CreateAssetMenu(fileName = "ItemAsset", menuName = "ScriptableObject/ItemAsset")] 
public class ItemAsset : ScriptableObject 
{
	[Serializable]
	public class BuildingAssets
	{
		public BuildingType buildingType;
		public Building building; 
	}

	public enum BuildingType
	{
        HQ = 1,
        HideOut = 2 ,
		Hospital = 3,
		Workshop = 4, 
        OldPowerPlant = 5, 
        RobotStore = 6,
        PoliceStation = 7,
        OldWaterTank = 8,
    }

	public enum TileType
	{
		Road = 0,
		Placable = 1,
		NonInteracble = 2,
		HeadQuarter = 3,
		BossHeadQuarter = 4
 
	};

	public enum LandType
	{
		Glass,
		Desert,
        Snow,
	}

	[Serializable]
	public class Item
	{
		[SerializeField] protected TileType _nameTag;
		[SerializeField] protected Sprite _sprite;
		[SerializeField] protected LandType _landType;

		public TileType getNameTag() { return _nameTag; }
		public Sprite getSprite() { return _sprite; }
		public LandType getLandType() { return _landType; }

        public void SetLandType(LandType _landType)
        {
            this._landType = _landType; 
        }
	};

	public List<GameObject> cardsAssets ;
	public List<BuildingAssets> buildingsAssets ;
    public List<Hero> heros ;

    public List<EquimentFactorySO> EquimentFactorySOs;
    public List<SupportFactorySO> SupportFactorySOs;
    public List<Item> items ;

    [Serializable]
    public class ItemContainer 
    {
        public LandType LandType ;
        public List<Item> Items ; 
    }

    [SerializeField]
    private List<ItemContainer> _itemContainer;


	public List<Sprite> roadSprites;

 
	#region Getter 

	public Item GetItem(TileType nameTag )
	{
		
		GameObject s;
 
		LandType lt = FindObjectOfType<MapGenerator>().levelLandType;

        List<Item> itemWithSameType = new List<Item>();

        for (int i = 0;  i < _itemContainer.Count; i++)
        {
            if (_itemContainer[i].LandType == lt)
            {
                for (int j = 0;  j < _itemContainer[i].Items.Count; j++)
                {
                    if (_itemContainer[i].Items[j].getNameTag() == nameTag)
                        itemWithSameType.Add(_itemContainer[i].Items[j]);
                }
                break;
            }
        }

		if (itemWithSameType.Count == 0)
		{
            Debug.Log("NULL  "+ nameTag );
			return null;
		}

		int RandomIndex = Random.Range(0, itemWithSameType.Count);

		if (Random.Range(1,11) > 6 )
			return itemWithSameType[RandomIndex] ;

		return itemWithSameType[0]; 
	}

	public Building GetBuilding(BuildingType bt)
	{
		for (int i =0; i < buildingsAssets.Count ; i++)
		{
			if (buildingsAssets[i].buildingType == bt)
				return buildingsAssets[i].building; 
		}
 
		return null ;
	}



	#endregion
}
