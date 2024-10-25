using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{
    private void Start()
    {
        Boing();
    }

    public void Boing()
    {
        var startingScale = transform.localScale;
        var newScale = transform.localScale + transform.localScale*0.3f;
        transform.LeanScale(newScale, .1f)
            .setEaseInExpo()
            .setOnComplete(() =>
            {
                transform.LeanScale(startingScale, .5f).setEaseInQuad();
            });

    }
}
