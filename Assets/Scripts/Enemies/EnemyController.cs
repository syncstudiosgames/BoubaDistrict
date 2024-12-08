using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject _modelHolder;
    
    float _moveSpeed = 15;
    float _initialMoveSpeed;

    bool _isStunned;

    Vector3 _moveDirection;
    public void SetUp(float moveSpeed)
    {
        _moveSpeed = moveSpeed; 
        _initialMoveSpeed = moveSpeed;

        _moveDirection = transform.forward;

        EnterAnimation();
    }

    public void IncreaseMovespeed(float speedIncrement)
    {
        _moveSpeed += speedIncrement;
    }
    public void ResetMovespeed()
    {
        _moveSpeed = _initialMoveSpeed;
    }

    public void Stun(float stunTime)
    {
        _isStunned = true;
        Invoke("UnStun", stunTime);
    }
    public void UnStun()
    {
        _isStunned = false;
    }

    void EnterAnimation()
    {
        if(_modelHolder == null) return;

        _modelHolder.transform.localScale = Vector3.zero;

        LeanTween.scale(_modelHolder, Vector3.one, 0.5f)
            .setEase(LeanTweenType.easeOutBack);
    }

    private void FixedUpdate()
    {
        if(!_isStunned)
        {
            transform.position = transform.position + _moveDirection.normalized * _moveSpeed / 1000;
        }
        
    }
}
