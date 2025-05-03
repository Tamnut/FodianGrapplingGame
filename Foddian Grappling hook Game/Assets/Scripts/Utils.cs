using UnityEngine;

public class Utils : MonoBehaviour
{
    void Update()
{
    if(Input.GetKeyDown(KeyCode.P))
    {
        Debug.Break();
    }
}
}
