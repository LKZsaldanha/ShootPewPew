using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBullet : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        transform.LookAt(GameObject.Find("Main Camera").transform);
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(GameObject.Find("Main Camera").transform);
    }
}
