using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBull : MonoBehaviour
{
    

    public GameObject[] waypoints;
    private int current = 0;
    public float speedWayPoint = 1.5f;
    public float WPradius = 0.1f;

    [SerializeField] private GameObject cam;

	private bool moving = true;
    
    private void Awake()
    {
        cam = GameObject.Find("Main Camera");
    }

    void FixedUpdate(){
        if(moving){
            if(Vector3.Distance(waypoints[current].transform.position, transform.position) < WPradius)
            {
                current ++;
                if (current >= waypoints.Length)
                {

                    moving = false;
                    cam.GetComponent<CameraFollow>().Perc();
                }
            }
            if(moving){
                transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speedWayPoint);
            }  
        }
    }
}
