using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFatigueAspect : MoveAspect
{
    [Header("Ref [Auto Added]")]
    public JumpAspect jumpAspect;
    [Header("JumpFatigueAspect Settings")]
    [Range(0f,1f)]
    [Tooltip("0 is no fatigue, 1 completely halts movement at max")]
    public float maxFatigue = 1f;
    [Range(-1f, 0f)]
    [Tooltip("Negative values are a buffer before fatigue kicks in")]
    public float minFatigue = 0f;
    [Space(5)]

    //private members
    [SerializeField]
    private float curFatigue = 0f;

    public override void InitializeMoveAspect()
    {
        if (GetComponent<JumpAspect>() != null)
            jumpAspect = GetComponent<JumpAspect>();
        else
        {
            DisableAspect();
            Debug.LogError("JumpFatigueAspect could not find attached jumpAspect! Are they on the same Gameobject? JumpFatigueAspect disabling!");
        }

    }

    // Update is called once per frame
    public override void DoUpdate()
    {
        if (curFatigue <= 0f)
            jumpAspect.ResetJumpHeight();
        else
        {
            jumpAspect.SetJumpHeight(jumpAspect.GetJumpHeightDefault() * (1 - curFatigue));
            moveSystem.AppendDynamicSpeedMultiplier(-curFatigue);
        }

        if (moveSystem.IsGrounded())
        {
            UpdateFatigue();
        }

        
    }

    

    void OnJumpEvent()
    {
        AddFatigue(.25f);
    }

    void UpdateFatigue()
    {
        curFatigue -= .33333f * Time.deltaTime;
        curFatigue = Mathf.Clamp(curFatigue, minFatigue, maxFatigue);
    }

    void AddFatigue(float x) { curFatigue += x; curFatigue = Mathf.Clamp(curFatigue, 0f, maxFatigue); }
}
