using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {


    public AudioClip shoot;
    AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
    //chamado por "Actor" em animShoot();
	public void ShootSound()
    {
        audioSource.PlayOneShot(shoot, 1F);
    }
}
