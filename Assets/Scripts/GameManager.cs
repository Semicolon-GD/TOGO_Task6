using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
{ 
    [SerializeField]List<Transform> finishPositions = new List<Transform>();
    [SerializeField] private Transform player;
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TMPro.TextMeshProUGUI resultText;
    [SerializeField] private TMPro.TextMeshProUGUI resultScore; 
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
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

    private void Start()
    {
        tutorialPanel.SetActive(true);
    }
    

    private void GameWon(float score)
    {
        int _score = Convert.ToInt32(score);
        StartCoroutine(MoveTowardsFinishPoint(_score));
        var color = resultPanel.GetComponent<Image>().color;
        color = Color.green;
        color.a = 0.5f;
        resultPanel.GetComponent<Image>().color = color;
        resultPanel.SetActive(true);
        inputPanel.SetActive(false);
        resultScore.text=score.ToString();
    }

    private IEnumerator MoveTowardsFinishPoint(int score)
    {
        Transform targetPosition = finishPositions[score-1];
        while (Vector3.Distance(player.transform.position, targetPosition.position) > 0.1f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition.position, 15f * Time.deltaTime);
            yield return null; // Bir sonraki frame'i bekle
        }
        Debug.Log("Player Reached Finish Point");
        
    }

    void StartGame()
    {
       tutorialPanel.SetActive(false);
    }
    
    void FailGame()
    {
        resultPanel.SetActive(true);
        inputPanel.SetActive(false);
        resultScore.gameObject.SetActive(false);
        resultPanel.GetComponent<Image>().color = Color.red;
        resultText.text = "Game Over";
    }
    
    void FinishGame()
    {
        Debug.Log("Game Finished");
    }
    
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}