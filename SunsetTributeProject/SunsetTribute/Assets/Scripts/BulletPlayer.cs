using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour {
    //true player, false Enemy
    [SerializeField] private bool id;
    [SerializeField] private float speedBullet;

	// Use this for initialization
	void Start () {
        StartCoroutine("life");
	}

    private void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speedBullet;
    }

    IEnumerator life()
    {
        yield return new WaitForSeconds(2);
        StopCoroutine("life");
        Destroy(gameObject);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(id)
        {
            if (collision.gameObject.tag == "BulletEnemy")
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }

            if(collision.gameObject.tag == "chao")
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.gameObject.tag == "chao")
            {
                Destroy(gameObject);
            }
        }

    }
}
