using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LevelDataContainer", menuName = "ScriptableObject/LevelDataContainer")]
public class LevelDataContainer : ScriptableObject
{
	[System.Serializable]
	public class DataForALevel
	{
		private List<Card>  _cardForThisLevel; //Use in Deck.cs 
		private List<Entity>  _heroForThisLevel ; //Use in HeroStock.cs
		private List<Entity> _enemyForThisLevel;  //Use in ..

        public  void SetCardForThisLevel(List<Card> cards)
        {
            if (_cardForThisLevel == null)
                _cardForThisLevel = new List<Card>();
            
            _cardForThisLevel.Clear() ;

            _cardForThisLevel = cards ; 
        }

        public void SetHeroForThisLevel(List<Entity> entities)
        {
            if (_heroForThisLevel == null)
                _heroForThisLevel = new List<Entity>();

            _heroForThisLevel.Clear();

            _heroForThisLevel = entities ;  
        }

        public void SetEnemyForThisLevel(List<Entity> entities)
        {
            if (_enemyForThisLevel == null)
                _enemyForThisLevel = new List<Entity>() ;

            _enemyForThisLevel.Clear();

            _enemyForThisLevel = entities ; 
        }

        public List<Card> GetCardForThisLevel()
        {
            return _cardForThisLevel;  
        }

        public List<Entity> GetHeroForThisLevel()
        {
            return _heroForThisLevel;
        }

        public List<Entity> GetEnemyForThisLevel()
        {
            return _enemyForThisLevel;
        }
	}

	/*public void Init()
	{
 		int CurLevel = Database.sCurLevel;

		FindObjectOfType<Deck>().SetCardForThisLevel(dataForALevel[CurLevel].GetCardForThisLevel()) ;
		FindObjectOfType<HeroStock>().SetHEROForThisLevel(dataForALevel[CurLevel].GetHeroForThisLevel() ); 
	}*/

	public List<DataForALevel> dataForALevel;
}
