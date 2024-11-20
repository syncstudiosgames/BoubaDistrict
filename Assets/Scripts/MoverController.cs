using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MoverController : MonoBehaviour
{
    private void Start()
    {
        //MoveTo(new Vector3(6.80999994f, 4.05999994f, 9.14000034f));
    }

    public void MoveUpBy(float units)
    {
        MoveTo(transform.position + new Vector3(0, units, 0), 1f);
    }

    public void MoveTo(Vector3 pos, float moveDuration)
    {
        LeanTween.move(gameObject, pos, moveDuration).setEase(LeanTweenType.easeInOutQuad);
    }

    public void MoveTo(Transform transform)
    {
        MoveTo(transform.position, 1f);
    }
}

