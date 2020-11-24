using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeDetect : MonoBehaviour
{
    
    public CarAgent agent;  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("tree"))
        {
            agent.hitATree();
        }
    }
    
}
