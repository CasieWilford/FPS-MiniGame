using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FPSInput : MonoBehaviour
{
    [SerializeField] bool _invertVertical = false;

    public event Action<Vector3> MoveInput = delegate { };
    public event Action<Vector3> RotateInput = delegate { };
    public event Action JumpInput = delegate { };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectMoveInput();
        DetectRotateInput();
        DetectJumpInput();
    }

    void DetectMoveInput()
    {
        // Process input as a 0 or 1 value, if we have it.
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        // If we have either horizontal or vertical input.
        if (xInput != 0 || yInput != 0)
        {
            // Convert to local directions, based on player orientation.
            Vector3 _horizontalMovement = transform.right * xInput;
            Vector3 _forwardMovement = transform.forward * yInput;

            // Combine movenets into a single vector.
            Vector3 movement = (_horizontalMovement + _forwardMovement).normalized;

            // Notify that we have moved.
            MoveInput?.Invoke(movement);
        }

    }

    void DetectRotateInput()
    {
        // Get our inputs from input controller.
        float xInput = Input.GetAxisRaw("Mouse X");
        float yInput = Input.GetAxisRaw("Mouse Y");

        if (xInput !=0 || yInput !=0)
        {
            //Account for inverted camera movement, if specified.
            if(_invertVertical == true)
            {
                yInput = -yInput;
            }

            // Mouse left/right should be y axis, up/down x axis.
            Vector3 rotation = new Vector3(yInput, xInput, 0);

            // Notify that we have rotated.
            RotateInput?.Invoke(rotation);
        }
    }

    void DetectJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput?.Invoke();
        }
    }
}
