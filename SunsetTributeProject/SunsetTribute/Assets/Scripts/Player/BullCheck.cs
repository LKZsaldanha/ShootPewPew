using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullCheck : MonoBehaviour {
    private Animator myAnimator;
    
    
    private void Start () {
         myAnimator = transform.parent.gameObject.transform.GetComponentInChildren<Animator>();
	}
	private void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Bulls"){
            myAnimator.GetComponent<Animator>().SetLayerWeight (myAnimator.GetComponent<Animator>().GetLayerIndex ("Aim"), 0);
            myAnimator.SetBool("Bulls", true);
            transform.parent.gameObject.transform.SetParent(other.transform);
        }
    }
    private void OnTriggerExit(Collider other){
        
        if(other.gameObject.tag == "Bulls"){
            myAnimator.SetBool("Bulls", false);
            myAnimator.GetComponent<Animator>().SetLayerWeight (myAnimator.GetComponent<Animator>().GetLayerIndex ("Aim"), 1);
            transform.parent.gameObject.transform.SetParent(null);
        }
    }
}
