using System.Collections.Generic;
using UnityEngine;

public class BossDialogueAndMusicInfo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TextAsset dialogueText;

    public TextAsset songText;


    List<List<string>> turnMessages = new List<List<string>>();
    List<string> songNames = new List<string>();

    private void Awake()
    {
        if (dialogueText != null)
        {
            parseDialogue(dialogueText.text);
        }
        if (songText != null)
        {
            parseSongNames(songText.text);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void parseDialogue(string text)
    {
        string[] lines = text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] dialogueStep = line.Split('_');

            List<string> dialogueStepList = new List<string>(dialogueStep);

            turnMessages.Add(dialogueStepList);
        }
    }

    void parseSongNames(string text) {
        string[] lines = text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        songNames = new List<string>(lines);
    }

    public List<string> getSongNames() {
        return songNames;
    }

    public List<List<string>> getMessages() {
        return turnMessages;
    }



}
