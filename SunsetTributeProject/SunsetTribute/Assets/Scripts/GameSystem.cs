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

    public int numberSpawn;//forçar spawn a ser mostrado
    /// <summary>
    /// Ao chamar o metodo deve colocar o nome do player para
    /// a função remover o player morto correto da lista
    /// </summary>
    /// <param name="namePlayer"></param>
    void Start()
    {
        //numberSpawn = 0;
    }
    private void Update()
    {
        //Para reinicar o jogo na amostra
        if(Input.GetButtonDown("Reiniciar"))
        {
            SceneManager.LoadScene("Demo_Level_2");
        }

        spawnPlayer();
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

        nPlayerVivos.RemoveAll(c => c == null);
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

    void spawnPlayer()
    {
        if(Input.GetButtonDown("StartP1"))
        {
            if (GameObject.Find("Cube_Player") == null)
            {
                GameObject aux;
                aux  =Instantiate(opcoesPlayer[0], new Vector3(GameObject.Find("Main Camera").transform.position.x, GameObject.Find("Main Camera").transform.position.y, 0f), opcoesPlayer[0].transform.rotation);
                aux.name = "Cube_Player";
            }
            
        }

        if (Input.GetButtonDown("StartP2"))
        {
            if (GameObject.Find("Cube_Player2") == null)
            {
                GameObject aux;
                aux = Instantiate(opcoesPlayer[1], new Vector3(GameObject.Find("Main Camera").transform.position.x, GameObject.Find("Main Camera").transform.position.y, 0f), opcoesPlayer[1].transform.rotation);
                aux.name = "Cube_Player2";
            }

            

        }
    }
}
