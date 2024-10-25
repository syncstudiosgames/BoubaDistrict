using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BeatDisplay : MonoBehaviour
{
    [SerializeField] BeatManager _beatManager;
    [SerializeField] Slider _sliderLeftToRight;
    [SerializeField] Slider _sliderRightToLeft;

    [SerializeField] Color _fillColor;
    [SerializeField] Color _fillHighlightColor;
    [SerializeField] Image _fillLeftToRight;
    [SerializeField] Image _fillRightToLeft;

    private void Start()
    {
        _fillLeftToRight.color = _fillColor;
        _fillRightToLeft.color = _fillColor;

        _beatManager.OnBeat += HighlightSliders;
        _beatManager.OnWindowClose += UnHighlightSliders;
    }
    private void FixedUpdate()
    {
        _sliderLeftToRight.value = MyNormalize(_beatManager.ClipProgress);
        _sliderRightToLeft.value = MyNormalize(_beatManager.ClipProgress);
    }

    void HighlightSliders()
    {
        _fillLeftToRight.color = _fillHighlightColor;
        _fillRightToLeft.color = _fillHighlightColor;
    }
    void UnHighlightSliders()
    {
        _fillLeftToRight.color = _fillColor;
        _fillRightToLeft.color = _fillColor;
    }

    float MyNormalize(float value)
    {
        var result = (value % 1.0f) * 10;
        return float.IsNaN(result) ? 0 : result;
    }
}
