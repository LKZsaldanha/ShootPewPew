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

    protected override void Attack()
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
                    objAnimado.GetComponent<Animator>().SetTrigger("atirou");
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

    IEnumerator IsShowing()
    {
        if (isShowing)
        {
            objAnimado.GetComponent<Animator>().SetBool("idle", false);
            GetComponent<BoxCollider>().enabled = false;
            if (isHideBehind)
            {
                objAnimado.GetComponent<Animator>().SetTrigger("isHideBehind");
                yield return new WaitForSeconds(delayShootHideBehind - 0.3f);
                GetComponent<BoxCollider>().enabled = true;
                yield return new WaitForSeconds(0.3f);
                isHideBehind = false;
            }
            else if (walkFromCamera)
            {
                objAnimado.GetComponent<Animator>().SetTrigger("walkFromCamera");
                yield return new WaitForSeconds(delayShootwalkFromCamera - 0.3f);
                GetComponent<BoxCollider>().enabled = true;
                yield return new WaitForSeconds(0.3f);
                walkFromCamera = false;
            }
            else if (walkBehind)
            {
                objAnimado.GetComponent<Animator>().SetTrigger("walkBehind");
                yield return new WaitForSeconds(delayShootWalkBehind - 0.3f);
                GetComponent<BoxCollider>().enabled = true;
                yield return new WaitForSeconds(0.3f);
                walkBehind = false;
            }
            else if (crounched)
            {
                objAnimado.GetComponent<Animator>().SetTrigger("crounched");
                yield return new WaitForSeconds(delayShootCrounched - 0.3f);
                GetComponent<BoxCollider>().enabled = true;
                yield return new WaitForSeconds(0.3f);
                crounched = false;
            }
            else if (walkFromInside)
            {
                objAnimado.GetComponent<Animator>().SetTrigger("walkFromInside");
                yield return new WaitForSeconds(delayShootWalkFromInside - 0.3f);
                GetComponent<BoxCollider>().enabled = true;
                yield return new WaitForSeconds(0.3f);
                walkFromInside = false;
            }
            objAnimado.GetComponent<Animator>().SetBool("idle", true);
            isShowing = false;
            
            //Attack();
            StopCoroutine("IsShowing");
        }
    }
}
