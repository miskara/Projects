using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

	public int[] scores;
	public string winner;
	public TextMesh[] scoretext;
	public Color32[] splshC;
	public int player=0;
	void Start () {
		for (int i = 0; i<scores.Length; i++) {
			scoretext[i].text="0";
			scores[i]=0;
		}
	}
	void Awake(){
		for (int i = 0; i<scores.Length; i++) {
			scoretext[i].text="0";
			scores[i]=0;
		}
	}

	public void AddScore (int i, int j){
		scores[i] += j;
		scoretext [i].text = scores[i].ToString ();
	}

	public void getWinner(){

		int winningScore = 0;
		//winner = "It's a draw! :)\n Touch Burpy to enter menu";
		for (int i =0; i<scores.Length; i++) {
			if(scores[i]> winningScore){
				player=i+1;
				winningScore=scores[i];
			//	winner = "Player "+player+" Wins!!\n Score: "+winningScore+"\n Touch Burpy to enter menu";
			}
			else if(scores[i] == winningScore){
				//int draw = i+1;
			//	winner = "It's a draw! :)\n Touch Burpy to enter menu";
			}
		}
	}
}
