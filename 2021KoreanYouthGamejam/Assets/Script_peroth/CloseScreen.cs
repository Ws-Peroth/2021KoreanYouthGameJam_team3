using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            gameObject.SetActive(false);
    }
    public void CloseButtonDown() => gameObject.SetActive(false);

}

