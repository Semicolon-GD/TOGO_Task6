using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI collectedCount;
    
    int _count = 0;
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
            case "Collected":
                EventManager.Trigger(EventList.OnObstacleHit);
                UpdateText();
                Debug.Log("Obstacle Hit");
                break;
        }
    }
    
    private void UpdateText()
    {
        _count++;
        collectedCount.text = _count.ToString();
    }
    
    
}