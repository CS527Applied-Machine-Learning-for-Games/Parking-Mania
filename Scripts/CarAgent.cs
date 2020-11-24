using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

//This is a script where in how the agent will be trained on and what are failure and success specific rewards.
// moreover,all the neccessary methods for implementation are included in this script.
public class CarAgent : Agent
{
   
    public List<GameObject> agentSpawnPositions;

    [HideInInspector]
    public Bounds areaBounds;


   
    public GameObject parking;
   
    public GameObject area;
    public float maxStep = 50000;

    [HideInInspector]
    public ParkingArea areaSettings;

    
    public bool useVectorObs;
    
    
    CarSetting carSetting;

    Rigidbody m_CarRb;  
    public List<WheelCollider> throttleWheels;
    public List<GameObject> steeringWheels;
    public List<GameObject> wheelMeshes;

    public float maxTurnDegree = 20f;
    

    //force applied to the driving wheels
    public float strengthCoefficient = 20000f;

    private GameObject ground;
   
    void Awake()
    {
        carSetting = FindObjectOfType<CarSetting>();
        areaSettings = area.GetComponent<ParkingArea>();
    }

   
    public override void Initialize()
    {
        m_CarRb = GetComponent<Rigidbody>();

        GetRandomSpawnPosition();
        //SetResetParameters();
    }
    
    public Vector3 GetRandomSpawnPosition()
    {
        ground = agentSpawnPositions[Mathf.FloorToInt(Random.Range(0, agentSpawnPositions.Count))];

        areaBounds = ground.GetComponent<Collider>().bounds;
        var foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            var randomPosX = Random.Range(-areaBounds.extents.x * carSetting.spawnAreaMarginMultiplier,
                areaBounds.extents.x * carSetting.spawnAreaMarginMultiplier);

            var randomPosZ = Random.Range(-areaBounds.extents.z * carSetting.spawnAreaMarginMultiplier,
                areaBounds.extents.z * carSetting.spawnAreaMarginMultiplier);
            randomSpawnPos = ground.transform.position + new Vector3(randomPosX, 1f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
    }

    
    public void hitACar()
    {
        Debug.Log("Hit Car");
        // punish the agent
        AddReward(-0.1f);
        //EndEpisode();
    }
   
    public void hitATree()
    {
        // punish the agent
        Debug.Log("Hit tree");
        AddReward(-0.1f);
    }

    public void hitAWall()
    {
        // punish the agent
        Debug.Log("Hit Wall");
        AddReward(-0.1f);
    }
    
    public void parked()
    {
        // reward the agent
        Debug.Log("Parked");
        AddReward(5f);
        EndEpisode();
    }

    
    public void MoveAgent(float[] act)
    {
        var steering = act[0];
        var throttle = act[1];

        foreach (WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = strengthCoefficient * Time.fixedDeltaTime * throttle;
        }
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = maxTurnDegree * steering;
            wheel.transform.localEulerAngles = new Vector3(0f, steering * maxTurnDegree, 0f);
        }
    }

    
    private void FixedUpdate()
    {
        //visual turning of the wheels
        foreach (GameObject mesh in wheelMeshes)
        {
            mesh.transform.Rotate(m_CarRb.velocity.magnitude * (transform.InverseTransformDirection(m_CarRb.velocity).z >= 0 ? 1 : -1) / (2 * Mathf.PI * 0.17f), 0f, 0f);
        }
        
    }

  
    public override void OnActionReceived(float[] vectorAction)
    {
        // Move the agent using the action.
        MoveAgent(vectorAction);
        // Penalty given each step to encourage agent to finish task quickly.
        AddReward(-1f / maxStep);
    }

    public float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
    
    
    public override void OnEpisodeBegin()
    {
        var rotation = Random.Range(0, 4);
        var rotationAngle = rotation * 90f;
        area.transform.Rotate(new Vector3(0f, rotationAngle, 0f));

        areaSettings.resetArea();


        transform.position = GetRandomSpawnPosition();
        transform.Rotate(new Vector3(0f, 0f, 0f));

        m_CarRb.velocity = Vector3.zero;
        
        m_CarRb.angularVelocity = Vector3.zero;

        //removing torque from the wheels
        foreach (WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = 0f;
        }
        //removing steer angle from the wheels
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle  = 0f;
            wheel.transform.localEulerAngles = new Vector3(0f, 0f * maxTurnDegree, 0f);
        }
       
    }

}
