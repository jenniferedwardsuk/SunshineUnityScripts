using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkOverTime : MonoBehaviour {

    public float shrinkSpeed = 0.99f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.gameObject.transform.localScale *= shrinkSpeed;
	}
}
