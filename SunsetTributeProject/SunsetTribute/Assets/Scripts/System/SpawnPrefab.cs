using UnityEngine;

public class SpawnPrefab : MonoBehaviour {

    [SerializeField] private GameObject prefabToInstantiate;

	public void OnEnable () {
        Instantiate(prefabToInstantiate, transform.position, transform.rotation);
        Destroy(gameObject);
	}
	
}
