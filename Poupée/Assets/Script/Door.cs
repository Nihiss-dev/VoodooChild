using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	Animator anim;
	public bool _state = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("ouverture", _state);
	}
}
