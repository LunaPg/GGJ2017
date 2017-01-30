using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;


namespace Completed
{
	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.
	
	public class GameDefinition : MonoBehaviour
	{
		public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
		public int life = 3;						//Starting value for Player life number.
		public static GameDefinition instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		public Text gameOver;
		public Text Score;
		public float waveCall;
		public float lootCall;
		public float timeStart;
		public float gameTime;

		public GameObject player;
		public GameObject slider;
		public List<Obstacle> obstacle;
		public List<Bonus> bonus;
		public List<Bonus> grabbed;

		public int kills;
		private int items;
				
		private int level = 1;									//Current level number, expressed in game as "Day 1".
		private int scoreNum = 0;
		private float interval;



		//Awake is always called before any Start functions
		void Awake(){
			interval = 1 / (gameTime*100);
			this.items =   bonus.Count;
			timeStart = Time.time;
            //Check if instance already exists
            if (instance == null)
                //if not, set instance to this
                instance = this;
            //If instance already exists and it's not this:
            else if (instance != this)
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);				
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);			
			//Call the InitGame function to initialize the first level 
			player.SetActive (true);
			InitGame();
		}

		void Start(){
			
			
		}

        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            //register the callback to be called everytime the scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //This is called each time a scene is loaded.
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)  {           
        }
		
		//Initializes the game for each level.
		void InitGame() {			
		}

		//Update is called every frame.
		void Update() {
			if (life == 0) {
				GameOver ();
			}  else if(slider.GetComponent<Scrollbar> ().value  == 1 ){
				youWin ();
			}
			else {
				float spawnTime = Time.time - timeStart;
				if (spawnTime > waveCall * kills + 1 && obstacle.Count > 0) {
					if (obstacle.Count > 0)
						obstacle [0].gameObject.SetActive (true);
				}
				if (spawnTime > lootCall * (items - bonus.Count) + 1 && bonus.Count > 0)
					bonus [0].gameObject.SetActive (true);
			}
			slider.GetComponent<Scrollbar> ().value += interval;
		}

		public void removeObsctacle(Obstacle obstacleDies){
			kills = +1;
			Destroy (obstacleDies.gameObject);
			obstacle.Remove (obstacleDies);
		}

		public void removeLife(){
			life -= 1;
		}


		public void removeBonus(Bonus bonusLost){
			bonus.Remove (bonusLost);
			Destroy (bonusLost.gameObject);
		}

		public void grabbedBonus(Bonus bonusGrabbed){
			grabbed.Add (bonusGrabbed);
			bonusGrabbed.gameObject.SetActive (false);
			this.scoreNum += 10;
			Score.text = scoreNum.ToString ();
			bonus.Remove (bonusGrabbed);
		}
		
		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{
			//Enable black background image gameObject.
			gameOver.gameObject.SetActive(true);
			player.gameObject.SetActive (false);
			//Disable this GameManager.
			enabled = false;
		}

		//GameOver is called when the player reaches 0 food points
		public void youWin()
		{
			//Enable black background image gameObject.
			gameOver.GetComponent<Text>().text =  "YOU WIN ! !!";
			gameOver.gameObject.SetActive(true);
			obstacle.Clear ();
			bonus.Clear ();
			player.gameObject.SetActive (false);
			//Disable this GameManager.
			enabled = false;
		}
		
//		//Coroutine to move enemies in sequence.
//		IEnumerator MoveEnemies()
//		{
//			//Loop through List of Enemy objects.
//			for (int i = 0; i < obstacle.Count; i++)
//			{
//				//Call the MoveEnemy function of Enemy at index i in the enemies List.
//				obstacle[i].MoveEnemy ();
//				
//				//Wait for Enemy's moveTime before moving next Enemy, 
//				yield return new WaitForSeconds(obstacleTimeInterval);
//			}
//		}
	}
}

