using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBombFlyInArc : MonoBehaviour {


    //Todas cordenadas é o "EnemyBomb" Script que passa
    public Vector3 startPos; // onde começa
    public Vector3 endPos; //onde cai
    public float trajectoryHeight = 5.0f; //o quanto o tiro sobe
    public float velocidadeTiro   = 0.5f;


    private float  cTime; //tempo somado para ter trajetoria

    public Transform explosaoFogo;
    public Transform impactoMark;


    public Vector3 impactoPos;
    public float offSet = 0.85f;
    private bool endPosTaked = false;
//var dano : Transform;


    void Start()
    {
        cTime = 0f; //começa sempre como zero
    }

    void Update()
    {

        if(!endPosTaked){
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(endPos, transform.TransformDirection(Vector3.down), out hit, 50f))
            {
                if(hit.transform.tag == "chao"){
                    endPos.y = hit.point.y;
                    Transform impactoMark1;
                    impactoMark1 = Instantiate(impactoMark, new Vector3(impactoPos.x, endPos.y - offSet, impactoPos.z), Quaternion.Euler(0, 0, 0));
                    endPosTaked = true;
                }
                Debug.Log("Did Hit");
            }
        }

        // calculate current time within our lerping time range
        cTime += Time.deltaTime * velocidadeTiro;

        // calculate straight-line lerp position:
        Vector3 currentPos = Vector3.Lerp(startPos, endPos, cTime);

        // add a value to Y, using Sine to give a curved trajectory in the Y direction
        currentPos.y += trajectoryHeight * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);

        // finally assign the computed position to our gameObject:
        transform.position = currentPos;
        if (currentPos == endPos)
        {
            explode();
        }

    }

    void explode()
    {
        var explosao1 = Instantiate(explosaoFogo, transform.position, transform.rotation);
        //var dano1 = Instantiate(dano, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    /*function OnTriggerEnter(other : Collider)
    {
        if (other.transform.tag == "caixa")
        {
            Destroy(gameObject);
        }
    }*/
}
