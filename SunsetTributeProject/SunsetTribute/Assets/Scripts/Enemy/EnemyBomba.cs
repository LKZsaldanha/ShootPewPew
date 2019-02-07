using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomba : Enemy
{
    private bool isShowing;

    [SerializeField] private bool canThrow = false;
    [SerializeField] private bool isHideBehind, walkFromCamera, walkBehind, crounched, walkFromInside = false;
    [SerializeField] private GameObject bomb;

    [SerializeField] private float timerToThrowBomb = 2.0f;
    [SerializeField] private float timerToThrowBombAnim = 0.5f;

    [SerializeField] private float delayShootHideBehind = 1.0f;
    [SerializeField] private float delayShootwalkFromCamera = 1.0f;
    [SerializeField] private float delayShootWalkBehind = 1.0f;
    [SerializeField] private float delayShootCrounched = 1.0f;
    [SerializeField] private float delayShootWalkFromInside = 1.0f;


    public GameObject[] waypoints;
    private int current = 0;
    public float speedWayPoint = 1.5f;
    public float WPradius = 0.1f;

	private bool moving = true;
    public bool triggered = true;

    private bool startAnimation = true;

    private void Awake()
    {
        blockAction = true;
        gameSystem = GameObject.Find("GameSystem");
        if (canThrow)
        {
            //isShowing = false;
            objAnimado.GetComponent<Animator>().SetBool("idle", false);
            //objAnimado.GetComponent<Animator>().SetLayerWeight(1, 0);
            objAnimado.GetComponent<Animator>().SetBool("canThrow", true);
        }
        else if (isHideBehind || walkFromCamera || walkBehind || crounched || walkFromInside)
        {
            //print("entrou");
            isShowing = true;
            objAnimado.GetComponent<Animator>().SetBool("idle", false);
            StartCoroutine("IsShowing");
            //IsShowing();
        }
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        base.normalEnemy = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
        if(!isShowing){
            base.Update();
        }
	}

    IEnumerator Attack()
    {
        if (players.Count != 0)
        {
            distancePlayer = Vector3.Distance(players[0].position, transform.position);


            if (isAttack && !isColver)
            {
                if (canThrow)
                {
                    StartCoroutine("throwBombCooldown");
                    isAttack = false;
                }
                else
                {
                    if(triggered){
                        objAnimado.GetComponent<Animator>().SetTrigger("atirou");
                        triggered = false;
                    }
                    yield return new WaitForSeconds(0.5f);
                    Instantiate(bullet, spawnBullet[0].position, spawnBullet[0].rotation);
                    isAttack = false;
                }

            }
            else if (isColver && distancePlayer <= distanceAttack)
            {
                if (life > 0)
                    StartCoroutine("cowndownHide");
                isColver = false;
            }
            triggered = true; //pode atirar novamente
            StopCoroutine("Attack");
        }

    }

    IEnumerator throwBombCooldown()
    {
        if (canThrow)
        {
            objAnimado.GetComponent<Animator>().SetBool("throwBomb", true);


            yield return new WaitForSeconds(0.1f);
            objAnimado.GetComponent<Animator>().SetBool("throwBomb", false);

            yield return new WaitForSeconds(timerToThrowBombAnim);
//            print("werwer12");
            bomb.GetComponent<EnemyBomb>().impacto = players[idPlayer].position;
            bomb.GetComponent<EnemyBomb>().Tiro();

            StopCoroutine("throwBombCooldown");
        }
    }

    void FixedUpdate(){
        if(isHideBehind || walkFromCamera || walkBehind || crounched || walkFromInside){
            if(base.isDead == false){
                if(moving){
                    if(Vector3.Distance(waypoints[current].transform.position, transform.position) < WPradius)
                    {
                        current ++;
                        if (current >= waypoints.Length)
                        {
                            moving = false;
                            objAnimado.GetComponent<Animator>().SetBool("idle", true);
                            startAnimation = false;
                            StartCoroutine("IsShowing");
                        }
                    }
                    if(moving){
                        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speedWayPoint);
                    }  
                }
            }
        }
    }

    IEnumerator IsShowing()
    {
        if (isShowing)
        {
            objAnimado.GetComponent<Animator>().SetLayerWeight (objAnimado.GetComponent<Animator>().GetLayerIndex ("Aim"), 0);
            //GetComponent<BoxCollider>().enabled = false;
            
            if (isHideBehind)
            {
                if(startAnimation){
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                    objAnimado.GetComponent<Animator>().SetTrigger("isHideBehind");
                }else{
                    yield return new WaitForSeconds(1f);
                    objAnimado.GetComponent<Animator>().SetLayerWeight (objAnimado.GetComponent<Animator>().GetLayerIndex ("Aim"), 1);
                    isShowing = false;
                }
            }
            else if (walkFromCamera)
            {
                
                if(startAnimation){
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                    objAnimado.GetComponent<Animator>().SetTrigger("walkFromCamera");
                }else{
                    yield return new WaitForSeconds(1f);
                    objAnimado.GetComponent<Animator>().SetLayerWeight (objAnimado.GetComponent<Animator>().GetLayerIndex ("Aim"), 1);
                    isShowing = false;
                }
            }
            else if (walkBehind)
            {
                if(startAnimation){
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                    objAnimado.GetComponent<Animator>().SetTrigger("walkBehind");
                }else{
                    yield return new WaitForSeconds(1f);
                    objAnimado.GetComponent<Animator>().SetLayerWeight (objAnimado.GetComponent<Animator>().GetLayerIndex ("Aim"), 1);
                    isShowing = false;
                }
            }
            else if (crounched)
            {
                if(startAnimation){
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                    objAnimado.GetComponent<Animator>().SetTrigger("crounched");
                }else{
                    yield return new WaitForSeconds(1f);
                    objAnimado.GetComponent<Animator>().SetLayerWeight (objAnimado.GetComponent<Animator>().GetLayerIndex ("Aim"), 1);
                    isShowing = false;
                }
            }
            else if (walkFromInside)
            {
                if(startAnimation){
                    objAnimado.GetComponent<Animator>().SetBool("idle", false);
                    objAnimado.GetComponent<Animator>().SetTrigger("walkFromInside");
                }else{
                    yield return new WaitForSeconds(1f);
                    objAnimado.GetComponent<Animator>().SetLayerWeight (objAnimado.GetComponent<Animator>().GetLayerIndex ("Aim"), 1);
                    isShowing = false;
                }
            }
            StopCoroutine("IsShowing");
        }
        //return null;
    }
}
