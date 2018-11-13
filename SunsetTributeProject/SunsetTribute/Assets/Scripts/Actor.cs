using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour {
    protected bool isground, isRight;
    [SerializeField] protected float speed,jump;
    [SerializeField] protected GameObject bullet, barraLife, objAnimado;
    [SerializeField] private string[] inputs;
    [SerializeField] private Transform[] localSpawnBullet;
    protected float life, lifeMax;
    

	// Use this for initialization
	void Start () {
        lifeMax = life = 3;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject.Find("Main Camera").transform.position = new Vector3(transform.position.x, GameObject.Find("Main Camera").transform.position.y, GameObject.Find("Main Camera").transform.position.z);

        Move();
        attack();
	}

    private void Move()
    {
        //walk Direita ou esquerda
        if (Input.GetAxis("Horizontal")>0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", true);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (!isRight)
            {
                isRight = true;
                transform.Rotate(0, 180, 0);
            }

            transform.Translate(-speed*Time.deltaTime,0,0);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", true);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (isRight)
            {
                isRight = false;
                transform.Rotate(0, 180, 0);
            }
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }

        //walk DiagCima
        if (Input.GetAxis("Horizontal") > 0.7f && Input.GetAxis("Vertical") > 0f)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (!isRight)
            {
                
                isRight = true;
                transform.Rotate(0, 180, 0);
            }

            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetAxis("Horizontal") < -0.7f && Input.GetAxis("Vertical") > 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);

            if (isRight)
            {
                isRight = false;
                transform.Rotate(0, 180, 0);
            }
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }

        //walk DiagBaixo
        if (Input.GetAxis("Horizontal") > 0.7f && Input.GetAxis("Vertical") < 0f)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
            if (!isRight)
            {

                isRight = true;
                transform.Rotate(0, 180, 0);
            }

            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetAxis("Horizontal") < -0.7f && Input.GetAxis("Vertical") < 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);

            if (isRight)
            {
                isRight = false;
                transform.Rotate(0, 180, 0);
            }
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }

        //Cima ou Baixo
        if (Input.GetAxis("Vertical") > 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", true);
            objAnimado.GetComponent<Animator>().SetBool("baixo", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (!isRight)
            {
                isRight = true;
                transform.Rotate(0, 180, 0);
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            objAnimado.GetComponent<Animator>().SetBool("frente", false);
            objAnimado.GetComponent<Animator>().SetBool("cima", false);
            objAnimado.GetComponent<Animator>().SetBool("baixo", true);
            objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
            objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
            if (isRight)
            {
                isRight = false;
                transform.Rotate(0, 180, 0);
            }
        }

        //Jump
        if (Input.GetButtonDown("Jump") && isground)
        {
            isground = false;
            GetComponent<Rigidbody>().AddForce(0,jump,0);
        }
    }

    private void attack()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject aux;
            aux = Instantiate(bullet,localSpawnBullet[0].position, localSpawnBullet[0].rotation);
            
        }
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
            barraLife.GetComponent<Image>().fillAmount = life / lifeMax;
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
