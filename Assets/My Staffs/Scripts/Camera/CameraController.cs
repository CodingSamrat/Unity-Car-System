using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("Follow Target")] 
    [SerializeField] private Transform playerCar;
    [SerializeField] private Transform lookAtTarget;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 offset;


    /// <summary>
    /// Private Variables.
    /// </summary>
    private Rigidbody playerRb;
    
    void Start() {
        playerRb = playerCar.GetComponent<Rigidbody>();
    }

    
    void FixedUpdate() {
        Vector3 _playerForward = (playerCar.transform.forward).normalized;

        transform.position = Vector3.Lerp(
            transform.position,
            playerCar.position + playerCar.transform.TransformVector(offset) + _playerForward * (-5.5f),
            // playerCar.position + offset + _playerForward * (-5.5f),
            cameraSpeed * Time.deltaTime
        );
        
        transform.LookAt(lookAtTarget);

    }
}
