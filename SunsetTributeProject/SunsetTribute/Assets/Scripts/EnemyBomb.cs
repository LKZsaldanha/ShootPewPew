using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour {


    //public float ray = 1000;

    public bool podeAtirar = false;
    public float tempoTiro = 3.0f;
    public float atirar = 0.0f;

    public float heightShoot = 3.0f;
    public float speedShoot = 0.5f;

    public Transform bombaArco;


    public float contador = 0.0f;
    //public LayerMask mask;

    public Vector3 impacto;
    public Transform impactoMark;
    public float offSet = 0.85f;

    public Transform player;

    public bool controllable = false;

    /*void Update()
    {
        if (controllable)
        {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            //RaycastHit hit;

            if (contador == atirar)
            {
                if (podeAtirar)
                {
                    impacto = player.transform.position;
                    Tiro();
                }
                podeAtirar = false;
            }
            Debug.DrawRay(transform.position, fwd * ray, Color.red);

            if (!podeAtirar)
            {
                contador += Time.deltaTime;
                if (contador >= tempoTiro)
                {
                    podeAtirar = true;
                    contador = 0;
                }
            }
        }
    }*/

    public void Tiro()
    {
        //transform.GetComponent(AnimacaoInimigo).TiroAnim();
        //impacto = player.transform.position;
        
        Transform  bombaArco1 = Instantiate(bombaArco, transform.position, Quaternion.identity);
        Transform impactoMark1;
        impactoMark1 = Instantiate(impactoMark, new Vector3(impacto.x, impacto.y - offSet, impacto.z), Quaternion.Euler(0, 0, 0));

        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().startPos = transform.position;
        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().endPos = new Vector3 (impacto.x, impacto.y - offSet + 0.1f, impacto.z);
        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().trajectoryHeight = heightShoot;
        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().velocidadeTiro = speedShoot;
    }
}
