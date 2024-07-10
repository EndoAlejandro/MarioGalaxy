using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ForwadMovement : MonoBehaviour
{
    [SerializeField]float _gravityForce;
    [SerializeField]float _speed;
    [SerializeField]float _rotationSpeed;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    
    Rigidbody _rigidBody;

    private void Start() 
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() 
    {
        Vector3 gravityDirection = GetGravityDirection();
        _rigidBody.velocity = ((transform.forward * _speed) - (gravityDirection * _gravityForce) ) * 10  * Time.fixedDeltaTime;

        Vector3 LerpDir = Vector3.Lerp(transform.up, gravityDirection, _rotationSpeed * 10 * Time.fixedDeltaTime);
        _rigidBody.rotation = Quaternion.FromToRotation(transform.up, LerpDir) * _rigidBody.rotation;
    }

    Vector3 GetGravityDirection()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, _groundDistance, _groundMask);
        return hit.normal;
    } 
}
