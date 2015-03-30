using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public GameObject winPos;
	public FruitSpawns spawner;
	public ScoreController controller;
	public TouchDragPowerV2[] tdp;
	public int timeRemaining;
	public Burpy burpy;
	public Vector3 big = new Vector3 (0f, 2.5f, 0f);
	public Vector3 small = new Vector3 (0f, -2.5f, 0f);
	public Vector3 timepos;
	Vector3 next;
	public Vector3 winnerpos = new Vector3 (0f, 0f, 26f);
	public Vector3 startPos, endPos;
	public Vector3 DegreePerSec = new Vector3 (0, 4f, 0);
	public Transform arrow;
	public GameObject fader;
	public Color faded = new Color (0,0,0,1);
	public Color defaultc;
	public bool ended = false;
	public float timer = 0.0f;
	public float speed = 0.0f;
	public SpriteRenderer Srndr;
	public GameObject clock;
	public GameObject winRays;
	public ParticleSystem winRain;
	string[] fleaks = {"blue","green","purple","yellow"};
	Vector3 camPos;
	public GameObject endmenu;

	void Start()
	{	
		winRain.Stop ();
		winRain.Clear ();
		winRain.renderer.sortingLayerName = "UI";
		//winPos.SetActive (false);
		//timepos = time.transform.position;
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController> ();
		//spawner = GameObject.FindGameObjectWithTag ("Spawner").GetComponent<FruitSpawns>();
		if(Application.loadedLevelName.Contains ("7")) spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GiantbugFrtSpawns> ();
		else spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FruitSpawns> ();
		//time.transform.position = winnerpos;
		//time.color = Color.cyan;
		//time.fontSize=50;
		//time.text = "Select players by touching their launchpads! \n Touch Burpy to start the game";
		startPos = fader.transform.position;
		endPos = new Vector3 (0f, 50f, 0f);
		camPos = GameObject.Find ("Main Camera").transform.position;

	}


	public IEnumerator PlayAndStopV2(ParticleSystem ps){
		ps.Play ();
		yield return new WaitForSeconds (6.0f);
		ps.Stop ();
		yield return new WaitForSeconds (ps.duration);
		ps.Clear ();
	}

	public void Update(){
	
		if (ended) {
			float newPos = Mathf.SmoothDamp (fader.transform.position.z,0.0f,ref speed,1.5f);
			fader.transform.position = new Vector3(fader.transform.position.x,fader.transform.position.y,newPos);
		}
	}


	public void BurpyInvokes(){
		burpy.InvokeRepeating ("StartFlip",15.0f,15.0f);
		burpy.InvokeRepeating("DrawFruits", 10.0f, 10.0f);
	}

	public void decreaseTimeRemaining()
	{
		//time.color = Color.Lerp (Color.red, Color.green, (float)timeRemaining / 120);
		timeRemaining--;
		if (timeRemaining % 2 == 0) {
			clock.transform.localScale = big;
		} else {
			clock.transform.localScale = small;
		}
		if (timeRemaining == 0) {
			GameObject.Find ("Pause").SetActive (false);
			clock.SetActive (false);
			endmenu.SetActive(true);
			burpy.CancelInvoke ();
			burpy.End ();
			ended=true;
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/timer_end", camPos);
			TouchDragPowerV2.setEnded ();
			for(int i = 0; i<4; i++){
				tdp[i].CancelInvoke();
				tdp[i].gameEnd=true;
				if(tdp[i].clone!=null) tdp[i].rmFirstFleak();
			}
			setWinner ();

		} else if (timeRemaining == 10) {
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/timer_warning", camPos);
			Invoke ("decreaseTimeRemaining", 1.0f);
			next = arrow.transform.rotation.eulerAngles + DegreePerSec;
			arrow.transform.rotation = Quaternion.Euler(next);
		}
		else { 
			next = arrow.transform.rotation.eulerAngles + DegreePerSec;
			arrow.transform.rotation = Quaternion.Euler(next);
			Invoke ("decreaseTimeRemaining",1.0f);
		}
	}

	public void setWinner(){
		ended = true;
		controller.getWinner();
		for(int i = 0; i<4; i++){
			tdp[i].CancelInvoke();
			tdp[i].gameEnd=true;
			if(tdp[i].clone!=null) tdp[i].rmFirstFleak();
		}
		//time.color = controller.scoretext [controller.player - 1].color;
		//GameObject winfleak = PoolingSystem.Instance.InstantiateAPS ("Fleak"+controller.player, winPos.position,winPos.rotation);
		//winPos.SetActive (true);
		int x = Random.Range (1, 5);
		Srndr.sprite = Resources.Load ("Textures/WinScreen/fleak_pose" +x+"_"+fleaks[controller.player-1], typeof(Sprite)) as Sprite;
		//winPos.gameObject.renderer.material.mainTexture=Resources.Load ("Textures/Characters/Burpy_halffull") as Texture;
		//winfleak.rigidbody.useGravity = false;
		//time.text = winner;
		//time.fontSize=100;
		//time.transform.position = new Vector3 (0f, 0f, 23f);
		StartCoroutine (PlayAndStopV2 (winRain));
		spawner.CancelInvoke();
		//burpy.End ();
		PoolingSystem.Instance.DeactiveAll ();
	}

}
