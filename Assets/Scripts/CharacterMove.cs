using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private CharacterController cc;
    public Transform charact;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        StartCoroutine(moveCast());
    }

    public IEnumerator moveCast()
    {
        do
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                cc.SimpleMove(charact.forward * speed);
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    cc.SimpleMove(charact.right * speed);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    cc.SimpleMove(charact.right * -speed);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                cc.SimpleMove(charact.forward * -speed);
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    cc.SimpleMove(charact.right * speed);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    cc.SimpleMove(charact.right * -speed);
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                cc.SimpleMove(charact.right * speed);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                cc.SimpleMove(charact.right * -speed);
            }
            yield return new WaitForSeconds(0.016f);
        } while (true);
    }
}
