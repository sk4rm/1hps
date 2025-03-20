using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopMenu : MonoBehaviour
    {
        [field: SerializeField] public Button SellWoodButton { get; private set; }
        [field: SerializeField] public Button UpgradeAxeButton { get; private set; }
        
        public void Open()
        {
            gameObject.SetActive(true);
            GameManager.Instance.UnlockCursor();
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
            GameManager.Instance.LockCursor();
        }
    }
}