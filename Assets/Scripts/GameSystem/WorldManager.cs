using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public Camera cam;
    public TMP_Text worldNameText;
    public TMP_Text worldDescriptionText;

    private string selectedWorldSceneName;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                WorldCollider area = hit.collider.GetComponent<WorldCollider>();
                if (area != null)
                {
                    worldNameText.text = area.worldName;
                    worldDescriptionText.text = area.worldDescription;
                    selectedWorldSceneName = area.sceneName;
                }
            }
        }
    }

    public void onGoButtonClicked()
    {
        if (!string.IsNullOrEmpty(selectedWorldSceneName))
        {
            SceneManager.LoadScene(selectedWorldSceneName);
        }
    }
}
