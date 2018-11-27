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
//var dano : Transform;


    void Start()
    {
        cTime = 0f; //começa sempre como zero
    }

    void Update()
    {

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
