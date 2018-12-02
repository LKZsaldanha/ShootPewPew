using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitBoxPlayer : MonoBehaviour {

    private PlayerSound playerSound;

    //valor do cx dois
    [SerializeField] private int valorDinheiro;
    public GameObject playerHUD;

    [SerializeField] private GameObject objAnimado, gameSystem,cam;

    private bool isInvencivel;

    //angulo para onde está a mira
    public int life;

    private void Awake()
    {
        gameSystem = GameObject.Find("GameSystem");
        cam = GameObject.Find("Main Camera");

        if (cam.GetComponent<CameraFollow>().target == null)
        {
            cam.GetComponent<CameraFollow>().target = transform.parent.GetComponentInChildren<Collider2D>().transform;
        }            
        else
        {
            cam.GetComponent<CameraFollow>().target2 = transform.parent.GetComponentInChildren<Collider2D>().transform;
        }
           
    }

    // Use this for initialization
    void Start () {
        isInvencivel = true;
        StartCoroutine("invencivel");

        gameSystem.GetComponent<GameSystem>().nPlayerVivos.Add(gameObject.transform.parent.gameObject);
        


        if (gameObject.name == "InteractionColliderYAGO")
        {
            playerHUD = GameObject.Find("HUD_Player_1");
            playerHUD.GetComponent<PlayerHUD>().playerHUDState = PlayerHUDState.playing;
            playerHUD.GetComponent<PlayerHUD>().SwitchPlayerHUDState(playerHUD.GetComponent<PlayerHUD>().playerHUDState);
        }
        else if (gameObject.name == "InteractionColliderJOHN")
        {
            playerHUD = GameObject.Find("HUD_Player_2");
            playerHUD.GetComponent<PlayerHUD>().playerHUDState = PlayerHUDState.playing;
            playerHUD.GetComponent<PlayerHUD>().SwitchPlayerHUDState(playerHUD.GetComponent<PlayerHUD>().playerHUDState);
        }


        life = 5;
        playerSound = GetComponent<PlayerSound>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void morreuuu()
    {
        life--;
        playerHUD.GetComponent<PlayerHUD>().UpdateHUDLives(-1);

        transform.parent.GetComponent<CharacterMovement>().enabled = false;

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
            if (gameObject.name == "InteractionColliderYAGO")
                gameSystem.GetComponent<GameSystem>().gameOver1 = true;
            else
                gameSystem.GetComponent<GameSystem>().gameOver2 = true;

            StartCoroutine("morreu");

            playerHUD.GetComponent<PlayerHUD>().playerHUDState = PlayerHUDState.gameOver;
            playerHUD.GetComponent<PlayerHUD>().SwitchPlayerHUDState(playerHUD.GetComponent<PlayerHUD>().playerHUDState);

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<HitBoxPlayer>().enabled = false;
            gameSystem.GetComponent<GameSystem>().lifePlayers(1, gameObject.name);
            gameSystem.GetComponent<GameSystem>().nPlayerAtivos(gameObject.name);
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<HitBoxPlayer>().enabled = false;
            StartCoroutine("morreu");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
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
            if (life < 5)
            {
                life++;
                playerHUD.GetComponent<PlayerHUD>().UpdateHUDLives(1);
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bomb")
        {
            morreuuu();
            Destroy(other.gameObject);
        }

        if (other.tag == "spawner")
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
        Destroy(transform.parent.gameObject);
        StopCoroutine("morreu");
    }

}
