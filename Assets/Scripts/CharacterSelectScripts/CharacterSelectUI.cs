using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    public void OnUpgradeButtonClicked()
    {
        SceneManager.LoadScene("Upgrade"); 
    }
}
