using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPanelManager : MonoBehaviour
{
    [SerializeField] UnityEvent _onTutorialFinish;
    [SerializeField] GameObject _startButton;

    public void FinishTutorial()
    {
        _onTutorialFinish?.Invoke();

        gameObject.SetActive(false);
    }
}
