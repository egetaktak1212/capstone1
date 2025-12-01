using Minis;
using UnityEngine;
using UnityEngine.InputSystem;

public class MidiInputDetector : MonoBehaviour
{
    public GameObject pianomodel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        foreach (var device in InputSystem.devices)
        {
            if (device is MidiDevice midi)
            {
                midi.onWillNoteOn += MidiInputs.instance.OnNoteOn;
                midi.onWillNoteOff += MidiInputs.instance.OnNoteOff;
            }
        }
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is MidiDevice midi && change == InputDeviceChange.Added)
        {
            midi.onWillNoteOn += MidiInputs.instance.OnNoteOn;
            midi.onWillNoteOff += MidiInputs.instance.OnNoteOff; ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
