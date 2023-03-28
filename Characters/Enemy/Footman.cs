using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Footman : EnemyController
{
    [Header("Skill")]
    public float force = 10f;
    public void kickOff(){
        if(AttackTarget!= null){
            transform.LookAt(AttackTarget.transform);
        Vector3 direction = AttackTarget.transform.position - transform.position;
        direction.Normalize();
        AttackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        AttackTarget.GetComponent<NavMeshAgent>().velocity = force * direction;
        AttackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
    
}
