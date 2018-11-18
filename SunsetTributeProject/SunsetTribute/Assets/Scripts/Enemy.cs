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
    private bool isRight, isAttack, isIdPlayer;
    private float lifeMax, distancePlayer;
    private int idPlayer;

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
            print("test");
            if (!isIdPlayer)
            {
                idPlayer = Random.Range(0,players.Length-1);
                isIdPlayer = true;
            }
            if (transform.position.x > players[idPlayer].position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            //mesmo nivel de altura do player
            if (players[idPlayer].position.y == transform.position.y)
            {
                print("nivel 1");
                StartCoroutine("cowdown");
                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                objAnimado.GetComponent<Animator>().SetBool("frente", true);
                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                objAnimado.GetComponent<Animator>().SetBool("idle", false);
            }

            if (players[idPlayer].position.y < transform.position.y)
            {
                if(distancePlayer > DiagMinimo)
                {
                    StartCoroutine("cowdown");
                    objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                    objAnimado.GetComponent<Animator>().SetBool("frente", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                }
            }

            if (players[idPlayer].position.y > transform.position.y)
            {
                if (distancePlayer > DiagMinimo)
                {
                    StartCoroutine("cowdown");
                    objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
                    objAnimado.GetComponent<Animator>().SetBool("frente", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                }
            }


        }
        else
        {
            StopCoroutine("cowdown");
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("idle", true);
        }
    }

    private void Attack()
    {
            if (isAttack)
            {
                Instantiate(bullet, spawnBullet[0].position, spawnBullet[0].rotation);
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
