using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotManager : MonoBehaviour
{
    public GameObject SaveSlotPanel; 
    private string saveFolder;
    private int slotCount = 3;

    [Header("슬롯 텍스트")] 
    public TMP_Text[] coinText;      
    public TMP_Text[] playtimeTexts;   

    private void Awake()
    {
        saveFolder = Application.persistentDataPath;
    }

    public void PopulateSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            string path = Path.Combine(saveFolder, $"save{i + 1}.json");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                coinText[i].text = $"Coins: {data.coin}";

                int hours = (int)(data.playTime / 3600);
                int minutes = (int)((data.playTime % 3600) / 60);
                int seconds = (int)(data.playTime % 60);
                playtimeTexts[i].text = $"Playtime: {hours:00}:{minutes:00}:{seconds:00}";
            }
            else
            {
                coinText[i].text = "Coin: 0";
                playtimeTexts[i].text = "Playtime: 00:00:00";
            }
        }
    }

    public void OnSelectSlot(int slotNumber)
    {
        GameManager.Instance.currentSlot = slotNumber; 
        GameManager.Instance.LoadGame(slotNumber);
        SceneManager.LoadScene("1_WorldMap");
    }


    public void OnStartButtonClicked()
    {
        SaveSlotPanel.SetActive(true);
        PopulateSlots(slotCount); 
    }

    public void OnBackButtonClicked()
    {
        SaveSlotPanel.SetActive(false);
    }

    public void OnClickLoadMostRecent()
    {
        GameManager.Instance.LoadMostRecent();
        SceneManager.LoadScene("1_WorldMap");
    }
}
