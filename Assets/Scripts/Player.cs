using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _straveSpeed = 3f;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _verticalTurnSensivity = 10f;
    [SerializeField] private float _horizontalTurnSensitivity =10f;
    [SerializeField] private float _verticalMinAngle = -89f;
    [SerializeField] private float _verticalMaxAngle = 89f;

    private Transform _transform;
    private CharacterController _characterController;
    private float _cameraAngle = 0f;

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _cameraAngle = _cameraTransform.localEulerAngles.x;
    }

    private void Update()
    {
        Vector3 forward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;

        _cameraAngle -= Input.GetAxis("Mouse Y") * _verticalTurnSensivity;
        _cameraAngle = Mathf.Clamp(_cameraAngle, _verticalMinAngle, _verticalMaxAngle);
        _cameraTransform.localEulerAngles = Vector3.right * _cameraAngle;

        if (_characterController != null)
        {
            Vector3 playerSpeed = forward * Input.GetAxis("Vertical") * _speed + right * Input.GetAxis("Horizontal") * _straveSpeed;
            playerSpeed *= Time.deltaTime;

            if (_characterController.isGrounded)
            {
                _characterController.Move(playerSpeed + Physics.gravity);
                //можно так
                //_characterController.Move(playerSpeed + Vector3.down);
            }
            else
            {
                _characterController.Move(_characterController.velocity + Physics.gravity * Time.deltaTime);
            }
        }
    }

    //Obstacle with rigidbody fly up when player collides it
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit != null && hit.rigidbody != null)
    //        hit.rigidbody.velocity = Vector3.up * 100f;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var character = GetComponent<CharacterController>();

        Gizmos.DrawWireCube(transform.position, Vector3.right + Vector3.forward + Vector3.up * character.height);
    }
}
