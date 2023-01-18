using Controllers.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemCollectManager : MonoBehaviour
{
    [SerializeField] public int generalGemValue;
    [SerializeField] private TextMeshProUGUI generalGemValueText;
    [SerializeField] private PlayerMovementController _playerMovementController;

    private void Start()
    {
        _playerMovementController = GameObject.Find("GameRoot/LevelHolder/level0(Clone)/PlayerManager").GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        GameFinishControl();
    }

    private void GameFinishControl()
    {
        if (_playerMovementController._scoreValue <= 0)
        {
            GeneralTextManagement();
        }
    }
    private void GeneralTextManagement()
    {
        generalGemValueText.text = generalGemValue.ToString();
    }
}
