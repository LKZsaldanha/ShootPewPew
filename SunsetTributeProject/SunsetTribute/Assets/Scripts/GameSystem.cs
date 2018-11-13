using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameSystem : MonoBehaviour {

    [SerializeField] private GameObject[] enemys;
    [SerializeField] private GameObject[] portais,gameSystem;
    [SerializeField] private float cowdownEnemy;
    [SerializeField] private List<GameObject> objetosDestrutivos, principalEnemy, scoresMenu;
    [SerializeField] private List<int> scorePhases;
    [SerializeField] private GameObject principal, player;

    private GameObject uiScore,uiCoin;
    private bool isLook,isLookScore;
    public int score, coin, idSkin;


    private int maiorScore;

    private void Awake()
    {
        /*
         Adiciona o gameSystem numa lista e faz com que ele seja o unico em qualquer scena
         */
        gameSystem = GameObject.FindGameObjectsWithTag("GameSystem");
        if (gameSystem.Length >= 2)
        {
            Destroy(gameSystem[1]);
        }
        DontDestroyOnLoad(gameObject);
    }

    /*  private void Start()
     {
         //serve para salvar informações do game
         if(PlayerPrefs.HasKey("skinPlayer"))
         {
             PlayerPrefs.SetInt("skinPlayer", idSkin);
         }
     }*/

    // Use this for initialization
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "SelectionMap")
        {
            if(!isLookScore)
            {
                scoresMenu[0] = GameObject.Find("Score");
                scoresMenu[1] = GameObject.Find("Score2");
                /*
                scoresMenu[2] = GameObject.Find("Score3");
                scoresMenu[3] = GameObject.Find("Score4");
                scoresMenu[4] = GameObject.Find("Score5");
                scoresMenu[5] = GameObject.Find("Score6");
                scoresMenu[6] = GameObject.Find("Score7");
                scoresMenu[7] = GameObject.Find("Score8");
                 */
                isLookScore = true;
            }
            scoresMenu[0].GetComponent<Text>().text = "Score: " + scorePhases[0].ToString();
            scoresMenu[1].GetComponent<Text>().text = "Score: " + scorePhases[1].ToString();
            /*
            scoresMenu[2].GetComponent<Text>().text = "Score: " + scorePhases[2].ToString();
            scoresMenu[3].GetComponent<Text>().text = "Score: " + scorePhases[3].ToString();
            scoresMenu[4].GetComponent<Text>().text = "Score: " + scorePhases[4].ToString();
            scoresMenu[5].GetComponent<Text>().text = "Score: " + scorePhases[5].ToString();
            scoresMenu[6].GetComponent<Text>().text = "Score: " + scorePhases[6].ToString();
            scoresMenu[7].GetComponent<Text>().text = "Score: " + scorePhases[7].ToString();
            */

        }

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            if (!isLook)
            {
                maiorScore = scorePhases[0];
                score = 0;
                uiScore = GameObject.Find("Score");
                uiCoin = GameObject.Find("CoinUI");
                principal = GameObject.Find("Principal");
                principalEnemy.Add(GameObject.Find("PrincipalEnemy"));
                isLook = true;
            }
            uiScore.GetComponent<Text>().text = "Score: " + score;
            uiCoin.GetComponent<Text>().text = "Coin: " + coin;
            //objetosDestrutivos.RemoveAll(c => c == null);
            principalEnemy.RemoveAll(c => c == null);
            StartCoroutine("cowdown");
            loseGame();
            winGame();
        }

        if (SceneManager.GetActiveScene().name == "phase2")
        {
            if (!isLook)
            {
                maiorScore = scorePhases[1];
                score = 0;
                uiScore = GameObject.Find("Score");
                uiCoin = GameObject.Find("CoinUI");
                principal = GameObject.Find("Principal");
                principalEnemy.Add(GameObject.Find("PrincipalEnemy"));
                isLook = true;
            }
            uiScore.GetComponent<Text>().text = "Score: " + score;
            uiCoin.GetComponent<Text>().text = "Coin: " + coin;
            //objetosDestrutivos.RemoveAll(c => c == null);
            principalEnemy.RemoveAll(c => c == null);
            StartCoroutine("cowdown");
            loseGame();
            winGame();
        }
    }

    void loseGame()
    {
        //Game Over
        if(principal == null || player == null)
        {
            isLook = false;
            StopAllCoroutines();
            SceneManager.LoadScene("SelectionMap");
        }
    }

    void winGame()
    {
        //Win Game
        if(principalEnemy.Count == 0)
        {
            if (SceneManager.GetActiveScene().name == "SampleScene")
            {
                isLook = false;
                if (maiorScore < score)
                    scorePhases[0] = score;
                else
                    scorePhases[0] = maiorScore;
            }

            if (SceneManager.GetActiveScene().name == "phase2")
            {
                isLook = false;
                if (maiorScore < score)
                    scorePhases[1] = score;
                else
                    scorePhases[1] = maiorScore;
            }

            isLookScore = false;
            StopAllCoroutines();
            SceneManager.LoadScene("SelectionMap");
        }
    }

    IEnumerator cowdown()
    {
        yield return new WaitForSeconds(cowdownEnemy);
        Instantiate(enemys[0], portais[0].transform.position, portais[0].transform.rotation);
        StopCoroutine("cowdown");
    }
}
