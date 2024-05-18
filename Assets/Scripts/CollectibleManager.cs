using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private GameObject stackParent;
    [SerializeField] private GameObject stackPoint;
    [SerializeField] private GameObject player;
    
    
    
    private List<GameObject> _stackList = new List<GameObject>();
   private void OnEnable()
   {
     EventManager.Subscribe(EventList.OnCollectiblePickup, StackCollectible);
     EventManager.Subscribe(EventList.OnObstacleHit, RemoveStack);
     EventManager.Subscribe(EventList.GameFinished, CalculateScore);
   }
   

   private void OnDisable()
   {
     EventManager.Unsubscribe(EventList.OnCollectiblePickup, StackCollectible);
     EventManager.Unsubscribe(EventList.OnObstacleHit, RemoveStack);
     EventManager.Unsubscribe(EventList.GameFinished, CalculateScore);
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

       switch (_stackList.Count)
       {
           case 1:
               _stackList[0].GetComponent<CollectibleBehaviour>().SetLeadTransform(player.transform);
               break;
           case > 1:
               _stackList[^1].GetComponent<CollectibleBehaviour>().SetLeadTransform(_stackList[^2].transform);
               break;
       }
   }
   
   private void RemoveStack()
   {
       switch (_stackList.Count)
       {
           case >= 1:
           {
               var lostCollectible = _stackList[^1];
               _stackList.Remove(lostCollectible);
               Destroy(lostCollectible);
               break;
           }
           case 0:
              EventManager.Trigger(EventList.GameFailed);
               break;
       }
       stackPoint.transform.localPosition -= Vector3.forward * 2;
   }
   
   private void CalculateScore()
   {
       float score = _stackList.Count;
       if (score>0)
       {
           EventManager.Trigger(EventList.GameWon, score);
       }
       else
       {
              EventManager.Trigger(EventList.GameFailed);
       }
         
   }
  
}