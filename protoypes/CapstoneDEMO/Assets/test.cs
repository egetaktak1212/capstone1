using UnityEngine;
using UnityEngine.InputSystem;
using Minis;
using MidiPlayerTK;
using System.Collections.Generic;

public class MidiInputs : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;
    public GameObject pianomodel;
    public static MidiInputs instance;

    Dictionary<string, Transform> keys = new Dictionary<string, Transform>();

    List<int> pressed = new List<int>();

    public List<int> supposedToPress = new List<int>();

    public List<int> upcomingNotes = new List<int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

        foreach (Transform child in pianomodel.transform)
        {
            if (child != null)
            {
                keys[child.name] = child;
            }
        }



    }




    public void OnNoteOn(MidiNoteControl note, float velocity)
    {
        //Debug.Log($"note: {note.noteNumber} vel={velocity}");

        if (!pressed.Contains(note.noteNumber))
        {
            pressed.Add(note.noteNumber);
        }

        var noteOn = new MPTKEvent()
        {
            Command = MPTKCommand.NoteOn,
            Value = note.noteNumber,
            Channel = 0,
            Velocity = 100,
            Duration = -1
        };

        midiStreamPlayer.MPTK_PlayEvent(noteOn);

        string noteName = note.noteNumber.ToString();

        if (keys.TryGetValue(noteName, out Transform key))
        {
            key.GetComponent<PressLift>().pressed = true;
        }

        if (!supposedToPress.Contains(note.noteNumber))
        {
            PlayerInfo.instance.decreaseHealth(5);
            PlayerInfo.instance.madeAMistake();
        }


    }

    public void OnNoteOff(MidiNoteControl note)
    {
        //Debug.Log($"note: {note.noteNumber}");

        if (pressed.Contains(note.noteNumber))
        {
            pressed.Remove(note.noteNumber);
        }


        var noteOff = new MPTKEvent()
        {
            Command = MPTKCommand.NoteOff,
            Value = note.noteNumber,
            Channel = 0,
            Velocity = 0
        };

        midiStreamPlayer.MPTK_PlayEvent(noteOff);

        string noteName = note.noteNumber.ToString();

        List<int> blackkeys = new List<int> { 49, 51, 54, 56, 58, 61, 63, 66, 68, 70 };


        if (keys.TryGetValue(noteName, out Transform key))
        {
            key.GetComponent<PressLift>().pressed = false;
        }

    }

    public List<int> getPressed()
    {
        return pressed;
    }

    private void Update()
    {
        //Debug.Log("-------------------");
        //foreach (int value in upcomingNotes)
        //{ 
        //    Debug.Log(value);
        
        
        //}
        //Debug.Log("-------------------");
    }


}
