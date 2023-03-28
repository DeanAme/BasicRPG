using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//DRAG:using UnityEngine.Events;
/* Create a hitInfo of type Raycast to hold information about the object that the ray collided with
Create a ray
control the mouse, return information about what the ground is and pass it back to the hitInfo where the information is stored
Modify the singleton model in order to mobilise some of these methods without dragging and dropping */
public class MouseManager :Singleton<MouseManager>
{   //public Raycast hitInfo;
   // public static MouseManager Instance;
    public Texture2D point, doorway, attack, target, arrow;
    RaycastHit hitInfo;
    //DRAG:public EventVector3 OnMouseClicked; 
    // Start is called before the first frame update
    public event Action <Vector3> OnMouseClicked; 
    public event Action <GameObject> OnEnemyClicked;
    protected override void Awake()
    {
        base.Awake();
    }
    void Update()
    {
        MouseControl();
        SetCursorTexture();
    }
    void SetCursorTexture(){
        //Raycast ray = Camera.main.ScreenToPoint(Input.mousePosition);
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
    if(Physics.Raycast(ray, out hitInfo))
    {
    switch(hitInfo.collider.gameObject.tag)
    { case "Ground" :
      Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);
      break;
       case "Enemy" :
      Cursor.SetCursor(attack,new Vector2(16,16),CursorMode.Auto);
      break;
      case "Portal":
      Cursor.SetCursor(doorway,new Vector2(16,16),CursorMode.Auto);
      break;
    }
    }
}
    void MouseControl(){
        if(Input.GetMouseButtonDown(0)&& hitInfo.collider != null)
        {
            if(hitInfo.collider.gameObject.CompareTag("Ground"))
            OnMouseClicked?. Invoke(hitInfo.point);
          //（public class EventVector3 : UnityEvent<Vector3> {}）
           if(hitInfo.collider.gameObject.CompareTag("Enemy"))
            OnEnemyClicked?. Invoke(hitInfo.collider.gameObject);
             if(hitInfo.collider.gameObject.CompareTag("Attackable"))
            OnEnemyClicked?. Invoke(hitInfo.collider.gameObject);
             if(hitInfo.collider.gameObject.CompareTag("Portal"))
            OnEnemyClicked?. Invoke(hitInfo.collider.gameObject);
           
            }
        }
    }

