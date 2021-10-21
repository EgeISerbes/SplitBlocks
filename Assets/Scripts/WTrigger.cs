using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("WinPlatform") && PlayerControl.Instance.gState == PlayerControl.GameState.Run)
        {
            PlayerControl.Instance.gState = PlayerControl.GameState.Win;
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("LastChest") && PlayerControl.Instance.gState == PlayerControl.GameState.Win)
        {
            PlayerControl.Instance.gState = PlayerControl.GameState.Stop; 
        }
    }
}
