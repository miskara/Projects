using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	
	FruitSpawns spawner;
	ScoreController controller;
	public int timeRemaining;
	public TextMesh time;
	public Burpy burpy;

	void Start()
	{	
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController> ();
		spawner = GameObject.FindGameObjectWithTag ("Spawner").GetComponent<FruitSpawns>();
		burpy = GameObject.FindGameObjectWithTag ("Burpy").GetComponent<Burpy> ();
		time.text = "Select players by touching their launchpads! \n Touch Burpy to start the game";
		//InvokeRepeating ("decreaseTimeRemaining", 0.0f, 1.0f);
	}

	
	void decreaseTimeRemaining()
	{

		timeRemaining = timeRemaining - 1;
		if (timeRemaining <= 0) {
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/timer_end", transform.position);
			time.text = "Game Over!!!";
			TouchDragPowerV2.setEnded ();
			StartCoroutine(setWinner ());
			CancelInvoke ("decreaseTimeRemaining");
			burpy.CancelInvoke("DrawFruits");
		}

		else if (timeRemaining == 10) FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/timer_warning", transform.position);

		else time.text = timeRemaining + " Seconds remaining!";
	}

	public IEnumerator setWinner(){
		float overTime = 5;
		float startTime = Time.time;
		string winner = controller.getWinner();
		while(Time.time < startTime+overTime){
			time.color = Color.Lerp (time.color, Color.cyan,(Time.time-startTime)/overTime);
		}
		time.text = winner;
		time.fontSize=100;
		time.transform.position = new Vector3 (0f, 0f, 23f);
		spawner.CancelInvoke("SpawnFruits");
		yield return 0;
	}

}
