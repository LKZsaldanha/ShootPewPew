﻿using System.Collections;
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
    private bool leftAim, rightAim, upAim, downAim;
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
        //walk Direita ou esquerda
        if (Input.GetAxis(inputs[0])>0 && Input.GetAxis(inputs[1]) == 0f)
        {
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
            if (!isRight)
            {
                isRight = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (Input.GetAxis(inputs[1]) < 0)
        {
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

    private void directionInput()
    {
        //Compara inputs para setar booleans
        if (Input.GetAxis(inputs[0]) > inputDeadZoneValue)
        {
            leftAim = false;
            rightAim = true;
            lastSideWasRight = true;
        }
        else if (Input.GetAxis(inputs[0]) < -inputDeadZoneValue)
        {
            leftAim = true;
            rightAim = false;
            lastSideWasRight = false;
        }
        else
        {
            leftAim = false;
            rightAim = false;
        }


        if (Input.GetAxis(inputs[1]) > inputDeadZoneValue)
        {
            upAim = true;
            downAim = false;
        }
        else if (Input.GetAxis(inputs[1]) < -inputDeadZoneValue)
        {
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
        if (rightAim)
        {
            //diagonal para frente e cima
            if (upAim)
            {
                diagCima();

                aimAngle = 45;
                return;
            }
            //diagonal para frente e baixo
            if (downAim)
            {
                diagBaixo();

                aimAngle = 135;
                return;
            }
            //frente
            aimAngle = 90;
            return;
        }

        if (leftAim)
        {
            if (upAim)
            {
                //diagonal para trás e cima
                diagCima();
                aimAngle = 315;
                return;
            }

            if (downAim)
            {
                //diagonal para trás e baixo

                diagBaixo();

                aimAngle = 225;
                return;
            }
            //trás
            frente();
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

    //animações
    #region animações
    private void diagCima()
    {
        objAnimado.GetComponent<Animator>().SetBool("frente", false);
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", true);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
    }

    private void diagBaixo()
    {
        objAnimado.GetComponent<Animator>().SetBool("frente", false);
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", true);
    }

    private void frente()
    {
        objAnimado.GetComponent<Animator>().SetBool("frente", true);
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
    }

    private void baixo()
    {
        objAnimado.GetComponent<Animator>().SetBool("frente", false);
        objAnimado.GetComponent<Animator>().SetBool("cima", false);
        objAnimado.GetComponent<Animator>().SetBool("baixo", true);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
    }

    private void cima()
    {
        objAnimado.GetComponent<Animator>().SetBool("frente", false);
        objAnimado.GetComponent<Animator>().SetBool("cima", true);
        objAnimado.GetComponent<Animator>().SetBool("baixo", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagCima", false);
        objAnimado.GetComponent<Animator>().SetBool("DiagBaixo", false);
    }
    #endregion

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

}
