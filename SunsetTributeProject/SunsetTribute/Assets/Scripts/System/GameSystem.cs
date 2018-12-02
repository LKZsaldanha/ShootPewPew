using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameSystem : MonoBehaviour {

    public int money;
    public List<GameObject> nPlayerVivos;

    [SerializeField] List<GameObject> opcoesPlayer;
    //variavel que indicara qual player morreu para o inimigo
    public string nameplayer;

    //[SerializeField] private List<GameObject> enemys;
    [SerializeField] private List<Transform> quadrante1;

    public int numberSpawn;

    private int life1, life2;

    private bool noSpawn1, noSpawn2;

    public bool gameOver1, gameOver2;

    [SerializeField] GameObject[] uiPlayers;

    private void Start()
    {
        life1 = life2 = 5;
    }

    private void Update()
    {
        nPlayerVivos.RemoveAll(c => c == null);
        //Para reinicar o jogo na amostra
        if (Input.GetButtonDown("Reiniciar"))
        {
            SceneManager.LoadScene("Demo_Level_2");
        }

        spawnPlayer();


        // GameOver for the players
        if(uiPlayers[0].activeSelf)
        {
            if (uiPlayers[1].activeSelf)
            {
                StartCoroutine("returnMenu");
            }                
            else if(uiPlayers[2].activeSelf)
            {
                StartCoroutine("returnMenu");
            }
        }
    }

    public void nPlayerAtivos(string namePlayer)
    {
        this.nameplayer = namePlayer;
        if(nPlayerVivos[0].name == namePlayer)
        {
            nPlayerVivos.RemoveAt(0);
        }
        else
        {
            nPlayerVivos.RemoveAt(1);
        }
    }

    public void quadranteSpawn()
    {
        


        while (numberSpawn < quadrante1.Count) 
        {
            for (var e = quadrante1[numberSpawn].transform.childCount - 1; e >= 0; e--)
            {
                quadrante1[numberSpawn].GetChild(e).gameObject.SetActive(true);  
            }
            break; 
        }

        numberSpawn++;

    }

    /// <summary>
    /// Metodo onde controlará o GameOver no game
    /// </summary>
    public void lifePlayers(int lifePlayer, string nameplayer)
    {
        if (nameplayer == "NewPlayerPrefab")
        {
            life1 -= lifePlayer;
            if(life1 < 0)
            {
                noSpawn1 = true;
            }
        }
        else
        {
            life2 -= lifePlayer;
            if (life2 < 0)
            {
                noSpawn2 = true;
            }
        }
    }

    IEnumerator returnMenu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("SplashScreen");
        StopCoroutine("returnMenu");
    }

    void spawnPlayer()
    {

            if (GameObject.Find("NewPlayerPrefab") == null)
            {                
                    StartCoroutine("monsterReborn1");             
                
            }
        
            if(Input.GetButtonDown("StartP2"))
            {
                if (GameObject.Find("NewPlayerPrefab2") == null)
                {

                    if (!gameOver2)
                    {
                        GameObject aux;
                        aux = Instantiate(opcoesPlayer[1], new Vector3(GameObject.Find("Main Camera").transform.position.x, GameObject.Find("Main Camera").transform.position.y, 0f), opcoesPlayer[1].transform.rotation);
                        aux.name = "NewPlayerPrefab";
                    }
                }
            }


        
    }


    IEnumerator monsterReborn1()
    {
        yield return new WaitForSeconds(1);
        if (!gameOver1)
        {
            GameObject aux;
            aux = Instantiate(opcoesPlayer[0], new Vector3(GameObject.Find("Main Camera").transform.position.x, GameObject.Find("Main Camera").transform.position.y, 0f), opcoesPlayer[0].transform.rotation);
            aux.name = "NewPlayerPrefab";
        }
        StopCoroutine("monsterReborn1");
    }
}
