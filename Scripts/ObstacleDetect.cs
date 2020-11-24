using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetect : MonoBehaviour
{
    
    public CarAgent agent;  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            agent.hitACar();
        }
    }
   
}
