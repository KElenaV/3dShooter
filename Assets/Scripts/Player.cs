using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _strafeSpeed = 7f;
    [SerializeField] private float _jumpSpeed = 7f;
    [SerializeField] private float _gravityFactor = 2f;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _horizontalTurnSensitivity = 10f;
    [SerializeField] private float _verticalTurnSensitivity = 10f;
    [SerializeField] private float _verticalMinAngle = -89f;
    [SerializeField] private float _verticalMaxAngle = 89f;

    private Transform _transform;
    private CharacterController _characterController;
    private float _cameraAngle = 0f;
    private Vector3 _verticalVelocity;

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _cameraAngle = _cameraTransform.localEulerAngles.x;
    }

    private void Update()
    {
        if(_characterController != null)
        {
            Vector3 playerSpeed = GetPlayerSpeed();

            VerticalRotate();
            
            HorizontalRotate();

            Move(playerSpeed);
        }
    }

    private Vector3 GetPlayerSpeed()
    {
        Vector3 forward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;
        Vector3 playerSpeed = forward * Input.GetAxis("Vertical") * _speed +
                          right * Input.GetAxis("Horizontal") * _strafeSpeed;

        return playerSpeed * Time.deltaTime;
    }

    private void VerticalRotate()
    {
        _cameraAngle -= Input.GetAxis("Mouse Y") * _verticalTurnSensitivity;
        _cameraAngle = Mathf.Clamp(_cameraAngle, _verticalMinAngle, _verticalMaxAngle);
        _cameraTransform.localEulerAngles = Vector3.right * _cameraAngle;
    }
    
    private void HorizontalRotate()
    {
        _transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _horizontalTurnSensitivity);
    }

    private void Move(Vector3 playerSpeed)
    {
        if (_characterController.isGrounded)
            HandleGroundedPlayerMovement(playerSpeed);
        else
            HandleAirborneMovement();
    }

    private void HandleAirborneMovement()
    {
        Vector3 horizontalVelocity = _characterController.velocity;
        horizontalVelocity.y = 0;
        _verticalVelocity += Physics.gravity * Time.deltaTime * _gravityFactor;
        _characterController.Move((horizontalVelocity + _verticalVelocity) * Time.deltaTime);
    }

    private void HandleGroundedPlayerMovement(Vector3 playerSpeed)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _verticalVelocity = Vector3.up * _jumpSpeed;
        else
            _verticalVelocity = Vector3.down;

        _characterController.Move(playerSpeed + _verticalVelocity * Time.deltaTime);
    }
}
