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
    public Events events; // 대화 저장소
    [HideInInspector]
    public string path;

    public DialogueElements dialogues;
    
    public TalkingManager manager;

    private void Start()
    {
        // 카메라 플레이어 따라다니게
        var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
        CM.LookAt = transform;
        CM.Follow = transform;
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
        // path = Application.dataPath + "/StreamingAssets" + "/Local/" + targetNPC.jsonName + ".json"; // 유저 패치가 있을 수도 있는 버젼의 주소
        jsonText = Resources.Load<TextAsset>("Dialogues/" + targetNPC.jsonName); // 원본 파일 (리소스)
        
        // 불러온 JSON 저장
        
        events = new Events(); // 객체 생성
        
        string readText = File.ReadAllText(path); // 주소에서 JSON 읽기
        events = (Events) JsonUtility.FromJson<Events>("{\"elements\":" + readText + "}"); // 객체에다 읽은 JSON 넣기
        
        dialogues = (DialogueElements) JsonUtility.FromJson<DialogueElements>("{\"elements\":" + jsonText.text + "}"); // 리소스를 이용해 대화를 표시하는 방법. 씀.

    }
    
    public void ResetTarget()
    {
        targetNPC = null;
        path = null;
    }
    
    public void CheckInput()
    {
        Event element = events.elements[targetNPC.posNum];
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!manager.isDisplayingDialogue)
            {
                manager.NextDialogue(); // 다음으로 넘어가기
            }
        }
        /*if (Input.GetKeyDown(KeyCode.Period))
        {
            targetNPC.posNum = 0;
            targetNPC.txtNum = 0;
            dialogueActive = false;
            manager.panel.gameObject.SetActive(false);
        }*/
        
    }
}
