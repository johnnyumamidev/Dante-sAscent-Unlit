using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimateWeapon : MonoBehaviour
{
    public Transform weaponHold;
    public Transform animationTarget;

    [SerializeField] UnityEvent startCombo;
    [SerializeField] UnityEvent endCombo;

    private void Update()
    {
        weaponHold.position = animationTarget.position;
        weaponHold.rotation = animationTarget.rotation;
    }

    public void StartCombo()
    {
        startCombo?.Invoke();
    }

    public void EndCombo()
    {
        endCombo?.Invoke();
    }
}
