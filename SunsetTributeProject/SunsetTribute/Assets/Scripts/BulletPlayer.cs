using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour {
    //true player, false Enemy
    [SerializeField] private bool id;
    [SerializeField] private float speedBullet;
    [SerializeField] GameObject muzzleParticlesPrefab, hitParticlesPrefab;

    public float lifebullet = 10.0f;
    

    public GameObject target; //tentado passar pelo proprio script do inimigo

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        lifebullet = 10.0f;
        CreateAndDestroyParticle(muzzleParticlesPrefab, transform.position, Quaternion.identity);
        
	}
    void Awake () {
        if(transform.tag == "BulletEnemy"){
            if(target != null){
                StartCoroutine("lifeBulletEnemy");
                transform.LookAt(new Vector3(target.transform.position.x, target.transform.position.y + 0.3f, target.transform.position.z));
            }else{
                target = GameObject.FindWithTag("Player");
                transform.LookAt(new Vector3(target.transform.position.x, target.transform.position.y + 0.3f, target.transform.position.z));
            }
        }
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

    
    private void FixedUpdate(){

        GetComponent<Rigidbody>().velocity = transform.forward * speedBullet * Time.deltaTime;
        Destroy(gameObject,lifebullet);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(transform.tag == "Bullet")
        {
            if(collision.gameObject.tag == "LimitSize")
            {
                Destroy(gameObject);
            }
        }


        if (collision.gameObject.tag == "chao")
        {
            Destroy(gameObject);
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            CreateAndDestroyParticle(hitParticlesPrefab, pos, rot);
        }

        if (id)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                CreateAndDestroyParticle(hitParticlesPrefab, pos, rot);
            }

        }
        else
        {
            if (collision.gameObject.tag == "PlayerHitbox")
            {
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                CreateAndDestroyParticle(hitParticlesPrefab, pos, rot);
            }
        }

        

    }

    IEnumerator lifeBulletEnemy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        StopCoroutine("lifeBulletEnemy");
    }
}
