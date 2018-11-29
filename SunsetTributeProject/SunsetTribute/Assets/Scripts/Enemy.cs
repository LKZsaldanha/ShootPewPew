using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //baixoMinimo é o valor minimo para chamar a animação de mira baixa e o baixoMaximo a mesma coisa
    [SerializeField] protected float life,DiagMinimo,baixoMinimo,baixoMaximo;
    [SerializeField] protected float speed, distanceAttack, cowdownFire;
    [SerializeField] protected GameObject bullet,objAnimado, gameSystem, colisorHide;
    [SerializeField] protected List<GameObject> itens;
    [SerializeField] protected Transform[] spawnBullet, mira;
    [SerializeField] protected List<Transform> players;
    [SerializeField] protected Transform mySpawn;

    public float delayFirstAttack = 0.5f;
    private bool inDelay = true;

    protected bool blockAction;

    protected GameObject colver;
    protected bool isRight, isAttack, isIdPlayer, isDead, isColver, lookinPlayer;
    protected float lifeMax, distancePlayer, distancePlayer2, menorDistancia;
    protected int idPlayer;

    private EnemySound enemySound;

    private void Awake()
    {
        blockAction = true;
        gameSystem = GameObject.Find("GameSystem");
    }

    // Use this for initialization
    void Start () {
        //Vai setar o numero de player que estão em game
        StartCoroutine("DelayAttack");
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
    protected void Update () {
        players.RemoveAll(c => c == null);

        //Aqui deve-se ser inserido as animações pré-Violencia (antes do tiroteio)
        if (blockAction)
        {
            distancePlayer = Vector3.Distance(players[0].position, transform.position);
            menorDistancia = distancePlayer;

            if (players.Count > 1)
                distancePlayer2 = Vector3.Distance(players[1].position, transform.position);
            if (distancePlayer > distancePlayer2)
                menorDistancia = distancePlayer2;
            else
                menorDistancia = distancePlayer;

            if (menorDistancia < distanceAttack)
            {
                blockAction = false;
            }
        }
        else
        {
            if (players.Count != 0)
            {
                distancePlayer = Vector3.Distance(players[0].position, transform.position);
                if (!isDead && players.Count >= 1)
                {
                    if (transform.position.x < players[players.Count - 1].position.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }

                    modNPlayers();

                    if (colver == null){
                        Move();
                    }
                    if (menorDistancia < distanceAttack)
                    {
                        Attack();
                    }
                }
            }
        }
            

	}


    //Sinaliza aos Inimigos quantos players estão em jogo
    private void modNPlayers()
    {
        if (gameSystem.GetComponent<GameSystem>().nPlayerVivos.Count > players.Count)
        {
            if(players[0].name != gameSystem.GetComponent<GameSystem>().nameplayer)
            {
                print("erro aqui: "+ gameSystem.GetComponent<GameSystem>().nameplayer);
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
        if (players.Count != 0)
        {
            distancePlayer = Vector3.Distance(players[0].position, transform.position);
            menorDistancia = distancePlayer;
            if (players.Count > 1)
            {
                distancePlayer2 = Vector3.Distance(players[1].position, transform.position);
                if (distancePlayer <= distancePlayer2)
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
                    idPlayer = Random.Range(0, players.Count);
                    isIdPlayer = true;
                }

                //caso tiver apenas um player em jogo o idPlayer fica em 0
                if (players.Count == 1)
                {
                    idPlayer = 0;
                }

                //caso não haja player em jogo esse processo não ativa
                if (players.Count != 0)
                {
                    if (transform.position.x > players[idPlayer].position.x)
                        transform.localScale = new Vector3(-1, 1, 1);
                    else
                        transform.localScale = new Vector3(1, 1, 1);

                    //mesmo nivel de altura do player
                    if (players[idPlayer].position.y < transform.position.y + 0.5f && players[idPlayer].position.y > transform.position.y - 0.5f)
                    {
                        if(!inDelay){
                            StartCoroutine("cowdown");
                        }
                        //print(""+gameObject.name);
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

                        objAnimado.GetComponent<Animator>().SetBool("cima", false);
                        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                        objAnimado.GetComponent<Animator>().SetBool("frente", true);
                        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                        objAnimado.GetComponent<Animator>().SetBool("idle", false);
                    }
                    else
                    if (players[idPlayer].position.y < transform.position.y)
                    {
                        if (distancePlayer > DiagMinimo)
                        {
                            if(!inDelay){
                                StartCoroutine("cowdown");
                            }
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
                    else
                    if (players[idPlayer].position.y > transform.position.y && menorDistancia > 2)
                    {
                        if (distancePlayer > DiagMinimo)
                        {
                            if(!inDelay){
                                StartCoroutine("cowdown");
                            }
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
                    else if (players[idPlayer].position.y > 2.4f)
                    {
                        if (players[idPlayer].position.x > transform.position.x - 0.5f && players[idPlayer].position.x < transform.position.x + 0.5f)
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
                    else if (players[idPlayer].position.y < 2.0f && menorDistancia < 1)
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
    }


    IEnumerator DelayAttack()
    {
        if(inDelay){
            objAnimado.GetComponent<Animator>().SetBool("idle", true);
            yield return new WaitForSeconds(delayFirstAttack);
            inDelay = false;
            StopCoroutine("DelayAttack");
        }
    }

    private void Attack()
    {
        if(players.Count!=0)
        {
            

            if (isAttack && !isColver)
            {
                print("Atirou");
                //enemySound.ShootSound();
                objAnimado.GetComponent<Animator>().SetTrigger("atirou");
                Instantiate(bullet, spawnBullet[0].position, spawnBullet[0].rotation);
                isAttack = false;

            }
            else if (isColver && distancePlayer <= distanceAttack)
            {
                if (life > 0)
                    StartCoroutine("cowndownHide");
                isColver = false;
            }
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

                enemySound.DeadSound();

                objAnimado.GetComponent<Animator>().SetTrigger("isDied");
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().useGravity = false;

                Instantiate(itens[0], new Vector3(transform.position.x, transform.position.y, players[0].position.z), transform.rotation);

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

            StartCoroutine("hideTrue");
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
            GetComponent<BoxCollider>().enabled = true;
            colisorHide.GetComponent<BoxCollider>().enabled = false;


            objAnimado.GetComponent<Animator>().SetTrigger("isHideAttack");

            isColver = true;
            StartCoroutine("hideTrue");
        }
        StopCoroutine("cowndownHide");
    }

    //ativa o Hide / colver
    IEnumerator hideTrue()
    {
        yield return new WaitForSeconds(1);
        if (life > 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("isHide", true);
            //diminui o colisor para a bullet não acertar ele se o player estiver na mesma altura
            GetComponent<BoxCollider>().enabled = false;
            colisorHide.GetComponent<BoxCollider>().enabled = true;


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
            //enemySound.ShootSound();

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
