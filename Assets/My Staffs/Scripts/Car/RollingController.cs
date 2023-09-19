using UnityEngine;

public class RollingController : MonoBehaviour {
    private InputManager inputManager;
    
    void Awake() {
        inputManager = InputManager.Instance;
    }

    private void OnEnable() {
        inputManager.OnResetPosition += CheckResetPosition;
    }
    
    private void OnDisable() {
        inputManager.OnResetPosition -= CheckResetPosition;
    }
    

    void CheckResetPosition(bool isPressed) {
        transform.position = new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        Debug.Log("Reset Position");
    }
}
