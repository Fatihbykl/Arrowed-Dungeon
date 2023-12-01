using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ArrowType arrowType;
    public int health;
    public int coinReward;
    public float speed;
    public bool isAlive = true;


    private void Start()
    {
        var arrowRenderer = GetComponent<Renderer>();

        arrowRenderer.materials[0].color = arrowType.arrowBodyColor;
        arrowRenderer.materials[1].color = arrowType.arrowMetalSideColor;
        arrowRenderer.materials[2].color = arrowType.arrowTailColor;
        
        health = arrowType.baseHealth;
        coinReward = arrowType.basecoinReward;
        speed = arrowType.baseSpeed;
    }
}
