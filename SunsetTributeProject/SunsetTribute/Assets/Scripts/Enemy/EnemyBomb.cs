using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour {


    public float heightShoot = 3.0f;
    public float speedShoot = 0.5f;

    public Transform bombaArco;

    public Vector3 impacto; //trazido pelo EnemyBomb em throwBombCooldown()
    public Transform impactoMark;
    public float offSet = 0.85f;

    public Transform player;


   

    public void Tiro()
    {
        //transform.GetComponent(AnimacaoInimigo).TiroAnim();
        //impacto = player.transform.position;
        Transform  bombaArco1 = Instantiate(bombaArco, transform.position, Quaternion.identity);

        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().startPos = transform.position;
        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().endPos = new Vector3 (impacto.x, impacto.y - offSet, impacto.z);
        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().trajectoryHeight = heightShoot;
        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().velocidadeTiro = speedShoot;
        bombaArco1.transform.GetComponent<EnemyBombFlyInArc>().impactoPos = impacto; //passado para ter a ultima posição correta da marca onde cai a bomba
        
    }
}
