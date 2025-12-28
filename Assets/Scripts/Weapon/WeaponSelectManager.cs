using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSelectManager : MonoBehaviour
{
    public static WeaponSelectManager Instance;

    private WeaponInfo selectedWeaponInfo;
    private GameObject selectedWeaponPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "7_InGameScene")
        {
            StartCoroutine(SpawnWeapon());
        }
    }

    public void SelectWeapon(WeaponInfo info, GameObject prefab)
    {
        selectedWeaponInfo = info;
        selectedWeaponPrefab = prefab;
        Debug.Log(info.weaponName + " 선택됨");
    }

    private IEnumerator SpawnWeapon()
    {
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && selectedWeaponPrefab != null)
        {
            GameObject weapon = Instantiate(selectedWeaponPrefab, player.transform.position, player.transform.rotation);
            weapon.transform.SetParent(player.transform);

            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.currentWeapon = weapon;
            }
        }
    }

    public WeaponInfo GetSelectedWeaponInfo() => selectedWeaponInfo;

    public void onSelectButtonClicked()
    {
        SceneManager.LoadScene("3_CharacterSelect");
    }
}
