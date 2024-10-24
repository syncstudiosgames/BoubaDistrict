using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private bool isKicking;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsDancing", true);
        isKicking = false;
    }

    void Update()
    {
        if (isKicking)
        {
            animator.SetBool("IsKicking", true);
            animator.SetBool("IsDancing", false);
        }
        else
        {
            animator.SetBool("IsKicking", false);
            animator.SetBool("IsDancing", true);
        }
    }

    public void PerformKick()
    {
        StartCoroutine(DoKick());
    }

    private IEnumerator DoKick()
    {
        isKicking = true;

        yield return new WaitForSeconds(1.0f);

        isKicking = false;
    }
}
