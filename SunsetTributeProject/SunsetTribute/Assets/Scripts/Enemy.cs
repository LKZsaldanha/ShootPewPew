using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //baixoMinimo é o valor minimo para chamar a animação de mira baixa e o baixoMaximo a mesma coisa
    [SerializeField] private float life,DiagMinimo,baixoMinimo,baixoMaximo;
    [SerializeField] private float speed, distanceAttack, cowdownFire;
    [SerializeField] private GameObject bullet,objAnimado, gameSystem;
    [SerializeField] private List<GameObject> itens;
    [SerializeField] private Transform[] spawnBullet, mira;
    [SerializeField] private List<Transform> players;
    [SerializeField] private Transform mySpawn;

    private GameObject colver;
    private bool isRight, isAttack, isIdPlayer, isDead, isColver, lookinPlayer;
    private float lifeMax, distancePlayer, distancePlayer2, menorDistancia;
    private int idPlayer;

    private EnemySound enemySound;

    private void Awake()
    {
        gameSystem = GameObject.Find("GameSystem");
    }

    // Use this for initialization
    void Start () {
        //Vai setar o numero de player que estão em game
        if(gameSystem.GetComponent<GameSystem>().nPlayerVivos.Count == 2)
        {
            players.Add( gameSystem.GetComponent<GameSystem>().nPlayerVivos[0].transform );
            players.Add( gameSystem.GetComponent<GameSystem>().nPlayerVivos[1].transform );
        }
        else
        {
            players.Add( gameSystem.GetComponent<GameSystem>().nPlayerVivos[0].transform );
        }

        enemySound = GetComponent<EnemySound>();

        lifeMax = life;
	}
	
	// Update is called once per frame
	void Update () {


        if (!isDead && players.Count >= 1)
        {
            if (transform.position.x < players[players.Count-1].position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            players.RemoveAll(c => c == null);
            modNPlayers();

            if (colver == null)
                Move();


            Attack();
        }

	}


    //Sinaliza aos Inimigos quantos players estão em jogo
    private void modNPlayers()
    {
        if (gameSystem.GetComponent<GameSystem>().nPlayerVivos.Count > players.Count)
        {
            if(players[0].name != gameSystem.GetComponent<GameSystem>().nameplayer)
            {
                players.Add(GameObject.Find(gameSystem.GetComponent<GameSystem>().nameplayer).transform);
            }
        }
        else if (gameSystem.GetComponent<GameSystem>().nPlayerVivos.Count < players.Count)
        {
            if (players[0].name == gameSystem.GetComponent<GameSystem>().nameplayer)
            {
                players.RemoveAt(0);
            }
            else
            {
                players.RemoveAt(1);
            }
        }
    }

    private void Move()
    {

        distancePlayer = Vector3.Distance(players[0].position, transform.position);
        menorDistancia = distancePlayer;
        if (players.Count > 1)
        {
            distancePlayer2 = Vector3.Distance(players[1].position, transform.position);
            if(distancePlayer <= distancePlayer2)
            {
                menorDistancia = distancePlayer;
            }
            else
            {
                menorDistancia = distancePlayer2;
            }
                
        }

        


        if (menorDistancia <= distanceAttack)
        {
            if (!isIdPlayer)
            {
                idPlayer = Random.Range(0,players.Count);
                isIdPlayer = true;
            }

            if (transform.position.x > players[idPlayer].position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            //mesmo nivel de altura do player
            if (players[idPlayer].position.y <= 1.9f )
            {
                StartCoroutine("cowdown");

                //posição e rotação da mira (frente)
                if(transform.localScale.x == 1)
                {
                    spawnBullet[0].position = mira[2].position;
                    spawnBullet[0].rotation = Quaternion.Euler(180, 90, 0);
                }
                else
                {
                    //(Costas)
                    spawnBullet[0].position = mira[2].position;
                    spawnBullet[0].rotation = Quaternion.Euler(0, 90, 0);
                }

                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                objAnimado.GetComponent<Animator>().SetBool("frente", true);
                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                objAnimado.GetComponent<Animator>().SetBool("idle", false);
            }

            if (players[idPlayer].position.y < transform.position.y -1f)
            {
                if(distancePlayer > DiagMinimo)
                {
                    StartCoroutine("cowdown");
                    //posição e rotação da mira (diagonal Baixo frente)
                    if (transform.localScale.x == 1)
                    {
                        spawnBullet[0].position = mira[3].position;
                        spawnBullet[0].rotation = Quaternion.Euler(225, 90, 0);
                    }
                    else
                    {
                        //(diagonal Baixo Costas)
                        spawnBullet[0].position = mira[3].position;
                        spawnBullet[0].rotation = Quaternion.Euler(315, 90, 0);
                    }
                    objAnimado.GetComponent<Animator>().SetBool("cima", false);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                    objAnimado.GetComponent<Animator>().SetBool("frente", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                }

            }

            if (players[idPlayer].position.y > 2.5f && menorDistancia > 2)
            {
                if (distancePlayer > DiagMinimo)
                {
                    StartCoroutine("cowdown");
                    //posição e rotação da mira (diagonal cima frente)
                    if (transform.localScale.x == 1)
                    {
                        spawnBullet[0].position = mira[1].position;
                        spawnBullet[0].rotation = Quaternion.Euler(135, 90, 0);
                    }
                    else
                    {
                        //(diagonal cima Costas)
                        spawnBullet[0].position = mira[1].position;
                        spawnBullet[0].rotation = Quaternion.Euler(405, 90, 0);
                    }
                    objAnimado.GetComponent<Animator>().SetBool("cima", false);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
                    objAnimado.GetComponent<Animator>().SetBool("frente", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                }


            }
            //cima
            if (players[idPlayer].position.y > 2.4f)
            {
                if(players[idPlayer].position.x > transform.position.x - 0.5f && players[idPlayer].position.x < transform.position.x + 0.5f)
                {
                    //posição e rotação da mira (Cima frente)
                    if (transform.localScale.x == 1)
                    {
                        spawnBullet[0].position = mira[0].position;
                        spawnBullet[0].rotation = Quaternion.Euler(135, 90, 0);
                    }
                    else
                    {
                        //(diagonal cima Costas)
                        spawnBullet[0].position = mira[0].position;
                        spawnBullet[0].rotation = Quaternion.Euler(405, 90, 0);
                    }

                    objAnimado.GetComponent<Animator>().SetBool("cima", true);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                    objAnimado.GetComponent<Animator>().SetBool("frente", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                }
          

            }

            //baixo
            if (players[idPlayer].position.y < 2.0f && menorDistancia < 1)
            {
                if (players[idPlayer].position.x > transform.position.x - 0.5f && players[idPlayer].position.x < transform.position.x + 0.5f)
                {
                    //posição e rotação da mira (Baixo frente)
                    if (transform.localScale.x == 1)
                    {
                        spawnBullet[0].position = mira[4].position;
                        spawnBullet[0].rotation = Quaternion.Euler(90, 90, 0);
                    }
                    else
                    {
                        //(diagonal Baixo Costas)
                        spawnBullet[0].position = mira[4].position;
                        spawnBullet[0].rotation = Quaternion.Euler(270, 90, 0);
                    }

                    objAnimado.GetComponent<Animator>().SetBool("baixo", true);
                    objAnimado.GetComponent<Animator>().SetBool("cima", false);
                    objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
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
        distancePlayer = Vector3.Distance(players[0].position, transform.position);

        if (isAttack && !isColver )
        {
            print("Atirou");
                enemySound.ShootSound();
                objAnimado.GetComponent<Animator>().SetTrigger("atirou");
                Instantiate(bullet, spawnBullet[0].position, spawnBullet[0].rotation);
                isAttack = false;

        }
        else if (isColver && distancePlayer <= distanceAttack)
        {
                if(life>0)
                    StartCoroutine("cowndownHide");
            isColver = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            life--;
            if (life == 0)
            {
                isDead = true;

                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                objAnimado.GetComponent<Animator>().SetBool("idle", false);

                objAnimado.GetComponent<Animator>().SetTrigger("isDied");
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().useGravity = false;
                StopCoroutine("cowndownHide");
                StartCoroutine("morreu");
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Colver")
        {
            colver = other.gameObject;
            isColver = true;

            //diminui o colisor para a bullet não acertar ele se o player estiver na mesma altura
            GetComponent<BoxCollider>().size = new Vector3(0.712278f, 1.23f, 1);
            objAnimado.transform.position = new Vector3(objAnimado.transform.position.x, 0.23f, -0.3f);

            objAnimado.GetComponent<Animator>().SetBool("isHide", true);
        }
    }

    //tempo de tiro para inimigo no colver
    IEnumerator cowndownHide()
    {
        yield return new WaitForSeconds(cowdownFire*2);
        if(life>0)
        {
            objAnimado.GetComponent<Animator>().SetBool("isHide", false);
            //normaliza o colisor para a bullet poder acertar ele se o player estiver na mesma altura ou nao
            GetComponent<BoxCollider>().size = new Vector3(0.712278f, 1.744769f, 1);
            objAnimado.transform.position = new Vector3(objAnimado.transform.position.x, -0.23f, -0.3f);

            objAnimado.GetComponent<Animator>().SetTrigger("isHideAttack");

            isColver = true;
            StartCoroutine("hideTrue");
        }
        StopCoroutine("cowndownHide");
    }

    //ativa o Hide / colver
    IEnumerator hideTrue()
    {
        yield return new WaitForSeconds(1f);
        if (life > 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("isHide", true);
            //diminui o colisor para a bullet não acertar ele se o player estiver na mesma altura
            GetComponent<BoxCollider>().size = new Vector3(0.712278f, 1.23f, 1);
            objAnimado.transform.position = new Vector3(objAnimado.transform.position.x, 0.23f, -0.3f);


            StartCoroutine("timerSpawnBullet");
        }

        StopCoroutine("hideTrue");
    }

    //atraso no spawn da bullet para a saida do colver
    IEnumerator timerSpawnBullet()
    {
        yield return new WaitForSeconds(0.1f);
        if(life>0)
        {
            enemySound.ShootSound();

            //posição e rotação da mira (frente)
            if (transform.localScale.x == 1)
            {
                spawnBullet[0].position = mira[2].position;
                spawnBullet[0].rotation = Quaternion.Euler(180, 90, 0);
            }
            else
            {
                //(Costas)
                spawnBullet[0].position = mira[2].position;
                spawnBullet[0].rotation = Quaternion.Euler(0, 90, 0);
            }

            Instantiate(bullet, spawnBullet[0].position, spawnBullet[0].rotation);
        }

        StopCoroutine("hideTrue");
    }
   
    //tempo de tiro sem colver
    IEnumerator cowdown()
    {
        yield return new WaitForSeconds(cowdownFire);
        isAttack = true;
        if(colver != null)
        {
            isColver = true;
            objAnimado.GetComponent<Animator>().SetBool("isHide", true);
        }
        StopCoroutine("cowdown");
    }

    IEnumerator morreu()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        StopCoroutine("morreu");
    }
}
