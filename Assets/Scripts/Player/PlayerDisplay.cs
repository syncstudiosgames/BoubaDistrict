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

    [SerializeField] int _maxHealthValueForIncreasingSlider;

    float _lastMaxHealth;

    private void Awake()
    {
        _player.OnHealthValueChange += UpdateDisplayData;

        _lastMaxHealth = _player.MaxHealthPoints;

        UpdateDisplayData();
    }

    void UpdateDisplayData()
    {
        // Update slider data:
        _healthSlider.maxValue = _player.MaxHealthPoints;
        _healthSlider.value = _player.HealthPoints;

        if (_player.MaxHealthPoints < _maxHealthValueForIncreasingSlider)
        {
            // Update slider size (scaling with maxHealth):
            var rectTrans = _healthSlider.gameObject.GetComponent<RectTransform>();
            var sizeDelta = rectTrans.sizeDelta;

            var newWidth = sizeDelta.x + (_player.MaxHealthPoints - _lastMaxHealth);
            
            sizeDelta = new Vector2(newWidth, sizeDelta.y);
            rectTrans.sizeDelta = sizeDelta;
        }

        _lastMaxHealth = _player.MaxHealthPoints;

        // Update text:
        string text = $"{_player.HealthPoints}/{_player.MaxHealthPoints}";
        _healthDisplay.text = text;

    }
}
