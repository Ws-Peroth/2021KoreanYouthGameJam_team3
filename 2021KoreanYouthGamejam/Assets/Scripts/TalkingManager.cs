using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TalkingManager : MonoBehaviour
{
    public Player player;
    
    public RectTransform panel;
    public Text nameTxt;
    public Text textTxt;
    
    public bool isDisplayingDialogue = false;
    
    public void StartDialogue() // 대화 시작하기
    {
        player.dialogueActive = true; // 대화 진행중 상태로 변경
        panel.gameObject.SetActive(true);
        StopCoroutine(DisplayDialogue());
        StartCoroutine(DisplayDialogue()); // 대사 출력 시작
    }

    private IEnumerator DisplayDialogue() // 대사 출력
    {
        isDisplayingDialogue = true;
        // nameTxt.text = "";
        textTxt.text = ""; // 값 리셋
        nameTxt.text = player.events.elements[player.targetNPC.posNum].name; // 이름 출력

        for (int i = 0; i < player.events.elements[player.targetNPC.posNum].text[player.targetNPC.txtNum].Length; i++) // 내용을 차례대로 출력
        {
            yield return new WaitForSeconds(0.05f); // 뜸 주고
            textTxt.text += player.events.elements[player.targetNPC.posNum].text[player.targetNPC.txtNum][i].ToString(); // 한글자 출력하고
            if (player.targetNPC.voice) player.targetNPC.voice.Play(); // 목소리 있으면 재생하고
        }
        isDisplayingDialogue = false;
    }

    public void NextDialogue() // 다음으로 넘어가기
    {
        if (player.targetNPC.txtNum != player.events.elements[player.targetNPC.posNum].text.Length-1) // 대사가 남아 있을 때
        {
            player.targetNPC.txtNum += 1; // 대사 진행도 1 올리고
            StopCoroutine(DisplayDialogue());
            StartCoroutine(DisplayDialogue()); // 대사 출력
        }
        else // 대사가 더 없을 때
        {
            if (player.targetNPC.posNum == player.events.elements.Length-1) // 대사의 end 값이 true 일때, 또는 대화의 끝에 도달했을 때
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
}