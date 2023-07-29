using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager> {
    
    #region EVENTS
    public delegate void ResetPositionEvent(bool isPressed);
    public event ResetPositionEvent OnResetPosition;
    #endregion
    
    
    [Header("Input Actions")]
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction breakAction;
    private InputAction resetPositionAction;


    [Header("Inputs")] 
    private Vector2 moveInput;
    private bool breakInput;
    private bool resetPositionInput;
    
    
    void Awake() {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        breakAction = playerInput.actions["Break"];
        resetPositionAction = playerInput.actions["ResetPosition"];
    }

     
    private void OnEnable() {
        moveAction.performed += Move;
        moveAction.canceled += Move;
        
        breakAction.performed += Break;
        breakAction.canceled += Break;
        
        resetPositionAction.started += ResetPosition;
    }

    private void OnDisable() {
        moveAction.performed -= Move;
        moveAction.canceled -= Move;
        
        breakAction.performed -= Break;
        breakAction.canceled -= Break;
        
        resetPositionAction.started -= ResetPosition;
    }
    
    
    // Input Action event Handler
    void Move(InputAction.CallbackContext _ctx) {
        moveInput = _ctx.ReadValue<Vector2>();
    }
    
    void Break(InputAction.CallbackContext _ctx) {
        breakInput = _ctx.ReadValueAsButton();
    }
    
    void ResetPosition(InputAction.CallbackContext _ctx) {
        if (OnResetPosition != null) OnResetPosition(_ctx.ReadValueAsButton());
    }
    
    
    // Getter Functions
    public Vector2 GetMoveInput() {
        return moveInput;
    }

    public bool GetBreakInput() {
        return breakInput;
    }

}
