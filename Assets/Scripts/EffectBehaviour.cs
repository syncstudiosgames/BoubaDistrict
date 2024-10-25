using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{
    Vector3 _initialScale;
    private void Start()
    {
        _initialScale = transform.localScale;
    }

    public void Boing()
    {
        transform.localScale = _initialScale;

        var newScale = transform.localScale + _initialScale * 0.7f;
        transform.localScale = newScale;
        //transform.LeanScale(newScale, .2f).setEaseInExpo();

        //transform.localScale = _initialScale;
        transform.LeanScale(_initialScale, .2f).setEaseInQuad();
    }
}
