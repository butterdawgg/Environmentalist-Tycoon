using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopWindow : MonoBehaviour
{
    [SerializeField] private Structure structure;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button unlockButton;
    [SerializeField] private Button buildButton;
    [SerializeField] private Button cancelButton;

    private void Awake()
    {
        unlockButton.onClick.AddListener(OnUnlockButtonClick);
        buildButton.onClick.AddListener(OnBuildButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);

        if (unlockButton.TryGetComponent(out MenuButton unlockMenuButton))
        {
            unlockMenuButton.DefaultText = "unlock (" + structure.UnlockPrice + "RU)";
            unlockMenuButton.HighlightedText = "> unlock (" + structure.UnlockPrice + "RU) <";
        }

        if (buildButton.TryGetComponent(out MenuButton buildMenuButton))
        {
            buildMenuButton.DefaultText = "build (" + structure.BuildPrice + "CU)";
            buildMenuButton.HighlightedText = "> build (" + structure.BuildPrice + "CU) <";
        }

        nameText.text = structure.Name;

        if (structure.CurrencyUnitsPerMinute > 0f)
            infoText.text = structure.CurrencyUnitsPerMinute + "CU per minute";
        else if (structure.ResearchUnitsPerMinute > 0f)
            infoText.text = structure.ResearchUnitsPerMinute + "RU per minute";

        if (SerializeManager.GetStructureLockedState(structure.ID))
        {
            unlockButton.gameObject.SetActive(true);
            buildButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
        }
        else
        {
            unlockButton.gameObject.SetActive(false);
            buildButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(false);
        }
    }

    private void OnUnlockButtonClick()
    {
        if (SerializeManager.GetResearchUnits() >= structure.UnlockPrice)
        {
            SerializeManager.SetStructureLockedState(structure.ID, false);
            SerializeManager.SetResearchUnits(SerializeManager.GetResearchUnits() - structure.UnlockPrice);

            unlockButton.gameObject.SetActive(false);
            buildButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(false);
        }
    }

    private void OnBuildButtonClick()
    {
        if (!BoardManager.GetBuildingState() && !BoardManager.GetDemolishingState())
        {
            BoardManager.SetBuildingState(true);
            BoardManager.SetActiveStructureID(structure.ID);

            unlockButton.gameObject.SetActive(false);
            buildButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(true);
        }
    }

    private void OnCancelButtonClick()
    {
        BoardManager.SetBuildingState(false);

        unlockButton.gameObject.SetActive(false);
        buildButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
    }
}