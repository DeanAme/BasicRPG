using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem :EnemyController
//这就叫子类
{
    [Header("Skill")]
    public float force = 10;
    public GameObject rockprefabs;
    public Transform generatePoint;
    //animation event
    public void KickOff(){
        if(AttackTarget!= null&& transform.IsFacingTarget(AttackTarget.transform)){
            var targetStats = AttackTarget.GetComponent<CharacterStats>();
            Vector3 directionGolem = AttackTarget.transform.position-transform.position;
            directionGolem.Normalize();
           
           targetStats.GetComponent<NavMeshAgent>().isStopped=true;
           targetStats.GetComponent<NavMeshAgent>().velocity=force*directionGolem;
           targetStats.GetComponent<Animator>().SetTrigger("Dizzy");

            targetStats.TakeDemage(characterStats,targetStats);
          }
    }
    //animation event
    public void GenerateRocks(){
        if(AttackTarget!=null){
            var rock = Instantiate(rockprefabs,generatePoint.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = AttackTarget;
        }
    }
    
}
