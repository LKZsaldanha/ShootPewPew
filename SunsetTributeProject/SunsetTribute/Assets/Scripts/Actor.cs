using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour {
    protected bool isground, isRight,isRasteira, isAgachado;
    [SerializeField] protected float speed , jump, cooldownRasteira, speedRasteira;
    [SerializeField] protected GameObject bullet, objAnimado;
    [SerializeField] private string[] inputs;
    [SerializeField] private Transform[] localSpawnBullet;
    protected float life, lifeMax;

    //direções da mira
    private bool leftAim, rightAim, upAim, downAim,isIdle, isUp;
    //ultima direção horizontal
    private bool lastSideWasRight = true;

    //angulo para onde está a mira
    public int aimAngle = 0;

    [SerializeField] private float inputDeadZoneValue;

    // Use this for initialization
    void Start () {
        lifeMax = life = 3;
	}
	
	// Update is called once per frame
	void Update () {
         Move();

        attack();

        directionInput();
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
            GetComponent<Rigidbody>().AddForce(0,jump,0);
        }

        //Agachar
        if(Input.GetButtonDown(inputs[5]) && isground && !isRasteira)
        {
            if(!isAgachado)
            {
                GetComponent<BoxCollider>().size = new Vector3(0.712278f, 0.5f, 1);
                isAgachado = true;
                agacharAnim();
            }
            else
            {
                GetComponent<BoxCollider>().size = new Vector3(0.712278f, 1.744769f, 1);
                isAgachado = false;
                agacharAnim();
            }
            
        }
    }

    private void directionInput()
    {
        //Compara inputs para setar booleans
        if (Input.GetAxis(inputs[0]) > inputDeadZoneValue)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", true);
            isIdle = false;
            leftAim = false;
            rightAim = true;
            lastSideWasRight = true;
        }
        else if (Input.GetAxis(inputs[0]) < -inputDeadZoneValue)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", true);
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
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.Translate(speed * Time.deltaTime, 0, 0);

                    diagCima();

                    aimAngle = 45;
                    return;
                }
                //diagonal para frente e baixo
                if (downAim)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.Translate(speed * Time.deltaTime, 0, 0);

                    diagBaixo();

                    aimAngle = 135;
                    return;
                }
                //frente
                transform.localScale = new Vector3(1, 1, 1);
                transform.Translate(speed * Time.deltaTime, 0, 0);
                walkAnim();
                aimAngle = 90;
                return;
            }

            if (leftAim)
            {
                if (upAim)
                {
                    //diagonal para trás e cima
                    transform.localScale = new Vector3(-1, 1, 1);
                    transform.Translate(-speed * Time.deltaTime, 0, 0);

                    diagCima();
                    aimAngle = 315;
                    return;
                }

                if (downAim)
                {
                    //diagonal para trás e baixo
                    transform.localScale = new Vector3(-1, 1, 1);
                    transform.Translate(-speed * Time.deltaTime, 0, 0);

                    diagBaixo();

                    aimAngle = 225;
                    return;
                }
                //trás
                transform.localScale = new Vector3(-1, 1, 1);
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                walkAnim();
                aimAngle = 270;
                return;
            }

            if (upAim)
            {
                //cima
                cima();

                aimAngle = 0;
                return;
            }

            if (downAim)
            {
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
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
        objAnimado.GetComponent<Animator>().SetBool("idle", false);
        objAnimado.GetComponent<Animator>().SetBool("walk", true);
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
            objAnimado.GetComponent<Animator>().SetBool("rasteira", false);
        }
        else
        {
            objAnimado.GetComponent<Animator>().SetBool("rasteira", true);
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
        rasteiraAnim();
        isRasteira = false;
        StopCoroutine("Rasteira");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "chao" || collision.gameObject.tag == "chaoUp")
        {
            isground = true;
        }

        if(collision.gameObject.tag == "enemy")
        {
            life--;
            if (life<=0)
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.tag == "BulletEnemy")
        {
            Destroy(collision.gameObject);
            life--;
            if (life <= 0)
            {
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
