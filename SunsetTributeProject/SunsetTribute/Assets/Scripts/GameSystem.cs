using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameSystem : MonoBehaviour {

    public int money;
    public List<GameObject> nPlayerVivos;
    //variavel que indicara qual player morreu para o inimigo
    public string nameplayer;

    [SerializeField] private List<GameObject> enemys;
    [SerializeField] private List<Transform> quadrante1, quadrante2, quadrante3;


    /// <summary>
    /// Ao chamar o metodo deve colocar o nome do player para
    /// a função remover o player morto correto da lista
    /// </summary>
    /// <param name="namePlayer"></param>
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

    public void quadranteSpawn1()
    {
        for(int i=0; i<=quadrante1.Count-1;i++)
        {
            Instantiate(enemys[Random.Range(0,enemys.Count)], quadrante1[i].position,quadrante1[i].rotation);
        }        
    }

    public void quadranteSpawn2()
    {
        for (int i = 0; i <= quadrante1.Count - 1; i++)
        {
            Instantiate(enemys[Random.Range(0, enemys.Count)], quadrante2[i].position, quadrante2[i].rotation);
        }
    }

    public void quadranteSpawn3()
    {
        for (int i = 0; i <= quadrante1.Count - 1; i++)
        {
            Instantiate(enemys[Random.Range(0, enemys.Count)], quadrante3[i].position, quadrante3[i].rotation);
        }
    }
}
