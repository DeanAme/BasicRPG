using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
   private static T instance;
   public static T Instance
   {
    get{return instance;}
   }

   //可以在子类中继承（protected）和改写（virtual）的awake
   protected virtual void Awake(){
    if(instance!= null)
    Destroy (gameObject);
    else
    instance = (T)this;
   }
   public static bool IsInitialized{
    get{return instance!= null;}

   }
//活动中销毁被修改的方式，可以用重载
   protected virtual void OnDestory()
   {
      if(instance == this)
      instance = null;  
      
   
   }
}
