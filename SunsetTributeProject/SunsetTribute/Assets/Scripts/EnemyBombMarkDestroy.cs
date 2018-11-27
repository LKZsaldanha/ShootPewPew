using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBombMarkDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (gameObject != null)
        {
            Destroy(gameObject, 2.5f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bomb"))
        {
            Destroy(gameObject);
            print("entrou");
        }
    }
       
}
