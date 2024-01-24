using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f; //적 인식 범위
    [SerializeField] float turnSpeed = 5f;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity; //초기값
    bool isProvoked = false; // 도발당함 여부

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position); // 대상과 자신 사이 거리

        if (isProvoked) //도발 당하면
        {
            EngageTarget(); //쫓아감
        }
       else if(distanceToTarget <= chaseRange) //가까워지면
        {
            isProvoked = true; //도발 당함
        }
    }

    void EngageTarget()
    {
        FaceTarget();
        if ( distanceToTarget >= navMeshAgent.stoppingDistance ) //타겟과 일정 거리가 될 때까지 추적
        {
            ChaseTarget();
        }
        if (distanceToTarget <= navMeshAgent.stoppingDistance) //일정 거리 도달 시 공격
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z )); //어느 방향 봐야하는지
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,Time.deltaTime * turnSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
