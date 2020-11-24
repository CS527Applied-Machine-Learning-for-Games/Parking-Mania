using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class ParkingArea : MonoBehaviour
{
    //get a list of all the parked cars
    public List<GameObject> Carsparked;
    //get parking spot gameObject
    public GameObject parking_Spot;

    //Spawn the cars and parking spots
    public List<GameObject> parkedCarSpawnAreas;
  
    public void randomSpawnCarsAndParking()
    {
        foreach(GameObject g in parkedCarSpawnAreas)
        {
            float i = Random.Range(0f, 1f);
            if (i <= 0.85f)
            {
                placeObject(Carsparked[Mathf.FloorToInt(Random.Range(0,Carsparked.Count))],g);
            }
            else
            {
                placeObject(parking_Spot, g);
            }
        }
    }

    public void placeObject(GameObject parkedCarObject, GameObject gameObject)
    {
        Transform spawnLocation = gameObject.transform;
        Instantiate(parkedCarObject, spawnLocation);
    }

    public void clearParking()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag("obstacle") || child.CompareTag("parking")) 
            {
                Destroy(child.gameObject);
            }
        }
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            resetArea();
        }
    }
    
    public void resetArea()
    {
        clearParking();
        randomSpawnCarsAndParking();
    }
}
