using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //baixoMinimo é o valor minimo para chamar a animação de mira baixa e o baixoMaximo a mesma coisa
    [SerializeField] private float life,DiagMinimo,baixoMinimo,baixoMaximo;
    [SerializeField] private float speed, distanceAttack, cowdownFire;
    [SerializeField] private GameObject bullet,objAnimado;
    [SerializeField] private List<GameObject> itens;
    [SerializeField] private Transform[] spawnBullet, players;
    [SerializeField] private Transform mySpawn;
    private bool isRight, isAttack;
    private float lifeMax, distancePlayer;

    // Use this for initialization
    void Start () {
        /*
         alterar para a variavel mySpawn para localizar a posição do spawn do inimigo e setar a escala de X do inimigo 
         
        if(mySpawn.position.x < players[0].position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
            transform.localScale = new Vector3(-1, 1, 1);
            */
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
        distancePlayer = Vector3.Distance(players[0].position, transform.position);
        if(distancePlayer <= distanceAttack)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            print("distancia: "+distancePlayer);
            if (players[0].position.y < transform.position.y)
            {
                if(distancePlayer>DiagMinimo)
                {
                    print("Diagonal");
                    objAnimado.GetComponent<Animator>().SetBool("DiagBaixo",true);
                }
                else if(distancePlayer<baixoMaximo && distancePlayer>baixoMinimo)
                {
                    print("baixo");
                    objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", true);
                }
            }

            StartCoroutine("cowdown");
        }
        else if(distancePlayer >= distanceAttack)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            StopCoroutine("cowdown");
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("idle", true);
        }
    }

    private void Attack()
    {
            if (isAttack)
            {
                GameObject aux;
                aux = Instantiate(bullet, spawnBullet[0].position, spawnBullet[0].rotation);
                if (isRight)
                {
                    aux.GetComponent<Rigidbody>().AddForce(-1000, 0, 0);
                }
                else
                {
                    aux.GetComponent<Rigidbody>().AddForce(1000, 0, 0);
                }
                isAttack = false;
                
            }        
    }

    /// <summary>
    /// Chamar como evento de animação
    /// </summary>
    /// <returns></returns>
    public IEnumerator cowdown()
    {
        yield return new WaitForSeconds(cowdownFire);
        isAttack = true;
        StopCoroutine("cowdown");
    }


    #region animacoes

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            life--;
            if (life <= 0)
            {
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }
    }
}
