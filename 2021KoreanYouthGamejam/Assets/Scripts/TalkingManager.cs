using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TalkingManager : MonoBehaviour
{
    public Player player;
    
    public RectTransform panel;
    public Text nameTxt;
    public Text textTxt;
    public Image rightImage;
    public Image leftImage;
    public Image eventIllustration;
    public List<string> charactersSpeaking = new List<string>();

    private Dictionary<string, Sprite> dialogueImages = new Dictionary<string, Sprite>();

    private string dialogueType = "conversation";
    public bool isDisplayingDialogue = false;

    private void Start()
    {
        charactersSpeaking.Add(String.Empty);
        charactersSpeaking.Add(String.Empty);
        player = FindObjectOfType<Player>();
    }

    public void StartDialogue() // 대화 시작하기
    {
        foreach (DialogueElement element in player.dialogues.elements) {
            if (!dialogueImages.ContainsKey(element.name))
            {
                dialogueImages.Add(element.name, Resources.Load<Sprite>("Images/Chat/" + element.name));
            }
        }   

        player.dialogueActive = true; // 대화 진행중 상태로 변경
        panel.gameObject.SetActive(true);
        StopCoroutine(DisplayDialogue());
        StartCoroutine(DisplayDialogue()); // 대사 출력 시작
    }

    private IEnumerator DisplayDialogue() // 대사 출력
    {
        isDisplayingDialogue = true;
        textTxt.text = ""; // 값 리셋
        nameTxt.text = player.dialogues.elements[player.targetNPC.posNum].name; // 이름 출력
        if (player.dialogues.elements[player.targetNPC.posNum].type != null)
        {
            dialogueType = player.dialogues.elements[player.targetNPC.posNum].type;
        }

        SetDisplayedCharacters();
        SetDialogueAlign();

        if (dialogueType == "event")
        {
            leftImage.gameObject.SetActive(false);
            rightImage.gameObject.SetActive(false);
            try
            {
                leftImage.gameObject.SetActive(false);
                rightImage.gameObject.SetActive(false);
                Sprite illustration = Resources.Load<Sprite>("Images/Illustrations/" + player.dialogues.elements[player.targetNPC.posNum].image);
                if (illustration != null)
                {
                    eventIllustration.sprite = illustration;
                }
                eventIllustration.gameObject.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
        else
        {
            eventIllustration.gameObject.SetActive(false);
            // 이미지 교체
            if (dialogueImages.ContainsKey(charactersSpeaking[0]))
            {
                leftImage.sprite = dialogueImages[charactersSpeaking[0]];
                leftImage.gameObject.SetActive(true);
            }
            else
            {
                leftImage.gameObject.SetActive(false);
            }
            if (dialogueImages.ContainsKey(charactersSpeaking[1]))
            {
                rightImage.sprite = dialogueImages[charactersSpeaking[1]];
                rightImage.gameObject.SetActive(true);
            }
            else
            {
                rightImage.gameObject.SetActive(false);
            }
        }

        // 출력
        for (int i = 0; i < player.dialogues.elements[player.targetNPC.posNum].txt[player.targetNPC.txtNum].Length; i++) // 내용을 차례대로 출력
        {
            yield return new WaitForSeconds(0.05f); // 뜸 주고
            textTxt.text += player.dialogues.elements[player.targetNPC.posNum].txt[player.targetNPC.txtNum][i].ToString(); // 한글자 출력하고
            if (player.targetNPC.voice) player.targetNPC.voice.Play(); // 목소리 있으면 재생하고
        }
        isDisplayingDialogue = false;
    }

    public void NextDialogue() // 다음으로 넘어가기
    {
        if (player.targetNPC.txtNum != player.dialogues.elements[player.targetNPC.posNum].txt.Length-1) // 대사가 남아 있을 때
        {
            player.targetNPC.txtNum += 1; // 대사 진행도 1 올리고
            StopCoroutine(DisplayDialogue());
            StartCoroutine(DisplayDialogue()); // 대사 출력
        }
        else // 대사가 더 없을 때
        {
            if (player.targetNPC.posNum == player.dialogues.elements.Length-1) // 대사의 end 값이 true 일때, 또는 대화의 끝에 도달했을 때
            {
                player.targetNPC.posNum = 0;
                player.targetNPC.txtNum = 0;
                player.dialogueActive = false;
                panel.gameObject.SetActive(false); // 대화를 끝낸다
            }
            else // 대사의 end 값이 false 일때
            {
                player.targetNPC.txtNum = 0;
                player.targetNPC.posNum += 1;
                StartDialogue(); // 다음 대화를 시작한다
            }
        }
    }

    private void SetDisplayedCharacters()
    {
        if (charactersSpeaking.Contains(player.dialogues.elements[player.targetNPC.posNum].name))
        {
            return;
        }
        else
        {
            if (charactersSpeaking.Contains(player.dialogues.elements[player.targetNPC.posNum + 1].name))
            {
                int pos = charactersSpeaking.IndexOf(player.dialogues.elements[player.targetNPC.posNum + 1].name);
                switch (pos)
                {
                    case 0:
                        charactersSpeaking[1] = player.dialogues.elements[player.targetNPC.posNum].name;
                        break;
                    case 1:
                        charactersSpeaking[0] = player.dialogues.elements[player.targetNPC.posNum].name;
                        break;
                    default:
                        charactersSpeaking[0] = player.dialogues.elements[player.targetNPC.posNum].name;
                        break;
                }
            }
            else
            {
                int pos = charactersSpeaking.IndexOf(player.dialogues.elements[player.targetNPC.posNum - 1].name);
                switch (pos)
                {
                    case 0:
                        charactersSpeaking[1] = player.dialogues.elements[player.targetNPC.posNum].name;
                        break;
                    case 1:
                        charactersSpeaking[0] = player.dialogues.elements[player.targetNPC.posNum].name;
                        break;
                    default:
                        charactersSpeaking[0] = player.dialogues.elements[player.targetNPC.posNum].name;
                        break;
                }
            }
        }
    }

    private void SetDialogueAlign()
    {
        if (charactersSpeaking[0] == player.dialogues.elements[player.targetNPC.posNum].name)
        {
            nameTxt.alignment = TextAnchor.UpperLeft;
            textTxt.alignment = TextAnchor.UpperLeft;
        }
        else if (charactersSpeaking[1] == player.dialogues.elements[player.targetNPC.posNum].name)
        {
            nameTxt.alignment = TextAnchor.UpperRight;
            textTxt.alignment = TextAnchor.UpperRight;
        }
    }
}