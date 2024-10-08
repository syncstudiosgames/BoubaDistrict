using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatDisplay : MonoBehaviour
{
    [SerializeField] BeatManager _beatManager;
    [SerializeField] Slider _sliderL2R;
    [SerializeField] Slider _sliderR2L;

    private void Update()
    {
        _sliderL2R.value = MyNormalize(_beatManager.ClipProgress);
        _sliderR2L.value = MyNormalize(_beatManager.ClipProgress);
    }

    float MyNormalize(float value)
    {
        return (value - Mathf.Floor(value))*10;
    }
}
