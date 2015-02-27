using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

	private bool isRunning;
	private bool wasRunningLastUpdate;
	private float elapsedSeconds;
	private float timeLastUpdate;
	private string levelTimeString;
	
	
	void Start() {
		InvokeRepeating("updateTimer", 0, 0.2f);
	}
	
	public void StartTimer() {
		isRunning = true;
	}
	
	public void ResetTimer() {
		elapsedSeconds = 0;
	}
	
	public void StopTimer() {
		isRunning = false;
	}
	
	private void updateTimer() {
		if (!isRunning) {
			wasRunningLastUpdate = false;
			return;
		}
		
		if (wasRunningLastUpdate) {
			var deltaTime = Time.time - timeLastUpdate;
			elapsedSeconds += deltaTime;
		}
		
		var timeSpan = TimeSpan.FromSeconds(elapsedSeconds);
		levelTimeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		PlayerPrefs.SetString("LevelTime", levelTimeString);
		
		timeLastUpdate = Time.time;
		wasRunningLastUpdate = true;
	}
}