using UnityEngine;

public class BasicPlayerController : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private Transform cameraTransform;
    
    
    // Private variables
    private CharacterController characterController;
    private InputManager inputManager;
    
    private Vector3 moveDirection;
    private Vector3 velocity;
    
    
    void Awake() {
        characterController = GetComponent<CharacterController>();
        inputManager        = InputManager.Instance;
        moveDirection       = transform.position;
    }

    void Update() {
        // Getting Move Direction
        moveDirection  = transform.right * inputManager.GetMoveInput().x + transform.forward * inputManager.GetMoveInput().y;
        
        // Converting Move Direction Relative to the Main Camera
        moveDirection  = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirection;
        
        //Calculating Velocity
        velocity       = moveDirection * speed;
        
        // Finally applying the Calculated Velocity to the Character 
        characterController.Move(velocity * Time.deltaTime);
    }
}
