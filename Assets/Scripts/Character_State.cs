using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character_State : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindNearstObjectByTag("Enemy");
    }

    private GameObject FindNearstObjectByTag(string tag)
    {
        var _obj = GameObject.FindGameObjectsWithTag(tag).ToList();

        var neareastObject = _obj.OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        }).FirstOrDefault();

        Debug.Log(neareastObject);
        return neareastObject;
    }
}
