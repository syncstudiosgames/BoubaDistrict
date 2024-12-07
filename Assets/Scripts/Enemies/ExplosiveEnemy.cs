using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemy : Enemy
{
    public float _explosiveRadius;

    [SerializeField] CircleDrawer _circleDrawer;

    bool _restoring;

    private void Start()
    {
        _circleDrawer.DrawCircle(_explosiveRadius);
    }

    public void Explode()
    {
        if (!_restoring) { _restoring = true; }

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosiveRadius);
        foreach (Collider col in colliders)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null && enemy != this)
            {
                enemy.Restore();
            }
        }
    }

    public override void Restore()
    {
        if(_restoring) { return; }
        Explode();
        base.Restore();
    }
    
}
