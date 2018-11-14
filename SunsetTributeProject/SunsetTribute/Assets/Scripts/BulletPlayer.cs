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
        StartCoroutine("size");
        GetComponent<Rigidbody>().velocity = transform.forward * speedBullet;
    }

    IEnumerator life()
    {
        yield return new WaitForSeconds(2);
        StopCoroutine("life");
        StopCoroutine("size");
        Destroy(gameObject);
        
    }

    IEnumerator size()
    {
        yield return new WaitForSeconds(1f);
        if(transform.localScale.x>0)
            gameObject.transform.localScale /= 2f;
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

    }
}
