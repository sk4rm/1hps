using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopMenu : MonoBehaviour
    {
        [field: SerializeField] public Button SellWoodButton { get; private set; }
        [field: SerializeField] public Button UpgradeAxeButton { get; private set; }

        public event Action OnOpen;
        public event Action OnClose;
        
        public void Open()
        {
            gameObject.SetActive(true);
            GameManager.Instance.UnlockCursor();
            PlayerInputManager.Instance.Actions.Player.Disable();
            OnOpen?.Invoke();
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
            GameManager.Instance.LockCursor();
            PlayerInputManager.Instance.Actions.Player.Enable();
            OnClose?.Invoke();
        }
    }
}