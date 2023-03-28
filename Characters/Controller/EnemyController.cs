using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates{GUARD,PATROL,CHASE,DEAD};
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemyStates;
    protected CharacterStats characterStats;
    private NavMeshAgent agent;
    private Animator anim;
    private Collider coll;

    [Header("Basic Settings")]
    public float sightRadius;
    protected GameObject AttackTarget;
    public bool isGuard; 
    private float speed;
    public float lookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;
    private Quaternion guardRotation;
     [Header("Patrol State")]
     private Vector3 wayPoint;
     private Vector3 guardPos;
    public float patrolRange;
    bool isFollow;
    bool isChase;
    bool isWalk;
    bool isDead;
    bool playerDead;
    void Awake() {
      agent = GetComponent<NavMeshAgent>();  
      anim = GetComponent<Animator>();
      speed = agent.speed;
      guardPos= transform.position ;
      guardRotation = transform.rotation;
      remainLookAtTime = lookAtTime;
      characterStats = GetComponent<CharacterStats>();
      coll = GetComponent<Collider>();
    }
   void Start() {
      if(isGuard){
        enemyStates =  EnemyStates.GUARD;
      }
      else{
        enemyStates =  EnemyStates.PATROL;
        GetNewWayPoint();
      }

       GameManager.Instance.AddObserver(this);
    }
    void OnDisable() {
      if(!GameManager.IsInitialized) return;
       GameManager.Instance.RemoveObserver(this);
    }
    void Update() {
      if(characterStats.currentHealth == 0)
      isDead = true;
      if(!playerDead){
       SwitchStates();  
       SwitchAnim();

       lastAttackTime-=Time.deltaTime;
       }
    }
    void SwitchAnim(){
    anim.SetBool("Walk", isWalk);
    anim.SetBool("Follow", isFollow);
    anim.SetBool("Chase", isChase);
    anim.SetBool("Critical", characterStats.isCritical);
    anim.SetBool("Death",isDead);

    }
    void SwitchStates(){
      if(isDead)
      enemyStates = EnemyStates.DEAD;
      else if(FoundPlayer()){
       enemyStates = EnemyStates.CHASE;
        Debug.Log("Finding player");
      }
        switch(enemyStates){
         case EnemyStates.GUARD:
         isChase = false;
         if(transform.position!=guardPos){
          isWalk = true;
          agent.isStopped = false;
          agent.destination = guardPos;
          if(Vector3.SqrMagnitude(guardPos-transform.position)<=agent.stoppingDistance)
          isWalk = false;
          transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.01f);
         }
         break;
         case EnemyStates.PATROL:
         isChase = false;
         agent.speed = speed * 0.5f;
          //Determining whether the object has reached a patrol point
        if(Vector3.Distance(wayPoint,transform.position)<=agent.stoppingDistance){
        isWalk = false;
        //The enemy patrols at regular intervals.
        if(remainLookAtTime >0){
            remainLookAtTime-=Time.deltaTime;}
        else{GetNewWayPoint();}}
        else{
          isWalk = true;
          agent.destination = wayPoint;}
        break;
         case EnemyStates.CHASE:
         isWalk = false;
         isChase = true;
         agent.speed =speed;
         if(!FoundPlayer())
         {
          isFollow =false;
          if(remainLookAtTime>0){
            agent.destination = transform.position;
             remainLookAtTime-=Time.deltaTime; }
          else if(isGuard)
          enemyStates = EnemyStates.GUARD;
          else
          enemyStates = EnemyStates.PATROL;
         }
         else{
        isFollow = true;
         agent.isStopped = false; 
        agent.destination= AttackTarget.transform.position;
         }
        if(TargetInAttackRange()||TargetInSkillRange()){
          isFollow = false;
          agent.isStopped = true;
          if(lastAttackTime<0){
            lastAttackTime = characterStats.attackData.coolDown;
            characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
          Attack();
          }
        }
         break;
         case EnemyStates.DEAD:
         coll.enabled = false;
          agent.enabled = false;
         agent.radius = 0;
         Destroy(gameObject, 2f);
         break;
        }
    }
    void Attack(){
          transform.LookAt(AttackTarget.transform);
          if(TargetInAttackRange()){
            anim.SetBool("attack", true);
            Debug.Log("shuxing"+ anim.GetBool("attack"));
          }
          if(TargetInSkillRange()){
            anim.SetTrigger("Skill");
          }
        }
    bool FoundPlayer(){
      //Collider[] Returns an array with all colliders touching or inside the sphere.
          var colliders = Physics.OverlapSphere(transform.position,sightRadius);
          foreach(var target in colliders){
            if(target.CompareTag("Player")){
              AttackTarget = target.gameObject;
              return true;
            }
          }
          AttackTarget = null;
          return false;
        }
       private bool TargetInAttackRange(){
          if(AttackTarget!=null){
           Debug.Log(Vector3.Distance(AttackTarget.transform.position,transform.position)<=characterStats.attackData.attackRange); 
 return Vector3.Distance(AttackTarget.transform.position,transform.position)<=characterStats.attackData.attackRange;

          }
          else
          return false;
        }
        bool TargetInSkillRange(){
           if(AttackTarget!=null)
          return Vector3.Distance(AttackTarget.transform.position,transform.position)<=characterStats.attackData.skillRange;
          else
          return false;

        }
        //patrol
  void GetNewWayPoint(){
    remainLookAtTime = lookAtTime ;
    float randomX = Random.Range(-patrolRange, patrolRange);
    float randomZ = Random.Range(-patrolRange, patrolRange);
    Vector3 randomPoint = new Vector3(guardPos.x+randomX,transform.position.y,guardPos.z+randomZ);
     wayPoint = randomPoint;
    NavMeshHit hit;
    wayPoint = NavMesh.SamplePosition(randomPoint, out hit,patrolRange,1)?hit.position: transform.position;
        
  }
        private void OnDrawGizmosSelected() {
          Gizmos.color = Color.blue;
          Gizmos.DrawWireSphere(transform.position,sightRadius);
        }
         Animation Event;

        void Hit(){
          if(AttackTarget!= null&& transform.IsFacingTarget(AttackTarget.transform)){
            var targetStats = AttackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDemage(characterStats,targetStats);
          }
        }
        public void EndNotify(){
       anim.SetBool("Win",true);
       playerDead = true;
       isChase= false;
       isWalk= false;
       AttackTarget= null;
        }


        
}
