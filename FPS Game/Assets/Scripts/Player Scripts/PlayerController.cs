using UnityEngine;

[RequireComponent(typeof(FPSInput))]
[RequireComponent(typeof(FPSMotor))]
public class PlayerController : MonoBehaviour
{
    FPSInput _input = null;
    FPSMotor _motor = null;

    [SerializeField] float _moveSpeed = 0.1f;
    [SerializeField] float _turnSpeed = 6f;
    [SerializeField] float _jumpStrength = 10f;    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }

    private void Awake()
    {
        _input = GetComponent<FPSInput>();
        _motor = GetComponent<FPSMotor>();
    }

    private void OnEnable()
    {
        _input.MoveInput += OnMove;
        _input.RotateInput += OnRotate;
        _input.JumpInput += OnJump;
    }

    private void OnDisable()
    {
        _input.MoveInput -= OnMove;
        _input.RotateInput -= OnRotate;
        _input.JumpInput -= OnJump;
    }

    void OnMove(Vector3 movement)
    {
        // Incorporate our move speed.
        _motor.Move(movement * _moveSpeed);
    }

    void OnRotate(Vector3 rotation)
    {
        // Camera looks vertical, body rotates left/ right.
        _motor.Turn(rotation.y * _turnSpeed);
        _motor.Look(rotation.x * _turnSpeed);
    }

    void OnJump()
    {
        // Apply our jump for to our motor.
        _motor.Jump(_jumpStrength);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _moveSpeed = 0.2f;
        }
        else
            _moveSpeed = 0.1f;

        // Exits Program.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}