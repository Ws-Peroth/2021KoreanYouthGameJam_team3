using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloakingParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var renderer = GetComponent<Renderer>();
        renderer.sortingLayerName = "Obstacles";
        renderer.sortingOrder = -1;
    }
    
}
