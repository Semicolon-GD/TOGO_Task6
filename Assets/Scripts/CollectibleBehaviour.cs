using System;
using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    private float _currentVelocity=0f;
    private float _smoothTime=0.2f;
    private Transform _currentLeadTransform;


    private void OnEnable()
    {
        EventManager.Subscribe(EventList.GameFinished, StopFollowing);
    }

    private void OnDisable()
    {
    EventManager.Unsubscribe(EventList.GameFinished, StopFollowing);
    }

    private void Update()
    {
        if (!_currentLeadTransform)
            return;
        else
        {
            transform.position=new Vector3(Mathf.SmoothDamp(transform.position.x,_currentLeadTransform.position.x,ref _currentVelocity,_smoothTime),
                transform.position.y,transform.position.z);
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
            case "Collected":
                EventManager.Trigger(EventList.OnCollectiblePickup,this.gameObject);
                break;
            case "Gate":
                EventManager.Trigger(EventList.OnObstacleHit,this.gameObject);
                break;
        }
    }
    private void StopFollowing()
    {
        _currentLeadTransform = null;
        transform.GetComponent<BoxCollider>().isTrigger = true;
    }
    public void SetLeadTransform(Transform leadTransform)
    {
        _currentLeadTransform = leadTransform;
    }
}