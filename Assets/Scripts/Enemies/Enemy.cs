using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    NoteManager _noteManager;
    EnemyManager _enemyManager;
    List<Note> _deathSequence = new List<Note>();
    int _complexity;

    [SerializeField] private EnemyDisplay _enemyDisplay;
    [SerializeField] GameObject _modelHolder;
    [SerializeField] public GameObject _healingEffect; 

    public void SetUp(int complexity, NoteManager noteManager, EnemyManager enemyManager, bool renderSequence = true)
    {
        _noteManager = noteManager;
        _enemyManager = enemyManager;
        _complexity = Mathf.Clamp(complexity, 1, 4);

        CreateSequence();
        _noteManager.OnNoteLogged += CheckSequence;


        if (_enemyDisplay != null)
        {
            _enemyDisplay.SetUp(_deathSequence, _noteManager, renderSequence);
        }
        else
        {
            Debug.LogError("No asignado");
        }

        if (_healingEffect != null)
        {
            _healingEffect.SetActive(false);
        }
    }

    public void Print()
    {
        Debug.Log($"Enemy. Complexity: {_complexity}, Death Sequence:");
        foreach (Note note in _deathSequence)
        {
            Debug.Log(note);
        }
    }

    void CreateSequence()
    {
        for (int i = 0; i < _complexity; i++)
        {
            _deathSequence.Add(GetRandomNote());
        }
    }

    Note GetRandomNote()
    {
        var notes = _noteManager.Notes;
        return notes[Random.Range(0, notes.Count)];
    }

    void CheckSequence(Note inputNote)
    {
        var noteBuffer = _noteManager.NoteBuffer;


        if (noteBuffer.Count < _complexity) return;

        for (int myNote = 0, bufferNote = noteBuffer.Count - _complexity; myNote < _deathSequence.Count; myNote++, bufferNote++)
        {
            if (_deathSequence[myNote] != noteBuffer[bufferNote]) return;
        }

        Restore();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyGoal")
        {
            Die();
        }
    }

    void Restore()
    {
        if (_healingEffect != null)
        {
            _healingEffect.SetActive(true);

            LeanTween.value(gameObject, 1f, 0f, 1f).setOnUpdate((float value) =>                                                                      // Animate alpha.
            {
                var renderer = _modelHolder.GetComponentInChildren<MeshRenderer>();
                Color color = renderer.material.color;
                color.a = value;
                renderer.material.color = color;

            }).setOnComplete(() =>                                                                                                                  // Destroy GO when the animation is done.
            {
                _enemyManager.EnemyCured(_complexity);
                Destroy(gameObject);
            });

            //StartCoroutine(PlayHealingAnimation());
        }
    }

    IEnumerator PlayHealingAnimation()
    {
        yield return new WaitForSeconds(1.0f); 

        _enemyManager.EnemyCured(_complexity);
        Destroy(gameObject);
    }

    void Die()
    {
        _enemyManager.EnemyHit(_complexity);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        _noteManager.OnNoteLogged -= CheckSequence;
    }
}
