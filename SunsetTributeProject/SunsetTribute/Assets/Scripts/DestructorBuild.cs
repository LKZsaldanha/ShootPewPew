using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructorBuild : MonoBehaviour {

    [SerializeField] private GameObject destrocos, barraLife;
    [SerializeField] private float life;
    private float lifeMax;
    private void Start()
    {
        lifeMax = life;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BulletEnemy" && gameObject.tag != "enemy")
        {
            life--;
            barraLife.GetComponent<Image>().fillAmount = life / lifeMax;
            if (life <= 0)
            {
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }

        //objs inimigos
        if(gameObject.tag != "Player" && collision.gameObject.tag == "Bullet")
        {
            life--;
            barraLife.GetComponent<Image>().fillAmount = life / lifeMax;
            if (life <= 0)
            {
                GameObject.Find("GameSystem").GetComponent<GameSystem>().score += 100;
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }
    }
}
