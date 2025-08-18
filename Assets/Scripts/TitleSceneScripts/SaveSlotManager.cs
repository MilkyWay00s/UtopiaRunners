using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotManager : MonoBehaviour
{
    public GameObject SaveSlotPanel;

    public void OnSelectSlot(int slotNumber)
    {
        GameManager.Instance.LoadGame(slotNumber);
        SceneManager.LoadScene("WorldMap");
    }

    public void OnStartButtonClicked()
    {
        SaveSlotPanel.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        SaveSlotPanel.SetActive(false);
    }

    public void OnClickLoadMostRecent()
    {
        GameManager.Instance.LoadMostRecent();
        UnityEngine.SceneManagement.SceneManager.LoadScene("WorldMap");
    }
}
