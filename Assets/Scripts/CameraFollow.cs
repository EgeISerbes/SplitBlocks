using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform Target;
    [SerializeField] private float smoothRate;
    private float velocity = 0;
    private void LateUpdate()
    {
        if (PlayerControl.Instance.gState == PlayerControl.GameState.Run || PlayerControl.Instance.gState == PlayerControl.GameState.Win)
        {
            Vector3 targetPosition = Target.position;
            //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothRate);
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.SmoothDamp(transform.position.z, Target.position.z, ref velocity, smoothRate));
        }

    }
    
}
