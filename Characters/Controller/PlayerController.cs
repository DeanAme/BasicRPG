using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private CharacterStats characterStats;
    private float lastAttackTime;
    private GameObject attackTarget;
  
    private NavMeshAgent agent;
    Animator anim;
    private bool isDead;
    private float StopDistance;
    void Awake() {
      anim = GetComponent<Animator>();
      agent = GetComponent<NavMeshAgent>() ;     
      characterStats = GetComponent<CharacterStats>();
      StopDistance = agent.stoppingDistance;
    }
    void Start(){
      //This allows the EventAttack method to be called whenever the OnEnemyClicked event is triggered on the MouseManager instance.
      MouseManager.Instance.OnMouseClicked += MoveToTarget;
      MouseManager.Instance.OnEnemyClicked += EventAttack;
      characterStats.MaxHealth = 2;
      GameManager.Instance.RigisterPlayer(characterStats);
    }
    private void Update() {
      isDead = characterStats.currentHealth==0;
      if(isDead)
        GameManager.Instance.NotifyObservers();
      SwitchAnimation();
      lastAttackTime-= Time.deltaTime;
    }
      private void SwitchAnimation(){
      anim.SetFloat("Speed", agent.velocity.sqrMagnitude);  
        //Debug.Log("Speed"+ agent.velocity.sqrMagnitude);
      anim.SetBool("Death",isDead);
     }
    public void MoveToTarget(Vector3 Target){
      StopAllCoroutines();
      if(isDead) return;
      agent.stoppingDistance = StopDistance;
      agent.isStopped = false;
    agent.destination = Target;
    }
      private void EventAttack(GameObject target)
    {
      if(isDead) return;
      if(target!=null){
       attackTarget= target;
       characterStats.isCritical = UnityEngine.Random.value<characterStats.attackData.criticalChance;
       StartCoroutine(MoveToAttackTarget());
      }  
    }
  IEnumerator MoveToAttackTarget(){
    agent.isStopped = false;
    //close
    agent.stoppingDistance=characterStats.attackData.attackRange;
    transform.LookAt(attackTarget.transform);
    //far
    while(Vector3.Distance(attackTarget.transform.position,transform.position)>characterStats.attackData.attackRange){
    agent.destination = attackTarget.transform.position;
    yield return null;
    }
    agent.isStopped = true;
    if(lastAttackTime<1){
      anim.SetBool("Critical",characterStats.isCritical);
      anim.SetTrigger("Attack");
      lastAttackTime = characterStats.attackData.coolDown;
    }
    }
    void Hit(){
      if(attackTarget.CompareTag("Attackable")&&attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing){
        if(attackTarget.GetComponent<Rock>()){
          attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
          attackTarget.GetComponent<Rigidbody>().velocity= Vector3.one;
          attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward*20, ForceMode.Impulse);
        }
      }
      else{   
      var targetStats = attackTarget.GetComponent<CharacterStats>();
      targetStats.TakeDemage(characterStats, targetStats);
      }
    }
        }
