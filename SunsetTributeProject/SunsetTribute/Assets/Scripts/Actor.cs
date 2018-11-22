using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour {
    protected bool isground, isRight,isRasteira, isAgachado;
    [SerializeField] protected float speed , jump, cooldownRasteira, speedRasteira, lifeMax, life;
    [SerializeField] protected GameObject bullet, objAnimado;
    [SerializeField] private string[] inputs;
    [SerializeField] private Transform[] localSpawnBullet, mira;

    //direções da mira
    private bool leftAim, rightAim, upAim, downAim,isIdle, isUp;
    //ultima direção horizontal
    private bool lastSideWasRight = true;

    private PlayerSound playerSound;

    //angulo para onde está a mira
    public int aimAngle = 0;

    [SerializeField] private float inputDeadZoneValue;

    // Use this for initialization
    void Start () {
        isRight = true;
        lifeMax = life = 3;
        playerSound = GetComponent<PlayerSound>();
    }
	
	// Update is called once per frame
	void Update () {
        if(!isRasteira)
            Move();

        attack();

        if (!isAgachado)
        {
            directionInput();
        }
        
        SetAimStatus();

    }

    private void Move()
    {
        if(Input.GetAxis(inputs[0]) == 0 && Input.GetAxis(inputs[1]) == 0)
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
                GetComponent<Rigidbody>().AddForce(speedRasteira, 0, 0);
                GetComponent<BoxCollider>().size = new Vector3(3, 0.5f, 1);
            }                
            else
            {
                GetComponent<Rigidbody>().AddForce(-speedRasteira, 0, 0);
                GetComponent<BoxCollider>().size = new Vector3(3, 0.5f, 1);
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
                GetComponent<BoxCollider>().size = new Vector3(0.712278f, 1.3f, 1);
                objAnimado.transform.position = new Vector3(objAnimado.transform.position.x, 0.23f, -0.3f);
                agacharAnim();
                isAgachado = true;
            }
            else
            {
                GetComponent<BoxCollider>().size = new Vector3(0.712278f, 1.744769f, 1);
                objAnimado.transform.position = new Vector3(objAnimado.transform.position.x, -0.23f, -0.3f);
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
        print("baixo");
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", true);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", false);
    }

    private void cima()
    {
        print("cima");
        objAnimado.GetComponent<Animator>().SetBool("cima", true);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", false);
    }

    private void atirouAnim()
    {
        playerSound.ShootSound();//toca som de tiro no script PlayerSound
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
            objAnimado.GetComponent<Animator>().SetBool("isDash", true);
            print("rasteira");
        }
        else
        {
            transform.position = new Vector3(objAnimado.transform.position.x, 0.91f, -0.3f);
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
        yield return new WaitForSeconds(cooldownRasteira);
        GetComponent<BoxCollider>().size = new Vector3(0.712278f, 1.744769f, 1);
        isRasteira = false;
        rasteiraAnim();        
        StopCoroutine("Rasteira");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "chao" || collision.gameObject.tag == "chaoUp")
        {
            isground = true;
            jumpAnim();
        }

        if(collision.gameObject.tag == "enemy")
        {
            life--;
            if (life<=0)
            {
                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                objAnimado.GetComponent<Animator>().SetBool("idle", false);
                objAnimado.GetComponent<Animator>().SetBool("walk", false);
                objAnimado.GetComponent<Animator>().SetTrigger("isDied");

                GetComponent<Actor>().enabled = false;
               
                //Destroy(gameObject);
            }
        }

        if (collision.gameObject.tag == "BulletEnemy")
        {
            Destroy(collision.gameObject);
            life--;
            if (life == 0)
            {
                objAnimado.GetComponent<Animator>().SetBool("frente", false);
                objAnimado.GetComponent<Animator>().SetBool("cima", false);
                objAnimado.GetComponent<Animator>().SetBool("baixo", false);
                objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
                objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
                objAnimado.GetComponent<Animator>().SetBool("idle", false);
                objAnimado.GetComponent<Animator>().SetBool("walk", false);
                objAnimado.GetComponent<Animator>().SetTrigger("isDied");

                GetComponent<Actor>().enabled = false;
                //Destroy(gameObject);
            }          
        }

        if (collision.gameObject.tag == "life")
        {
            if(life<3)
            {
                life++;
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
}
