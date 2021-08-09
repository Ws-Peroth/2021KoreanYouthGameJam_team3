using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public string jsonName; // JSON 파일 이름
    
    public AudioSource voice;
    
    public int posNum; // 씬 진행도 (에디터 상에서 편집)
    [HideInInspector]
    public int txtNum = 0; // 대화 진행도

    public Player player;
    public float radius = 2f;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.gameObject.transform.position, transform.position);
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