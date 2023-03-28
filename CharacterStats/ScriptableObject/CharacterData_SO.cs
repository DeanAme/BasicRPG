using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{

    [Header("Stats Info")]
    public int currentHealth;
    public int MaxHealth;
    public int baseDefence;
    public int currrentDefence;
     [Header("Kill")]
    public int KillPoint;
    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;
 
    public float LevelMultipler
    {
        get{ return 1+(currentLevel-1)* levelBuff;}
    }
    public void UpdateEXp(int EnemyExpPoint){
        currentExp+=EnemyExpPoint;
        if(currentExp>=baseExp){
            LevelUp();
            
        }

    }
    private void LevelUp(){
        currentLevel = Mathf.Clamp(currentLevel+1,0,maxLevel);
        baseExp+=(int)(baseExp*LevelMultipler);
        MaxHealth= (int)(MaxHealth*LevelMultipler);
        currentHealth = MaxHealth;
        Debug.Log("currentLevel："+currentLevel+"levelHealth："+MaxHealth);
    }


    
}
