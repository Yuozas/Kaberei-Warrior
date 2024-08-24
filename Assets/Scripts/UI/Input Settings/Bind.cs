using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bind : MonoBehaviour
{
    [SerializeField]
    Text attackButtonText, jumpButtonText, walkLeftButtonText, walkRightButtonText, runButtonText, attackModeButtonText;
    List<Text> allKeys;

    private void Start()
    {
        LoadPrefs();
        allKeys = new List<Text>
        {
            attackButtonText,
            jumpButtonText,
            walkLeftButtonText,
            walkRightButtonText,
            runButtonText,
            attackModeButtonText
        };
    }
    #region Key binds for buttons
    public void BindAttack()
    {
        StartCoroutine(WaitForInput(attackButtonText, "attackInput"));
    }
    public void BindJump()
    {
        StartCoroutine(WaitForInput(jumpButtonText, "jumpInput"));
    }
    public void BindWalkLeft()
    {
        StartCoroutine(WaitForInput(walkLeftButtonText, "walkLeftInput"));
    }
    public void BindWalkRight()
    {
        StartCoroutine(WaitForInput(walkRightButtonText, "walkRightInput"));
    }
    public void BindRun()
    {
        StartCoroutine(WaitForInput(runButtonText, "runInput"));
    }
    public void BindAttackMode()
    {
        StartCoroutine(WaitForInput(attackModeButtonText, "attackModeInput"));
    }
    #endregion

    IEnumerator WaitForInput(Text text, string key)
    {
        text.text = "Enter any key...";

        //Wait for any input
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        //Get the entered input
        string gottenKey = GetKey().ToString();

        //Check if input is allowed to change
        if (!CheckIfChange(gottenKey))
        {
            text.text = PlayerPrefs.GetString(key);
        }
        else
        {
            text.text = gottenKey;
            PlayerPrefs.SetString(key, gottenKey);
        }
    }
    public bool CheckIfChange(string newKey)
    {
        if (NotAllowedInput(newKey))
            return false;
        if (CheckIfInUse(newKey))
            return false;
        return true;
    }
    private bool CheckIfInUse(string newKey)
    {
        foreach (Text t in allKeys)
        {
            if (t.text == newKey)
                return true;
        }
        return false;
    }
    private bool NotAllowedInput(string newKey)
    {
        /*
        // for when the c sharp 8 will be supported on unity
        return newKey switch
        {
            "Mouse0" => true,
            "Escape" => true,
            "Backspace" => true,
            _ => false,
        };
        */
        switch (newKey)
        {
            case "Mouse0":
                return true;
            case "Escape":
                return true;
            case "Backspace":
                return true;
            default:
                return false;
        }
    }
    private KeyCode GetKey()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                return kcode;
        }
        return KeyCode.None;
    }
    public void LoadPrefs()
    {
        attackButtonText.text = PlayerPrefs.GetString("attackInput");
        jumpButtonText.text = PlayerPrefs.GetString("jumpInput");
        walkLeftButtonText.text = PlayerPrefs.GetString("walkLeftInput");
        walkRightButtonText.text = PlayerPrefs.GetString("walkRightInput");
        runButtonText.text = PlayerPrefs.GetString("runInput");
        attackModeButtonText.text = PlayerPrefs.GetString("attackModeInput");
    }
    public void Default()
    {
        DefaultRun.Construct(gameObject).DefaultInputSettings();
        LoadPrefs();
    }
}
