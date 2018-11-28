using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour {
    //true player, false Enemy
    [SerializeField] private bool id;
    [SerializeField] private float speedBullet;
    [SerializeField] GameObject muzzleParticlesPrefab, hitParticlesPrefab;

    public float lifebullet = 2.0f;

	// Use this for initialization
	void Start () {
        CreateAndDestroyParticle(muzzleParticlesPrefab, transform.position, Quaternion.identity);

        StartCoroutine("life");
	}

    private void CreateAndDestroyParticle(GameObject go,Vector3 pos, Quaternion rot)
    {
        GameObject newPart = Instantiate(go, pos, rot);
        newPart.transform.forward = gameObject.transform.forward;
        ParticleSystem pSystem = newPart.GetComponent<ParticleSystem>();
        if (pSystem != null)
        {
            Destroy(newPart, pSystem.main.duration);
        }
        else
        {
            ParticleSystem psChild = newPart.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(newPart, psChild.main.duration);
        }
    }

    private void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speedBullet;
    }

    IEnumerator life()
    {
        yield return new WaitForSeconds(lifebullet);
        StopCoroutine("life");
        Destroy(gameObject);
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        CreateAndDestroyParticle(hitParticlesPrefab, pos, rot);

        if (id)
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
