using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    [SerializeField] GameEvent invincibilityTrue;
    [SerializeField] GameEvent invincibilityFalse;

    public void EnableInvicibility()
    {
        invincibilityTrue.Raise();
    }
    public void DisableInvicibility()
    {
        invincibilityFalse.Raise();
    }
}
