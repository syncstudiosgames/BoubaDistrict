using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatInteractor : MonoBehaviour
{
    [SerializeField] BeatManager _beatManager;
    [SerializeField] NoteManager _noteManager;
    [SerializeField] PlayerInput _playerInput;

    [SerializeField] Cooldown _cooldown;

    private void Start()
    {
        _playerInput.onActionTriggered += RecieveInput;
    }
    public void RecieveInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (_beatManager.IsOnBeat())
        {
            if (_cooldown.IsCoolingDown) return;

            _noteManager.LogNote(context.action);
            _noteManager.PrintNoteBuffer();

            _cooldown.StartCooldown();
        }
        else
        {
            Debug.Log("Not on beat!");
            _noteManager.ResetBuffer();
        }
       

    }
    private void Update()
    {

    }
}

[System.Serializable]
public class Cooldown
{
    #region Variables

    [SerializeField] private float cooldownTime;
    private float _nextFireTime;

    #endregion

    public bool IsCoolingDown => Time.time < _nextFireTime;
    public void StartCooldown() => _nextFireTime = Time.time + cooldownTime;
}