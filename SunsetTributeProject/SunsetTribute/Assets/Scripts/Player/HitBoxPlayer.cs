using System.Collections;
using UnityEngine;

public class HitBoxPlayer : MonoBehaviour {

    private PlayerSound playerSound;

    [SerializeField] private Collider dashCollider;
    [SerializeField] private Collider normalCollider;

    //valor do cx dois
    [SerializeField] private int valorDinheiro;
    public PlayerHUD playerHUD;

    [SerializeField] private GameObject objAnimado, gameSystem,cam;

    private bool isInvencivel;


    private IEnumerator invencibleCouroutine;


   

    //public SkinnedMeshRenderer[] originalColor;


    //angulo para onde está a mira
    public int life;

    private void Awake()    {

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
        invencibleCouroutine = invencivel(3.0f);
        StartCoroutine(invencibleCouroutine);

        gameSystem.GetComponent<GameSystem>().nPlayerVivos.Add(gameObject.transform.parent.gameObject);
        

        if (gameObject.name == "InteractionColliderYAGO")
        {
            GameObject pHUD = GameObject.Find("HUD_Player_1");
            playerHUD = pHUD.GetComponent<PlayerHUD>();
            playerHUD.SwitchPlayerHUDState(PlayerHUDState.playing);
        }
        else if (gameObject.name == "InteractionColliderJOHN")
        {
            GameObject pHUD2 = GameObject.Find("HUD_Player_2");
            playerHUD = pHUD2.GetComponent<PlayerHUD>();
            playerHUD.SwitchPlayerHUDState(PlayerHUDState.playing);
        }


        life = 5;
        playerSound = GetComponent<PlayerSound>();
    }

    private void Update()
    {
        if (GetComponentInParent<CharacterMovement>().dashLock)
        {
            float duration = GetComponentInParent<CharacterMovement>().dashDuration;
            invencibleCouroutine = invencivel(duration);
            StartCoroutine(invencibleCouroutine);
            dashCollider.enabled = true;
            normalCollider.enabled = false;
        }
        else
        {
            dashCollider.enabled = false;
            normalCollider.enabled = true;
        }
    }

    private void morreuuu()
    {
        life--;
        transform.parent.GetComponent<CharacterMovement>().isAlive = false;
        playerHUD.UpdateHUDLives(-1);

        transform.parent.GetComponent<CharacterMovement>().enabled = false;

        objAnimado.GetComponent<Animator>().SetTrigger("isDied");

        playerSound.DeadSound();

        if (playerHUD.playerLives <= 0)
        {
            if (gameObject.name == "NewPlayerPrefab")
                gameSystem.GetComponent<GameSystem>().gameOver1 = true;
            else
                gameSystem.GetComponent<GameSystem>().gameOver2 = true;

            StartCoroutine("morreu");

            playerHUD.SwitchPlayerHUDState(PlayerHUDState.gameOver);

            GetComponentInParent<Rigidbody>().isKinematic = true;
            GetComponentInParent<Rigidbody>().useGravity = false;
            GetComponentInParent<BoxCollider>().enabled = false;
            GetComponentInParent<HitBoxPlayer>().enabled = false;
            gameSystem.GetComponent<GameSystem>().lifePlayers(1, gameObject.name);
            gameSystem.GetComponent<GameSystem>().nPlayerAtivos(gameObject.name);
        }
        else
        {
            GetComponentInParent<Rigidbody>().isKinematic = true;
            GetComponentInParent<Rigidbody>().useGravity = false;
            GetComponentInParent<BoxCollider>().enabled = false;
            GetComponentInParent<HitBoxPlayer>().enabled = false;
            StartCoroutine("morreu");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "gold")
        {
            playerHUD.UpdateHUDScore(valorDinheiro);
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
                playerHUD.UpdateHUDLives(1);
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
   

    IEnumerator invencivel(float time)
    {
        yield return new WaitForSeconds(time);
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
