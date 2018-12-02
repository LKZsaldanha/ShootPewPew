using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class PlayerSound : MonoBehaviour {


    public AudioClip dead, pickItem, dash;
    private AudioSource audioSource;
    public AudioMixer mixer;
    // Use this for initialization
    void Start () {
        //audioOutput = GetComponent<AudioSource>().outputAudioMixerGroup;
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
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("playerDead")[0];
        audioSource.PlayOneShot(dead, 1F);
    }
    public void GoldSound()
    {   
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("playerPick")[0];
        audioSource.PlayOneShot(pickItem, 1F);   
    }
    public void DashSound()
    {   
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("playerDash")[0];
        audioSource.PlayOneShot(dash, 1F);   
    }

}
