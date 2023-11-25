using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    private Transform _transform;
    private CharacterController _characterController;

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(_characterController != null)
        {
            Vector3 playerSpeed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            playerSpeed *= Time.deltaTime * _speed;

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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit != null && hit.rigidbody != null)
            hit.rigidbody.velocity = Vector3.up * 100f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var character = GetComponent<CharacterController>();

        Gizmos.DrawWireCube(transform.position, Vector3.right + Vector3.forward + Vector3.up * character.height);
    }
}
