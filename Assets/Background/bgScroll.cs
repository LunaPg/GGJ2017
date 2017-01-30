using UnityEngine;

public sealed class bgScroll : MonoBehaviour
{
	public float speed;
	public Transform bg;
	public float size;

	private Vector3 startPosition;

	void Start (){
		startPosition = bg.position;
	}

	void Update() {

		float newPosition = Mathf.Repeat(Time.time * speed, size);
		bg.position = startPosition + Vector3.left * newPosition;
		
	}
}
