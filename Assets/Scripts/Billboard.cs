using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(
            transform.position + mainCamera.transform.forward, 
            mainCamera.transform.up
        );
    }
}