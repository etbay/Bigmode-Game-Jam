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
    public static bool IsSlowed = false;
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
            IsSlowed = true;
            ActivateSlowMode();
            SlickometerData.CurrentSlickDrainRate = SlickometerData.TimeslowSlickDrainRate;
        }
        else if ((Player.SlickValue <= 1.0f) || _inputActions.Player.Ability.WasPressedThisFrame() && IsSlowed)
        {
            IsSlowed = false;
            DeactivateSlowMode();
            SlickometerData.CurrentSlickDrainRate = SlickometerData.BaseSlickDrainRate;
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
