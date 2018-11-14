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
    

	// Use this for initialization
	void Start () {
        lifeMax = life = 3;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        attack();
	}

    private void Move()
    {
        //walk Direita ou esquerda
        if (Input.GetAxis(inputs[0])>0 && Input.GetAxis(inputs[1]) == 0f)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", true);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (!isRight && isground)
            {
                isRight = true;
            }
            if(isRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
               
        }
        else if (Input.GetAxis(inputs[0]) < 0 && Input.GetAxis(inputs[1]) == 0f)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", true);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (isRight && isground)
            {
                isRight = false;
                
            }
            if(!isRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
                
        }

        //walk DiagCima
        if (Input.GetAxis(inputs[0]) > 0.7f && Input.GetAxis(inputs[1]) > 0f)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (!isRight && isground)
            {
                isRight = true;
                
            }
            if (isRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
                
        }
        else if (Input.GetAxis(inputs[0]) < -0.7f && Input.GetAxis(inputs[1]) > 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);

            if (isRight && isground)
            {
                isRight = false;
               
            }
            if (!isRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }

        }

        //walk DiagBaixo
        if (Input.GetAxis(inputs[0]) > 0.7f && Input.GetAxis(inputs[1]) < 0f)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
            if (!isRight && isground)
            {

                isRight = true;
                
            }
            if (isRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
                
        }
        else if (Input.GetAxis(inputs[0]) < -0.7f && Input.GetAxis(inputs[1]) < 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);

            if (isRight && isground)
            {
                isRight = false;
               
            }
            if (!isRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
                
        }

        //Cima ou Baixo
        if (Input.GetAxis(inputs[1]) > 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", true);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (!isRight)
            {
                isRight = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (Input.GetAxis(inputs[1]) < 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", true);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (isRight)
            {
                isRight = false;
                transform.localScale = new Vector3(-1, 1, 1);
            }
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
                GetComponent<BoxCollider>().size = new Vector3(1, 0.5f, 1);
                isAgachado = true;
            }
            else
            {
                GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
                isAgachado = false;
            }
            
        }
    }

    private void attack()
    {
        if(Input.GetButtonDown(inputs[3]) && !isRasteira)
        {
            GameObject aux;
            aux = Instantiate(bullet,localSpawnBullet[0].position, localSpawnBullet[0].rotation);
            
        }
    }

    // Coroutine
    IEnumerator Rasteira()
    {
        yield return new WaitForSeconds(cooldownRasteira);
        GetComponent<BoxCollider>().size = new Vector3(1,1,1);
        isRasteira = false;
        StopCoroutine("Rasteira");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "chao")
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
                Destroy(gameObject);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "coin")
        {
            GameObject.Find("GameSystem").GetComponent<GameSystem>().coin += 10;
            Destroy(other.gameObject);
        }
    }
}
