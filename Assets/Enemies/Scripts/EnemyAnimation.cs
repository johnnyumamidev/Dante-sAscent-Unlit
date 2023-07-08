using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Transform parent;
    public Animator animator;
    public List<string> animStates;
    public int animStateIndex;
    [SerializeField] GameEvent attackEnded;
    [SerializeField] GameEvent enableHitbox;

    void Awake()
    {
        animator = GetComponent<Animator>();
        parent = transform.parent;
    }
    private void Start()
    {
        animStates = new List<string>
        {
            parent.name + "Idle",
            parent.name + "Search",
            parent.name + "Walk",
            parent.name + "Attack",
            parent.name + "Found",
            parent.name + "Death",
            parent.name + "RangeAttack"
        };
    }
    // Update is called once per frame
    void Update()
    {
        animator.CrossFade(animStates[animStateIndex], 0, 0);
    }

    // Attack animation events //
    // ============//============//============//============//
    public void EnableHitbox()
    {
        enableHitbox?.Raise();
    }
    public void EndAttackAnimation()
    {
        attackEnded?.Raise();
    }
    // ============//============//============//============//

}
