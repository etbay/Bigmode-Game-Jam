using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security;
using System.Timers;
using Microsoft.VisualBasic;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class Timeslow : MonoBehaviour
{
    public static Timeslow instance;
    private static bool IsSlowed = false;
    private PlayerInputActions _inputActions;

    void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (_inputActions.Player.Ability.WasPressedThisFrame() && !IsSlowed)
        {
            Debug.Log("Slowing Time");
            IsSlowed = true;
            ActivateSlowMode();
        }
        else if (_inputActions.Player.Ability.WasPressedThisFrame() && IsSlowed)
        {
            Debug.Log("Speeding up again");
            IsSlowed = false;
            DeactivateSlowMode();
        }
    }

    private void ActivateSlowMode()
    {
        Time.timeScale = 0.2f;
    }

    private void DeactivateSlowMode()
    {
        Time.timeScale = 1f;
    }
}
