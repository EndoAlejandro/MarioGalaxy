using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Animator _animator;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;
    private float _turnSmoothVelocity;
    private Transform _cameraTransform;
    private Transform _gravityCenter;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        RaycastHit hit;
        _isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, _groundDistance, _groundMask);

        if (_isGrounded)
        {
            _gravityCenter = hit.transform;
            _velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

            if (_isGrounded)
            {
                Vector3 groundNormal = (transform.position - _gravityCenter.position).normalized;
                Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, groundNormal) * targetRotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, Time.deltaTime * 10f);
            }

            Vector3 moveDirection = targetRotation * Vector3.forward;
            _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);

            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }

        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            Vector3 gravityDirection = (transform.position - _gravityCenter.position).normalized;
            _velocity = gravityDirection * Mathf.Sqrt(_jumpForce * 2f * _gravity);
        }

        if (_gravityCenter != null)
        {
            Vector3 gravityDirection = (_gravityCenter.position - transform.position).normalized;
            _velocity += gravityDirection * _gravity * Time.deltaTime;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * _groundDistance);
    }
}
