using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour

{   
    public enum GameState
    {
        Run,
        Lose,
        Win,
        Stop,
        Paused
    }
    
     public GameState gState = GameState.Paused;
   
    public static PlayerControl Instance { get; private set; }
    // Start is called before the first frame update
    public Vector3 speed = new Vector3(0, 0,0.1f);
    public bool isTower = false;
    

    /*[SerializeField] private float swerveSpeed = 0.5f;
    [SerializeField] private float maxSwerveSpeed = 1f;*/
    [SerializeField] private Rigidbody L_hand_Trigger;
    [SerializeField] private Rigidbody R_hand_Trigger;
    [SerializeField] private Rigidbody Character;
    [SerializeField] private Transform L_hand_Max;
    [SerializeField] private Transform R_hand_Max;
    [SerializeField] private Trigger L_hand_T;
    [SerializeField] private Trigger R_hand_T;
    [SerializeField] private Transform MinPosition;
    [SerializeField] private float ApproachRate;
    [SerializeField] private Transform Brick;
    [SerializeField] private Transform FloorCheck;
    private List<Collectible> tempList = new List<Collectible>();
    //private float TriggerDistance;



    private float _lastFrameFingerPositionX;
    private float _moveFactorX;
    private float _lastmoveFactor;
    [SerializeField] private bool isinSafeArea = false;
    
    private void Awake()
    {
      if(Instance == null)
        {
            Instance = this;
        }
       

        //TriggerDistance = L_hand_T.localScale.x / 1.75f;
    }
    private void Start()
    {
        gState = GameState.Paused;
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (gState == GameState.Run || gState == GameState.Win ||gState == GameState.Paused)
        {   
            if ( gState != GameState.Paused) MovePlayer();
            SwervePlayer();
        }
        
          
    }

    private void Update()
    {
        
         if (gState == GameState.Lose)
        {   

            StartCoroutine(GameLost());
        }
        else if (gState == GameState.Stop)
        {
            StartCoroutine(GameWon());
        }

        
    }


    private void SwervePlayer()
    {   if (gState == GameState.Paused)
        {


            if (Input.GetMouseButtonDown(0))
            {
                _lastFrameFingerPositionX = Input.mousePosition.x;
                gState = GameState.Run;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _moveFactorX = 0f;
                
            }

            if (Input.GetMouseButton(0))
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;
                gState = GameState.Run;
            }
            
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _moveFactorX = 0f;
            }

            if (Input.GetMouseButton(0))
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }

        }
        _lastmoveFactor = (_moveFactorX == 0) ? _lastmoveFactor : _moveFactorX;
        /*float swerveAmount = Time.deltaTime * _moveFactorX * swerveSpeed;
        swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveSpeed, maxSwerveSpeed);*/

        if(_lastmoveFactor>0)
        {

            /*L_hand_Trigger.MovePosition(Vector3.Lerp(L_hand_Trigger.position, L_hand_Max.position, swerveAmount));
            R_hand_Trigger.MovePosition(Vector3.Lerp(R_hand_Trigger.position, R_hand_Max.position, swerveAmount));*/
            L_hand_Trigger.MovePosition(new Vector3(Mathf.Lerp(L_hand_Trigger.position.x, L_hand_Max.position.x, ApproachRate),L_hand_Trigger.position.y,L_hand_Trigger.position.z +speed.z*Time.deltaTime));
            R_hand_Trigger.MovePosition(new Vector3(Mathf.Lerp(R_hand_Trigger.position.x, R_hand_Max.position.x, ApproachRate), R_hand_Trigger.position.y, R_hand_Trigger.position.z + speed.z * Time.deltaTime));
        }
        else if(_lastmoveFactor<0)
        {

            /*L_hand_Trigger.MovePosition(Vector3.Lerp(L_hand_Trigger.position, MinPosition.position, -swerveAmount));
            R_hand_Trigger.MovePosition(Vector3.Lerp(R_hand_Trigger.position, MinPosition.position, -swerveAmount));*/
            L_hand_Trigger.MovePosition(new Vector3(Mathf.Lerp(L_hand_Trigger.position.x, MinPosition.position.x, ApproachRate), L_hand_Trigger.position.y, L_hand_Trigger.position.z + speed.z * Time.deltaTime));
            R_hand_Trigger.MovePosition(new Vector3(Mathf.Lerp(R_hand_Trigger.position.x, MinPosition.position.x, ApproachRate), R_hand_Trigger.position.y, R_hand_Trigger.position.z + speed.z * Time.deltaTime));
        }

        /*if (swerveAmount > 0)
        {
            L_hand_T.position = Vector3.Lerp(L_hand_T.position, L_hand_Max.position, swerveAmount);
            R_hand_T.position = Vector3.Lerp(R_hand_T.position, R_hand_Max.position, swerveAmount);
            
        }
        else
        {
            L_hand_T.position = Vector3.Lerp(L_hand_T.position, MinPosition.position , -swerveAmount);
            R_hand_T.position = Vector3.Lerp(R_hand_T.position, MinPosition.position , -swerveAmount);
            
        }*/


    }


    public void DivideRoof()
    {
        
    }

    private IEnumerator GameLost()
    {
        Debug.Log("Game Lost !");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while (!asyncLoad.isDone) { yield return null; }
        
         
        
    }

    private IEnumerator GameWon()
    {
        Debug.Log("Game Won !");
        Scene scene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad;
        if (scene.name == "Level3")
        {
            asyncLoad = SceneManager.LoadSceneAsync(0);

        }
        asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
        while (!asyncLoad.isDone) { yield return null; }
    }

    private void MovePlayer()
    {
        Vector3 returnVal = CheckForHeight();
        Character.MovePosition(Character.position + speed * Time.deltaTime+returnVal);
        L_hand_Trigger.MovePosition(L_hand_Trigger.position + speed * Time.deltaTime);
        R_hand_Trigger.MovePosition(R_hand_Trigger.position + speed * Time.deltaTime);
        

    }
    
    public void Merge()
    {
        Sync();
        tempList = new List<Collectible>();
        List<Collectible> lList = L_hand_T.BrickStack;
        List<Collectible> rList = R_hand_T.BrickStack;
        
        for (int i = 0; i <rList.Count && i <lList.Count; i++)
        {
            tempList.Add(rList[i]);
            tempList.Add(lList[i]);
        }
        
        if(lList.Count>rList.Count)
        {
            tempList.Add(lList[lList.Count - 1]);
        }else if(rList.Count>lList.Count)
        {
            tempList.Add(rList[rList.Count - 1]);
        }
        lList.Clear();
        R_hand_T.BrickStack = tempList;
        //tempList.Clear();
        
    }

    public void ExitMerge()
    {
        tempList = new List<Collectible>();
        tempList = R_hand_T.BrickStack;
        if (tempList == null || tempList.Count == 0) return;
        List<Collectible> lList = new List<Collectible>();
        List<Collectible> rList = new List<Collectible>();

        for (int i = 0; i <tempList.Count ; i++)
        {
            if (i % 2 == 0) rList.Add(tempList[i]);
            else lList.Add(tempList[i]);
        }
        L_hand_T.BrickStack = lList;
        R_hand_T.BrickStack = rList;
        
        /*lList.Clear();
        rList.Clear();*/
    }

    /*public bool isOdd(Collectible coll)
    {
        return R_hand_T.BrickStack.IndexOf(coll) % 2 == 1;
    }*/

    /*public void StackBridge(GameObject InvBrick)
    {
        Transform firstBrick = R_hand_T.BrickStack[0].transform;
        Transform secondBrick;
        if (isTower)
        {
            secondBrick = R_hand_T.BrickStack[1].transform;
            R_hand_T.BrickStack.RemoveAt(1);
            
            

        }
        else
        {
            secondBrick = L_hand_T.BrickStack[0].transform;
            L_hand_T.BrickStack.RemoveAt(0);
            
        }
        R_hand_T.BrickStack.RemoveAt(0);
        Vector3 pos = InvBrick.transform.position;
        firstBrick.localScale = new Vector3(1, 1, 1);
        secondBrick.localScale = new Vector3(1, 1, 1);

        firstBrick.position = new Vector3(pos.x + 1.5f, pos.y, pos.z -0.5f);
        secondBrick.position = new Vector3(pos.x + 0.5f,pos.y,pos.z -0.5f);
        InvBrick.GetComponent<Collider>().isTrigger = true;
        

    }*/ 
    public void StackBridge(GameObject InvBrick, bool isRight)
    {
        if (InvBrick.gameObject.GetComponent<InvBlocks>().getIsTouching()) return;
        Transform Brick;
        
        if (isTower ||isRight)
        {   
            
            Brick = R_hand_T.BrickStack[0].transform;
            R_hand_T.BrickStack.RemoveAt(0);
            
            

        }
        else
        {
            Brick = L_hand_T.BrickStack[0].transform;
            L_hand_T.BrickStack.RemoveAt(0);
            
        }
        Vector3 pos = InvBrick.transform.position;
        Vector3 scale = InvBrick.transform.localScale;
        Brick.localScale = new Vector3(scale.x, scale.y, scale.z);
        Brick.position = new Vector3(pos.x +scale.x/2, pos.y, pos.z -scale.z/2);
        InvBrick.GetComponent<InvBlocks>().setIsTouching(true);
        InvBrick.GetComponent<Collider>().isTrigger = true;
        

    }
     public void Sync()
     {
         float leftCount = L_hand_T.BrickStack.Count;
         float rightCount = R_hand_T.BrickStack.Count;
         float subtract = leftCount - rightCount;
         if(subtract == 0)
         {
             return;
         }
         else if(subtract>1)
         {
             for (int i = 0; i < Mathf.Floor((Mathf.Abs(subtract)/2)); i++)
             {

                 R_hand_T.BrickStack.Add(L_hand_T.BrickStack[0]);
                 L_hand_T.BrickStack.Remove(L_hand_T.BrickStack[0]);
             }
         }else if(subtract<-1)
         {
             for (int i = 0; i < Mathf.Floor((Mathf.Abs(subtract)/2)); i++)
             {

                 L_hand_T.BrickStack.Add(R_hand_T.BrickStack[0]);
                 R_hand_T.BrickStack.Remove(R_hand_T.BrickStack[0]);
             }
         }


     }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("wTrigger"))
        {
            if (gState == GameState.Win)
            {
                gState = GameState.Stop;
                return;
            }
            gState = GameState.Win;
            speed *= 1.3f;
        }
        else if (other.gameObject.CompareTag("SafeZone"))
        {
            isinSafeArea = true;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("InvBricks"))
        {   

            if (gState == GameState.Run)
            {
                gState = GameState.Lose;
            }
            else if (gState == GameState.Win)
            {
                collision.gameObject.GetComponent<InvBlocks>().setWinningMaterial();
                gState = GameState.Stop;
            }

            //gState = GameState.Lose;
        }
        /*else if (collision.gameObject.CompareTag("CollBricks") && collision.gameObject.GetComponent<Collectible>().isStep)
        {
            Transform collbrick = collision.gameObject.GetComponent<Transform>();

            Character.position +=  new Vector3(0,collbrick.localScale.y,0); 
        }*/

        /*else if (collision.gameObject.CompareTag("CollBricks") && collision.gameObject.GetComponent<Collectible>().isStep)
        {
            Transform collbrick = collision.gameObject.GetComponent<Transform>();

            Character.position += new Vector3(0, collbrick.localScale.y, 0);
        }*/
        else if (collision.gameObject.CompareTag("MidBlock") && !isinSafeArea)
        {
            /*ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            if(Mathf.Abs(rot.x) <=0.5 )
            {
                gState = GameState.Lose;
            }
            else
            {
                Debug.Log("test");
            }*/
            gState = GameState.Lose;
            
            
        }
    }
    /*public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("MidBlock"))
        {
            Debug.Log("ok");
            Character.position -= new Vector3(0, Brick.localScale.y*5, 0);
        }
        
    }*/
    public void StackLadder(GameObject midBlock)
    {
        if (isTower)
        {
                int totalCount = L_hand_T.BrickStack.Count + R_hand_T.BrickStack.Count;
            midBlock.GetComponent<MidBlock>().isHit = true;
            Rigidbody rb = midBlock.GetComponent<Rigidbody>();
            float stairsAmount = midBlock.transform.localScale.y / (Brick.localScale.y);
            rb.position = new Vector3(rb.position.x, rb.position.y,rb.position.z + Brick.transform.localScale.z);
            //rb.MovePosition(new Vector3(rb.position.x, rb.position.y, rb.position.z + Brick.transform.localScale.z));
            //rb.MovePosition(new Vector3(rb.position.x, rb.position.y,Mathf.Lerp(rb.position.z,rb.position.z + Brick.transform.localScale.z,1)));
        
                if (totalCount >stairsAmount)
                {

                
                
                        Vector3 pos = new Vector3(rb.position.x, rb.position.y - 2 * Brick.localScale.y, rb.position.z - (stairsAmount*0.5f + 0.5f) * Brick.localScale.z);
                        for (int i = 0; i < stairsAmount; i++)
                        {
                            R_hand_T.BrickStack[i].GetComponent<Rigidbody>().position = new Vector3(pos.x, pos.y, pos.z);
                            R_hand_T.BrickStack[i].isStep = true;
                            
                            pos += new Vector3(0, Brick.localScale.y, Brick.localScale.z / 2);
                        }
                        R_hand_T.BrickStack.RemoveRange(0,(int) stairsAmount);


            }

                else
                {
                        Vector3 pos = new Vector3(rb.position.x, rb.position.y - 2 * Brick.localScale.y, rb.position.z - (stairsAmount *0.5f + 0.5f) * Brick.localScale.z);
                for (int i = 0; i < totalCount; i++)
                {
                    R_hand_T.BrickStack[i].GetComponent<Rigidbody>().position = new Vector3(pos.x, pos.y, pos.z);
                    R_hand_T.BrickStack[i].isStep = true;
                    
                    pos += new Vector3(0, Brick.localScale.y, Brick.localScale.z / 2);
                }
                R_hand_T.BrickStack.RemoveRange(0, totalCount);

            }






        }
    }

    

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SafeZone"))
        {
            isinSafeArea = false;
        }
    }

    public Vector3 CheckForHeight() // For constantly checking height and setting it
    {
        RaycastHit hit;
        int layermask = 1 << 6; // Ebabling  EnableRayCast Layer
        
        if (Physics.Raycast(FloorCheck.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, layermask))
        {
            if (hit.collider.gameObject.GetComponent<Collectible>() != null)
            {
                Collectible col = hit.collider.gameObject.GetComponent<Collectible>();
                if (!col.isStep) return Vector3.zero;
            }
            Vector3 pos = Character.position;
            //pos.y = (hit.transform.position.y + hit.transform.localScale.y*0.5f);
            pos.y = (hit.transform.position.y + hit.transform.localScale.y);
            float difference = (pos.y - Character.position.y);
            //Character.MovePosition(Character.position + new Vector3(0,difference,0));
            return  new Vector3(0,difference,0);
        }
        else return Vector3.zero;
    }

}