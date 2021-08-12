using System.Collections;
using System.Collections.Generic;
using System.Linq;
using peroth;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool isDetected;

    [SerializeField] private Vector2 isGroundCheckCirclePos;

    [SerializeField] private int speed = 3;
    [SerializeField] private int runningSpeed = 5;

    public NPC targetNPC;

    [HideInInspector] public DialogueElements dialogues; // 대화 저장소
    [HideInInspector] public bool isUsingItem;
    private readonly Color cloackedColor = new Color(1f, 1f, 1f, 0.3f);

    private readonly Color normalColor = new Color(1f, 1f, 1f, 1f);

    private Animator anim;

    private bool isGround;

    private TextAsset jsonText;

    private TalkingManager manager;
    private bool manipulatingCam;
    private bool moving;

    private Rigidbody2D rb;
    private bool running;
    private List<SpriteRenderer> srList = new List<SpriteRenderer>();

    private CCTVEnemy targetCCTV;
    private List<CCTVEnemy> visibleCCTVList = new List<CCTVEnemy>();

    private void Start()
    {
        manager = FindObjectOfType<TalkingManager>();
        srList = GetComponentsInChildren<SpriteRenderer>().ToList();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();

        UseItem();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere( transform.position + (Vector3) isGroundCheckCirclePos, 0.07f);
    }

    private void UseItem()
    {
        #region 아이템 사용

        // 광학 미채 망토
        CloakCape();

        // 영상 조작기

        VideoManipulator();

        #endregion
    }

    private void VideoManipulator()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (isDetected) return;
            manipulatingCam = !manipulatingCam;
            isUsingItem = manipulatingCam;

            if (manipulatingCam)
            {
                if (TurnOnManipulator()) return;
            }
            else
            {
                TurnOffManipulator();
            }
        }

        if (manipulatingCam)
        {
            if (visibleCCTVList.Count <= 0) return;
            if (targetCCTV == null) targetCCTV = visibleCCTVList[0];

            Vector2 pos = targetCCTV.transform.position;
            CCTVEnemy tempCCTV = null;

            SelectCCTV(tempCCTV, pos);
        }
    }

    private bool TurnOnManipulator()
    {
        var cctvList = FindObjectsOfType<CCTVEnemy>().ToList();
        visibleCCTVList = new List<CCTVEnemy>();

        if (cctvList.Count <= 0) return true;

        foreach (var t in cctvList)
            if (Camera.main is { })
            {
                var worldPos = Camera.main.WorldToViewportPoint(t.transform.position);
                Debug.Log(t.gameObject.name + worldPos);
                if (PositionCheck(worldPos.x) && PositionCheck(worldPos.y) && worldPos.z > 0)
                    visibleCCTVList.Add(t);
            }

        Debug.Log(visibleCCTVList.Count);

        if (visibleCCTVList.Count <= 0) return true;
        targetCCTV = visibleCCTVList[0];

        var targetColor = targetCCTV.spriteRenderer.color;
        foreach (var cctv in visibleCCTVList)
        {
            var cctvSR = cctv.spriteRenderer;
            cctvSR.color = new Color(cctvSR.color.r, cctvSR.color.g, cctvSR.color.b, 0.3f);
        }

        targetCCTV.spriteRenderer.color = targetColor;
        return false;
    }

    private void TurnOffManipulator()
    {
        targetCCTV = null;
        foreach (var cctv in visibleCCTVList)
        {
            var cctvSR = cctv.spriteRenderer;
            cctvSR.color = new Color(cctvSR.color.r, cctvSR.color.g, cctvSR.color.b, 1f);
        }
    }

    private void SelectCCTV(CCTVEnemy tempCCTV, Vector2 pos)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("r");
            foreach (var cctv in visibleCCTVList)
            {
                if (tempCCTV != null)
                    if (tempCCTV.transform.position.x > cctv.transform.position.x)
                        continue;
                if (cctv.transform.position.x > pos.x) tempCCTV = cctv;
            }

            UpdateCCTV(tempCCTV);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("l");
            foreach (var cctv in visibleCCTVList)
            {
                if (tempCCTV != null)
                    if (tempCCTV.transform.position.x < cctv.transform.position.x)
                        continue;
                if (cctv.transform.position.x < pos.x) tempCCTV = cctv;
            }

            UpdateCCTV(tempCCTV);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("u");
            foreach (var cctv in visibleCCTVList)
            {
                if (tempCCTV != null)
                    if (tempCCTV.transform.position.y > cctv.transform.position.y)
                        continue;
                if (cctv.transform.position.y > pos.y) tempCCTV = cctv;
            }

            UpdateCCTV(tempCCTV);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("d");
            foreach (var cctv in visibleCCTVList)
            {
                if (tempCCTV != null)
                    if (tempCCTV.transform.position.y < cctv.transform.position.y)
                        continue;
                if (cctv.transform.position.y < pos.y) tempCCTV = cctv;
            }

            UpdateCCTV(tempCCTV);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetCCTV.StartCoroutine(targetCCTV.Neutralize());
            manipulatingCam = false;
            foreach (var cctv in visibleCCTVList)
            {
                var cctvSR = cctv.spriteRenderer;
                cctvSR.color = new Color(cctvSR.color.r, cctvSR.color.g, cctvSR.color.b, 1f);
            }

            isUsingItem = false;
            targetCCTV = null;
        }
    }

    private void CloakCape()
    {
        if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(Cloak());

        if (Input.GetKeyUp(KeyCode.W)) StartCoroutine(Uncloak());
    }

    private void Movement()
    {
        #region 이동

        isGround = Physics2D.OverlapCircle(
            (Vector2) transform.position + isGroundCheckCirclePos,
            0.07f,
            1 << LayerMask.NameToLayer("Ground"));

        if (!isUsingItem)
        {
            var axis = 0f;

            if (Input.GetKey(KeyCode.LeftArrow))
                axis = -1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                axis = 1f;
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) axis = 0f;

            // 플레이어 X축 전환
            if (axis != 0)
            {
                FlipCharacter(axis);
                moving = true;
            }
            else
            {
                moving = false;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.velocity = new Vector2(runningSpeed * axis, rb.velocity.y);
                running = moving;
            }
            else
            {
                rb.velocity = new Vector2(speed * axis, rb.velocity.y);
                running = false;
            }

            // 점프
            // anim.SetBool("jump", !isGround);
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGround) Jump();
        }
        else
        {
            moving = false;
        }

        if (running)
        {
            anim.SetBool("run", moving);
            anim.SetBool("walk", false);
        }
        else
        {
            anim.SetBool("walk", moving);
            anim.SetBool("run", false);
        }

        anim.SetBool("idle", !moving);

        #endregion
    }

    private bool PositionCheck(float position)
    {
        return position >= 0 && position <= 1;
    }

    private void UpdateCCTV(CCTVEnemy temp)
    {
        if (temp != null) targetCCTV = temp;

        var targetColor = targetCCTV.spriteRenderer.color;
        foreach (var cctv in visibleCCTVList)
        {
            var cctvSR = cctv.spriteRenderer;
            cctvSR.color = new Color(cctvSR.color.r, cctvSR.color.g, cctvSR.color.b, 0.3f);
        }

        targetCCTV.spriteRenderer.color = new Color(targetColor.r, targetColor.g, targetColor.b, 1f);
    }

    private void FlipCharacter(float axis)
    {
        if (axis == -1f) transform.rotation = Quaternion.Euler(0, 180, 0);

        if (axis == 1f) transform.rotation = Quaternion.Euler(0, 0, 0);
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
        // path = null;
    }

    public void CheckInput()
    {
        if (manager.hidingUI) return;
        if (MenuTabManager.instance.isMenuOn) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!manager.isDisplayingDialogue)
                manager.NextDialogue(); // 다음으로 넘어가기
            else
                manager.instantComplete = true;
        }
    }

    private IEnumerator Cloak()
    {
        isUsingItem = true;
        StartCoroutine(ChangeColorOverTime(normalColor, cloackedColor, 0.5f));
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator Uncloak()
    {
        StartCoroutine(ChangeColorOverTime(cloackedColor, normalColor, 0.5f));
        yield return new WaitForSeconds(0.5f);
        isUsingItem = false;
    }

    private IEnumerator ChangeColorOverTime(Color start, Color end, float duration)
    {
        for (var t = 0f; t < duration; t += Time.deltaTime)
        {
            var normalizedTime = t / duration;
            // right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            setChildrenColor(Color.Lerp(start, end, normalizedTime));
            yield return null;
        }

        setChildrenColor(end); // without this, the value will end at something like 0.9992367
    }

    private void setChildrenColor(Color color)
    {
        foreach (var sr in srList) sr.color = color;
    }
}