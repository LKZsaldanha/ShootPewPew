﻿using System.Collections;
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
                //quadrante1[i].GetChild.gameObject.SetActive(true);
                quadrante1[numberSpawn].GetChild(e).gameObject.SetActive(true);
               // print("entrou " + quadrante1[i].gameObject.name);
            }
            print("entrou " + numberSpawn);
            break;
            
        }
        /*for (int i = 0; i <= quadrante1.Count - 1; i++)
        {
            quadrante1[i].GetChild.gameObject.SetActive(true);
            break;
        }*/
        numberSpawn++;

    }

   

    /*public void quadranteSpawn1()
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
    }*/
}
