using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatDisplay : MonoBehaviour
{
    [SerializeField] BeatManager _beatManager;
    [SerializeField] Slider _sliderLeftToRight;
    [SerializeField] Slider _sliderRightToLeft;

    private void FixedUpdate()
    {
        _sliderLeftToRight.value = MyNormalize(_beatManager.ClipProgress);
        _sliderRightToLeft.value = MyNormalize(_beatManager.ClipProgress);

        Debug.Log(MyNormalize(_beatManager.ClipProgress));
    }

    float MyNormalize(float value)
    {
        var result = (value % 1.0f) * 10;
        return float.IsNaN(result) ? 0 : result;
    }
}
