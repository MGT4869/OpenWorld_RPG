using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAni_State : MonoBehaviour
{
    private Animator animator;

    float timer;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Attack();
        StartCoroutine(Skill());
    }
    public bool Ani_Name(string name)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Run", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("Run", false);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("Walk", true);
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            animator.SetBool("Jump", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            animator.SetBool("Jump", false);
        }
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)
            && !Ani_Name("Attack02") && !Ani_Name("Skill") && !Ani_Name("Skill_Main"))
        {
            animator.Play("Attack01");
            if ((Ani_Name("Attack01") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f
                && Input.GetKeyDown(KeyCode.LeftControl)))
            {
                animator.Play("Attack02");
            }
        }
    }

    IEnumerator Skill()
    {
        if (Input.GetKey("q") && Ani_Name("Idle") || Input.GetKeyDown("q") && Ani_Name("WalkForward")
            || Input.GetKeyDown("q") && Ani_Name("Run"))
        {
            animator.Play("Skill");
            yield return new WaitForSecondsRealtime(3f);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_Main"))
            {
                animator.Play("Idle");
            }
        }
        else if (Input.GetKeyUp("q") && Ani_Name("Skill_Main") || Input.GetKeyUp("q") && Ani_Name("Skill"))
        {
            animator.Play("Idle");
        }

        if (Input.GetKey("w") && Ani_Name("Idle") || Input.GetKeyDown("w") && Ani_Name("WalkForward")
            || Input.GetKeyDown("w") && Ani_Name("Run"))
        {
            animator.Play("Skill02");
            yield return new WaitForSecondsRealtime(6f);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Skill02_Main"))
            {
                animator.Play("Idle");
            }
        }
        else if (Input.GetKeyUp("w") && Ani_Name("Skill02_Main") || Input.GetKeyUp("w") && Ani_Name("Skill02"))
        {
            animator.Play("Idle");
        }
    }
}