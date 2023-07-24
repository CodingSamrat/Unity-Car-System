using System;
using UnityEngine;

public class BasicFollowCamera : MonoBehaviour
{
    public GameObject target;
    public GameObject T;
    public float speed = 1.5f;
    public Vector3 offset;

   
    private void Update() {
        offset = target.transform.position + new Vector3(0, 3, -6);
        
        if (InputManager.Instance.GetBreakInput()) {
            offset = Vector3.Lerp(offset, target.transform.position + new Vector3(0, 3, 12), 0.25f);
        }
    }

    void FixedUpdate()
    {
        transform.LookAt(target.transform);
        float _carMove = Mathf.Abs(Vector3.Distance(transform.position, offset) * speed);
        transform.position = Vector3.MoveTowards(transform.position, offset, _carMove * Time.deltaTime);
    }

}