using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float dragSensitive;
    [SerializeField] private Animator playerAnimator;


    private float _forwardSpeed;
    private float _dragLimit = 4.2f;
    private void OnEnable()
    {
        EventManager.Subscribe(EventList.GameStarted, StartMovement);
        EventManager.Subscribe(EventList.OnHorizontalDrag, MovePlayer);
        EventManager.Subscribe(EventList.GameFinished, () => _forwardSpeed = 0);
        EventManager.Subscribe(EventList.GameFailed, GameFailed);
        EventManager.Subscribe(EventList.GameWon,GameWon);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventList.GameStarted, StartMovement);
        EventManager.Unsubscribe(EventList.OnHorizontalDrag, MovePlayer);
        EventManager.Unsubscribe(EventList.GameFinished, () => _forwardSpeed = 0);
        EventManager.Unsubscribe(EventList.GameFailed, GameFailed);
        EventManager.Unsubscribe(EventList.GameWon,GameWon);
    }

    private void Start()
    {
        _forwardSpeed = 0;
    }

    private void Update()
    {
        transform.parent.position += Vector3.forward * (_forwardSpeed * Time.deltaTime);
    }
    
    private void StartMovement()
    {
        _forwardSpeed = playerSpeed;
        playerAnimator.SetTrigger("Run");
    }
    
    private void MovePlayer(float horizontal)
    {
        transform.position += Vector3.right * (horizontal * dragSensitive * Time.deltaTime);
        var playerPosition = transform.position;
        playerPosition.x = Mathf.Clamp(transform.position.x,-_dragLimit,_dragLimit);
        transform.position = playerPosition;
    }
    
    private void GameFailed()
    {
        playerAnimator.SetTrigger("Fail");
    }
    
    private void GameWon()
    {
        playerAnimator.SetTrigger("Win");
    }
    
    
    
    
}