using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "Shop Offer", menuName = "Shop/Offer", order = 1)]
    public class Offer : ScriptableObject
    {
        public List<Item> price;
        public List<Item> reward;
    }
}