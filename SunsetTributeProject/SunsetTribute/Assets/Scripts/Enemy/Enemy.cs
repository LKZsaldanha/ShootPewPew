using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //baixoMinimo é o valor minimo para chamar a animação de mira baixa e o baixoMaximo a mesma coisa
    [SerializeField] protected float life,DiagMinimo,baixoMinimo,baixoMaximo;
    [SerializeField] protected float speed, distanceAttack, cowdownFire, cowdownHide;
    public float delayFirstAttack = 0.5f;
    [SerializeField] protected GameObject bullet,objAnimado, gameSystem, colisorHide;
    [SerializeField] protected List<GameObject> itens;
    [SerializeField] protected Transform[] spawnBullet, mira;
    [SerializeField] protected List<Transform> players;

    
    protected bool inDelay = true, isBlockAtuli, isBlockAtuli2,isBlock1, isBlock2;

    protected bool blockAction;

    protected GameObject colver;
    protected bool isRight, isAttack, isIdPlayer, isDead, isColver, lookinPlayer;
    protected float lifeMax, distancePlayer, distancePlayer2, menorDistancia;
    protected int idPlayer;

    protected EnemySound enemySound;
    protected bool enemyCover; //verificação pra saber se é um inimigo de cover

    public bool normalEnemy = true;

    private void Awake()
    {
        blockAction = true;
        gameSystem = GameObject.Find("GameSystem");
    }

    // Use this for initialization
    protected virtual void Start () {
        enemySound = GetComponent<EnemySound>();

        lifeMax = life;
	}

    // Update is called once per frame
    protected virtual void Update () {
        players.RemoveAll(c => c == null);

        atualizaEnemy();

        if(players.Count>0)
        {
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

                        if (colver == null)
                        {
                            Move();
                        }

                        if (distanceAttack > distancePlayer)
                        {
                            StartCoroutine("DelayAttack");
                            distanceAttack = 10f;
                            if (inDelay)
                             {//delay pra personagem que entra correndo ou pulando
                                 if (menorDistancia < distanceAttack - 2.0f)
                                {
                                    StartCoroutine("DelayAttack");
                                }
                           }
                            else if (!isColver)
                            {
                                Attack();
                            }
                        }
                    }
                }
            }
        }

            

	}

    void atualizaEnemy()
    {
        if (GameObject.Find("NewPlayerPrefab") != null && !isBlockAtuli)
        {
            if(gameSystem.GetComponent<GameSystem>().nPlayerVivos[0].name == "NewPlayerPrefab")
                players.Add(gameSystem.GetComponent<GameSystem>().nPlayerVivos[0].transform);

            isBlockAtuli = true;
            isBlock1 = true;
        }
        else if (GameObject.Find("NewPlayerPrefab2") != null && !isBlockAtuli2)
        {
            if (gameSystem.GetComponent<GameSystem>().nPlayerVivos[1].name == "NewPlayerPrefab2")
                players.Add(gameSystem.GetComponent<GameSystem>().nPlayerVivos[1].transform);

            isBlockAtuli2 = true;
            isBlock2 = true;
        }

        if (GameObject.Find("NewPlayerPrefab") == null && isBlock1)
        {
            isBlockAtuli = false;
            isBlock1 = false;
        }
        else if (GameObject.Find("NewPlayerPrefab2") == null && isBlock2)
        {
            isBlockAtuli2 = false;
            isBlock2 = false;
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
                if(!inDelay){
                    StartCoroutine("cowdown");
                }
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
                        
                        //print(""+gameObject.name);
                        //posição e rotação da mira (frente)
                        if (transform.localScale.x == 1)
                        {
                            objAnimado.GetComponent<Animator>().SetBool("frente", true);
                            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                            objAnimado.GetComponent<Animator>().SetBool("cima", false);
                            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                            spawnBullet[0].position = mira[2].position;
                            spawnBullet[0].rotation = Quaternion.Euler(180, 90, 0);
                        }
                        else
                        {
                            //(Costas)
                            objAnimado.GetComponent<Animator>().SetBool("frente", true);
                            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                            objAnimado.GetComponent<Animator>().SetBool("cima", false);
                            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                            spawnBullet[0].position = mira[2].position;
                            spawnBullet[0].rotation = Quaternion.Euler(0, 90, 0);
                        }

                    }
                    else
                    if (players[idPlayer].position.y < transform.position.y)
                    {
                        if (distancePlayer > DiagMinimo)
                        {
                            
                            //posição e rotação da mira (diagonal Baixo frente)
                            if (transform.localScale.x == 1)
                            {
                                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
                                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                                
                                spawnBullet[0].position = mira[3].position;
                                spawnBullet[0].rotation = Quaternion.Euler(225, 90, 0);
                            }
                            else
                            {
                                //(diagonal Baixo Costas)
                                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
                                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                                objAnimado.GetComponent<Animator>().SetBool("baixo", false);

                                spawnBullet[0].position = mira[3].position;
                                spawnBullet[0].rotation = Quaternion.Euler(315, 90, 0);
                            }
                        }

                    }
                    else
                    if (players[idPlayer].position.y > transform.position.y && menorDistancia > 2)
                    {
                        if (distancePlayer > DiagMinimo)
                        {
                            
                            //posição e rotação da mira (diagonal cima frente)
                            if (transform.localScale.x == 1)
                            {
                                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
                                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                                spawnBullet[0].position = mira[1].position;
                                spawnBullet[0].rotation = Quaternion.Euler(135, 90, 0);
                            }
                            else
                            {
                                //(diagonal cima Costas)
                                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
                                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                                spawnBullet[0].position = mira[1].position;
                                spawnBullet[0].rotation = Quaternion.Euler(405, 90, 0);
                            }
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
                                
                                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                                objAnimado.GetComponent<Animator>().SetBool("cima", true);
                                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                                objAnimado.GetComponent<Animator>().SetBool("idle", false);

                                spawnBullet[0].position = mira[0].position;
                                spawnBullet[0].rotation = Quaternion.Euler(135, 90, 0);
                            }
                            else
                            {
                                //(diagonal cima Costas)
                                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                                objAnimado.GetComponent<Animator>().SetBool("cima", true);
                                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                                objAnimado.GetComponent<Animator>().SetBool("idle", false);
                                spawnBullet[0].position = mira[0].position;
                                spawnBullet[0].rotation = Quaternion.Euler(405, 90, 0);
                            }
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
                                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                                objAnimado.GetComponent<Animator>().SetBool("baixo", true);
                                objAnimado.GetComponent<Animator>().SetBool("idle", false);
                                
                                spawnBullet[0].position = mira[4].position;
                                spawnBullet[0].rotation = Quaternion.Euler(90, 90, 0);
                            }
                            else
                            {
                                //(diagonal Baixo Costas)
                                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                                objAnimado.GetComponent<Animator>().SetBool("baixo", true);
                                objAnimado.GetComponent<Animator>().SetBool("idle", false);

                                spawnBullet[0].position = mira[4].position;
                                spawnBullet[0].rotation = Quaternion.Euler(270, 90, 0);
                            }
                        }
                    }
                }


            }
            else
            {
                StopCoroutine("cowdown");
            }
        }
    }


   

    protected virtual void Attack()
    {
        
        if(players.Count!=0)
        {        
            if (isAttack && !isColver)
            {
                //print("Atirou");
                
                objAnimado.GetComponent<Animator>().SetTrigger("atirou");
                
                GameObject bulletNew = Instantiate(bullet, spawnBullet[0].position, spawnBullet[0].rotation);
                bulletNew.GetComponent<BulletPlayer>().target = players[0].gameObject;
                
                isAttack = false;
            }
            else if (isColver && distancePlayer <= distanceAttack)
            {
                if (life > 0){
                    StartCoroutine("cowndownHide");
                }
                isColver = false;
            }
        }
        

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            life--;
            if (life <= 0)
            {
                isDead = true;

                
                colisorHide.GetComponent<BoxCollider>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().useGravity = false;

                if(itens.Count != 0 && normalEnemy){
                    Instantiate(itens[0], new Vector3(transform.position.x, transform.position.y, players[0].position.z), transform.rotation);
                }

                StartCoroutine("morreu");
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Colver" && !enemyCover)
        {
            enemyCover = true; //entrar nessa função somente 1 vez;
            print("entrouTrigger");
            colver = other.gameObject;
            isColver = true;
            objAnimado.GetComponent<Animator>().SetBool("isHide", true);
            objAnimado.GetComponent<Animator>().SetTrigger("isHideCrounch");
            GetComponent<BoxCollider>().enabled = false;
            
            if(colisorHide != null){
                colisorHide.GetComponent<BoxCollider>().enabled = true;
            }

            StartCoroutine("hideTrue");
            
        }
    }

    IEnumerator DelayAttack()
    {
        if (inDelay && !isColver)
        {
            objAnimado.GetComponent<Animator>().SetBool("idle", true);
            yield return new WaitForSeconds(delayFirstAttack);
            isAttack = true;
            Attack();
            inDelay = false;

            //yield return new WaitForSeconds(cowdownFire - delayFirstAttack);

            StopCoroutine("DelayAttack");
        }
    }

    //ativa o Hide / colver
    IEnumerator hideTrue()
    {
        
        if (life > 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("isHide", true);
            //diminui o colisor para a bullet não acertar ele se o player estiver na mesma altura
            GetComponent<BoxCollider>().enabled = false;
            if(colisorHide != null){
                colisorHide.GetComponent<BoxCollider>().enabled = true;
            }
            yield return new WaitForSeconds(cowdownHide);
            StartCoroutine("cowndownHide");
        }

        StopCoroutine("hideTrue");
    }
    //tempo de tiro para inimigo no colver
    IEnumerator cowndownHide()
    {
        objAnimado.GetComponent<Animator>().SetBool("isHide", false);
        //normaliza o colisor para a bullet poder acertar ele se o player estiver na mesma altura ou nao
        GetComponent<BoxCollider>().enabled = true;
        if(colisorHide != null){
            colisorHide.GetComponent<BoxCollider>().enabled = false;
        }

        yield return new WaitForSeconds(cowdownFire);
        if(life>0)
        {
            objAnimado.GetComponent<Animator>().SetTrigger("isHideAttack");
           // if(pla != null){
                GameObject bulletNew = Instantiate(bullet, spawnBullet[0].position, spawnBullet[0].rotation);
                if(players.Count != null){
                    bulletNew.GetComponent<BulletPlayer>().target = players[0].gameObject;
                }
            //}
        yield return new WaitForSeconds(cowdownFire);
            isColver = true;
            StartCoroutine("hideTrue");
        }
        StopCoroutine("cowndownHide");
    }
   
    //tempo de tiro sem colver
    IEnumerator cowdown()
    {
        yield return new WaitForSeconds(cowdownFire);
        isAttack = true;

        StopCoroutine("cowdown");
    }

    IEnumerator morreu()
    {
        enemySound.DeadSound();
        objAnimado.GetComponent<Animator>().SetLayerWeight (objAnimado.GetComponent<Animator>().GetLayerIndex ("Aim"), 0);
        objAnimado.GetComponent<Animator>().SetTrigger("isDied");

        StopCoroutine("cowdown");
        StopCoroutine("DelayAttack");
        StopCoroutine("hideTrue");
        StopCoroutine("hideTrStopCoroutineue");
        StopCoroutine("cowdown");
        StopCoroutine("cowndownHide");
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        StopCoroutine("morreu");
    }
}
