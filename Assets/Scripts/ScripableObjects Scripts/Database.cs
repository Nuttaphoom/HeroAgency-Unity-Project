
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System; 
public class Database : MonoBehaviour
{
	#region Singleton
	public static Database instance;
	private void Awake()
	{

		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
    #endregion

    private void Start()
    {
        EventManager.instance.OnEnterLevel += LoadNewMap;
    }

    private void OnDestroy()
    {
        EventManager.instance.OnEnterLevel -= LoadNewMap;
    }

    public class TileMapData //For Reading Data 12 34 when 12 is TileType, and 34 is the BuildingType 
	{
		public string TileType = "";
		public string BuildingType = "";
	}

	static public int sMapWidth = 10 , sMapHeight = 10;
	static public int sCurLevel; 
	private static TileMapData[,] sMapData = new TileMapData[sMapWidth , sMapHeight];
 	public ItemAsset itemAsset ;
   
 

    public object GetSupportFactorySO<SupportType>(SupportType s = null) where SupportType : SupportFactorySO , new()
    {
        if (s == null)
            s = new SupportType();

        for (int i = 0; i < itemAsset.SupportFactorySOs.Count; i++)
        {
            if (itemAsset.SupportFactorySOs[i].GetType() == s.GetType())
            {
                return itemAsset.SupportFactorySOs[i];
            }

        }
        return null;
    }

 

    public object GetEquipmentFactorySO<EquipmentType>(EquipmentType e = null) where EquipmentType : EquimentFactorySO, new()
    {
        if (e == null)
            e = new EquipmentType() ; 

        for (int i = 0; i < itemAsset.EquimentFactorySOs.Count; i++)
        {
            if (itemAsset.EquimentFactorySOs[i].GetType() == e.GetType())
            {
                return itemAsset.EquimentFactorySOs[i];
            }
        }
        return null; 
    }
    
    public Hero GetHero<HeroType>(HeroType heroT = null ) where HeroType : Hero, new()  
	{
        if (heroT == null)
			heroT = new HeroType();

 		for (int i = 0; i < itemAsset.heros.Count; i++)
		{
			if (itemAsset.heros[i].GetType() == heroT.GetType() )
			{
				return itemAsset.heros[i] ;  
			}
		}
		return null;  
	}

	public GameObject GetCard<BuildingType>(BuildingType buildingType = null) where BuildingType : Building , new()
	{
		if (buildingType == null)
			buildingType = new BuildingType();

		for (int i = 0; i < itemAsset.cardsAssets.Count; i++)
		{
			if (itemAsset.cardsAssets[i].GetComponent<Card>().buildingType.GetType() == buildingType.GetType() )
			{
				GameObject newCard =  itemAsset.cardsAssets[i]  ;
 				return newCard; 
			}
		}
		return null ;
	}
	 
	public void LoadNewMap(Level level)
	{
         LoadMapData(level.GetDataPath()); 

    }


	public void LoadMapData(string path)
	{
 		int curRow = 0, curCol = sMapHeight - 1;

 
        string readFromFilePath = path;

		string s = Resources.Load<TextAsset>(path).text ;
 
        string[] fileLines = s.Split('\n');
        
		string data = "";
		for (int i = 0; i < fileLines.Length; i++)
		{
			string line = fileLines[i];
			for (int j = 0; j < line.Length; j++)
			{
				if (line[j] == ' ' || j == line.Length - 1)
				{
					if (j == line.Length - 1)
						data += line[j];

					if (data.Length > 0)
					{
						sMapData[curCol, curRow] = new TileMapData(); 
						sMapData[curCol, curRow].TileType += data[0];
						sMapData[curCol, curRow].TileType += data[1];
						sMapData[curCol, curRow].BuildingType += data[2];
						sMapData[curCol, curRow].BuildingType += data[3];
						curRow++;
						data = "";
						
					}
				}
				else
				{
					data += line[j];
				}
			}
			curCol--;
			curRow = 0;
		}
	}


	#region Getter
	static public TileMapData[,] GetMapData()
	{
		return sMapData; 
	}



    

    #endregion
}
