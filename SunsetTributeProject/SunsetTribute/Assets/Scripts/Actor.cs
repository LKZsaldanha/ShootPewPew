using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour {
    private bool isground, isRight,isRasteira, isAgachado;
    //[SerializeField] private float gravity = 15f;
    [SerializeField] private float speed , jump, cooldownRasteira, speedRasteira;
    [SerializeField] private GameObject bullet, objAnimado, gameSystem;
    [SerializeField] private string[] inputs;
    [SerializeField] private Transform[] localSpawnBullet, mira;

    //direções da mira
    private bool leftAim, rightAim, upAim, downAim,isIdle, isUp, isInvencivel;
    //ultima direção horizontal
    private bool lastSideWasRight = true;

    private PlayerSound playerSound;

    //angulo para onde está a mira
    public int aimAngle = 0, lifeMax, life;

    [SerializeField] private float inputDeadZoneValue;

    //colisores
    private GameObject colisorRasteira, colisorAgachar;


    //valor do cx dois
    [SerializeField] private int valorDinheiro;
    public GameObject playerHUD;

    [SerializeField] private GameObject cam;

    private void Awake()
    {
        colisorRasteira = gameObject.transform.GetChild(0).gameObject;
        colisorAgachar = gameObject.transform.GetChild(1).gameObject;
        gameSystem = GameObject.Find("GameSystem");

    }
    // Use this for initialization
    void Start () {
        isInvencivel = true;
        StartCoroutine("invencivel");

        gameSystem.GetComponent<GameSystem>().nPlayerVivos.Add(gameObject);
        cam = GameObject.Find("Main Camera");

        if(cam.GetComponent<CameraFollow>().target == null)
            cam.GetComponent<CameraFollow>().target = transform.GetChild(2).transform;
        else
            cam.GetComponent<CameraFollow>().target2 = transform.GetChild(2).transform;

        colisorRasteira.GetComponent<CapsuleCollider>().enabled = false;
        colisorAgachar.GetComponent<BoxCollider>().enabled = false;

        if (gameObject.name == "Cube_Player")
        {
            playerHUD = GameObject.Find("HUD_Player_1");
            playerHUD.GetComponent<PlayerHUD>().playerHUDState = PlayerHUDState.playing;
            playerHUD.GetComponent<PlayerHUD>().SwitchPlayerHUDState(playerHUD.GetComponent<PlayerHUD>().playerHUDState);
        }
        else
        {
            playerHUD = GameObject.Find("HUD_Player_2");
            playerHUD.GetComponent<PlayerHUD>().playerHUDState = PlayerHUDState.playing;
            playerHUD.GetComponent<PlayerHUD>().SwitchPlayerHUDState(playerHUD.GetComponent<PlayerHUD>().playerHUDState);
        }


        isRight = true;
        lifeMax = life = 5;
        playerSound = GetComponent<PlayerSound>();
    }
	
	// Update is called once per frame
	void Update () {


        if (!isRasteira){
            Move();
            attack();
            directionInput();
            SetAimStatus();
        }

        
        if (!isAgachado)
        {
            directionInput();
        }
    }

    private void Move()
    {
        if(Input.GetAxis(inputs[0]) == 0 && Input.GetAxis(inputs[1]) == 0 && !isRasteira)
        {
            isIdle = true;
            objAnimado.GetComponent<Animator>().SetBool("walk", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            objAnimado.GetComponent<Animator>().SetBool("idle", true);
        }

        //Rasteira
        if (Input.GetButtonDown(inputs[4]) && isground && !isRasteira)
        {
            isRasteira = true;
            if(isRight)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().AddForce(speedRasteira, 0, 0);
                colisorRasteira.GetComponent<CapsuleCollider>().enabled = true;
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }                
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().AddForce(-speedRasteira, 0, 0);
                colisorRasteira.GetComponent<CapsuleCollider>().enabled = true;
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            rasteiraAnim();
            StartCoroutine("Rasteira");
        }

        //Jump
        if (Input.GetButtonDown(inputs[2]) && isground && !isRasteira)
        {
            isground = false;
            jumpAnim();
            GetComponent<Rigidbody>().AddForce(0,jump,0);
        }

        //Agachar
        if(Input.GetButtonDown(inputs[5]) && isground && !isRasteira)
        {
            if(!isAgachado)
            {
                colisorAgachar.GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                agacharAnim();
                isAgachado = true;
            }
            else
            {
                colisorAgachar.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<BoxCollider>().enabled = true;
                agacharAnim();
                isAgachado = false;
            }
            
        }
    }

    private void directionInput()
    {
        //Compara inputs para setar booleans
        if (Input.GetAxis(inputs[0]) > inputDeadZoneValue)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", true);
            isRight = true;
            isIdle = false;
            leftAim = false;
            rightAim = true;
            lastSideWasRight = true;
        }
        else if (Input.GetAxis(inputs[0]) < -inputDeadZoneValue)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", true);
            isRight = false;
            isIdle = false;
            leftAim = true;
            rightAim = false;
            lastSideWasRight = false;
        }
        else
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            leftAim = false;
            rightAim = false;
        }


        if (Input.GetAxis(inputs[1]) > inputDeadZoneValue)
        {
            isIdle = false;
            upAim = true;
            downAim = false;
        }
        else if (Input.GetAxis(inputs[1]) < -inputDeadZoneValue)
        {
            isIdle = false;
            upAim = false;
            downAim = true;
        }
        else
        {
            upAim = false;
            downAim = false;
        }

        //Reseta de volta para frente se nenhum input está sendo segurado
        if (!leftAim && !rightAim && !upAim && !downAim)
        {
            
            if (lastSideWasRight)
            {
                rightAim = true;
            }
            else
            {
                leftAim = true;
            }
        }
    }

    //converte a informação das booleans em um valor de angulo em sentido horario
    private void SetAimStatus()
    {
        if (!isIdle)
        {
            if (rightAim)
            {
                //diagonal para frente e cima
                if (upAim)
                {
                    if(!isAgachado)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        transform.Translate(speed * Time.deltaTime, 0, 0);
                        localSpawnBullet[0].position = mira[1].position;
                        localSpawnBullet[0].rotation = Quaternion.Euler(135,90,0);
                    }
                    

                    diagCima();

                    aimAngle = 45;
                    return;
                }
                //diagonal para frente e baixo
                if (downAim)
                {
                    if(!isAgachado)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        transform.Translate(speed * Time.deltaTime, 0, 0);
                        localSpawnBullet[0].position = mira[3].position;
                        localSpawnBullet[0].rotation = Quaternion.Euler(225, 90, 0);
                    }
                    

                    diagBaixo();

                    aimAngle = 135;
                    return;
                }
                //frente
                if(!isAgachado)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                    localSpawnBullet[0].position = mira[2].position;
                    localSpawnBullet[0].rotation = Quaternion.Euler(180, 90, 0);
                    walkAnim();
                }
                
                aimAngle = 90;
                return;
            }

            if (leftAim)
            {
                if (upAim)
                {
                    //diagonal para trás e cima
                    if(!isAgachado)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        transform.Translate(-speed * Time.deltaTime, 0, 0);
                        localSpawnBullet[0].position = mira[1].position;
                        localSpawnBullet[0].rotation = Quaternion.Euler(405, 90, 0);
                    }
                    

                    diagCima();
                    aimAngle = 315;
                    return;
                }

                if (downAim)
                {
                    //diagonal para trás e baixo
                    if(!isAgachado)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        transform.Translate(-speed * Time.deltaTime, 0, 0);
                        localSpawnBullet[0].position = mira[3].position;
                        localSpawnBullet[0].rotation = Quaternion.Euler(315, 90, 0);
                    }


                    diagBaixo();

                    aimAngle = 225;
                    return;
                }
                //trás
                if(!isAgachado)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    transform.Translate(-speed * Time.deltaTime, 0, 0);
                    localSpawnBullet[0].position = mira[2].position;
                    localSpawnBullet[0].rotation = Quaternion.Euler(0, 90, 0);
                    walkAnim();
                }

                aimAngle = 270;
                return;
            }

            if (upAim)
            {
                 localSpawnBullet[0].position = mira[0].position;
                localSpawnBullet[0].rotation = Quaternion.Euler(90,90,0);
                //cima
                cima();

                aimAngle = 0;
                return;
            }

            if (downAim)
            {
                localSpawnBullet[0].position = mira[4].position;
                localSpawnBullet[0].rotation = Quaternion.Euler(270, 90, 0);
                //baixo
                baixo();

                aimAngle = 180;
                return;
            }
        }
    }

    //animações
    #region animações
    private void diagCima()
    {
        objAnimado.GetComponent<Animator>().SetBool("frente", false);
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", true);
    }

    private void diagBaixo()
    {
        objAnimado.GetComponent<Animator>().SetBool("frente", false);
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", true);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);

    }
    
    private void baixo()
    {
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", true);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", false);
    }

    private void cima()
    {
        objAnimado.GetComponent<Animator>().SetBool("cima", true);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", false);
    }

    private void atirouAnim()
    {
        //playerSound.ShootSound();//toca som de tiro no script PlayerSound
        objAnimado.GetComponent<Animator>().SetTrigger("atirou");
    }


    private void walkAnim()
    {
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", true);
    }

    private void agacharAnim()
    {
        if (!isAgachado) { 
            objAnimado.GetComponent<Animator>().SetBool("agachou", true);
        }
        else
        {
            objAnimado.GetComponent<Animator>().SetBool("agachou", false);
        }
    }

    private void rasteiraAnim()
    {
        if (isRasteira) {
            playerSound.DashSound();
            objAnimado.GetComponent<Animator>().SetBool("isDash", true);
        }
        else
        {
            objAnimado.GetComponent<Animator>().SetBool("isDash", false);
        }
    }
    private void jumpAnim()
    {
        if (!isground)
        {
            objAnimado.GetComponent<Animator>().SetBool("jumpLand", false);
            objAnimado.GetComponent<Animator>().SetTrigger("jump");
        }
        else
        {
            objAnimado.GetComponent<Animator>().SetBool("jumpLand", true);
        }
    }

    private void morreuuu()
    {
        life--;
        playerHUD.GetComponent<PlayerHUD>().UpdateHUDLives(-1);

        objAnimado.GetComponent<Animator>().SetBool("frente", false);
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", false);
        objAnimado.GetComponent<Animator>().SetTrigger("isDied");

        playerSound.DeadSound();

        if (playerHUD.GetComponent<PlayerHUD>().playerLives <= 0)
        {
            if (gameObject.name == "Cube_Player")
                gameSystem.GetComponent<GameSystem>().gameOver1 = true;
            else
                gameSystem.GetComponent<GameSystem>().gameOver2 = true;

            StartCoroutine("morreu");

            playerHUD.GetComponent<PlayerHUD>().playerHUDState = PlayerHUDState.gameOver;
            playerHUD.GetComponent<PlayerHUD>().SwitchPlayerHUDState(playerHUD.GetComponent<PlayerHUD>().playerHUDState);

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Actor>().enabled = false;
            gameSystem.GetComponent<GameSystem>().lifePlayers(1, gameObject.name);
            gameSystem.GetComponent<GameSystem>().nPlayerAtivos(gameObject.name);
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Actor>().enabled = false;
            StartCoroutine("morreu");
        }
    }
    #endregion

    private void attack()
    {
        if(Input.GetButtonDown(inputs[3]) && !isRasteira)
        {
            atirouAnim();
            Instantiate(bullet,localSpawnBullet[0].position, localSpawnBullet[0].rotation);
        }
    }

    // Coroutine
    IEnumerator Rasteira()
    {
        objAnimado.GetComponent<Animator>().SetLayerWeight(1, 0);
        yield return new WaitForSeconds(cooldownRasteira);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        colisorRasteira.GetComponent<CapsuleCollider>().enabled = false;
        isRasteira = false;
        rasteiraAnim();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        objAnimado.GetComponent<Animator>().SetLayerWeight(1, 1);
        StopCoroutine("Rasteira");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "chao" || collision.gameObject.tag == "chaoUp" || collision.gameObject.tag == "Colver")
        {
            isground = true;
            jumpAnim();
        }


        if (collision.gameObject.tag == "gold")
        {
            playerHUD.GetComponent<PlayerHUD>().UpdateHUDScore(valorDinheiro);
            playerSound.GoldSound();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "enemy" && !isInvencivel)
        {
            life--;
            morreuuu();
        }

        if (collision.gameObject.tag == "BulletEnemy" && !isInvencivel)
        {
            Destroy(collision.gameObject);
            morreuuu();
        }
        else if (collision.gameObject.tag == "BulletEnemy" && isInvencivel)
            Destroy(collision.gameObject);

        if (collision.gameObject.tag == "life")
        {
            if(life<5)
            {
                life++;
                playerHUD.GetComponent<PlayerHUD>().UpdateHUDLives(1);
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "chaoUp")
        {
            if(Input.GetButtonDown(inputs[6]))
            {
              gameObject.transform.position = new Vector3(transform.position.x, other.gameObject.GetComponent<SensorUp>().localUp.position.y, other.gameObject.GetComponent<SensorUp>().localUp.position.z);                       
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bomb")
        {
            morreuuu();
            Destroy(other.gameObject);
        }

        if(other.tag == "spawner")
        {
            other.GetComponent<BoxCollider>().enabled = false;
            gameSystem.GetComponent<GameSystem>().quadranteSpawn();
        }
    }

    IEnumerator invencivel()
    {
        yield return new WaitForSeconds(3);
        isInvencivel = false;
        StopCoroutine("invencivel");
    }

    IEnumerator morreu()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        StopCoroutine("morreu");
    }
}
