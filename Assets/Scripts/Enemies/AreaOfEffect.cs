using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AreaOfEffect : MonoBehaviour
{
    [SerializeField] float _effectRadious;
    [SerializeField] CircleDrawer _circleDrawer;

    float speedBoostValue = 15f;

    private void Start()
    {
        _circleDrawer.DrawCircle(_effectRadious);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.BoostSpeed(speedBoostValue);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.UnboostSpeed();
        }
    }

    private void OnDestroy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _effectRadious);
        foreach (Collider col in colliders)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null && enemy != this)
            {
                enemy.UnboostSpeed();
            }
        }
    }
}
