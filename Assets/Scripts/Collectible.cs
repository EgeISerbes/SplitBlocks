using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{


    public Trigger Trigger { get; set; }  
    public bool isPicked = false;
    public bool isStep = false;
    public bool isHit = false;
    public bool isInSafeArea = false;
    private Collider coll;

    public void Start()
    {
        coll = gameObject.GetComponent<Collider>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        /*if (!isHit && collision.gameObject.CompareTag("Block"))
        {

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            if (rot.x != 0)
            {
                isInSafeArea = true;
                return;
            }
            if (Trigger != null)
                {
                    Trigger.HandleBrickCollision(this, collision.gameObject.tag);
                }
            
        }*/

        if (collision.gameObject.CompareTag("Block"))
        {
            if(Trigger != null)
            {
                Trigger.HandleBrickCollision(this, collision.gameObject.tag);
            }
        }


        /*else if (!isHit && collision.gameObject.CompareTag("ArchBlock"))
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            if (rot.x != 0) isInSafeArea = true;
            if (Trigger != null)
            {
                Trigger.HandleBrickCollision(this, collision.gameObject.tag);
            }
        }*/
        if (collision.gameObject.CompareTag("CollBricks"))
        {
            Physics.IgnoreCollision(coll, collision.gameObject.GetComponent<Collider>());
        }

        

       

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SafeZone") && !isHit)
        {
            isInSafeArea = true;
        }
        else if (other.gameObject.CompareTag("ArchBlock"))
        {
            if (Trigger != null)
            {
                isInSafeArea = true;
                Trigger.HandleBrickCollision(this, other.gameObject.tag);
            }
        }
    }

    /*public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("ArchBlock"))
        {
            isInSafeArea = false;
            
        }
    }*/


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SafeZone")|| other.gameObject.CompareTag("ArchBlock"))
        {
            isInSafeArea = false;
        }
    }








}
