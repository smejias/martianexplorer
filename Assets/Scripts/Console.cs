using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour {

    public static Console instance;

    public delegate void FunctionPrototype();
    public Dictionary<string, string> allCommands = new Dictionary<string, string>();
    public Dictionary<string, FunctionPrototype> allCommandsEffects = new Dictionary<string, FunctionPrototype>();

    public InputField input;
    public Text output;
    public GameObject visualConsole;
    public Manager manager;

    public void Awake()
    {
        Singleton();        
    }

    void Start () {
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

                if (allCommands.ContainsKey(input.text))
                {
                    allCommandsEffects[input.text].Invoke();
                }
                else
                {
                    Write("The command " + input.text + " does not exist");
                }

                input.text = "";
            }
        }
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

    public void Registercomand(string commandName, FunctionPrototype function, string description)
    {
        allCommandsEffects.Add(commandName, function);
        allCommands.Add(commandName, description);
    }

    public void Help()
    {
        foreach (var item in allCommands)
        {
            Write(item.Key + ": " + item.Value);
        }
    }
}
