using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSettings{

	private static GameSettings gameSetting;
	private GameSettings(){}
	public static GameSettings Instance(){

		if(gameSetting == null){

			gameSetting = new GameSettings();
		}
		return gameSetting;
	}


	public enum GameDifficulty {Easy, Medium, Hard}
	public GameDifficulty difficulty = GameDifficulty.Easy;
	//private string[] easyDifficulty = {"Joejoe1","Joejoe2","Joejoe3","Fleak1","Fleak2","Fleak3","Morphy1",/*"Morphy2","Morphy3",*/"Morphy4","Morphy5"};
	private string[] easyDifficulty = {"Joejoe1","Joejoe2","Joejoe3","Joejoe5","Joejoe6","Joejoe7","Joejoe8","Fleak1","Fleak2","Fleak3","Fleak4",
		"Fleak5","Fleak6","Fleak7","Fleak8","Morphy1","Morphy2","Morphy3","Morphy4","Morphy5"};
//	public string[] temp = easyDifficulty;
	//private string[] easyDifficulty = {"Blue" ,"Gold","Green"};
	//private string[] mediumDifficulty= {"Blue" ,"Gold","Green","Pink","Purple"};
	//private string[] hardDifficulty= {"Blue" ,"Gold","Green","Pink","Purple", "Red","Teal"};

	public List<string> CardTypes{

		get{

			List<string> tempList = new List<string>();
			switch(difficulty){

			case GameDifficulty.Easy:
				tempList.AddRange(easyDifficulty);
				break;
			case GameDifficulty.Medium:
//				tempList.AddRange(mediumDifficulty);
				break;
			case GameDifficulty.Hard:
//				tempList.AddRange(hardDifficulty);
				break;
			}
			return tempList;
		}
	}
	public void SetDifficulty(GameDifficulty diff){

		difficulty = diff;
	}

	public string GetRandomType(){
		string type = CardTypes[Random.Range (0, CardTypes.Count)];
		CardTypes.Remove (type);
		return type;
	}
}
