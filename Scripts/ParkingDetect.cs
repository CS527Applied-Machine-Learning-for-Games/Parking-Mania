using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingDetect : MonoBehaviour
{
    
    public CarAgent agent;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("parking"))
        {
            agent.parked();
        }
    }
    
}
