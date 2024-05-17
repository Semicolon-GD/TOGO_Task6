using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float dragSensitive;


    private float _forwardSpeed;
    private float _dragLimit = 4.2f;
    private void OnEnable()
    {
        EventManager.Subscribe(EventList.GameStarted, StartMovement);
        EventManager.Subscribe(EventList.OnHorizontalDrag, MovePlayer);
        EventManager.Subscribe(EventList.GameFinished, () => _forwardSpeed = 0);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventList.GameStarted, StartMovement);
        EventManager.Unsubscribe(EventList.OnHorizontalDrag, MovePlayer);
        EventManager.Unsubscribe(EventList.GameFinished, () => _forwardSpeed = 0);
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
    }
    
    private void MovePlayer(float horizontal)
    {
        transform.position += Vector3.right * (horizontal * dragSensitive * Time.deltaTime);
        var playerPosition = transform.position;
        playerPosition.x = Mathf.Clamp(transform.position.x,-_dragLimit,_dragLimit);
        transform.position = playerPosition;
    }
    
    
}