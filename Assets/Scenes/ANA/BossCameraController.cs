using System.Collections;
using UnityEngine;

public class BossCameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform; 
    [SerializeField] private float _moveDuration;   
    [SerializeField] private float _waitDuration;     
    [SerializeField] private Vector3 _closeOffset; 

    private bool _hasMoved = false; 
    private Vector3 _originalPosition; 

    private void Start()
    {
        _originalPosition = _cameraTransform.position;
    }

    private void Update()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss != null && !_hasMoved)
        {
            _hasMoved = true; 
            StartCoroutine(CameraSequence(boss.transform));
        }
    }

    IEnumerator CameraSequence(Transform bossTransform)
    {
        Vector3 targetPosition = bossTransform.position + _closeOffset;

        yield return StartCoroutine(MoveCamera(_cameraTransform.position, targetPosition, _moveDuration));

        yield return new WaitForSeconds(_waitDuration);

        yield return StartCoroutine(MoveCamera(_cameraTransform.position, _originalPosition, _moveDuration));
    }

    IEnumerator MoveCamera(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _cameraTransform.position = targetPosition; 
    }
}
