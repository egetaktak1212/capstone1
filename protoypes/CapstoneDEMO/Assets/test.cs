using UnityEngine;
using UnityEngine.InputSystem;
using Minis;
using MidiPlayerTK;

public class MidiDebug : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;


    void Start()
    {

        InputSystem.onDeviceChange += OnDeviceChange;


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
        Debug.Log($"note: {note.noteNumber} vel={velocity}");

        var noteOn = new MPTKEvent()
        {
            Command = MPTKCommand.NoteOn,
            Value = note.noteNumber,              
            Channel = 0,                          
            Velocity = Mathf.RoundToInt(velocity * 127),
            Duration = -1                         
        };

        midiStreamPlayer.MPTK_PlayEvent(noteOn);
    }

    void OnNoteOff(MidiNoteControl note)
    {
        Debug.Log($"note: {note.noteNumber}");

        var noteOff = new MPTKEvent()
        {
            Command = MPTKCommand.NoteOff,        
            Value = note.noteNumber,              
            Channel = 0,                          
            Velocity = 0
        };

        midiStreamPlayer.MPTK_PlayEvent(noteOff);
    }
}
