using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthOnAttack;
    //Add a data template
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    [HideInInspector]
    public bool isCritical;
    void Awake() {
        if(templateData!=null)
        characterData = Instantiate(templateData);
    }
    //Do not read values step by step
#region Read ftom Data_SO
    // value Represents an external assignment
    public int MaxHealth{
        get{ if(characterData != null) return characterData.MaxHealth; else return 0;}
        set{ characterData.MaxHealth = value;}}
        //return characterData?.MaxHealth ：0;
         public int currentHealth{
        get{ if(characterData != null) return characterData.currentHealth; else return 0;}
        set{ characterData.currentHealth = value;}}
         public int baseDefence{
        get{ if(characterData != null) return characterData.baseDefence; else return 0;}
        set{ characterData.baseDefence = value;}}
         public int currrentDefence{
        get{ if(characterData != null) return characterData.currrentDefence; else return 0;}
        set{ characterData.MaxHealth = value;}}
#endregion
#region Character Combat
public void TakeDemage(CharacterStats attacker, CharacterStats defender){
    int demage = Mathf.Max(attacker.CurrentDamage()-defender.currrentDefence,0);
    currentHealth = Mathf.Max(currentHealth-demage,0);
    if(attacker.isCritical){
        defender.GetComponent<Animator>().SetTrigger("Hit");
    }

    //TODO:UPDATE UI EXP
    UpdateHealthOnAttack?.Invoke(currentHealth, MaxHealth);
    if(currentHealth<=0){
        attacker.characterData.UpdateEXp(characterData.KillPoint);
    }

}
public void TakeDemage(int DemageRock, CharacterStats defender){
    int currentDamage = Mathf.Max(DemageRock - defender.currrentDefence,0);
    currentHealth = Mathf.Max(currentHealth-currentDamage,0);
    UpdateHealthOnAttack?.Invoke(currentHealth, MaxHealth);
    if(currentHealth<=0)
    GameManager.Instance.playerStats.characterData.UpdateEXp(characterData.KillPoint);
}
 
    private int CurrentDamage()
    {
        float coreDemage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);
       if(isCritical){
        coreDemage *= attackData.criticalMultiplier;
        Debug.Log("critical"+coreDemage);
       }
        return (int)coreDemage;
    }

    #endregion
}


