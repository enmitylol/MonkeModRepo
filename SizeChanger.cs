using easyInputs;
using GorillaLocomotion;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GrowMod : BaseMod
{
    private GameObject PlayerModel;
    private bool IsMaxReached;
    private bool IsMinReached;
    private Vector3 NormalPlayerSize;
    private Player PlayerScript;
    private float NormalArmLength;
    public float MaxSize = 2.0f;
    public float MinSize = 0.1f;
    public float Speed = 0.5f;
    public bool usePrimaryButtonForReset = false;

    private bool isInitialized = false;

    public override void OnLoad()
    {
        Debug.Log("woah grow now monke");
    }

    public override void OnUpdate()
    {
        if (!isInitialized)
        {
            InitializeMod();
            return;
        }
        Physics.gravity = new Vector3(0f, -(9.81f * PlayerScript.transform.localScale.x), 0f);
        if (PlayerModel == null) // this is old logic lol, just ignore some of this
        {
            PlayerModel = GameObject.Find("Player(Clone)");
            if (PlayerModel != null)
            {
                Debug.Log("GrowMod: Found Player(Clone) object.");
            }
        }
        if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand) && !IsMaxReached)
        {
            Vector3 newScale = PlayerScript.transform.localScale + Vector3.one * Speed * Time.deltaTime;
            PlayerScript.maxArmLength += 1 * Speed * Time.deltaTime;
            PlayerScript.transform.localScale = newScale;
            if (PlayerModel != null)
            {
                PlayerModel.transform.localScale = newScale;
            }
            IsReached(true, false);
        }
        else if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand) && !IsMinReached)
        {
            Vector3 newScale = PlayerScript.transform.localScale - Vector3.one * Speed * Time.deltaTime;
            PlayerScript.maxArmLength -= 1 * Speed * Time.deltaTime;
            PlayerScript.transform.localScale = newScale;
            if (PlayerModel != null)
            {
                PlayerModel.transform.localScale = newScale;
            }
            IsReached(false, true);
        }
        if ((usePrimaryButtonForReset && EasyInputs.GetPrimaryButtonDown(EasyHand.RightHand)) || (!usePrimaryButtonForReset && EasyInputs.GetSecondaryButtonDown(EasyHand.LeftHand)))
        {
            PlayerScript.transform.localScale = NormalPlayerSize;
            PlayerScript.maxArmLength = NormalArmLength;
            if (PlayerModel != null)
            {
                PlayerModel.transform.localScale = NormalPlayerSize;
            }
        }
    }

    private void InitializeMod()
    {
        PlayerScript = Object.FindObjectOfType<Player>();

        if (PlayerScript != null)
        {
            NormalPlayerSize = PlayerScript.transform.localScale;
            NormalArmLength = PlayerScript.maxArmLength;
            isInitialized = true;
        }
        else
        {
// not needed anymore lol
        }
    }

    private void IsReached(bool max, bool min)
    {
        if (max)
        {
            IsMaxReached = PlayerScript.transform.localScale.x >= MaxSize;
        }
        if (min)
        {
            IsMinReached = PlayerScript.transform.localScale.x <= MinSize;
        }
    }
}

