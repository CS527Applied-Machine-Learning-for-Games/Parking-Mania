using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is associated with Controlling the car movement and its specifications regarding how we are going to manipulate the movement in the game for parking.
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{

    public float throttle;
    public float steer;
    public float strengthCoefficient = 20000f;
    public float brakingTorque = 10f;

    public List<WheelCollider> throttlingWheels;
    public List<GameObject> steeringWheels;
    public List<WheelCollider> brakingWheels;
    public List<GameObject> wheelMeshes;

    public float maxTurnDegree = 20f;
    public float handBrake;
    public Rigidbody car_rigidBody;

    public Transform cm;


    //public LightingManager lm;

    private void Start()
    {
        car_rigidBody = GetComponent<Rigidbody>();
        car_rigidBody.centerOfMass = cm.position;
    }

    void Update()
    {
        throttle = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handBrake = 1;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            handBrake = 0;
        }
    }

    
    void FixedUpdate()
    {
        foreach (WheelCollider wheel in throttlingWheels)
        {
            wheel.motorTorque = strengthCoefficient * Time.fixedDeltaTime * throttle;
        }
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = maxTurnDegree * steer;
            wheel.transform.localEulerAngles = new Vector3(0f, steer*maxTurnDegree, 0f);
        }
        foreach (WheelCollider wheel in brakingWheels)
        {
            wheel.brakeTorque = brakingTorque * handBrake;
        }

        foreach(GameObject mesh in wheelMeshes)
        {
            mesh.transform.Rotate(car_rigidBody.velocity.magnitude * (transform.InverseTransformDirection(car_rigidBody.velocity).z >=0 ? 1 : -1)/ (2 * Mathf.PI * 0.17f), 0f,0f);
        }
    }
}
