using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World1Pointer : MonoBehaviour
{
    [SerializeField] private Transform pointer;
    [SerializeField] private Transform[] stagePositions;
    private SpriteRenderer[] stageRenderers;

    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (stagePositions.Length > 0)
            pointer.position = stagePositions[currentIndex].position + new Vector3(0, 1f, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePointer(currentIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePointer(currentIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePointer(currentIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePointer(currentIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneManager.LoadScene("CharacterSelect");
        }
    }

    void MovePointer(int newIndex)
    {
        int count = stagePositions.Length;
        Transform currentStage = stagePositions[currentIndex];

        if (count == 0) return;

        currentIndex = (newIndex + count) % count;

        if (currentStage.CompareTag("BossStage"))
        {
            pointer.position = currentStage.position + new Vector3(0, 3.5f, 0);
        }

        else if (currentStage.CompareTag("EliteStage"))
        {
            pointer.position = currentStage.position + new Vector3(0, 2.5f, 0);
        }
        else 
        { 
            pointer.position = currentStage.position + new Vector3(0, 1f, 0); 
        }
    }
}
