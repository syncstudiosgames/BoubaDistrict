using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BeatDisplay : MonoBehaviour
{
    [SerializeField] BeatManager _beatManager;
    [SerializeField] Slider _sliderLeftToRight;
    [SerializeField] Slider _sliderRightToLeft;

    [SerializeField] Image _pulseImage;
    [SerializeField] Color _pulseImageColor;
    [SerializeField] Color _pulseImageHighlightColor;

    [SerializeField] Sprite _pulseHelperSprite;
    [SerializeField] float _pulseHelperSize;

    [SerializeField] RectTransform _leftToRightInitialAnchor;
    [SerializeField] RectTransform _rightToLeftInitialAnchor;
    [SerializeField] RectTransform _convergencePoint;

    GameObject _l2rIcon;
    GameObject _r2lIcon;

    float ClipProgressNormalized { get { return MyNormalize(_beatManager.ClipProgress); } }

    private void Start()
    {
        _l2rIcon = CreateIcon(_leftToRightInitialAnchor, false);
        _r2lIcon = CreateIcon(_rightToLeftInitialAnchor, true);

        _beatManager.OnBeat += HighlightImage;
    }

    private void FixedUpdate()
    {
        //_sliderLeftToRight.value = ClipProgressNormalized;
        //_sliderRightToLeft.value = ClipProgressNormalized;

        AnimateIcon(_l2rIcon, _leftToRightInitialAnchor.localPosition, _convergencePoint.localPosition, ClipProgressNormalized);
        AnimateIcon(_r2lIcon, _rightToLeftInitialAnchor.localPosition, _convergencePoint.localPosition, ClipProgressNormalized);
    }

    GameObject CreateIcon(RectTransform anchor, bool invert)
    {
        // Create and set up GameObject:
        var icon = new GameObject("Pulse", typeof(RectTransform));
        icon.transform.SetParent(transform);

        // Set up icon:
        var rectTransform = icon.GetComponent<RectTransform>();
        rectTransform.localPosition = anchor.position;
        rectTransform.sizeDelta = new Vector2(_pulseHelperSize, _pulseHelperSize);
        if(invert) rectTransform.rotation = Quaternion.Euler(0, 0, 180);

        // Create image and assign sprite:
        var image = icon.AddComponent<Image>();
        image.sprite = _pulseHelperSprite;

        return icon;
    }
    void AnimateIcon(GameObject icon, Vector3 from, Vector3 to, float value)
    {
        // Animate position:
        var rectTransform = icon.GetComponent<RectTransform>();
        var pos = InterpolatePosition(from, to, value);
        rectTransform.localPosition = pos;

        value = Mathf.Pow(value, 2);    // Cuadratic interpolation.

        rectTransform.sizeDelta = new Vector2(_pulseHelperSize, _pulseHelperSize);

        // Interpolate alpha:
        var image = icon.GetComponent<Image>();
        var color = image.color;
        color.a = 1*value;
        image.color = color;
    }
    
    void HighlightImage()
    {
        _pulseImage.color = _pulseImageHighlightColor;
        Invoke("UnHighlightImage", 0.2f);
    }
    void UnHighlightImage()
    {
        _pulseImage.color = _pulseImageColor;
    }

    Vector3 InterpolatePosition(Vector3 initPos, Vector3 finalPos, float value)
    {
        value = Mathf.Clamp01(value);
        var distance = finalPos-initPos;
        return initPos + distance * value;
    }

    float MyNormalize(float value)
    {
        var result = value % 1.0f;
        return float.IsNaN(result) ? 0 : result;
    }
}
