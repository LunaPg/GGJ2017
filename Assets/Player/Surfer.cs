using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

namespace Completed
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Surfer : MovingObject
	{
		public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
		public int life = 3;				//Number of points to add to player food points when picking up a food object.
		public int pointsPerSoda = 20;				//Number of points to add to player food points when picking up a soda object.
		public int damageTaken = 1;					//How much damage a player does to a wall when chopping it.
		public Text speak;						//UI Text to display current player food total.
		public AudioClip jump;				//1 of 2 Audio clips to play when player moves.
		public AudioClip hit;				//2 of 2 Audio clips to play when player moves.
		public AudioClip bonus;					//1 of 2 Audio clips to play when player collects a food object.
		public AudioClip yeah;					//2 of 2 Audio clips to play when player collects a food object.
		public AudioClip gameOverSound;				//Audio clip to play when player dies.
		public AudioSource audioSource;
		public float speed;
		public float waveInertia;
		public Vector2 damageMove;

		private float startpress;

		
		private Animator animator;					//Used to store a reference to the Player's animator component.
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.
#endif
		
		void Awake(){
			audioSource = GetComponent<AudioSource> ();
		}

		//Start overrides the Start function of MovingObject
		protected override void Start () {
			//Get a component reference to the Player's animator component
			animator = GetComponent<Animator>();
			
			//Get the current food point total stored in GameManager.instance between levels.
			//life = GameDefinition.instance.life;

			//Call the Start function of the MovingObject base class.
			base.Start ();
		}
		
		
		//This function is called when the behaviour becomes disabled or inactive.
		private void OnDisable () {
			//When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
			GameDefinition.instance.life = life;
		}
		
		
		private void Update () {
			float horizontal = 0;  	//Used to store the horizontal move direction.
			float vertical = 0;		//Used to store the vertical move direction.
			
			//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
			horizontal = Input.GetAxisRaw ("Horizontal") *speed;
			
			//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
			vertical = Input.GetAxisRaw ("Vertical") * speed;
			
			//Check if we have a non-zero value for horizontal or vertical
			Move (-1 *waveInertia, 0 );
			if(horizontal != 0) {
				Move (horizontal, 0);
			}

			if (Input.GetButtonDown ("Jump")) {
				startpress = Time.time;
			}

			if (Input.GetButtonUp ("Jump")){
				float pressTime = Time.time - startpress;
				if (pressTime *10 > 2){
					Jump (0, 30);
				} else {
					Jump (0, 20 * (pressTime *10 )+1 );
				}
			}
		}

		public void Jump (float x, float y){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y), ForceMode2D.Impulse);			
		}

		public void Move(float x, float y){
			
			//Store start position to move from, based on objects current transform position.
			Vector2 start = transform.position;

			// Calculate end position based on the direction parameters passed in when calling Move.
			Vector2 end = start + new Vector2 (x, y);

			//If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
			// If time, TWINE it
			//StartCoroutine (SmoothMovement (end));
			transform.position = end;
		}
			
		void OnCollisionEnter2D (Collision2D item){
			if (item.gameObject.tag == "hurts") {
				audioSource.PlayOneShot (hit, 10);
				Move (damageMove.x, damageMove.y);
			}

			if (item.gameObject.tag == "bonus") {
				//audioSource.PlayOneShot (yeah, 10);
				dialog();

			}
		}
		private void dialog(){
			speak.enabled = true;
			
		}
		
		
		//Restart reloads the scene when called.
		private void Restart ()
		{
			//Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
            //and not load all the scene object in the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}
		
		
		//LoseFood is called when an enemy attacks the player.
		//It takes a parameter loss which specifies how many points to lose.
		public void LoseFood (int loss)
		{
			//Set the trigger for the player animator to transition to the playerHit animation.
			animator.SetTrigger ("playerHit");
			
			//Subtract lost food points from the players total.
			life -= loss;
			
			//Update the food display with the new total.
			speak.text = "-"+ loss + " Food: " + life;
			
			//Check to see if game has ended.
			CheckIfGameOver ();
		}

//		public void OnCanMove(ArrayList T){
//			return true;
//			
//		}
		
		
		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (life <= 0) 
			{
				//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
				SoundManager.instance.PlaySingle (gameOverSound);
				
				//Stop the background music.
				SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();
			}
		}
	}
}