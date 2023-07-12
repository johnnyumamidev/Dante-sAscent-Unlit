using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Enemy/Data")]
public class EnemyData : ScriptableObject
{
    public string enemyType;
    public string enemyName;

    [Header("Movement")]
    public float speed;
    public LayerMask ground;
    public float weight;

    [Header("Health")]
    public int maxHealth;

    [Header("AI/Detection")]
    public float detectionRadius;
    public LayerMask detectionLayer;

    [Header("Attack")]
    public float attackRange = 1;
    public float attackSpeed;
    public float hitboxRadius = 0.05f;
    public float lungeForce = 10f;

    [Header("Animation")]
    public AnimatorController animatorController;
}
