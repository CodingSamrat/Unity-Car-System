using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
    [Header("Wheels")]
    [SerializeField] private WheelColliders wheelColliders;
    [SerializeField] private WheelMashes    wheelMashes;

    
    [Header("Car Specs")] 
    [SerializeField] private float motorPower;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float steeringRate;
    
    
    /// <summary>
    /// Components 
    /// </summary>
    private Rigidbody    rb;
    private InputManager inputManager;
    
    
    /// <summary>
    /// Inputs
    /// </summary>
    private float forwardInput;
    private float steeringInput;
    private bool  breakInput;

 
    /// <summary>
    /// Car Specs
    /// </summary>
    private float speed;
    public float currentBreakForce;
    public float currentSteerAngle;
    
    void Start() {
        rb           = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;

    }


    void Update() {
        // Get Speed from Car Rigidbody Component.
        speed = rb.velocity.magnitude;
      
        // Update all wheels Position & Rotation according to Wheel Colliders.
        UpdateWheels();
        
        // Getting player input.
        CheckInput();
        
        // Apply Movement - Forward, Backward, Left, Right, & Break.
        ApplyMotorPower();
        HandleSteering();
        HandleBreaking();
    }

    
    void CheckInput() {
        forwardInput  = inputManager.GetMoveInput().y;  // z -> y (vector3.forward)
        steeringInput = inputManager.GetMoveInput().x;  // x -> x (vector3.right)
        breakInput    = inputManager.GetBreakInput();   // Breaking (bool)
    }

    
    void UpdateWheels() {
        UpdateSingleWheel(wheelColliders.FRWheel, wheelMashes.FRWheel);
        UpdateSingleWheel(wheelColliders.FLWheel, wheelMashes.FLWheel);
        UpdateSingleWheel(wheelColliders.RRWheel, wheelMashes.RRWheel);
        UpdateSingleWheel(wheelColliders.RLWheel, wheelMashes.RLWheel);
    }
    
    
    void UpdateSingleWheel(WheelCollider _coll, MeshRenderer _wheelMesh) {
        Quaternion _quaternion;
        Vector3    _position;

        // Getting world Position & Rotation of Wheel Collider
        _coll.GetWorldPose(out _position, out _quaternion);

        _wheelMesh.transform.position = _position;
        _wheelMesh.transform.rotation = _quaternion;
    }

    
    void ApplyMotorPower() {
        wheelColliders.RLWheel.motorTorque = motorPower * forwardInput;
        wheelColliders.RRWheel.motorTorque = motorPower * forwardInput;
    }

    void HandleSteering() {
        // Getting Steering Angle
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, maxSteerAngle * steeringInput, steeringRate);

        // Apply Steering Angle to the Front Wheels.
        wheelColliders.FRWheel.steerAngle = currentSteerAngle;
        wheelColliders.FLWheel.steerAngle = currentSteerAngle;
    }

    void HandleBreaking() {
        // Check if pressing Break or not.
        currentBreakForce = breakInput ? breakForce : 0f;
        
        //Apply Break
        wheelColliders.FRWheel.brakeTorque = currentBreakForce;
        wheelColliders.FLWheel.brakeTorque = currentBreakForce;
        wheelColliders.RRWheel.brakeTorque = currentBreakForce;
        wheelColliders.RLWheel.brakeTorque = currentBreakForce;
    }
}



// Custom Datatype 
[System.Serializable]
public class WheelColliders {
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RRWheel;
    public WheelCollider RLWheel;
}

[System.Serializable]
public class WheelMashes {
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer RRWheel;
    public MeshRenderer RLWheel;
}