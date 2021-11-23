using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    private enum MergeState {Idle, Collided, Merged, Exited };
    private MergeState mState = MergeState.Idle;
    internal List<Collectible> BrickStack = new List<Collectible>();
    //private List<Collectible> ToBeDestroyed = new List<Collectible>();
    [SerializeField] private Transform Brick;
    private float distance = 0;
    [SerializeField] private float ApproachRate = 0.1f;
    [SerializeField] private bool isRight = false;
    private float mergeTimer = 0f;
    private float WaitTime = 0.2f;
    private List<Collectible> toBeRemovedList = new List<Collectible>();
    void Start()
    {
        distance = Brick.localScale.y;
        if (this.tag == "R_Trigger") isRight = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = this.transform.position;
        pos.z -= distance * 3;
        BrickStack.RemoveAll(removeAllhit);
        
        
        for (int i = 0; i < BrickStack.Count; i++)
        {
            if (BrickStack[i].isInSafeArea)
            {   
                pos.y = BrickStack[i].GetComponent<Rigidbody>().position.y; 

            }

           
            var brickR = BrickStack[i].GetComponent<Rigidbody>();
            Vector3 brickPos = brickR.position;
            brickPos.z = pos.z;
            brickPos.x = Mathf.Lerp(brickPos.x, pos.x, ApproachRate);
            brickPos.y = Mathf.Lerp(brickPos.y, pos.y, ApproachRate);
            brickR.MovePosition(brickPos);
            
            pos = brickPos;
            pos.y += distance;
            /* var brick = BrickStack[i];
            Vector3 brickPos = brick.transform.position;
            brickPos.z = pos.z;
            brickPos.x = Mathf.Lerp(brickPos.x, pos.x, ApproachRate);
            brickPos.y = Mathf.Lerp(brickPos.y, pos.y, ApproachRate);
            brick.transform.position = brickPos;
            
            pos = brickPos;
            pos.y += distance;*/
        }

        /*for (int i = 0; i < BrickStack.Count; i++)
        {
            var brick = BrickStack[i];
            Vector3 brickPos = brick.transform.position;
            brickPos.z = this.transform.position.z -distance*3 ;
            float addedY = distance * i;
            ApproachRate = (i > 3) ? 0.3f : ApproachRate;
            brickPos.y = Mathf.Lerp(brickPos.y, this.transform.position.y + addedY, ApproachRate);
            brickPos.x = Mathf.Lerp(brickPos.x, this.transform.position.x, ApproachRate);

            brick.transform.position = brickPos;

        }*/






    }

    public void Update()
    {
      if (mState == MergeState.Collided)
        {
            mergeTimer += Time.deltaTime;
        }
      else if(mState == MergeState.Exited)
        {
            mState = MergeState.Idle;
           // this.isRight = false;
            PlayerControl.Instance.isTower =false ;
            mergeTimer = 0f;
            PlayerControl.Instance.ExitMerge();
        }
      if (mergeTimer>WaitTime)
        {
            mergeTimer = 0f;
            mState = MergeState.Merged;
            //this.isRight = true;
            PlayerControl.Instance.isTower = true;
            PlayerControl.Instance.Merge();
        }
    }
    public void AddBrick(Collectible brick)
    {
        
        BrickStack.Insert(0,brick);
        Rigidbody brickR = brick.gameObject.GetComponent<Rigidbody>();
        //brickR.isKinematic = false;
        brickR.MovePosition(new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z-distance*3));
    }

    /*public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("CollBricks"))
        {
            var c = other.gameObject.GetComponent<Collectible>();
            if (c != null)
            {
                c.Trigger = this;
                c.isPicked = true;
                AddBrick(c);
            }
        }
        else if(other.gameObject.CompareTag("Merger") && this.tag == "R_Trigger")
        {
            mState = MergeState.Collided;
        }
    }*/
    

    public void HandleBrickCollision(Collectible collectible, String tag)
    {
        if(tag == "Block")
        {
            collectible.isHit = true;
            collectible.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            collectible.gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.forward*5f, ForceMode.Impulse);
            
            
            //PlayerControl.Instance.Sync();
            
            
            
        }
        else if(tag == "ArchBlock")
        {
                 int indexofSafe = BrickStack.IndexOf(collectible);
                 if (indexofSafe < 0) return;
                 int length = BrickStack.Count;
                 for (int i = indexofSafe+1; i < length; i++)
                 {      
                     
                     BrickStack[i].isHit = true;
                     BrickStack[i].gameObject.GetComponent<Rigidbody>().isKinematic = false;
                     BrickStack[i].gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.forward *7f, ForceMode.Impulse);
                     //tempList.Add(BrickStack[i]);

                 }
                 //BrickStack.RemoveRange(indexofSafe+1, length - (indexofSafe+1));
                 /*for (int i = 0; i < tempList.Count; i++)
                 {
                     tempList[i].gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.forward * 15f, ForceMode.Impulse);

                 }
                 // if (isRight) PlayerControl.Instance.Sync();*/
        }





    }

    public void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.CompareTag("CollBricks") && !collision.gameObject.GetComponent<Collectible>().isPicked)
        {
            if (PlayerControl.Instance.isTower && !isRight) return;
            var c = collision.gameObject.GetComponent<Collectible>();
            if (c != null)
            {   
                
                c.Trigger = this;
                c.isPicked = true;
                //Debug.Log(BrickStack.Contains(c));
                AddBrick(c);
            }
        }
        else if(collision.gameObject.CompareTag("L_Trigger"))
        {
            mState = MergeState.Collided;
        }
        /*else if (collision.gameObject.CompareTag("Block"))
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(),this.gameObject.GetComponent<Collider>());
        }*/
        else if (collision.gameObject.CompareTag("InvBricks") && BrickStack.Count != 0)
        {
            PlayerControl.Instance.StackBridge(collision.gameObject,isRight);
        }
        else if (collision.gameObject.CompareTag("MidBlock") && isRight && !collision.gameObject.GetComponent<MidBlock>().isHit)
        {
            PlayerControl.Instance.StackLadder(collision.gameObject);
        }


    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("L_Trigger"))
        {
            if (mState == MergeState.Merged) mState = MergeState.Exited;
            else mState = MergeState.Idle;
        }

    }

    /*public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Merger"))
        {
            if (mState == MergeState.Merged) mState = MergeState.Exited;
            else mState = MergeState.Idle;
        }
    }*/


    private bool getSafe(Collectible coll)
    {
        return coll.isInSafeArea;
    }
    
    private bool removeAllhit(Collectible coll)
    {
        return coll.isHit;
    }
    
    
}
