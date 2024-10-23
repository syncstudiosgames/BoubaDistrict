using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] TextMeshProUGUI _healthDisplay;
    [SerializeField] Slider _healthSlider;

    float _startingWidth;
    float _lastMaxHealth;

    private void Start()
    {
        _player.OnHealthValueChange += UpdateDisplayData;

        _startingWidth = _healthSlider.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        _lastMaxHealth = _player.MaxHealthPoints;

    }

    void UpdateDisplayData()
    {
        // Update slider data:
        _healthSlider.maxValue = _player.MaxHealthPoints;
        _healthSlider.value = _player.HealthPoints;


        // Update slider size (scaling with maxHealth):
        var rectTrans = _healthSlider.gameObject.GetComponent<RectTransform>();
        var sizeDelta = rectTrans.sizeDelta;

        var newWidth = sizeDelta.x + (_player.MaxHealthPoints - _lastMaxHealth);
        _lastMaxHealth = _player.MaxHealthPoints;

        sizeDelta = new Vector2(newWidth, sizeDelta.y);
        rectTrans.sizeDelta = sizeDelta; 

        // Update text:
        string text = $"{_player.HealthPoints}/{_player.MaxHealthPoints}";
        _healthDisplay.text = text;

    }
}
