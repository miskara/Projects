using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Playmat : VersionedView{

	public int bombs, candies;
	public char[] delimiter={'b','y'};
	public string[] xy;
	public UI ui;
	public bool gameWon = false;
	public int TotalPoints =0;
	public float Points = 0;
	public int NumberOfCardsFlipped =0;
	public int last,x,y;
	public enum BoardState {Flipping,Comparing}
	public BoardState state = BoardState.Flipping; 
	public List<List<Card>> pairMatrix = new List<List<Card>>();
	private static Playmat mat;
	public GameObject cardPrefab;
	public Layout layout;
	public GameObject board;
	public List<Card> cards = new List<Card>();
	public List<Card> all = new List<Card> ();
	public Card card1;
	public Card card2;
	public Card card3;
	string first;
	string type;
	int rand;
	public string chapter;
	public GameObject[] prefablist;
	public List<GameObject> preflist = new List<GameObject>();

	public int world, currWorld,level,currLevel;



	void Start (){
		//prefablist=prefablist;
		//if(prefablist.Count==0){
		//string[] prefablist = {"Joejoe1","Joejoe2","Joejoe3","Joejoe5","Joejoe6","Joejoe7","Joejoe8","Joejoe9","Fleak1","Fleak2","Fleak3","Fleak4",
		//"Fleak5","Fleak6","Fleak7","Fleak8","Morphy2","Morphy3","Morphy4","Morphy5","Morphy6","Morphy7","Morphy8","Morphy10"};
		//}
		Random.seed = System.Environment.TickCount;
		level = PlayerPrefs.GetInt ("Level");
		currLevel = PlayerPrefs.GetInt ("CurrentLevel");
		world = PlayerPrefs.GetInt ("World");
		currWorld = PlayerPrefs.GetInt ("CurrentWorld");
		preflist.AddRange (prefablist);
		GameSettings.Instance().difficulty = GameSettings.GameDifficulty.Easy;
		last = 0;
		ui = GameObject.FindGameObjectWithTag ("UI").GetComponent<UI> ();
		mat = this;
		xy = board.name.Split (delimiter);
		int.TryParse (xy [0], out x);
		int.TryParse (xy [2], out y);
	}
	
	public static Playmat GetPlaymat(){
		return mat;
	}
	
	public void CreateLayout(string s){
		board = layout.GetLayoutFromName(s);
		CreateCardsFromLayout();
		CreateCardTypes();
	}
	
	void CreateCardsFromLayout(){
		int i = 0;
		foreach(Slot s in layout.slots){
			Vector3  strans = s.transform.position;
			GameObject go = GameObject.Instantiate(cardPrefab,strans,Quaternion.Euler(0,-90,270)) as GameObject;
			go.transform.parent = s.transform;
			Destroy(s.GetComponent<BoxCollider>());
			cards.Add(go.GetComponent<Card>());
			all.Add(go.GetComponent<Card>());
			go.GetComponent<Card>().index=i;
			i++;
		}
		
		cards = shuffleCards (cards);
		cards = shuffleCards (cards);
		cards = shuffleCards (cards);

		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_shuffle", GameObject.Find("Main Camera").transform.position);
	}
	int CreateBombs(int i){
		int c = i+bombs;
		for (int j=i+1; j<cards.Count-candies; j++) {
			Card bomb = cards[j];
			bomb.GenerateCard ("Burpy");
			bomb.StartTwist ();
			//c++;
		}
		return c;
	}

	int CreateCandies(int i){
		int c = i+candies;
		for (int j=i+1; j<cards.Count; j++) {
			Card candy = cards[j]; 	
			candy.GenerateCard ("Candy");
			candy.StartTwist();
			//c++;
		}
		return c;
	}

	/*TODO: if world 1 (fleak), spawn 50% of the cards as fleak animations
	 * else if world 2 (joejoe), spawn 50% of the cards as joejoe animations
	 * else if world 3 (morphy), spawn 50% of the cards as morphy
	 * some levels (to be decided which), max 2 different characters, will be set in editor!!!
	 */

	void CreateCardTypes(){
		if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
			int pairs = (cards.Count-bombs-candies)/2;
			int maxMainChars = pairs/3;
			for (int i=0; i < pairs; i++) {
				Card c1 = cards [i*2];
				Card c2 = cards [(i*2)+1];
				rand = Random.Range (0,preflist.Count);
				if(i==1){
					type = preflist[rand].name;
					string sub = type.Substring (0,3);
					while(!first.Equals(sub)){
						rand = Random.Range (0,preflist.Count);
						type = preflist[rand].name;
						sub = type.Substring (0,3);
					}

					c1.GenerateCard (type);
					c2.GenerateCard (type);
					preflist.Remove (preflist[rand]);
					last = (i*2)+1;
					if(preflist.Count==0){
						Debug.Log ("Empty, refilling");
						preflist.AddRange (prefablist);
					}
				}
				else{
					if(i<maxMainChars){
						type=preflist[rand].name;
						while(!type.Contains(chapter)){
							rand = Random.Range (0,preflist.Count);
							type = preflist[rand].name;
						}
						c1.GenerateCard (type);
						c2.GenerateCard (type);
						if(i==0) first=type.Substring(0,3);
						preflist.Remove (preflist[rand]);

						last = (i*2)+1;
						if(preflist.Count==0){
							Debug.Log ("Empty, refilling");
							preflist.AddRange (prefablist);
						}
					}
					else{

						type = preflist[rand].name;
						if(i==0) first=type.Substring(0,3);
						preflist.Remove (preflist[rand]);
						if(preflist.Count==0){
							Debug.Log ("Empty, refilling");
							preflist.AddRange (prefablist);
						}
						c1.GenerateCard (type);
						c2.GenerateCard (type);
						last = (i*2)+1;
					}
				}
			}



			last = CreateBombs(last);
			last = CreateCandies(last);
			TotalPoints = pairs;
			foreach(Card c in cards){
				c.anim.SetBool ("Revealed",false);
			}
		} 
		
		else {
			for (int i=0; i < cards.Count/3-1; i++) {
				Card c1 = cards [i*3];
				Card c2 = cards [i*3+1];
				Card c3 = cards [i*3+2]; 
				string type = GameSettings.Instance ().GetRandomType ();
				c1.GenerateCard (type);
				c2.GenerateCard (type);
				c3.GenerateCard (type);
			}
			Card b1 = cards[cards.Count];
			Card b2 = cards[cards.Count-1];
			Card b3 = cards[cards.Count-2];
			b1.GenerateCard ("Bomb");
			b2.GenerateCard ("Bomb");
			b3.GenerateCard ("Bomb");
			TotalPoints = cards.Count / 3-1;
		}
		
	}

	public void shuffleSlots(List<Slot> alpha){
		for(int i = 0; i < alpha.Count; i++) {
			Slot tempslot = alpha[i];
			int randomIndex = Random.Range(0, alpha.Count);
			alpha[i] = alpha[randomIndex];
			alpha[randomIndex]= tempslot;
		}
	}

	public List<Card> SurroundingCards (List<Card> sorted, int i){

		List<Card> surrounding = new List<Card> ();
		foreach (Card c in sorted) {
			int j = c.GetComponent<Card> ().index;
			if (i == 0) {														//topleft corner
				//i+1, i+x, i+(x+1)
				if(j == (i + 1) || j == (i + x) || j == (i + (x + 1))){
					surrounding.Add (c);
				}
			} 
			else if (i / x < 1 && i != x - 1 && i != 0) {						//top row
				//i-1, i+1, i+(x-1), i+x, i+(x+1)
				if (j == i - 1 || j == i + 1 || j == i + (x - 1) || j == i + x || j == i + x + 1){
					surrounding.Add (c);
				}
			} 
			else if (i == (x - 1)) {											//topright corner
				// i-1, i+(x-1), i+x
				if (j == i - 1 || j == i + (x - 1) || j == i + x){
					surrounding.Add (c);	 
				}
			} 
			else if (i % x == 0 && i != 0 && i != (x * y) - x) {				//left column
				// i-x, i-(x-1), i+1, i+x, i+(x+1)
				if (j == i - x || j == i - (x - 1) || j == i + 1 || j == i + x || j == i + (x + 1)){
					surrounding.Add (c);
				}
			} 
			else if (i % x == x - 1 && i != x - 1 && i != (x * y) - 1) {		//right column
				// i-(x+1), i-x, i-1, i+(x-1), i+x
				if (j == i - (x + 1) || j == i - x || j == i - 1 || j == i + (x - 1) || j == i + x){
					surrounding.Add (c);	
				}
			} 
			else if (i == x * y - x) {											//bottomleft corner
				// i-x, i-(x-1), i+1
				if (j == i - x || j == i - (x - 1) || j == i + 1){
					surrounding.Add (c);	 
				}
			} 
			else if (i / x >= y - 1 && i != (x * y) - x && i != (x * y) - 1) {	//bottom row;
				// i-(x+1), i-x, i-(x-1), i-1, i+1
				if (j == i - (x + 1) || j == i - x || j == i - (x - 1) || j == i - 1 || j == i + 1){
					surrounding.Add (c);	 
				}
			}
			else if (i == x * y - 1) {											//bottomright corner
				// i-(x+1), i-x, i-1
				if (j == i - (x + 1) || j == i - x || j == i - 1){
					surrounding.Add (c); 
				}
			} 
			else {																//default
				if (j == i - (x + 1) || j == i - x || j == i - (x - 1) || j == i - 1 || j == i + 1 || j == i + (x - 1) || j == i + x || j == i + (x + 1)){
					surrounding.Add (c);
				}

			}
	}

		return surrounding;
	}

	public void FlipIfFound(){
		if(card1!=null){
			card1.cardView.animation["HideFast"].speed=3.0f;
			card1.cardView.animation.Play("HideFast");
			card1.state=Card.CardState.Hidden;
			card1.anim.SetBool ("Revealed",false);
			card1 = null;
		}
		if(card2!=null){
			card2.cardView.animation["HideFast"].speed=3.0f;
			card2.cardView.animation.Play("HideFast");
			card2.state=Card.CardState.Hidden;
			card2.anim.SetBool ("Revealed",false);
			card2 = null;
		}
		if (card3 != null) {
			card3.cardView.animation["HideFast"].speed=3.0f;
			card3.cardView.animation.Play("HideFast");
			card3.state=Card.CardState.Hidden;
			card3.anim.SetBool ("Revealed",false);
			card3 = null;
		}

		foreach (Card c in cards){
			if(c.pair == null && c.state==Card.CardState.Flipped){
				c.cardView.animation["HideFast"].speed=3.0f;
				c.cardView.animation["HideFast"].speed=3.0f;
				c.cardView.animation.Play("HideFast");
				c.state=Card.CardState.Hidden;
				c.anim.SetBool ("Revealed",false);
			}
		}
	}
	public IEnumerator CandyFound(List<Card> alpha, int i, Card fruit){
		//FlipIfFound ();
		List<Card> surrounding = SurroundingCards (alpha,i);

		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/fruit_open", GameObject.Find("Main Camera").transform.position);

		foreach (Card c in surrounding) {;
				c.cardView.animation ["HideFast"].speed = -3.0f;
				c.cardView.animation.Play("HideFast");
				c.state=Card.CardState.Flipped;
				c.anim.SetBool ("Revealed",true);
		}
		foreach (Card c in all) {
			c.abletoflip = false;
		}
		yield return new WaitForSeconds (3);

		GameObject resource = Resources.Load("Prefabs/CandyEaten") as GameObject;
		//GameObject eaten = GameObject.Instantiate(resource,fruit.character.transform.position,fruit.character.transform.rotation) as GameObject;
		//fruit.rlist.Remove (fruit.character.GetComponent<Renderer> ());
		//Destroy (fruit.character);
		fruit.character.GetComponentInChildren<Renderer>().enabled = false;
		//fruit.character = eaten;
		fruit.character = GameObject.Instantiate(resource,fruit.spawn.position,fruit.spawn.rotation) as GameObject;
		fruit.character.transform.parent = fruit.cardView.transform;
		//fruit.rlist.Add(eaten.GetComponent<Renderer>());
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/fruit_used", GameObject.Find("Main Camera").transform.position);

		foreach (Card c in all) {
			c.abletoflip = true;
		}
		foreach (Card c in surrounding) {
			if(c.pair==null){
			c.cardView.animation ["HideFast"].speed = 3.0f;
			c.cardView.animation.Play("HideFast");
			c.state=Card.CardState.Hidden;
			c.anim.SetBool ("Revealed",false);
			}
		}
		FlipIfFound ();
		state=BoardState.Flipping;
		foreach (Card c in all) {
			c.abletoflip = true;
		}
	}

	public IEnumerator BurpyFound(List<Card> alpha, int i){

		foreach (Card c in all) {
			c.abletoflip = false;
		}
		List<Card> surrounding = SurroundingCards (alpha,i);
		yield return new WaitForSeconds (0.72f);

		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_burp", GameObject.Find("Main Camera").transform.position);
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_fly", GameObject.Find("Main Camera").transform.position);	
		//some effect to turn all cards 360degree 
		yield return new WaitForSeconds (1.0f);
		foreach (Card c in surrounding) {
				if(c.state == Card.CardState.Flipped){
				//c.cardView.animation ["HideFast"].speed = 3.0f;
				c.cardView.animation.Play("BurpyFlip");
				c.state=Card.CardState.Hidden;
				c.anim.SetBool ("Revealed",false);

				if(c.pair != null){
					Points -= 1.0f;
					//c.pair.cardView.animation ["BurpyFlip"].speed = 3.0f;
					c.pair.cardView.animation.Play("BurpyFlip");
					c.pair.state=Card.CardState.Hidden;
					c.anim.SetBool ("Revealed",false);
					c.pair.pair = null;
					c.pair = null;
				}
			}
			else{
				c.cardView.animation.Play ("BurpyTwist");
			}
		}
		foreach (Card c in cards){
			if(c.state == Card.CardState.Flipped && c.pair == null){
				//c.cardView.animation ["HideFast"].speed = 3.0f;
			
				c.cardView.animation.Play("BurpyFlip");
				c.state=Card.CardState.Hidden;
				c.anim.SetBool ("Revealed",false);
			}
		}
		shuffleCards (surrounding);
		state=BoardState.Flipping;
		foreach (Card c in all) {
			c.abletoflip = true;
		}
		shuffleCards (surrounding);
		shuffleCards (surrounding);
		yield return null;
	}

	public List<Card> shuffleCards(List<Card> alpha){
		for(int i = 0; i < alpha.Count; i++) {
			Card tempcard = alpha[i];
			int randomIndex = Random.Range(0, alpha.Count);
			alpha[i] = alpha[randomIndex];
			alpha[randomIndex] = tempcard;
		}
		
		return alpha;
	}
	
	public void BombFound(List<Card> alpha){
		foreach (Card c in alpha) {
			if(c.state==Card.CardState.Flipped){
				c.cardView.animation ["HideFast"].speed = 1.0f;
				c.cardView.animation.Play("HideFast");
				c.state=Card.CardState.Hidden;
				c.anim.SetBool ("Revealed",false);
			}
		}
		
		card1=null;
		card2=null;
		card3=null;
		shuffleCards (cards);
		state=BoardState.Flipping;
	}
	
	

	public string GetPointsString(){
		
		return Points.ToString() + "/" + TotalPoints.ToString();
	}

	public void goThroughLists(){
		for (int i =0; i<this.pairMatrix.Count; i++) {
			for (int j =0; j<this.pairMatrix[i].Count; j++) {
				Debug.Log ("Pair "+(i+1)+" Card "+(j+1)+" Type:");
				Debug.Log (this.pairMatrix [i] [j].CardType);
				Debug.Log ("Pair "+(i+1)+" Card "+(j+1)+" Index:");
				Debug.Log (this.pairMatrix [i] [j].index);
			}
		}
	}
	
	public override void DirtyUpdate (){
		
		switch(state){
		case BoardState.Comparing:
			Compare();
			break;
		}
	}
	
	
	
	void Compare(){
		
		if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
			if (card1.CardType == card2.CardType) {

				StartCoroutine (card1.PlayAndStopV2());
				StartCoroutine (card2.PlayAndStopV2());
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_match", GameObject.Find("Main Camera").transform.position);

				NumberOfCardsFlipped = 0;
				Points++;
				ui.updateScore();
				List<Card> pair = new List<Card>();
				pair.Add (card1);
				pair.Add (card2);
				pairMatrix.Add(pair);
				card1.setPair (card2);
				card2.setPair (card1);
				if (TotalPoints == Points) {
					ui.ShowTime();
					gameWon = true;
					FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/win", GameObject.Find("Main Camera").transform.position);
					if(PlayerPrefs.GetInt ("CurrentLevel")+1 > PlayerPrefs.GetInt ("Level")) PlayerPrefs.SetInt ("Level",PlayerPrefs.GetInt ("CurrentLevel")+1);
					if(PlayerPrefs.GetInt ("Level") == 4) PlayerPrefs.SetInt ("World",2);
					if(PlayerPrefs.GetInt ("Level") == 7) PlayerPrefs.SetInt ("World",3);
					//foreach(Card c in all){
					//	if(c.CardType.Equals("Burpy")){
					//		c.anim.SetBool ("Ended",true);	
					//	}
					//}
				}
			}
			else {
				card1.Unflip ();
				card2.Unflip ();
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_no_match", GameObject.Find("Main Camera").transform.position);

			}
			card1 = null;
			card2 = null;
			state = BoardState.Flipping;
		} 
		
		else {
			
			if (card1.CardType == card2.CardType && card1.CardType == card3.CardType) {

				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_match", GameObject.Find("Main Camera").transform.position);

				NumberOfCardsFlipped = 0;
				Points++;
				if (TotalPoints == Points) {

					gameWon = true;
					GameController.currentLVL +=1;
				}
			} 
			
			else {
				card1.Unflip ();
				card2.Unflip ();
				card3.Unflip ();
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_no_match", GameObject.Find("Main Camera").transform.position);
			}
			card1 = null;
			card2 = null;
			card3 = null;
			state = BoardState.Flipping;
			
		}
	}
	
	public IEnumerator PlayAndStopV2(ParticleSystem ps){
		yield return new WaitForSeconds (0.25f);
		ps.Play ();
		yield return new WaitForSeconds (1f);
		ps.Stop ();
		yield return new WaitForSeconds (ps.duration);
		ps.Clear ();
	}


	public void SetCardsForMatch(Card c){
		int i;
		switch(c.CardType){
		case "Burpy":
			card1 = null;
			card2 = null;
			card3 = null;
			NumberOfCardsFlipped=0;
			i = c.GetComponent<Card>().index;
			cards.Remove (c);
			StartCoroutine (PlayAndStopV2 (c.burp));
			StartCoroutine(BurpyFound(cards, i));
			state = BoardState.Flipping;
			break;

		case "Candy":
			NumberOfCardsFlipped=0;
			i = c.GetComponent<Card>().index;
			cards.Remove (c);
			StartCoroutine(CandyFound(cards, i,c));
			state = BoardState.Flipping;
			card1 = null;
			card2 = null;
			card3 = null;
			break;

		default:
			if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
				if (card1 == null) {
					card1 = c;
					state = BoardState.Flipping;
				} 
				else {
					card2 = c;
					state = BoardState.Comparing;
				}
				
				MarkDirty ();
				
			} 
			else {
				if (card1 == null) {
					card1 = c;
					state = BoardState.Flipping;
				} 
				else if (card2 == null) {
					card2 = c;
					state = BoardState.Flipping;
				} 
				else {
					card3 = c;
					state = BoardState.Comparing;
				}
				MarkDirty ();
			}
			break;
		}
	}
}
