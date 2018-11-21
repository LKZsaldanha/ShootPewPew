using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour {


    public AudioClip shoot;
    AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
    //chamado por "Enemy" em Attack();
	public void ShootSound()
    {
        audioSource.PlayOneShot(shoot, 1F);
    }
}
