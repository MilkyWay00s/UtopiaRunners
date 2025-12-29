using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiChatMessageData", menuName = "ScriptableObject/MultiChatMessageData", order = 1)]
public class MultiChatMessageData : ScriptableObject
{
    [Serializable]
    public class MultiChatMessage
    {
        public ChatSpeakerType speakerName;
        public ChatSpeakerData.ChatSpeakerFace faceType = ChatSpeakerData.ChatSpeakerFace.None;
        public bool isLeft;
        public List<string> messages;
        public BackgroundImage backgroundImage;
        public ChatImage chatImage;
    }

    public string bgmName;
    public List<MultiChatMessage> chatMessages;

    public IEnumerator Play()
    {
        if (bgmName != null && bgmName.Length != 0)
        {
            //BgmManager.BgmStack.Push(bgmName);
        }

        yield return ChatScriptController.Instance.Open();
        foreach (var chat in chatMessages)
        {
            //SoundObject _soundObject = Sound.Play("EFFECT_Chat_Sound", true);
            yield return ChatScriptController.Instance.WaitRealtimePublic(0.2f);

            ChatScriptController.Instance.SetBackgroundImage(ChatScriptController.Instance.GetBackgroundImageSprite(chat.backgroundImage));
            ChatScriptController.Instance.SetChatImage(ChatScriptController.Instance.GetChatImageSprite(chat.chatImage));

            //Sound.Stop(_soundObject);
            yield return ChatScriptController.Instance.MultipleChat(chat, 3f);
        }
        ChatScriptController.Instance.Close();
        if (bgmName != null && bgmName.Length != 0)
        {
            //BgmManager.BgmStack.Pop();
        }
    }
}
