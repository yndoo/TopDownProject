using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpSlider;

    private void Start()
    {
        UpdateHpSlider(1);
    }

    public void UpdateHpSlider(float percentage)
    {
        hpSlider.value = percentage;
    }

    public void UpdateWaveText(int wave)
    {
        waveText.text = wave.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}
