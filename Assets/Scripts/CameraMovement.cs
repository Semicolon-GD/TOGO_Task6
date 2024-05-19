using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime;
    [SerializeField] private Transform wonLocation;
    
    private Vector3 _velocity=Vector3.zero;
    
    private Vector3 _offset;
    private void Awake()
    {
        _offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventList.GameWon, NewLocation);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventList.GameWon, NewLocation);
    }
    
    
    void NewLocation()
    {
        _offset= wonLocation.position - player.position;
        transform.position = wonLocation.position;
        transform.rotation = wonLocation.rotation;
        
    }
}
