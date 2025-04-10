using UnityEngine;

namespace ATG.Items
{
    [CreateAssetMenu(fileName = "item config", menuName = "Configs/New Item Config", order = 0)]
    public class ItemConfig : ScriptableObject
    {
        public Item Prototype;
    }
}