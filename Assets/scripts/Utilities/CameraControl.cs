using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO cameraShakeEvent;

    public VoidEventSO afterSceneLoadedEvent;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
        // Debug.Log(confiner2D.gameObject.name);
        // Debug.Log(">");
    }
    private void Start()
    {
        GetNewCameraBound();
    }
    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised += OnLoadedSceneEvent;
    }

    private void OnLoadedSceneEvent()
    {
        GetNewCameraBound();
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnLoadedSceneEvent;
    }
    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }
    public void GetNewCameraBound()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        // Debug.Log(confiner2D.gameObject.name);
        // Debug.Log(obj.name);
        // Debug.Log("A~A~");
        if (obj == null)
            return;
        // confiner2D.InvalidateCache();
        // Debug.Log(confiner2D.gameObject.name);
        // Debug.Log("O!");
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}
