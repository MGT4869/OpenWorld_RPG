using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAni_State : MonoBehaviour
{
    public Coroutine atkOne;
    private Animator animator;
    private PlayerInfo playInfo;

    void Start()
    {
        playInfo = GetComponent<PlayerInfo>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Attack_Ani();
        Drink();
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

    private GameObject FindNearstObjectByTag(string tag)
    {
        var _obj = GameObject.FindGameObjectsWithTag(tag).ToList();

        var neareastObject = _obj.OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        }).FirstOrDefault();

        return neareastObject;
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playInfo.AniState = "1";

            animator.SetBool("Run", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playInfo.AniState = "!1";

            animator.SetBool("Run", false);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            playInfo.AniState = "2";

            animator.SetBool("Walk", true);
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            playInfo.AniState = "!2";

            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            playInfo.AniState = "3";
            animator.SetBool("Jump", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {

            playInfo.AniState = "!3";
            animator.SetBool("Jump", false);
        }
    }

    public void Attack_Ani()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)
            && !Ani_Name("Attack02") && !Ani_Name("Skill") && !Ani_Name("Skill_Main") && !Ani_Name("PotionDrink"))
        {
            if (atkOne == null)
            {
                atkOne = StartCoroutine(Attack());
            }

            animator.Play("Attack01");
            if ((Ani_Name("Attack01") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f
                && Input.GetKeyDown(KeyCode.LeftControl)))
            {
                StartCoroutine(Attack());
                animator.Play("Attack02");
            }
        }
    }
    IEnumerator Attack()
    {
        do {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f && !Ani_Name("Idle"))
            {
                if (Vector3.Distance(transform.position, FindNearstObjectByTag("Enemy").transform.position) <= 3f)
                {
                    Debug.Log(FindNearstObjectByTag("Enemy"));
                }
                break;
            }
            
            yield return new WaitForEndOfFrame();
        } while (true);
        
        do
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f || Ani_Name("Attack02") || Ani_Name("Idle"))
                break;
            yield return new WaitForEndOfFrame();
        } while (true);
        atkOne = null;
    }

    public void Drink()
    {
        if (Input.GetKeyDown("1") && Ani_Name("Idle") || Input.GetKeyDown("1") && Ani_Name("WalkForward")
            || Input.GetKeyDown("1") && Ani_Name("Run"))
        {
            animator.Play("PotionDrink");
        }
    }

    IEnumerator Skill()
    {
        if (Input.GetKey("q") && Ani_Name("Idle") || Input.GetKey("q") && Ani_Name("WalkForward")
            || Input.GetKey("q") && Ani_Name("Run"))
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

        if (Input.GetKey("w") && Ani_Name("Idle") || Input.GetKey("w") && Ani_Name("WalkForward")
            || Input.GetKey("w") && Ani_Name("Run"))
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