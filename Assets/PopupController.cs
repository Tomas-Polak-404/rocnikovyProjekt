using UnityEngine;
using UnityEngine.UI; // Importujeme UI knihovnu pro použití Legacy Textu

public class PopupController : MonoBehaviour
{
    public GameObject popupObject;
    public Text popupText; // Legacy Text pro zobrazování zprávy
    public float popupDuration = 3f;

    private bool isActive = true;

    public void ShowPopup()
    {
        isActive = true;
        UpdatePopupText(); // Aktualizujeme text zprávy podle stavu zvuku
        Invoke("DeactivatePopup", popupDuration);
    }

    private void DeactivatePopup()
    {
        isActive = false;
    }

    private void Update()
    {
        popupObject.SetActive(isActive);
    }

    // Metoda pro aktualizaci textu zprávy podle stavu zvuku
    private void UpdatePopupText()
    {
        if (ButtonsFces.muteMusic)
        {
            popupText.text = "Zvuk byl vypnut";
        }
        else
        {
            popupText.text = "Zvuk je zapnut";
        }
    }
}
