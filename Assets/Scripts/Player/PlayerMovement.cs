using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private NoteManager noteManager; 
    [SerializeField] private float punchDuration = 0.2f; 
    private bool isPunching = false; 

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsDancing", true);

        if (noteManager != null)
        {
            noteManager.OnNoteLogged += HandleNoteLogged;
        }
        else
        {
            Debug.LogError("NoteManager no asignado en PlayerMovement");
        }
    }

    private void HandleNoteLogged(Note note)
    {
        if (!isPunching) 
        {
            PerformPunch();
        }
    }

    public void PerformPunch()
    {
        isPunching = true; 
        animator.SetBool("IsDancing", false);
        animator.SetBool("IsPunching", true);

        StartCoroutine(ReturnToDance());
    }

    private IEnumerator ReturnToDance()
    {
        yield return new WaitForSeconds(punchDuration);

        animator.SetBool("IsPunching", false);
        animator.SetBool("IsDancing", true);
        isPunching = false; 
    }

    public void Die()
    {
        animator.SetBool("IsDancing", false);
        animator.SetBool("IsKicking", false);
        animator.SetBool("IsPunching", false);

        animator.SetBool("IsDead", true);

        if (noteManager != null)
        {
            noteManager.OnNoteLogged -= HandleNoteLogged;
        }
    }

    private void OnDestroy()
    {
        if (noteManager != null)
        {
            noteManager.OnNoteLogged -= HandleNoteLogged;
        }
    }
}
