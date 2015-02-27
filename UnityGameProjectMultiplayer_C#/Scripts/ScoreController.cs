using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

	public int[] scores;
	public string winner;
	public TextMesh[] scoretext;

	void Start () {
		for (int i = 0; i<scores.Length; i++) {
			scoretext[i].text="0";
			scores[i]=0;
		}
	}

	public void AddScore (int i, int j){
		scores[i] += j;
		scoretext [i].text = scores[i].ToString ();
	}

	public string getWinner(){

		int winningScore = 0;
		winner = "It's a draw! :)\n Touch Burpy to enter menu";
		for (int i =0; i<scores.Length; i++) {
			if(scores[i]> winningScore){ 
				winningScore=scores[i];
				winner = "Player "+(i+1)+" Wins!!\n Score: "+winningScore+"\n Touch Burpy to enter menu";
			}
		}
		return winner;
	}
}
