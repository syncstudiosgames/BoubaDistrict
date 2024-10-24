using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatInteractor : MonoBehaviour
{
    #region Variables & Propeties
    [SerializeField] BeatManager _beatManager;
    [SerializeField] NoteManager _noteManager;
    [SerializeField] PlayerInput _playerInput;

    [SerializeField] Cooldown _cooldown;
    #endregion

    #region Public Methods
    public void RecieveInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // An action goes through three phases when trigger: Started, Performed and Cancelled.
                                        // Execute this code only if in the performed phase.

        if (_beatManager.IsOnBeat())    // If the note was performed on beat.
        {
            // Due to the error threshold the user may input multiple notes on the same beat so a cooldown implementation is required.
            // NOTE: The cooldown input must be grater than errorThreshold*2 wich can lead to potential problems. 
            // Ideally the cooldown resets when the beat window closes.
            // Cooldown time and clip progress are not messured by the same units so errors may arise.

            if (_cooldown.IsCoolingDown) return;

            _noteManager.InputNote(context.action, true);

            _cooldown.StartCooldown();
        }
        else
        {
            _noteManager.InputNote(context.action, false);
            Debug.Log("Not on beat!");
        }
       

    }
    #endregion

    #region Private Methods
    private void Start()
    {
        _playerInput.onActionTriggered += RecieveInput;
    }
    #endregion
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