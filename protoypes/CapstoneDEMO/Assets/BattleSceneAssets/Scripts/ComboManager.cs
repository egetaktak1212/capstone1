using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI comboNumber;

    int comboCount = 0;

    int currentRank = 0;

    List<string> comboRanks = new List<string>
    {
    "C",
    "B",
    "A",
    "S",
    "SS",
    "SSS"
    };

    List<int> comboReqs = new List<int>
    {
    5,
    8,
    10,
    10,
    10
    };

    int hitsPerRank = 0;

    public List<Image> comboSprites;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        updateVisuals();

    }

    public void noteHappened(bool pressed)
    {
        if (pressed)
        {
            addCombo();
        }
        else
        {
            resetCombo();
        }
        updateVisuals();
    }


    void updateVisuals()
    {
        comboText.text = comboRanks[currentRank].ToString();
        comboNumber.text = comboCount.ToString();
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void addCombo() { 
        comboCount++;

        if (currentRank == comboRanks.Count - 1) {
            return;
        }

        hitsPerRank++;

        comboSprites[currentRank].fillAmount = (float) hitsPerRank / (float) comboReqs[currentRank];

        if (hitsPerRank >= comboReqs[currentRank]) {
            comboSprites[currentRank].fillAmount = 1f;

            currentRank++;
            currentRank = Math.Clamp(currentRank, 0, comboRanks.Count - 1);
            PlayerInfo.instance.setComboRank(currentRank);
            hitsPerRank = 0;
        }


    }

    void resetCombo()
    {
        comboCount = 0;
        hitsPerRank = 0;
        currentRank = 0;
        PlayerInfo.instance.setComboRank(currentRank);

        foreach (Image image in comboSprites)
        {
            image.fillAmount = 0;
        }
    }


}
