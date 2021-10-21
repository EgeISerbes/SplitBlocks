using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform CharacterToFollow;
    void Awake()
    {
        gameObject.transform.position = CharacterToFollow.position;    
    }

    // Update is called once per frame
    
}
