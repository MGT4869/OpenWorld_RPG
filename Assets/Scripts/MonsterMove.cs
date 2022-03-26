using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterMove : MonoBehaviour
{
    private Vector3 defaultTransform;
    private Transform nowTransform;
    public Transform target;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultTransform = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //nowTransform = this.gameObject.transform;
        float perimeter = Mathf.Pow(10, 2);     // 기본 범위

        float circle = Mathf.Pow(defaultTransform.x - transform.position.x, 2) + Mathf.Pow(defaultTransform.z - transform.position.z, 2);    // 몹의 위치 변환 원
        //float circle = Mathf.Pow(radius, 2);

        //float absZ = Mathf.Abs(defaultTransform.position.z - nowTransform.position.z);
        float navCircle = Mathf.Pow(target.position.x - defaultTransform.x, 2) + Mathf.Pow(target.position.z - defaultTransform.z, 2);    // 타겟과 몹 간의 거리 원

        //float range = Mathf.Pow(transform.position.x, 2); //+ Mathf.Pow(transform.position.z, 2);
        if (circle >= perimeter)   //몹의 위치가 기본 범위를 벗어 나면
        {
            agent.SetDestination(defaultTransform);
        }
        else if (perimeter > navCircle)  // 기본 범위가 타겟과 몹 간의 거리 범위보다 클 때
        {
            agent.SetDestination(target.position);
        }
        else
        {
            agent.SetDestination(defaultTransform);
        }
    }
}
