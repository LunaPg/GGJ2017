using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Completed
{
	//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
	public class Obstacle : MovingObject
	{
		public int playerDamage; 							//The amount of food points to subtract from the player when attacking.
		public AudioClip shockSound;						//First of two audio clips to play when attacking the player.
		public float velocity;
		public AnimationCurve move;
		public AudioSource source;
		
		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private Transform target;							//Transform to attempt to move toward each turn.
		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.
		
		
		//Start overrides the virtual Start function of the base class.
		protected override void Start ()
		{
			//Get and store a reference to the attached Animator component.
			animator = GetComponent<Animator> ();
			
			//Find the Player GameObject using it's tag and store a reference to its transform component.
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			MoveEnemy ();	
			//Call the start function of our base class MovingObject.
			base.Start ();
		}

		public void Update(){
					
		}
		
		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
		public void MoveEnemy ()
		{
			//Store start position to move from, based on objects current transform position.
			Vector2 start = transform.position;
			transform.DOLocalMoveX(-10f, velocity).SetEase(move);
		}

		void OnCollisionEnter2D (Collision2D item){
			if (item.gameObject.tag == "wave") {
			//	Debug.Log("AOUTCH !");
				GameDefinition.instance.removeObsctacle (this);
			}
			if (item.gameObject.tag == "Player") {
				//Debug.Log("AOUTCH !");
				source.PlayOneShot (shockSound, 10);
				GameDefinition.instance.removeObsctacle (this);
				GameDefinition.instance.removeLife ();

			}
		}
	}
}
