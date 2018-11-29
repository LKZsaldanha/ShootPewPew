using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour {

    [SerializeField] private GameObject prefabToInstantiate;

	void OnEnable () {
        Instantiate(prefabToInstantiate, transform.position, transform.rotation);
        Destroy(gameObject);
	}
	
}
