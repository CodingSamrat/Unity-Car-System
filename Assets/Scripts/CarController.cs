using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
    [Header("Wheels")]
    [SerializeField] private WheelColliders wheelColliders;
    [SerializeField] private WheelMashes    wheelMashes;
    [SerializeField] private WheelParticles wheelParticles;
    [SerializeField] private float slipAllowance;

    
    [Header("Car Specs")] 
    [SerializeField] private float motorPower;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float steeringRate;
    
    [SerializeField] private Transform centerOfMass;
    
    
    [Header("Particle System")] 
    [SerializeField] private GameObject pfWheelSmoke;
    
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
    public float currentBreakForce;
    public float currentSteerAngle;
    
    [Header("Test Stats")] 
    public bool play;
    public float speed;
    
    void Start() {
        rb           = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
        
        // Modifying Center of Mass.
        rb.centerOfMass = centerOfMass.localPosition;
        
        // Set Wheel Particles
        InstantiateWheelSmoke();

    }
    
    void Update() {
        // Get Speed from Car Rigidbody Component.
        speed = rb.velocity.z;
      
        // Update all wheels Position & Rotation according to Wheel Colliders.
        UpdateWheels();
        
        // Getting player input.
        CheckInput();
        
        // Apply Movement - Forward, Backward, Left, Right, & Break.
        ApplyMotorPower();
        HandleSteering();
        HandleBreaking();
        
        // Particle System
        CheckSlip();
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

    void InstantiateWheelSmoke() {
        Transform _trFRWheelCollider = wheelColliders.FRWheel.transform;
        Transform _trFLWheelCollider = wheelColliders.FLWheel.transform;
        Transform _trRRWheelCollider = wheelColliders.RRWheel.transform;
        Transform _trRLWheelCollider = wheelColliders.RLWheel.transform;
        
        wheelParticles.FRWheelSmoke = Instantiate(pfWheelSmoke, _trFRWheelCollider.position - (Vector3.up * wheelColliders.FRWheel.radius), Quaternion.identity, _trFRWheelCollider).GetComponent<ParticleSystem>();
        wheelParticles.FLWheelSmoke = Instantiate(pfWheelSmoke, _trFLWheelCollider.position - (Vector3.up * wheelColliders.FLWheel.radius), Quaternion.identity, _trFLWheelCollider).GetComponent<ParticleSystem>();
        wheelParticles.RRWheelSmoke = Instantiate(pfWheelSmoke, _trRRWheelCollider.position - (Vector3.up * wheelColliders.RRWheel.radius), Quaternion.identity, _trRRWheelCollider).GetComponent<ParticleSystem>();
        wheelParticles.RLWheelSmoke = Instantiate(pfWheelSmoke, _trRLWheelCollider.position - (Vector3.up * wheelColliders.RLWheel.radius), Quaternion.identity, _trRLWheelCollider).GetComponent<ParticleSystem>();

        

    }

    void CheckSlip() {
        WheelHit _wheelHitFR;
        WheelHit _wheelHitFL;
        WheelHit _wheelHitRR;
        WheelHit _wheelHitRL;

        wheelColliders.FRWheel.GetGroundHit(out _wheelHitFR);
        wheelColliders.FLWheel.GetGroundHit(out _wheelHitFL);
        wheelColliders.RRWheel.GetGroundHit(out _wheelHitRR);
        wheelColliders.RLWheel.GetGroundHit(out _wheelHitRL);

        var main = wheelParticles.FRWheelSmoke.main;
        
        // Front Right
        if ((Mathf.Abs(_wheelHitFR.forwardSlip) + Mathf.Abs(_wheelHitFR.sidewaysSlip)) > slipAllowance) {
             wheelParticles.FRWheelSmoke.Play();
             main.startColor = new Color(1, 2, 3, 1);
        }
        else {
            wheelParticles.FRWheelSmoke.Stop();
        }
        
        // Front Left
        if ((Mathf.Abs(_wheelHitFL.forwardSlip) + Mathf.Abs(_wheelHitFL.sidewaysSlip)) > slipAllowance) {
            wheelParticles.FLWheelSmoke.Play();
        }

        else {
            wheelParticles.FLWheelSmoke.Stop();
        }
            
        // Rare Right
        if (Mathf.Abs(_wheelHitRR.forwardSlip) + Mathf.Abs(_wheelHitRR.sidewaysSlip) > slipAllowance) {
            wheelParticles.RRWheelSmoke.Play();
            
        }

        else {
            wheelParticles.RRWheelSmoke.Stop();
        }
        
        // Rare Left
        if (Mathf.Abs(_wheelHitRL.forwardSlip) + Mathf.Abs(_wheelHitRL.sidewaysSlip) > slipAllowance) {
            wheelParticles.RLWheelSmoke.Play();
        }

        else {
            wheelParticles.RLWheelSmoke.Stop();
        }
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

[System.Serializable]
public class WheelParticles {
    public ParticleSystem FRWheelSmoke;
    public ParticleSystem FLWheelSmoke;
    public ParticleSystem RRWheelSmoke;
    public ParticleSystem RLWheelSmoke;
}