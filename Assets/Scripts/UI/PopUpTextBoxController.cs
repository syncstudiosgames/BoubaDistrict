using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PopUpTextBoxController : MonoBehaviour
{
    //event Action _onNext;
    //public event Action OnNext {  add { _onNext += value; } remove { _onNext -= value; } }

    [SerializeField] bool _showOnStart;

    public UnityEvent onNext;

    private void Start()
    {
        gameObject.SetActive(_showOnStart);
    }

    public void Next()
    {
        onNext?.Invoke();
        Hide();
    }

    public void ShowWithDelay(float delay)
    {
        Invoke("Show", delay);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
