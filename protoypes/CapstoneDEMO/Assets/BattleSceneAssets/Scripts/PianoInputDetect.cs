using UnityEngine;
using Minis;
using MidiPlayerTK;
using System.Collections.Generic;

public class MidiInputs : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;

    public GameObject halfPianoModel;
    public GameObject fullPianoModel;

    GameObject pianomodel;

    public static MidiInputs instance;

    Dictionary<string, Transform> keys = new Dictionary<string, Transform>();

    List<int> pressed = new List<int>();

    public List<int> supposedToPress = new List<int>();

    public List<int> upcomingNotes = new List<int>();

    public List<int> validPresses = new List<int>();

    private void Awake()
    {
        halfPianoModel.SetActive(false);
        fullPianoModel.SetActive(false);

        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

        if (FullOrHalf.instance.IsItHalf())
        {
            halfPianoModel.SetActive(true);
            pianomodel = halfPianoModel;
        }
        else
        {
            fullPianoModel.SetActive(true);
            pianomodel = fullPianoModel;
        }

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

        if (!FullOrHalf.instance.IsItHalf() || (FullOrHalf.instance.IsItHalf() && note.noteNumber < 60))
        {
            if (!pressed.Contains(note.noteNumber))
            {
                pressed.Add(note.noteNumber);
            }

            if (!validPresses.Contains(note.noteNumber))
            {
                validPresses.Add(note.noteNumber);
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

            //if you pressed it when there were no notes to play, not a valid press
            if (!supposedToPress.Contains(note.noteNumber))
            {
                validPresses.Remove(note.noteNumber);
                PlayerInfo.instance.decreaseHealth(5);
                PlayerInfo.instance.madeAMistake();
            }
        }


    }

    public void OnNoteOff(MidiNoteControl note)
    {

        if (pressed.Contains(note.noteNumber))
        {
            pressed.Remove(note.noteNumber);
        }

        if (validPresses.Contains(note.noteNumber))
        {
            validPresses.Remove(note.noteNumber);
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
