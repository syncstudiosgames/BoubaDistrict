using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    float _moveSpeed = 15;
    Vector3 _moveDirection;
    public void SetUp(float moveSpeed)
    {
        _moveSpeed = moveSpeed; 
        _moveDirection = transform.forward;
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + _moveDirection.normalized * _moveSpeed/1000;
    }
}
