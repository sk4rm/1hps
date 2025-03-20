using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shop
{
    public class Shop : MonoBehaviour
    {
        [field: SerializeField] public List<Offer> Offers { get; private set; } = new();
        [field: SerializeField] public ShopMenu Menu { get; private set; }
        [field: SerializeField] public int MoneyPerWood { get; private set; }
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
            Menu.Open();
            PlayerInputManager.Instance.Actions.UI.Cancel.performed += CloseUI;
        }

        private void CloseUI(InputAction.CallbackContext _)
        {
            Menu.Close();
            PlayerInputManager.Instance.Actions.UI.Cancel.performed -= CloseUI;
        }
    }
}