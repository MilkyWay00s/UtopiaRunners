using UnityEngine;
using UnityEngine.UI;

public class LeftAnchoredSlider : MonoBehaviour
{
    public Slider slider;
    public RectTransform fill;
    public RectTransform handle;
    public RectTransform background;

    private float maxWidth;

    void Start()
    {
        // Background의 실제 폭을 기준으로 Fill 최대 길이 결정
        maxWidth = background.rect.width;
    }

    void Update()
    {
        float ratio = slider.value / slider.maxValue;

        // 1️⃣ Fill 크기 조절 (왼쪽 기준)
        fill.sizeDelta = new Vector2(maxWidth * ratio, fill.sizeDelta.y);

        // 2️⃣ Handle 위치를 Fill 끝에 맞추기
        // Fill의 로컬 위치 기준으로 오른쪽 끝으로 이동
        handle.anchoredPosition = new Vector2(fill.sizeDelta.x, handle.anchoredPosition.y);
    }
}
