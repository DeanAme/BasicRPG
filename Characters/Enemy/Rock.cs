using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour

{  public enum RockStates{HitPlayer, HitEnemy, HitNothing};
     private Rigidbody rb;
     public RockStates rockStates;
     //生成的时候才执行命令就写在start里面
     //写一个力给石头冲击addforce 记得石头向上抛起来 记得算方向 建立刚体
    [Header("Basic Settings")]
    public float flyforce;
    public GameObject target;
    private Vector3 directionRock;
    public int demageRock;
    public GameObject breakEffect;
    
    void Start(){
    rb = GetComponent<Rigidbody>();
    rb.velocity= Vector3.one;
    rockStates = RockStates.HitPlayer;
    flyRock();
    }
    void FixedUpdate() {
       if(rb.velocity.sqrMagnitude<1f){
        rockStates = RockStates.HitNothing;

       } 
    }
    public void flyRock(){
        if(target == null)
        target = FindObjectOfType<PlayerController>().gameObject;
        directionRock=(target.transform.position-transform.position+Vector3.up).normalized;
        rb.AddForce(flyforce*directionRock, ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision other) {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
            if(other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                other.gameObject.GetComponent<NavMeshAgent>().velocity = directionRock*flyforce;
                other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                other.gameObject.GetComponent<CharacterStats>().TakeDemage(demageRock, other.gameObject.GetComponent<CharacterStats>());
                rockStates = RockStates.HitNothing;
            }
            break;
            case RockStates.HitEnemy:
            if(other.gameObject.GetComponent<Golem>())
            {
                var otherStats = other.gameObject.GetComponent<CharacterStats>();
                otherStats.TakeDemage(demageRock,otherStats);
                Instantiate(breakEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            break;
        }
        
    }

}
