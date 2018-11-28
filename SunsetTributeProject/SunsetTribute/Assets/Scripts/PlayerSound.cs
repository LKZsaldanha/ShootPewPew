using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {


    public AudioClip dead;
    AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    //chamado por "Actor" em AtirouAnim();
    /*public void ShootSound()
    {
        //////// O Proprio tiro esta fazendo som quando lançado//////////
        audioSource.PlayOneShot(shoot, 1F);
    }*/

        /// <summary>
        /// Chamado pelo Actor quando life == 0
        /// </summary>
    public void DeadSound()
    {
        audioSource.PlayOneShot(dead, 1F);
    }
}
