using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager> {
    
    [Header("Input Actions")]
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction breakAction;


    [Header("Inputs")] 
    private Vector2 moveInput;
    private bool breakInput;
    
    
    void Awake() {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        breakAction = playerInput.actions["Break"];
    }

     
    private void OnEnable() {
        moveAction.performed += Move;
        moveAction.canceled += Move;
        breakAction.performed += Break;
        breakAction.canceled += Break;
    }

    private void OnDisable() {
        moveAction.performed -= Move;
        moveAction.canceled -= Move;
        breakAction.performed -= Break;
        breakAction.canceled -= Break;
    }
    
    
    // Input Action event Handler
    void Move(InputAction.CallbackContext _ctx) {
        moveInput = _ctx.ReadValue<Vector2>();
    }
    
    void Break(InputAction.CallbackContext _ctx) {
        breakInput = _ctx.ReadValueAsButton();
    }
    
    
    // Getter Functions
    public Vector2 GetMoveInput() {
        return moveInput;
    }

    public bool GetBreakInput() {
        return breakInput;
    }
}
