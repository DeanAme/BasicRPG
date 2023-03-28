using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text LevelText;
    Image HealthSlider;
    Image expSlider;
    void Awake(){
    LevelText = transform.GetChild(2).GetComponent<Text>();
    HealthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }
    void Update(){
        LevelText.text ="Level:  " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        updatePlayerHealth();
        updatePlayerExp();
    }
    void updatePlayerHealth(){
        //负责管理角色的是GameManager
        float sliderPercent = (float)GameManager.Instance.playerStats.currentHealth/GameManager.Instance.playerStats.MaxHealth;
        HealthSlider.fillAmount = sliderPercent;
    }
     void updatePlayerExp(){
        //负责管理角色的是GameManager
        float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentExp/GameManager.Instance.playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
   

}
