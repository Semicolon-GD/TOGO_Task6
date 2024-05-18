using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{ 
    [SerializeField]List<Transform> finishPositions = new List<Transform>();
    [SerializeField] private Transform player;
    private void OnEnable()
    {
     EventManager.Subscribe(EventList.GameStarted, StartGame);
     EventManager.Subscribe(EventList.GameFailed, FailGame);
     EventManager.Subscribe(EventList.GameFinished, FinishGame);
     EventManager.Subscribe(EventList.GameWon,GameWon);
     
    }
    
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventList.GameStarted, StartGame);
        EventManager.Unsubscribe(EventList.GameFailed, FailGame);
        EventManager.Unsubscribe(EventList.GameFinished, FinishGame);
        EventManager.Unsubscribe(EventList.GameWon,GameWon);
        
    }

    private void GameWon(float score)
    {
        int _score = Convert.ToInt32(score);
        StartCoroutine(MoveTowardsFinishPoint(_score));
    }

    private IEnumerator MoveTowardsFinishPoint(int score)
    {
        Transform targetPosition = finishPositions[score-1];
        while (Vector3.Distance(player.transform.position, targetPosition.position) > 0.1f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition.position, 50f * Time.deltaTime);
            yield return null; // Bir sonraki frame'i bekle
        }
        Debug.Log("Player Reached Finish Point");
        
    }

    void StartGame()
    {
        Debug.Log("Game Started");
    }
    
    void FailGame()
    {
        Debug.Log("Game Failed");
    }
    
    void FinishGame()
    {
        Debug.Log("Game Finished");
    }
}