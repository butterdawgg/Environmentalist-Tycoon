using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject powerStationsTab;
    [SerializeField] private GameObject researchStationsTab;
    [SerializeField] private Button tabSwitchLeftButton;
    [SerializeField] private Button tabSwitchRightButton;
    [SerializeField] private Button demolishButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI currencyUnitsText;
    [SerializeField] private TextMeshProUGUI researchUnitsText;

    private void Awake()
    {
        tabSwitchLeftButton.onClick.AddListener(OnTabSwitchLeftButtonClick);
        tabSwitchRightButton.onClick.AddListener(OnTabSwitchRightButtonClick);
        demolishButton.onClick.AddListener(OnDemolishButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);

        powerStationsTab.SetActive(true);
        researchStationsTab.SetActive(false);

        demolishButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        currencyUnitsText.text = (int)Mathf.Floor(SerializeManager.GetCurrencyUnits()) + " CU";
        researchUnitsText.text = (int)Mathf.Floor(SerializeManager.GetResearchUnits()) + " RU";
    }

    private void OnTabSwitchLeftButtonClick()
    {
        if (powerStationsTab.activeSelf)
        {
            powerStationsTab.SetActive(false);
            researchStationsTab.SetActive(true);
        }
        else if (researchStationsTab.activeSelf)
        {
            powerStationsTab.SetActive(true);
            researchStationsTab.SetActive(false);
        }
    }

    private void OnTabSwitchRightButtonClick()
    {
        if (powerStationsTab.activeSelf)
        {
            powerStationsTab.SetActive(false);
            researchStationsTab.SetActive(true);
        }
        else if (researchStationsTab.activeSelf)
        {
            powerStationsTab.SetActive(true);
            researchStationsTab.SetActive(false);
        }
    }

    private void OnDemolishButtonClick()
    {
        if (!BoardManager.GetDemolishingState() && !BoardManager.GetBuildingState())
        {
            BoardManager.SetDemolishingState(true);

            demolishButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(true);
        }
    }

    private void OnCancelButtonClick()
    {
        BoardManager.SetDemolishingState(false);

        demolishButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
    }
}
