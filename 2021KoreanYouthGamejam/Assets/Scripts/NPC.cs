using UnityEngine;

public class NPC : MonoBehaviour
{
    public string jsonName; // JSON 파일 이름

    public AudioSource voice;

    public int posNum; // 씬 진행도 (에디터 상에서 편집)

    [HideInInspector] public int txtNum; // 대화 진행도
    public float radius = 2f;

    private Player player;

    public bool didItTalk = false;

    [SerializeField] public GameObject notice;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        notice.SetActive(true);
    }

    private void Update()
    {
        var distance = Vector2.Distance(player.gameObject.transform.position, transform.position);

        if (!didItTalk && distance <= radius && Input.GetKeyDown(KeyCode.Space))
        {
            didItTalk = true;
            notice.SetActive(false);
        }
        if (distance <= radius)
        {
            player.SetTarget(this);
            player.CheckInput();
        }
        else
        {
            player.ResetTarget();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}