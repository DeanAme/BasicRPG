using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar: MonoBehaviour
{
   
    public GameObject HealthBar;
    public Transform HealthBarPoint;
    Image HealthSlide;
    Transform UIbar;
    Transform cam;
    CharacterStats currentStats;
    public bool AlwaysVisible;
    public float visibleTime;
    private float Timeleft;
    void Awake(){
    currentStats= GetComponent<CharacterStats>();
    currentStats.UpdateHealthOnAttack += UpdateHealthBar;
    }
    void OnEnable() {
        cam = Camera.main.transform;
        foreach(Canvas canvas in FindObjectsOfType<Canvas>()){
            if(canvas.renderMode == RenderMode.WorldSpace){
                UIbar = Instantiate(HealthBar, canvas.transform).transform;
                HealthSlide = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(AlwaysVisible);
                }
        }
    }
    private void UpdateHealthBar(int currentHealth, int MaxHealth){
    if(currentHealth<=0)
    Destroy(UIbar.gameObject);
    UIbar.gameObject.SetActive(true);
    Timeleft = visibleTime;
    float sliderPercent = (float)currentHealth/MaxHealth;
    HealthSlide.fillAmount = sliderPercent;
    }
    private void LateUpdate() {
        if(UIbar!=null){
            UIbar.position=HealthBarPoint.position;
            if(Timeleft<=0&&!AlwaysVisible)
            UIbar.gameObject.SetActive(false);
            else
            Timeleft-=Time.deltaTime;

            UIbar.forward = -cam.forward;
        }
    }

}
