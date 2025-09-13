using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    private void Start()
    {
        
    }
    public void OnUpgradeButtonClicked()
    {
        SceneManager.LoadScene("Upgrade"); 
    }
}
