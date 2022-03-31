using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class MonsterMove : MonoBehaviour
{
    private Vector3 defaultTransform;
    NavMeshAgent agent;
    List<GameObject> TagFind = new List<GameObject>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultTransform = transform.position;
    }

    // Update is called once per frame
    private GameObject FindNearTag()
    {
        string tag = "Player";
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        var nearObject = objects.OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        }).FirstOrDefault();

        return nearObject;
    }

    void Update()
    {
        float perimeter = Mathf.Pow(10, 2);     // 기본 범위

        float circle = Mathf.Pow(defaultTransform.x - transform.position.x, 2) + Mathf.Pow(defaultTransform.z - transform.position.z, 2);    // 몹의 위치 변환 원

        if (FindNearTag() != null)
        {
            float navCircle = Mathf.Pow(FindNearTag().transform.position.x - defaultTransform.x, 2) + Mathf.Pow(FindNearTag().transform.position.z - defaultTransform.z, 2);    // 타겟과 몹 간의 거리 원

            if (circle >= perimeter)   //몹의 위치가 기본 범위를 벗어 나면
            {
                agent.SetDestination(defaultTransform);
            }
            else if (perimeter > navCircle)  // 기본 범위가 타겟과 몹 간의 거리 범위보다 클 때
            {
                agent.SetDestination(FindNearTag().transform.position);
            }
            else
            {
                agent.SetDestination(defaultTransform);
            }
        }
        else
        {
            agent.SetDestination(defaultTransform);
        }
    }
}
