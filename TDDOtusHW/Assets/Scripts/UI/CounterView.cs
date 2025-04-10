using System;
using TMPro;
using UnityEngine;

namespace ATG.OtusHW.Inventory.UI
{
    [Serializable]
    public sealed class CounterView
    {
        [SerializeField] private CanvasGroup counterRoot;
        [SerializeField] private TMP_Text output;

        public void SetActive(bool isActive)
        {
            float alpha = isActive ? 1f : 0f;
            counterRoot.alpha = alpha;
            counterRoot.interactable = isActive;
        }

        public void UpdateCount(int cur, int max)
        {
            output.text = $"{cur}/{max}";
        }
    }
}