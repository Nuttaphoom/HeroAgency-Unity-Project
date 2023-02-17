using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public enum StateMachine
	{
		Idle,
		SelectingCard,
		ExaminABuilding,
		Combating,
	}	 

	public GameObject MouseEnter_TilePTR ; 
	public Card MouseEnter_CardPTR ;


	public Card selectedCard;
 
	public StateMachine stateMachine = StateMachine.Idle;

	private void Start()
	{
		EventManager.instance.OnCardPlay += OnCardPlay_InstantiateBuilding;
		EventManager.instance.OnCombatEnter += OnCombatEnter_ChangeState;
		EventManager.instance.OnCombatLeave += OnCombatLeave_ChangeState;
	}

	private void OnDestroy()
	{
		EventManager.instance.OnCardPlay -= OnCardPlay_InstantiateBuilding;
		EventManager.instance.OnCombatEnter -= OnCombatEnter_ChangeState;
		EventManager.instance.OnCombatLeave -= OnCombatLeave_ChangeState;
 	}

	private void Update()
	{
		UpdateState();
	}

	private void UpdateState()
	{
		//Reset color of every tile 
		FindObjectOfType<MapGenerator>().HightlightEveryTile(Color.white);

        if (stateMachine == StateMachine.Idle)
        {
            if (MouseEnter_TilePTR && MouseEnter_TilePTR.GetComponent<Tile>().buildingOnThisTile)
            {
                FindObjectOfType<MapGenerator>().HightlightEffectAreaOfThisBuilding(MouseEnter_TilePTR.GetComponent<Tile>().buildingOnThisTile);
                //FindObjectOfType<UI_CardDetailExplain>().UpdateCurrentCard(  );
            }

            //Update Card Explain Board
            FindObjectOfType<UI_CardDetailExplain>().UpdateCurrentCard(MouseEnter_CardPTR);


        }
        else if (stateMachine == StateMachine.SelectingCard)
		{

            //Highlight the tile that can be placed
            for (int i = 0; i < Database.sMapHeight; i++)
				{
					for (int j = 0; j < Database.sMapWidth; j++)
					{
						if (FindObjectOfType<MapGenerator>().tileMap[i, j].GetComponent<Tile>().CanBePlaced())
						{
							FindObjectOfType<MapGenerator>().tileMap[i, j].GetComponent<Tile>().Higlighted(Color.green);
						}
					}
				}
			 
			  if (MouseEnter_TilePTR && MouseEnter_TilePTR.GetComponent<Tile>().CanBePlaced())
			{
				MouseEnter_TilePTR.GetComponent<Tile>().buildingOnThisTile = Instantiate(selectedCard.buildingType, MouseEnter_TilePTR.transform) as Building;
				MouseEnter_TilePTR.GetComponent<Tile>().buildingOnThisTile.SetTileSubject(MouseEnter_TilePTR.GetComponent<Tile>());

				//Highlight the tile that will be affected after place at this tile
				for (int i = 0; i < Database.sMapHeight; i++)
				{
					for (int j = 0; j < Database.sMapWidth; j++)
					{
						if (MouseEnter_TilePTR.GetComponent<Tile>().buildingOnThisTile.InArea(FindObjectOfType<MapGenerator>().tileMap[i, j].GetComponent<Tile>()))
						{
							FindObjectOfType<MapGenerator>().tileMap[i, j].GetComponent<Tile>().Higlighted(Color.yellow);
						}
					}
				}
				Destroy(MouseEnter_TilePTR.GetComponent<Tile>().buildingOnThisTile.gameObject);
				MouseEnter_TilePTR.GetComponent<Tile>().buildingOnThisTile = null; 
			}  
		}
		 
	}

	public void MouseDown()
	{
 		if (stateMachine == StateMachine.Idle)
		{
			if (MouseEnter_CardPTR)
            {
                EventManager.instance.PlayOnPause(this.gameObject);

                stateMachine = StateMachine.SelectingCard;
				selectedCard = MouseEnter_CardPTR; 
				EventManager.instance.PlayOnCardSelect(gameObject);
			}
		}
		else if (stateMachine == StateMachine.SelectingCard)
		{
			if (MouseEnter_TilePTR)
			{
                if (MouseEnter_TilePTR.GetComponent<Tile>().CanBePlaced() && selectedCard.IsConiditonMet())
				{
					EventManager.instance.PlayOnCardPlay(selectedCard); 
				}
 
			}
			else
			{
				stateMachine = StateMachine.Idle;
				EventManager.instance.PlayOnCardSelectCancel(gameObject);
			}
		}else if (stateMachine == StateMachine.Combating)
		{
			
		}

	}
	#region EventFunction 
	private void OnCardPlay_InstantiateBuilding(Card c)
	{
		stateMachine = StateMachine.Idle; 

		MouseEnter_TilePTR.GetComponent<Tile>().CreateBuilding(selectedCard.buildingType); 

		FindObjectOfType<HQ>().IncreaseResource(selectedCard.cardData.cardCost * -1, 0);
	}

	private void OnCombatEnter_ChangeState(Tile tile)
	{
		if (stateMachine == StateMachine.SelectingCard)
			EventManager.instance.PlayOnCardSelectCancel(this.gameObject); 

		stateMachine = StateMachine.Combating; 
	}

	private void OnCombatLeave_ChangeState(int winner)
	{
		stateMachine = StateMachine.Idle; 
	}
	#endregion






}
