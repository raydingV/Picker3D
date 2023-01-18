using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemTriggerController : MonoBehaviour
{
    [SerializeField] private GemCollectManager _gemCollectManager;
    [SerializeField] private int _gemValue;
    [SerializeField] public TextMeshPro _gemValueText;


    private void Start()
    {
        _gemCollectManager = GameObject.Find("GameRoot/GemCollectManager").GetComponent<GemCollectManager>();
        TextManagement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collectGem(_gemValue);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            relaeseGem();
        }
    }

    private void collectGem(int amount)
    {
        _gemCollectManager.generalGemValue = amount;
    }

    private void relaeseGem()
    {
        _gemCollectManager.generalGemValue = 0;
    }

    private void TextManagement()
    {
        _gemValueText.text = _gemValue.ToString();
    }

}
