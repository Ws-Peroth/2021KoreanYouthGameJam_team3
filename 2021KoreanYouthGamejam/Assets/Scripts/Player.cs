using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isDetected;

    public Rigidbody2D rb;

    // public Animator anim; // 나중에 애니메이션용으로
    public SpriteRenderer sr;

    public int speed = 4;

    [HideInInspector] public NPC targetNPC;

    [HideInInspector] public bool dialogueActive;

    public TextAsset jsonText;

    [HideInInspector] public string path;

    public DialogueElements dialogues; // 대화 저장소

    public TalkingManager manager;
    public bool isCloaking;

    private bool isGround;

    private void Start()
    {
        manager = FindObjectOfType<TalkingManager>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        isGround = Physics2D.OverlapCircle(
            (Vector2) transform.position + new Vector2(0, -1f),
            0.07f,
            1 << LayerMask.NameToLayer("Ground"));
        
        #region 이동
        
        isGround = Physics2D.OverlapCircle(
            (Vector2) transform.position + new Vector2(0, -1f),
            0.07f,
            1 << LayerMask.NameToLayer("Ground"));

        if (!isCloaking)
        {
            var axis = 0f;
            if (Input.GetKey(KeyCode.LeftArrow))
                axis = -1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                axis = 1f;
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) axis = 0f;
            rb.velocity = new Vector2(speed * axis, rb.velocity.y);

            // 플레이어 X축 전환
            if (axis != 0)
                // anim.SetBool("walk", true);
                FlipCharacter(axis);
            // else anim.SetBool("walk", false);

            // 점프

            // anim.SetBool("jump", !isGround);
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGround) Jump();
        }

        #endregion

        #region 아이템 사용

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W Down");
            Cloak();
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("W Up");
            Uncloak();
        }

        #endregion
    }

    private void FlipCharacter(float axis)
    {
        sr.flipX = axis == -1;
    }

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
        dialogues = JsonUtility.FromJson<DialogueElements>("{\"elements\":" + jsonText.text +
                                                           "}"); // 리소스를 이용해 대화를 표시하는 방법.
    }

    public void ResetTarget()
    {
        targetNPC = null;
        path = null;
    }

    public void CheckInput()
    {
        if (manager.hidingUI) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!manager.isDisplayingDialogue)
                manager.NextDialogue(); // 다음으로 넘어가기
            else
                manager.instantComplete = true;
        }
    }

    private void Cloak()
    {
        StartCoroutine(ChangeColorOverTime(sr.color, new Color(1f, 1f, 1f, 0.3f), 0.5f));
        isCloaking = true;
    }

    private void Uncloak()
    {
        StartCoroutine(ChangeColorOverTime(sr.color, new Color(1f, 1f, 1f, 1f), 0.5f));
        isCloaking = false;
    }

    private IEnumerator ChangeColorOverTime(Color start, Color end, float duration)
    {
        for (var t = 0f; t < duration; t += Time.deltaTime)
        {
            var normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            sr.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }

        sr.color = end; //without this, the value will end at something like 0.9992367
    }
}