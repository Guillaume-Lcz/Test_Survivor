using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private PerkManager perkManager;

    private void Start()
    {
        perkManager.OnPerkChoicesReady.AddListener(ShowPerks);
        panel.SetActive(false);
    }

    private void ShowPerks()
    {
        foreach (Transform child in cardsContainer)
            Destroy(child.gameObject);

        foreach (var perk in perkManager.CurrentChoices)
        {
            var card = Instantiate(cardPrefab, cardsContainer);
            card.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = perk.perkName;
            card.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = perk.description;
            var captured = perk;
            card.GetComponent<Button>().onClick.AddListener(() => SelectPerk(captured));
        }

        panel.SetActive(true);
    }

    private void SelectPerk(PerkSO perk)
    {
        perkManager.SelectPerk(perk);
        panel.SetActive(false);
    }
}
