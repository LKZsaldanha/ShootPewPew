using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBackground : MonoBehaviour {


	public float timeToReplay = 1.0f;
	public bool isLoop = false;
	private float timer = 0.0f;
	private AudioSource audioSource;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		//StartCoroutine("CallSound");
	}
	
	 private void OnTriggerEnter(Collider other)
    {
		if(other.tag == "Player"){
			if(isLoop){
				audioSource.Play();
			}else{
				StartCoroutine("PlaySound");
			}
		}
	}
	 private void OnTriggerExit(Collider other)
    {
		if(other.tag == "Player"){
			if(isLoop){
				audioSource.Stop();
			}else{
				StopCoroutine("CallSound");
				StopCoroutine("PlaySound");
			}
		}
	}

	 IEnumerator PlaySound()
    {
		audioSource.Play();
		yield return new WaitForSeconds(timeToReplay);
		StartCoroutine("CallSound");
		StopCoroutine("PlaySound");
    }

	IEnumerator CallSound(){
		yield return new WaitForSeconds(timeToReplay);
		audioSource.Play();
		StartCoroutine("PlaySound");
		StopCoroutine("CallSound");
	}
}
