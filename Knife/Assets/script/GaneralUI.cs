using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaneralUI : MonoBehaviour
{
    public static GaneralUI Instance;

    [Header(header: "Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject shopPanel;

    [Header(header: "UI")]
    [SerializeField] private GameObject soundsOn;
    [SerializeField] private GameObject soundsOff;
    [SerializeField] private GameObject vibrateOn;
    [SerializeField] private GameObject vibrateOff;
    [SerializeField] private Text totalApple;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

   private void Update() 
    {
        totalApple.text = GameManager.Instance.TotalApple.ToString();
    }

    public void SoundsOnOff()
    {
        if (soundsOn.activeSelf)
        {
            soundsOn.SetActive(false);
            soundsOff.SetActive(true);
        }
        else
        {
            soundsOn.SetActive(true);
            soundsOff.SetActive(false);

        }
    }

    public void VibrateOnOff()
    {
        if (vibrateOn.activeSelf)
        {
            vibrateOn.SetActive(false);
            vibrateOff.SetActive(true);
        }
        else
        {
            vibrateOn.SetActive(true);
            vibrateOff.SetActive(false);

        }
    }

    public void openShop()
    {
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
