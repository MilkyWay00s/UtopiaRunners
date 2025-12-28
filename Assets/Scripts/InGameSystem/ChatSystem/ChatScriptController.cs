using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class ChatScriptController : SingletonObject<ChatScriptController>
{
    private Image chatWindow;
    private Image backGround;
    private Image characterImage;
    private Image characterImage2;
    private Image chattingImage;
    private const float CHAT_WINDOW_ALPHA = 0.8f;
    private const float SHOW_CHAT_DELAY = 0.1f;
    private TMP_Text nameText;
    private TMP_Text messageText;
    private bool isWorking = false;

    [Header("Chat Databases (Inspector Reference)")]
    [SerializeField] private ChatSpeakerData chatSpeakerData;
    [SerializeField] private BackgroundImageData backgroundImageData;
    [SerializeField] private ChatImageData chatImageData;
    [SerializeField] private MultiChatMessageData chatData;

    [SerializeField] float autoSkipTime = 3f;
    private void Awake()
    {
        chatWindow = transform.Find("ChatWindow").GetComponent<Image>();
        nameText = transform.Find("NameArea").GetComponent<TMP_Text>();
        messageText = transform.Find("MessageArea").GetComponent<TMP_Text>();
        backGround = transform.Find("BackgroundArea").GetComponent<Image>();
        characterImage = transform.Find("CharacterImageArea").GetComponent<Image>();
        characterImage2 = transform.Find("CharacterImageArea2").GetComponent<Image>();
        chattingImage = transform.Find("ChatImageArea").GetComponent<Image>();
    }
    private void Start()
    {
        StartCoroutine(ShowChat(chatData));
    }
    private IEnumerator ShowChat(MultiChatMessageData chatData)
    {
        PauseScene(true);

        yield return Open();
        foreach (var chat in chatData.chatMessages)
        {
            SetBackgroundImage(GetBackgroundImageSprite(chat.backgroundImage));
            SetChatImage(GetChatImageSprite(chat.chatImage));

            if (chat.chatImage != ChatImage.nothing)
            {
                yield return WaitRealtime(2f);
            }

            yield return MultipleChat(chat, autoSkipTime);
        }

        SetBackgroundImage(null);
        SetChatImage(null);
        SetCharacterImage(null, true);
        SetCharacterImage(null, false);
        Close();

        PauseScene(false);
    }
    public IEnumerator Open()
    {
        yield return new WaitWhile(() => isWorking);
        SetCharacterImage(null, true);
        isWorking = true;
        Color color = chatWindow.color;
        color.a = CHAT_WINDOW_ALPHA;
        chatWindow.color = color;
        color = messageText.color;
        color.a = 1;
        messageText.color = color;
        nameText.color = color;
    }
    public void Close()
    {
        Color color = chatWindow.color;
        color.a = 0;
        chatWindow.color = color;
        color = messageText.color;
        color.a = 0;
        messageText.color = color;
        nameText.color = color;
        nameText.text = "";
        messageText.text = "";
        SetCharacterImage(null, true);
        isWorking = false;
    }

    public IEnumerator MultipleChat(MultiChatMessageData.MultiChatMessage multiChatMessage, float autoSkipTime)
    {
        nameText.text = SetSpeakerName(multiChatMessage.speakerName);
        messageText.text = "";

        var speakerInfo = chatSpeakerData.ChatSpeakers.Find(info => info.speakerType == multiChatMessage.speakerName);

        if (speakerInfo.image == null)
        {
            Debug.LogWarning($"Speaker image not found: {multiChatMessage.speakerName}");
            yield break;
        }
        Sprite characterSprite = multiChatMessage.faceType == ChatSpeakerData.ChatSpeakerFace.None ? speakerInfo.image :
            speakerInfo.Faces.Find(info => info.faceType == multiChatMessage.faceType).faceImage;

        if (characterSprite == null)
        {
            characterSprite = speakerInfo.image;
        }

        SetCharacterImage(characterSprite, multiChatMessage.isLeft);

        string accumulatedText = "";

        foreach (string line in multiChatMessage.messages)
        {
            //SoundObject _soundObject = Sound.Play("EFFECT_Keyboard_Sound", true);
            yield return ShowMultipleChat(line, accumulatedText);
            //Sound.Stop(_soundObject);

            accumulatedText += line + "\n";

            float timer = Time.unscaledTime;
            yield return new WaitUntil(() => (Time.unscaledTime - timer) > autoSkipTime || Input.GetKeyDown(KeyCode.Space));
        }
    }

    private IEnumerator ShowMultipleChat(string newLine, string prefix)
    {
        string currentText = "";
        foreach (char c in newLine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentText = newLine;
                messageText.text = prefix + currentText;
                yield return WaitRealtime(SHOW_CHAT_DELAY);
                yield break;
            }
            currentText += c;
            messageText.text = prefix + currentText;
            yield return WaitRealtime(SHOW_CHAT_DELAY);
        }
    }

    public void SetBackgroundImage(Sprite sprite)
    {
        if (backGround != null)
        {
            SetImage(backGround, sprite);
        }
        else SetImage(backGround, null);
    }
    public void SetChatImage(Sprite sprite)
    {
        if (chattingImage == null) return;

        if (sprite != null)
        {
            chattingImage.sprite = sprite;
            chattingImage.color = new Color(1, 1, 1, 0);
            chattingImage.enabled = true;

            Vector3 startPos = chattingImage.rectTransform.localPosition + new Vector3(0, 50, 0);
            chattingImage.rectTransform.localPosition = startPos;

            chattingImage.rectTransform.DOLocalMoveY(startPos.y - 50, 0.5f).SetEase(Ease.OutCubic).SetUpdate(true);
            chattingImage.DOFade(1f, 0.5f).SetUpdate(true);
        }
        else
        {
            chattingImage.sprite = null;
            chattingImage.color = new Color(0, 0, 0, 0);
            chattingImage.enabled = false;
        }
    }
    public void SetCharacterImage(Sprite sprite, bool isLeft)
    {
        if (sprite == null)
        {
            SetImage(characterImage, sprite);
            SetImage(characterImage2, sprite);
            return;
        }

        if (isLeft)
        {
            SetImage(characterImage, sprite);

            if (characterImage2.enabled && characterImage2.sprite != null)
            {
                characterImage2.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }

        else
        {
            SetImage(characterImage2, sprite);

            if (characterImage.enabled && characterImage.sprite != null)
            {
                characterImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }
    }

    public string SetSpeakerName(ChatSpeakerType speakerName)
    {
        if (chatSpeakerData == null) return "";

        foreach (var info in chatSpeakerData.ChatSpeakers)
        {
            if (info.speakerType == speakerName)
            {
                if (info.speakerType == ChatSpeakerType.None) return "";
                return info.speakerName + " : ";
            }
        }
        return speakerName.ToString();
    }

    public Sprite GetBackgroundImageSprite(BackgroundImage backgroundImage)
    {
        foreach (var imageInfo in backgroundImageData.backgroundImages)
        {
            if (imageInfo.backgroundImage == backgroundImage)
            {
                return imageInfo.image;
            }
        }

        return null;
    }

    public Sprite GetChatImageSprite(ChatImage chatImage)
    {
        if (chatImageData == null) return null;
        foreach (var imageInfo in chatImageData.chatImages)
        {
            if (imageInfo.chatImage == chatImage)
            {
                return imageInfo.image;
            }
        }

        return null;
    }

    public Sprite GetCharacterImageSprite(ChatSpeakerType speakerType)
    {
        if (chatSpeakerData == null) return null;

        foreach (var imageInfo in chatSpeakerData.ChatSpeakers)
        {
            if (imageInfo.speakerType == speakerType)
                return imageInfo.image;
        }
        return null;
    }

    private static void SetImage(Image targetImage, Sprite sprite)
    {
        if (targetImage == null) return;

        if (sprite != null)
        {
            targetImage.sprite = sprite;
            targetImage.color = Color.white;
            targetImage.enabled = true;
        }
        else
        {
            targetImage.sprite = null;
            targetImage.color = new Color(0, 0, 0, 0);
            targetImage.enabled = false;
        }
    }
    //TimeScale영향 회피용, 이후 개선 필요
    private float prevTimeScale = 1f;

    private void PauseScene(bool pause)
    {
        if (pause)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = prevTimeScale;
        }
    }
    private IEnumerator WaitRealtime(float seconds)
    {
        float t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    public IEnumerator WaitRealtimePublic(float seconds) => WaitRealtime(seconds);//위 매서드 외부 접근용
}