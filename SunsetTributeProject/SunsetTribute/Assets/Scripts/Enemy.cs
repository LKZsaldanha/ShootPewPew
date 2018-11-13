using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    [SerializeField] private float life;
    [SerializeField] private int force, id;
    [SerializeField] private float speed;
    [SerializeField] private string target;
    [SerializeField] private GameObject bullet, barraLife;
    [SerializeField] private List<GameObject> itens;
    private bool isRight, isAttack;
    private GameObject gameSystem;
    private float lifeMax;

    // Use this for initialization
    void Start () {
        gameSystem = GameObject.Find("GameSystem");
        isAttack = true;
        lifeMax = life;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Attack();
	}

    private void Move()
    {
        if(target == "")
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }        
    }

    private void Attack()
    {
        if(target != "")
        {
            if (isAttack)
            {
                GameObject aux;
                aux = Instantiate(bullet, transform.Find("SpawnBulletEnemy").transform.position, transform.Find("SpawnBulletEnemy").transform.rotation);
                if (isRight)
                {
                    aux.GetComponent<Rigidbody>().AddForce(-1000, 0, 0);
                }
                else
                {
                    aux.GetComponent<Rigidbody>().AddForce(1000, 0, 0);
                }
                isAttack = false;
                StartCoroutine("cowdown");
            }
        }
    }

    IEnumerator cowdown()
    {
        yield return new WaitForSeconds(2);
        isAttack = true;
        StopCoroutine("cowdown");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Bullet" && other.gameObject.tag != "chao")
        {
            target = other.gameObject.tag;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            life--;
            barraLife.GetComponent<Image>().fillAmount = life/lifeMax;
            if (life <= 0)
            {
                //Spawn de um item após destruir um inimigo
                //Instantiate(itens[Random.Range(0, itens.Count - 1)], transform.position, transform.rotation);
                switch (id)
                {
                    case 0:
                        gameSystem.GetComponent<GameSystem>().score += 5;
                        break;

                    case 1:
                        gameSystem.GetComponent<GameSystem>().score += 10;
                        break;

                    case 2:
                        gameSystem.GetComponent<GameSystem>().score += 15;
                        break;

                    case 3:
                        gameSystem.GetComponent<GameSystem>().score += 20;
                        break;
                }

                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = "";
    }
}
