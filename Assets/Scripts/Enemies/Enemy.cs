using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    NoteManager _noteManager;
    EnemyManager _enemyManager;

    List<Note>[] _deathSequences;

    int _complexity;
    int _lives;
    int _currrentLivePointer = 0;

    [SerializeField] private EnemyDisplay _enemyDisplay;
    [SerializeField] EnemyController _enemyController;
    [SerializeField] EnemyModelLoader _enemyModelLoader;

    [SerializeField] GameObject _modelHolder;

    [SerializeField] GameObject _splashEffect;
    [SerializeField] GameObject _boostEffect;

    Transform modelHolderTransform;
    Animator animator;

    event Action _onRestore;
    public event Action OnRestore { add { _onRestore += value; } remove { _onRestore -= value; } }

    event Action _onDie;
    public event Action OnDie { add { _onDie += value; } remove { _onDie -= value; } }


    public void SetUp(int complexity, float moveSpeed, NoteManager noteManager, EnemyManager enemyManager, int lives = 1, bool renderSequence = true)
    {
        _noteManager = noteManager;
        _enemyManager = enemyManager;
        _complexity = Mathf.Clamp(complexity, 1, 4);
        _enemyController.SetUp(moveSpeed);

        _lives = lives;

        modelHolderTransform = transform.Find("ModelHolder");



        if (_enemyModelLoader != null)
        {
            _enemyModelLoader.AssignRandomModel();
            _splashEffect = _enemyModelLoader.splash;
            if (modelHolderTransform != null)
            {
                // Obtener el primer hijo de "ModelHolder"
                if (modelHolderTransform.childCount > 0)
                {
                    Transform targetChild = modelHolderTransform.GetChild(0); // Primer hijo

                    // Asegurarse de que tiene el componente Animator, y si no, añadirlo
                    Animator animator = targetChild.GetComponent<Animator>();
                    if (animator == null)
                    {
                        animator = targetChild.gameObject.AddComponent<Animator>();
                    }

                    // Asignar el nuevo RuntimeAnimatorController
                    if (_enemyModelLoader.controller != null)
                    {
                        animator.runtimeAnimatorController = _enemyModelLoader.controller;
                    }
                    else
                    {
                        Debug.LogWarning("Animator o RuntimeAnimatorController no asignado correctamente.");
                    }
                }
                else
                {
                    Debug.LogError("No se encontró un GameObject hijo llamado 'ModelLoader'.");
                }
            }
        }

        CreateSequence(lives);
        _noteManager.OnNoteLogged += CheckSequence;


        if (_enemyDisplay != null)
        {
            _enemyDisplay.SetUp(_deathSequences, _noteManager, renderSequence);
        }
        else
        {
            Debug.LogError("No asignado");
        }

        if (_splashEffect != null)
        {
            _splashEffect.SetActive(false);
        }
    }

    public void BoostSpeed(float speed)
    {
        _enemyController.IncreaseMovespeed(speed);
        _boostEffect?.SetActive(true);
    }
    public void UnboostSpeed()
    {
        _enemyController.ResetMovespeed();
        _boostEffect?.SetActive(false);
    }

    void CreateSequence(int lives)
    {
        _deathSequences = new List<Note>[lives];

        for(int i = 0; i < lives; i++)
        {
            _deathSequences[i] = new List<Note>();
            for (int j = 0; j < _complexity; j++)
            {
                _deathSequences[i].Add(GetRandomNote());
            }
        }
        
    }

    Note GetRandomNote()
    {
        var notes = _noteManager.Notes;
        return notes[UnityEngine.Random.Range(0, notes.Count)];
    }

    void CheckSequence(Note inputNote)
    {
        var noteBuffer = _noteManager.NoteBuffer;

        if (noteBuffer.Count < _complexity) return;

        List<Note> currentDeathSequence = _deathSequences[_currrentLivePointer];

        for (int myNote = 0, bufferNote = noteBuffer.Count - _complexity; myNote < currentDeathSequence.Count; myNote++, bufferNote++)
        {
            if (currentDeathSequence[myNote] != noteBuffer[bufferNote]) return;
        }

        // If this part of the code is reached the death sequence was complete:

        TakeDamage();
    }

    public void TakeDamage()
    {
        if (_currrentLivePointer >= _lives - 1)
        {
            Restore();
        }
        else
        {
            _enemyController.Stun(3);
            _enemyDisplay.DisplayNextDeathSequence(3);
            _currrentLivePointer++;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyGoal")
        {
            Die();
        }
    }

    public virtual void Restore()
    {
        _enemyDisplay.isDead = true;
        if (_splashEffect != null) { _splashEffect.SetActive(true); }

        _onRestore?.Invoke();

        LeanTween.value(gameObject, 1f, 0f, 1f).setOnUpdate((float value) =>                                                                      // Animate alpha.
        {
            _modelHolder.SetActive(false);
            _enemyDisplay.HideSequence();
            _enemyDisplay.HideLives();

        }).setOnComplete(() =>                                                                                                                  // Destroy GO when the animation is done.
        {
            _enemyManager.EnemyCured(_complexity);
            Destroy(gameObject);
        });
    }

    void Die()
    {
        _enemyManager.EnemyHit(_complexity);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        _noteManager.OnNoteLogged -= CheckSequence;

        if (_lives > 1)  // If it a was a boss notify enemy manager that is gone.
        {
            _enemyManager.BossGone();
        }
    }
}
