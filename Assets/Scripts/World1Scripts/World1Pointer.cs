using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World1Pointer : MonoBehaviour
{
    [SerializeField] private Transform pointer;
    [SerializeField] private Transform[] stagePositions;
    private int currentIndex = 0;

    void Start()
    {
        for (int i = 0; i < stagePositions.Length; i++)
        {
            if ($"Stage{i + 1}" == GameManager.Instance.currentStage)
            {
                currentIndex = i;
                break;
            }
        }
        MovePointer(currentIndex);
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
            GameManager.Instance.SetCurrentStage(currentIndex);

            SceneManager.LoadScene("CharacterSelect");
        }
    }

    void MovePointer(int newIndex)
    {
        int count = stagePositions.Length;
        if (count == 0) return;

        currentIndex = (newIndex + count) % count;

        Transform currentStage = stagePositions[currentIndex];

        if (currentStage.CompareTag("BossStage"))
            pointer.position = currentStage.position + new Vector3(0, 3.5f, 0);
        else if (currentStage.CompareTag("EliteStage"))
            pointer.position = currentStage.position + new Vector3(0, 2.5f, 0);
        else
            pointer.position = currentStage.position + new Vector3(0, 1f, 0);
    }
}
