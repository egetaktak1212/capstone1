using UnityEngine;
using UnityEngine.InputSystem;
using Minis;
using MidiPlayerTK;
using System.Collections.Generic;

public class MidiDebug : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;
    public GameObject pianomodel;

    Dictionary<string, Transform> keys = new Dictionary<string, Transform>();

    List<int> pressed = new List<int>();

    void Start()
    {


        InputSystem.onDeviceChange += OnDeviceChange;

        foreach (Transform child in pianomodel.transform) {
            if (child != null) {
                keys[child.name] = child;
            }
        }

        foreach (var device in InputSystem.devices)
        {
            if (device is MidiDevice midi)
            {
                Debug.Log("Found MIDI device: " + midi.device.description.product);
                midi.onWillNoteOn += OnNoteOn;
                midi.onWillNoteOff += OnNoteOff;
            }
        }
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is MidiDevice midi && change == InputDeviceChange.Added)
        {
            Debug.Log("MIDI device connected: " + midi.device.description.product);
            midi.onWillNoteOn += OnNoteOn;
            midi.onWillNoteOff += OnNoteOff;
        }
    }

    void OnNoteOn(MidiNoteControl note, float velocity)
    {
        //Debug.Log($"note: {note.noteNumber} vel={velocity}");

        if (!pressed.Contains(note.noteNumber)) {
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

        if (keys.TryGetValue(noteName, out Transform key)) {
            key.GetComponent<PressLift>().pressed = true;
        }

    }

    void OnNoteOff(MidiNoteControl note)
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

    public List<int> getPressed() {
        return pressed;
    }


}
