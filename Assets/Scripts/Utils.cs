using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils : MonoBehaviour {

    public KeyCode openConsole;
    public KeyCode interactWithObjects;
    public KeyCode jump;
    public KeyCode run;
    public KeyCode enter;
    public KeyCode pause;
    public KeyCode flashlight;

    public string help = "List of available commands";
    public string godMode = "Inmunity to everything and brutal damage";
    public string fasterMovement = "Increases your movement speed";
    public string undetect = "Enemies wont detect you anymore!";
    public string win = "Win the game";
    public string lose = "Lose the game";
    public string[] boolKeys = new string[]{"on", "true", "active", "off", "false", "inactive"};
    public string[] boolKeysTrue = new string[]{"on", "true", "active"};
    public string[] boolKeysFalse = new string[]{"off", "false", "inactive"};
}
