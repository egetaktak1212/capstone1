using UnityEngine;

public class FullOrHalf : MonoBehaviour
{
    public static FullOrHalf instance;

    public bool half;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsItHalf() {
        return half;
    }


}
