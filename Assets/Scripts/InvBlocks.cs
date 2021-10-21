using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvBlocks : MonoBehaviour
{
    // Start is called before the first frame update
   private bool isTouching = false;
    [SerializeField] private Material winMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;
    public bool getIsTouching()
    {
        return isTouching;
    }
    public void setIsTouching(bool value)
    {
        isTouching = value;
    }

    public void setWinningMaterial()
    {
        meshRenderer.material = winMaterial;
        meshRenderer.enabled = true;
    }

}
