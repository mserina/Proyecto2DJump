using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCount : MonoBehaviour
{
    [SerializeField] private int maxNumberOfItems = 3;
    
    [SerializeField] private Button yellowButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button redButton;
    
    [SerializeField] private TextMeshProUGUI yellowText;
    [SerializeField] private TextMeshProUGUI blueText;
    [SerializeField] private TextMeshProUGUI redText;
    
    private Dictionary<ItemType, int> _numItems = new();
    private Dictionary<ItemType, TextMeshProUGUI> _itemTexts = new();

    private void Start()
    {
        if (yellowText == null || blueText == null || redText == null)
        {
            yellowText = yellowButton.GetComponentInChildren<TextMeshProUGUI>();
            blueText = blueButton.GetComponentInChildren<TextMeshProUGUI>();
            redText = redButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        _itemTexts[ItemType.Yellow] = yellowText;
        _itemTexts[ItemType.Blue] = blueText;
        _itemTexts[ItemType.Red] = redText;

        foreach (var key in _itemTexts.Keys)
        {
            _numItems[key] = 0;
            _itemTexts[key].text = "0";
        }
    }

    public void AddItem(ItemType itemType)
    {
        _numItems[itemType]++;
        _itemTexts[itemType].text = _numItems[itemType].ToString();
    }
}