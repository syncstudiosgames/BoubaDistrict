using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatDisplay : MonoBehaviour
{
    Image _display;

    [SerializeField] Color _displayColor;
    [SerializeField] Color _highlightColor;

    const float DISPLAY_TIME = 0.2f;

    private void Start()
    {
        _display = GetComponent<Image>();
        _display.color = _displayColor;
    }

    public void Highlight()
    {
        _display.color = _highlightColor;
        Invoke("UnHighlight", DISPLAY_TIME);
    }

    void UnHighlight()
    {
        _display.color = _displayColor;
    }

}
