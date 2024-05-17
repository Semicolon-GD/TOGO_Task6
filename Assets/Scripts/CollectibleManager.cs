using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private GameObject stackParent;
    [SerializeField] private GameObject stackPoint;
    
    
    
    private List<GameObject> _stackList = new List<GameObject>();
   private void OnEnable()
   {
     EventManager.Subscribe(EventList.OnCollectiblePickup, StackCollectible);
     EventManager.Subscribe(EventList.OnObstacleHit, RemoveStack);
   }

   private void OnDisable()
   {
     EventManager.Unsubscribe(EventList.OnCollectiblePickup, StackCollectible);
     EventManager.Unsubscribe(EventList.OnObstacleHit, RemoveStack);
   }

   private void StackCollectible(GameObject collectible)
   {
       if (_stackList.Contains(collectible)) 
           return;
       collectible.transform.tag = "Collected";
       collectible.transform.GetComponent<BoxCollider>().isTrigger = false;
       _stackList.Add(collectible);
       collectible.transform.SetParent(stackParent.transform, true);
       collectible.transform.localPosition = stackPoint.transform.localPosition;
       stackPoint.transform.localPosition += Vector3.forward * 2;

       if (_stackList.Count==1)
       {
           _stackList[0].GetComponent<CollectibleBehaviour>().SetLeadTransform(transform);
       }
       else if (_stackList.Count>1)
       {
           _stackList[^1].GetComponent<CollectibleBehaviour>().SetLeadTransform(_stackList[^2].transform);
       }
   }
   
   private void RemoveStack(GameObject collectible)
   {
       switch (_stackList.Count)
       {
           case >= 1:
           {
               var lostCollectible = _stackList[^1];
               lostCollectible.transform.parent = null;
               _stackList.Remove(lostCollectible);
               lostCollectible.gameObject.SetActive(false);
               break;
           }
           case 0:
               EventManager.Trigger(EventList.GameFailed);
               break;
       }
       stackPoint.transform.localPosition -= Vector3.forward * 2;
   }
}