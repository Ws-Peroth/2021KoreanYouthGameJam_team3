using System.Collections;
using System.Collections.Generic;
using System.Linq;
using peroth;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] public bool isDebug = false;
     public bool isDetected;

    [SerializeField] private Vector2 isGroundCheckCirclePos;

    [SerializeField] private int speed = 3;
    [SerializeField] private int runningSpeed = 5;

    public NPC targetNPC;

    [HideInInspector] public DialogueElements dialogues; // 대화 저장소
    [HideInInspector] public bool isUsingItem;
    [HideInInspector] public bool isCloaked;

    [SerializeField] private GameObject targetIndicator;
    private readonly Color cloackedColor = new Color(1f, 1f, 1f, 0.3f);

    private readonly Color normalColor = new Color(1f, 1f, 1f, 1f);

    private Animator anim;

    [SerializeField] private bool isGround;

    private TextAsset jsonText;

    private bool manipulatingCam;
    private bool moving;
    private bool canManupulateCam = true;

    private Rigidbody2D rb;
    private bool running;
    private List<SpriteRenderer> srList = new List<SpriteRenderer>();
    private CCTVEnemy targetCCTV = null;

    private List<CCTVEnemy> visibleCCTVList = new List<CCTVEnemy>();

    private List<CCTVEnemy> cctvList;
    public GameObject cloakParticle;

    [SerializeField] public List<CCTVListNode> XposCCTVList = new List<CCTVListNode>();
    [SerializeField] public List<CCTVListNode> YposCCTVList = new List<CCTVListNode>();
    public int targetCode;
    #endregion

    #region Player

    private void Start()
    {
        srList = GetComponentsInChildren<SpriteRenderer>().ToList();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cctvList = FindObjectsOfType<CCTVEnemy>().ToList();

        manipulatingCam = false;
        isUsingItem = false;
        isCloaked = false;

        if (targetIndicator == null) targetIndicator = GameObject.Find("targetIndicator");
    }

    private void Update()
    {
        if (MenuTabManager.instance.isMenuOn) return;

        if (!isGround && isUsingItem)
        {
            if (isCloaked)
            {
                StartCoroutine(Uncloak());
            }

            if (manipulatingCam)
            {
                manipulatingCam = (!manipulatingCam);
                isUsingItem = isCloaked || manipulatingCam;
                TurnOffManipulator();
            }
        }
        
        Movement();

        UseItem();
    }

    private void Movement()
    {
        #region 이동

        isGround = Physics2D.OverlapCircle(
            (Vector2) transform.position + isGroundCheckCirclePos,
            0.07f,
            1 << LayerMask.NameToLayer("Ground"));

        if (TalkingManager.instance.isTalking || isDetected)
        {
            rb.velocity = Vector2.zero;
            return;
        }

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
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (Vector3) isGroundCheckCirclePos, 0.07f);
    }

    private void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 738);
    }

    private bool PositionCheck(float position)
    {
        return position >= 0 && position <= 1;
    }

    private void FlipCharacter(float axis)
    {
        if (axis == -1f) transform.rotation = Quaternion.Euler(0, 180, 0);

        if (axis == 1f) transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    #endregion

    #region Item

    private void UseItem()
    {
        #region 아이템 사용

        if (!isGround) return;
        
        if (TalkingManager.instance.isTalking)
        {
            return;
        }

        if (InventoryManager.instance.GetItemHavingCount(ItemCode.ItemA) >= 1 || isDebug)
        {
            // 광학 미채 망토
            CloakCape();
        }

        // 영상 조작기

        if (InventoryManager.instance.GetItemHavingCount(ItemCode.ItemB) >= 1 || isDebug)
        {
            VideoManipulator();
        }

        #endregion
    }

    private IEnumerator Cloak()
    {
        isUsingItem = true;
        rb.velocity = Vector2.zero;
        StartCoroutine(ChangeColorOverTime(normalColor, cloackedColor, 0.2f));
        cloakParticle.SetActive(true);
        // yield return new WaitForSeconds(0.2f);
        isCloaked = true;
        yield return null;
    }

    private IEnumerator Uncloak()
    {
        isCloaked = false;
        cloakParticle.SetActive(false);
        StartCoroutine(ChangeColorOverTime(cloackedColor, normalColor, 0.2f));
        // yield return new WaitForSeconds(0.2f);
        isUsingItem = isCloaked || manipulatingCam;
        yield return null;
    }

    private void SelectCCTV(CCTVEnemy tempCCTV, Vector2 pos)
    {
        #region MoveCam
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int index = CodeToXIndex(targetCode);
            Debug.Log($"RA : idx = {index}");

            if (index == -1) return;

            if (index + 1 < XposCCTVList.Count)
            {
                index++;
                tempCCTV = XposCCTVList[index].cctv;
                targetCode = XposIndexToCode(index);
                Debug.Log($"RA : new idx = {index}");

                UpdateCCTV(tempCCTV);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int index = CodeToXIndex(targetCode);
            Debug.Log($"LA : idx = {index}");

            if (index == -1) return;
            if (index - 1 >= 0)
            {
                tempCCTV = XposCCTVList[--index].cctv;
                targetCode = XposIndexToCode(index);
                Debug.Log($"LA : new idx = {index}");

                UpdateCCTV(tempCCTV);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int index = CodeToYIndex(targetCode);
            Debug.Log($"UA : idx = {index}");

            if (index == -1) return;
            if (index + 1 < YposCCTVList.Count)
            {
                tempCCTV = YposCCTVList[++index].cctv;
                targetCode = YposIndexToCode(index);
                Debug.Log($"UA : new idx = {index}");
                UpdateCCTV(tempCCTV);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int index = CodeToYIndex(targetCode);
            Debug.Log($"DA : idx = {index}");

            if (index == -1) return;
            if (index - 1 >= 0)
            {
                tempCCTV = YposCCTVList[--index].cctv;
                targetCode = YposIndexToCode(index);
                Debug.Log($"DA : new idx = {index}");


                UpdateCCTV(tempCCTV);
            }
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetCCTV.StartCoroutine(targetCCTV.Neutralize());
            targetIndicator.SetActive(false);
            manipulatingCam = false;
            isUsingItem = isCloaked || manipulatingCam;
            StartCoroutine(cooldownManipulation());
            targetCCTV = null;
            targetCode = 0;
        }
    }

    private IEnumerator cooldownManipulation()
    {
        canManupulateCam = false;
        yield return new WaitForSeconds(3f);
        canManupulateCam = true;
    }

    private void CloakCape()
    {
        if (!isCloaked)
        {
            if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(Cloak());
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.W)) StartCoroutine(Uncloak());
        }
    }
    
    private void UpdateCCTV(CCTVEnemy temp)
    {
        if (temp != null) targetCCTV = temp;

        targetIndicator.SetActive(true);
        targetIndicator.transform.position = targetCCTV.transform.position;
    }

    private void VideoManipulator()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (isDetected) return;
            if (!canManupulateCam) return;


            #region MakeList
            visibleCCTVList = new List<CCTVEnemy>();

            foreach (var t in cctvList)
                if (Camera.main is { })
                {
                    var worldPos = Camera.main.WorldToViewportPoint(t.transform.position);
                    Debug.Log(t.gameObject.name + worldPos);
                    if (PositionCheck(worldPos.x) && PositionCheck(worldPos.y) && worldPos.z > 0)
                        visibleCCTVList.Add(t);
                }
            
            Debug.Log(visibleCCTVList.Count);

            #endregion


            if (visibleCCTVList.Count <= 0)
            {
                TurnOffManipulator();
                return;
            }

            XposCCTVList = new List<CCTVListNode>();
            YposCCTVList = new List<CCTVListNode>();

            #region ArraySort
            for (int i = 0; i < visibleCCTVList.Count; i++)
            {
                CCTVListNode node = new CCTVListNode(visibleCCTVList[i], i, visibleCCTVList[i].transform.position);
                XposCCTVList.Add(node);
                YposCCTVList.Add(node);
            }

            XposCCTVList = XposCCTVList.OrderBy(x => x.position.x).ToList();
            YposCCTVList = YposCCTVList.OrderBy(x => x.position.y).ToList();
            #endregion

            manipulatingCam = (!manipulatingCam);
            isUsingItem = isCloaked || manipulatingCam;

            if (manipulatingCam)
            {
                if (TurnOnManipulator()) return;
            }
            else
            {
                TurnOffManipulator();
            }

            for (int i = 0; i < XposCCTVList.Count; i++)
            {
                Debug.Log($"X List[{i}] : {XposCCTVList[i].cctv.name}");
            }
            for (int i = 0; i < YposCCTVList.Count; i++)
            {
                Debug.Log($"Y List[{i}] : {YposCCTVList[i].cctv.name}");
            }

        }

        if (manipulatingCam)
        {
            if (visibleCCTVList.Count <= 0) return;

            if (targetCCTV == null)
            {
                targetCode = 0;
                targetCCTV = XposCCTVList[CodeToXIndex(targetCode)].cctv;
            }

            Vector2 pos = XposCCTVList[CodeToXIndex(targetCode)].position;

            SelectCCTV(targetCCTV, pos);
        }
    }

    private bool TurnOnManipulator()
    {
        targetCCTV = visibleCCTVList[0];

        rb.velocity = Vector2.zero;
        UpdateCCTV(null);
        return false;
    }

    private void TurnOffManipulator()
    {
        targetCCTV = null;
        targetIndicator.SetActive(false);
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

    #endregion

    #region Interact

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
        if (TalkingManager.instance.hidingUI) return;
        if (MenuTabManager.instance.isMenuOn) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            moving = false;
            running = false;
            anim.SetBool("walk", false);
            anim.SetBool("run", false);
            anim.SetBool("idle", true);
            if (!TalkingManager.instance.isDisplayingDialogue)
                TalkingManager.instance.NextDialogue(); // 다음으로 넘어가기
            else
                TalkingManager.instance.instantComplete = true;
        }
    }

    #endregion

    private int CodeToXIndex(int code)
    {
        for (int i = 0; i < XposCCTVList.Count; i++)
        {
            if (XposCCTVList[i].code == code)
                return i;
        }
        Debug.LogError("OutOfBound");
        return -1;
    }

    private int CodeToYIndex(int code)
    {
        for (int i = 0; i < YposCCTVList.Count; i++)
        {
            if (YposCCTVList[i].code == code)
                return i;
        }
        Debug.LogError("OutOfBound");
        return -1;
    }

    private int XposIndexToCode(int index)
    {
        if (index < 0)
        {
            Debug.LogError("OutOfBound");
            return -1;
        }
        if (XposCCTVList.Count <= index)
        {
            Debug.LogError("OutOfBound");
            return -1;
        }

        return XposCCTVList[index].code;
    }
    private int YposIndexToCode(int index)
    {
        if (index < 0)
        {
            Debug.LogError("OutOfBound");
            return -1;
        }
        if (YposCCTVList.Count <= index)
        {
            Debug.LogError("OutOfBound");
            return -1;
        }

        return YposCCTVList[index].code;
    }
}
public class CCTVListNode
{
    public CCTVEnemy cctv;
    public int code;
    public Vector2 position;

    public CCTVListNode(CCTVEnemy cctv, int code, Vector2 position)
    {
        this.cctv = cctv;
        this.code = code;
        this.position = position;
    }
}