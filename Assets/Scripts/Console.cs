using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Console : MonoBehaviour {

    public static Console instance;

    public delegate void FunctionPrototype();
    public Dictionary<string, string> allCommands = new Dictionary<string, string>();
    public Dictionary<string, Action<bool>> allCommandsEffects = new Dictionary<string, Action<bool>>();
    public InputField input;
    public Text output;
    public GameObject visualConsole;
    public Manager manager;
    public String[] boolKeys;

    public void Awake()
    {
        Singleton();        
    }

    void Start () {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        output.text += "To know all the available commands, use /help" + "\n";
        Invoke("CloseSelf", 0.01f);
	}
	
	void Update () {
        Execute();
    }

    public void Execute()
    {
        if (visualConsole.activeSelf)
        {
            if (Input.GetKeyDown(manager.utils.enter) && input.text != "")
            {
                Write(input.text);
                var functionToUse = input.text.Split(' ');

                if (allCommands.ContainsKey(functionToUse[0]))
                {
                    Boolean convertedBoolean = true;
                    if (functionToUse.Length > 1)
                    {            
                        bool hasKey = false; 
                        foreach(var key in manager.utils.boolKeys)     
                        {
                            if(key.Equals(functionToUse[1]))
                            {    
                                hasKey = true;        
                            }
                        }
                        if (hasKey)
                        {
                            convertedBoolean = ConvertToBoolean(functionToUse[1]);      
                            allCommandsEffects[functionToUse[0]](convertedBoolean);  
                        }
                        else
                        {
                            Write("The operator " + functionToUse[1] + " does not exist");
                        }
                    }
                    allCommandsEffects[functionToUse[0]](convertedBoolean);
                }
                else
                {
                    Write("The command " + functionToUse[0] + " does not exist");
                }
                input.text = "";
            }
        }
    }

    private Boolean ConvertToBoolean(string text)
    {
        Boolean result = false;
        foreach(var key in manager.utils.boolKeysTrue)     
        {
            if(key.Equals(text))
            {    
                result = true;        
            }       
        }            
        foreach(var key in manager.utils.boolKeysFalse)     
        {
            if(key.Equals(text))
            {    
                result = false;        
            }       
        }         
        return result;    
    }

    private void CloseSelf()
    {
        gameObject.SetActive(false);
    }

    public void Write(string text)
    {
        output.text += text + "\n";
    }

    public void Singleton()
    {
        if (instance != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void Initialize()
    {
        input.Select();
    }

    public void Registercomand(string commandName, Action<bool> function, string description)
    {
        allCommandsEffects.Add(commandName, function);
        allCommands.Add(commandName, description);
    }

    public void Help(bool state)
    {
        foreach (var item in allCommands)
        {
            Write(item.Key + ": " + item.Value);
        }
    }
}
