using UnityEngine;

public class SegmentedHPBar : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Segments (filled ones)")]
    [Tooltip("왼쪽부터 오른쪽 순서로 채워지는 칸들을 넣어주세요.")]
    [SerializeField] private GameObject[] filledSegments;

    [Header("Options")]
    [Tooltip("true면 체력이 조금 남아도 최소 1칸은 보이게(0hp 제외)")]
    [SerializeField] private bool keepAtLeastOneSegmentWhenAlive = true;

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    private void Start()
    {
        if (playerHealth != null)
            HandleHealthChanged(playerHealth.CurrentHealth, playerHealth.maxHealth);
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (filledSegments == null || filledSegments.Length == 0) return;

        int segCount = filledSegments.Length;

        if (max <= 0)
        {
            SetVisibleCount(0);
            return;
        }

        // 현재 체력 비율 -> 보여줄 칸 개수
        float ratio = Mathf.Clamp01((float)current / max);

        // "한 칸씩" 느낌: 내림(Floor)
        int visible = Mathf.FloorToInt(ratio * segCount);

        // 풀피일 때는 segCount가 되게 보정
        if (current >= max) visible = segCount;

        // 살아있는데 0칸이 되는게 보기 싫으면 1칸 유지
        if (keepAtLeastOneSegmentWhenAlive && current > 0)
            visible = Mathf.Max(visible, 1);

        SetVisibleCount(visible);
    }

    private void SetVisibleCount(int visible)
    {
        visible = Mathf.Clamp(visible, 0, filledSegments.Length);

        for (int i = 0; i < filledSegments.Length; i++)
        {
            if (filledSegments[i] != null)
                filledSegments[i].SetActive(i < visible);
        }
    }
}

/*인스펙터 연결 방법

HPBar 오브젝트에 SegmentedHPBar_Icons 붙이기

PlayerHealth가 붙어있는 플레이어 오브젝트를 playerHealth 슬롯에 드래그

filledSegments 배열 크기를 “칸 개수”로 맞추고
Seg_01 ~ Seg_10 이미지를 왼쪽부터 순서대로 넣기

keepAtLeastOneSegmentWhenAlive는 취향:

true: HP가 1이라도 남으면 1칸은 보임

false: 비율이 0.09 같은 경우 0칸이 될 수도 있음*/
