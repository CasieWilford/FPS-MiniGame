using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class FPSMotor : MonoBehaviour
{
    public event Action Land = delegate { };

    [SerializeField] Camera _camera = null;
    [SerializeField] float _cameraAngleLimit = 70f;
    [SerializeField] GroundDetector _groundDetector = null;

    bool _isGrounded = false;

    Rigidbody _rigidbody = null;

    Vector3 _movementThisFrame = Vector3.zero;

    // Tracking our own camera angle, to avoid weird 0 - 360 angle conversions.
    private float _currentCameraRotationX = 0;
    float _turnAmountThisFrame = 0;
    float _lookAmountThisFrame = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 requestedMovement)
    {
        // Store this movement for next FixedUpdate tick.
        _movementThisFrame = requestedMovement;
    }

    public void Turn(float turnAmount)
    {
        // Store this rotation for next FixedUpdate tick.
        _turnAmountThisFrame = turnAmount;
    }

    public void Look(float lookAmount)
    {
        // Store this rotation  for next LateUpdate tick.
        _lookAmountThisFrame = lookAmount;
    }
    
    public void Jump(float jumpForce)
    {
        // Only allow us to jump if we're on the ground.
        if (_isGrounded == false)
            return;

        _rigidbody.AddForce(Vector3.up * jumpForce);
    }

    private void FixedUpdate()
    {
        ApplyMovement(_movementThisFrame);
        ApplyTurn(_turnAmountThisFrame);
        ApplyLook(_lookAmountThisFrame);
    }

    void ApplyMovement(Vector3 moveVector)
    {
        // Confirm that we actually have movement, exit early if we don't.
        if (moveVector == Vector3.zero)
            return;

        // Move the rigidbody.
        _rigidbody.MovePosition(_rigidbody.position + moveVector);

        // Clear out movement, until we get a new move request.
        _movementThisFrame = Vector3.zero;
    }

    void ApplyTurn(float rotateAmount)
    {
        // Confirm that we actually have rotation, exit early if we don't.
        if (rotateAmount == 0)
            return;

        // Rotate the body. Convert x,y,z to Quaternion for MoveRotation().
        Quaternion newRotation = Quaternion.Euler(0, rotateAmount, 0);
        _rigidbody.MoveRotation(_rigidbody.rotation * newRotation);

        // Clear out turn amount, until we get a new turn request.
        _turnAmountThisFrame = 0;
    }

    void ApplyLook(float lookAmount)
    {
        // Confirm that we actually have rotation, exit early if we don't.
        if (lookAmount == 0)
            return;

        // Calculate and clamp our new camera rotation, before we apply it.
        _currentCameraRotationX -= lookAmount;
        _currentCameraRotationX = Mathf.Clamp
            (_currentCameraRotationX, -_cameraAngleLimit, _cameraAngleLimit);
        _camera.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0, 0);

        // Clear out x movement, until we get a new move request.
        _lookAmountThisFrame = 0;
    }

    private void OnEnable()
    {
        _groundDetector.GroundDetected += OnGroundDetected;
        _groundDetector.GroundVanished += OnGroundVanished;
    }

    private void OnDisable()
    {
        _groundDetector.GroundDetected -= OnGroundDetected;
        _groundDetector.GroundVanished -= OnGroundVanished;
    }

    void OnGroundDetected()
    {
        _isGrounded = true;

        // Notify others that we have landed (animations, etc.).
        Land?.Invoke();
    }

    void OnGroundVanished()
    {
        _isGrounded = false;
    }
}
