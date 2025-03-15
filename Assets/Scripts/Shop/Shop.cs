using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class Shop : MonoBehaviour
    {
        [field: SerializeField] public List<Offer> Offers { get; private set; } = new();
        [SerializeField] private Interactable interactable;

        private void OnEnable()
        {
            interactable.OnInteract += OnInteract;
        }

        private void OnDisable()
        {
            interactable.OnInteract -= OnInteract;
        }

        private void OnInteract()
        {
            print("interacted!");
        }
    }
}