using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Completed{
	public sealed class Bonus : MovingObject
	{
		public AudioClip grab;
		public GUIText yeah;
		public AnimationCurve moving;
		public float velocity;

		void Start(){
			MoveBonus ();	
		}

		void Update() {	
		}

		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
		public void MoveBonus ()
		{
			//Store start position to move from, based on objects current transform position.
			Vector2 start = transform.position;
			transform.DOLocalMove(new Vector2(-10f, 0), velocity).SetEase(moving);
		}

		void OnCollisionEnter2D (Collision2D item){
			Debug.Log (item.gameObject.tag);
			if (item.gameObject.tag == "wave") {
				Debug.Log("AOUTCH BONUUUSSS!");
				GameDefinition.instance.removeBonus (this);
			}
			if (item.gameObject.tag == "Player") {
				Debug.Log("Miam !");
				GameDefinition.instance.grabbedBonus (this);
			}
		}
	}

}