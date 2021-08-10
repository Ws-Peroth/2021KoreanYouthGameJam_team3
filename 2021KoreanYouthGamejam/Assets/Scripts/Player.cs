using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    
    public Rigidbody2D rb;
    // public Animator anim; // 나중에 애니메이션용으로
    public SpriteRenderer sr;
    
    private bool isGround;
    
    public int speed = 4;
    
    [HideInInspector]
    public NPC targetNPC;
    
    [HideInInspector]
    public bool dialogueActive;
    
    public TextAsset jsonText;
    
    [HideInInspector]
    public string path;

    public DialogueElements dialogues; // 대화 저장소
    
    public TalkingManager manager;

    private void Start()
    {
        manager = FindObjectOfType<TalkingManager>();

        /*// 카메라 플레이어 따라다니게
        CinemachineVirtualCamera CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
        CM.LookAt = transform;
        CM.Follow = transform;*/
    }

    private void Update()
    {
        // 이동
        float axis = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed * axis, rb.velocity.y);
        
        // 플레이어 X축 전환
        if (axis != 0)
        {
            // anim.SetBool("walk", true);
            FlipCharacter(axis);
        }
        // else anim.SetBool("walk", false);
            
        // 점프
        isGround = Physics2D.OverlapCircle(
            (Vector2) transform.position + new Vector2(0, -0.5f), 
            0.07f, 
            1 << LayerMask.NameToLayer("Ground"));
            
        // anim.SetBool("jump", !isGround);
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround) Jump();
        
    }
    
    private void FlipCharacter(float axis) => sr.flipX = axis == -1;
    
    private void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 700);
    }
    
    public void SetTarget(NPC obj)
    {
        targetNPC = obj;
        // JSON 불러올 준비
        jsonText = Resources.Load<TextAsset>("Dialogues/" + targetNPC.jsonName); // 원본 파일 (리소스)
        
        // 불러온 JSON 저장
        dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>("{\"elements\":" + jsonText.text + "}"); // 리소스를 이용해 대화를 표시하는 방법.

    }
    
    public void ResetTarget()
    {
        targetNPC = null;
        path = null;
    }
    
    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!manager.isDisplayingDialogue)
            {
                manager.NextDialogue(); // 다음으로 넘어가기
            }
        }
    }
    
}
