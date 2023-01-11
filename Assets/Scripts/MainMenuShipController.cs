using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuShipController : MonoBehaviour
{
   

    [Header("Barrel Roll")]
    public float barrelRollSpeed;
    public float barrelRollTimer;
    public float barrelRollMaxTime = 3;
    public bool isPerformingBarrelRoll = false;
    float rollDirection;

    [Header("Plane Movement")]
    [SerializeField] ParticleSystem mainEngine;
    [SerializeField] ParticleSystem leftEngine;
    [SerializeField] ParticleSystem rightEngine;
    [SerializeField] Material mainEngineMat;
    public float glide;
    public float speed = 2;
    public float boost = 1;
    public float verticalInput;
    public float horizontalInput;
    
    float flightDirection;
    // Rotation
    public float rotationPushSpeed;
    public int rotationSpeed;

    // Aiming
    [Header("Shooting")]
    [SerializeField] float fireRate;
    public GameObject gunCylinder;
    public Image crossHair;
    public Sprite nettingCrossHairSprite;
    public Sprite defaultCrossHairSprite;
    public GameObject defaultProjectilePrefab;
   
    public float force;
    public Image aimLimitImage;
    public Camera mainCamera;
    public Camera backCamera;
    float mouseX;
    float mouseY;
    Vector3 aim;

    [Header("Netting")]
    public bool isNetting = false;
    public GameObject nettingProjectilePrefab;
    public float maxEnergy = 100;
    private float currentEnergy;
    public float energyDepletionRate;

    [Header("Environment")]
    public int topBounds = 3000;
    public GameObject activeTerrain;


    void Start()
    {
       // gameManager = FindObjectOfType<GameManager>();
       // mainCamera.gameObject.SetActive(true);
       // backCamera.gameObject.SetActive(false);
        
       // activeTerrain = FindObjectOfType<PerlinNoiseMapChatGPT>().gameObject;
       // topBounds *= FindObjectOfType<PerlinNoiseMapChatGPT>().mapScale;
       //float terrainHeight = activeTerrain.GetComponent<Collider>().ClosestPoint(gameObject.transform.position).y;
       // crossHair = GetComponentInChildren<Image>();
       // Vector3 playerSpawnPos = new Vector3(activeTerrain.gameObject.GetComponent<Renderer>().bounds.center.x , 30 * activeTerrain.GetComponent<PerlinNoiseMapChatGPT>().mapScale, activeTerrain.gameObject.GetComponent<Renderer>().bounds.center.z-30* FindObjectOfType<PerlinNoiseMapChatGPT>().mapScale);

       // transform.position = playerSpawnPos;
       // gunCylinder.GetComponent<Transform>();
       // Physics.IgnoreLayerCollision(8, 9);
        
    }

    private void Update()
    { 
    //    fireRate -= Time.deltaTime;
    //    //Shooting();
    //    Netting();
        DoBarrelRoll();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (horizontalInput>0)
        {
            leftEngine.startSize = horizontalInput;
        }
        else
        {
            leftEngine.startSize = 0;
        }
        if (horizontalInput<0)
        {
            rightEngine.startSize = -horizontalInput;
        }
        else
        {
            rightEngine.startSize = 0;
        }
        //flightDirection = Vector3.Dot(gameObject.transform.forward, Vector3.down);
        //flightDirection = Vector3.Dot(gameObject.transform.forward, Vector3.down);
        //if (Mathf.Abs(flightDirection) < 0.1f)
        //{
        //    flightDirection = Vector3.Dot(gameObject.transform.right, Vector3.down);
        //}
        if (!isPerformingBarrelRoll)
        {
            
            Rotating();
            
        }
        
        //if (currentEnergy > maxEnergy)
        //{
        //    currentEnergy = maxEnergy;
        //}
        //if (transform.position.y > topBounds)
        //{
        //    transform.position = new Vector3(transform.position.x, topBounds, transform.position.z);
        ////}
        //if (glide < 30)
        //{
        //    glide = 30;
        //}
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    CameraSwitch();
        //}
        
        //if (mainCamera.isActiveAndEnabled)
        //{
        //    UpdateAim();
        //}
        //Gliding();
        Boosting();
        
        
        


    }
   
    //void UpdateAim()
    //{
        
    //    Vector3 mousePos = Input.mousePosition;
    //    int screenLimitX = 700;
    //    int screenLimitY = 200;
    //    mousePos += mainCamera.transform.forward * 30f;
    //    //mouseX = Mathf.Clamp(mousePos.x, screenLimitX, Screen.width - screenLimitX);
    //    //mouseY = Mathf.Clamp(mousePos.y, screenLimitY, Screen.height - screenLimitY);
    //    //aim = mainCamera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 1000f));
    //    aim = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1000f));
    //    /*crossHair.transform.position = new Vector3(mouseX, mouseY, 1000f);*/
    //    crossHair.transform.position = mousePos;
        
        
    //}



    public void Boosting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            mainEngineMat.SetColor("_EmissionColor", Color.cyan);
            mainEngine.startSize = 5f;
            mainEngine.startColor = Color.cyan;
            boost += 0.01f;
        }
        if (boost > 1 && !Input.GetKey(KeyCode.Space))
        {
            mainEngineMat.SetColor("_EmissionColor", Color.yellow);
            mainEngine.startSize = 3f;
            mainEngine.startColor = Color.yellow;
            boost -= 0.01f;
        }
    }
    //public void Gliding()
    //{

    //    //float speedBoost = -0.2f*flightDirection;
    //    //glide -= transform.forward.z * Time.deltaTime * speed+1*speedBoost;
    //    //transform.position += transform.forward * Time.deltaTime * glide * boost;
    //}

    
    public void Rotating()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(-verticalInput * Time.deltaTime * rotationSpeed, 0f, -horizontalInput * Time.deltaTime * rotationSpeed);
        float leanAngle = transform.localEulerAngles.z;

        // Clamp the lean angle to a range of -45 to 45 degrees
        if (leanAngle > 180) leanAngle -= 360;
        leanAngle = Mathf.Clamp(leanAngle, -90f, 90f);

        // Add movement in the direction of lean
        transform.position -= transform.right * leanAngle * Time.deltaTime * rotationPushSpeed;

    }

    //public void OnCollisionEnter(Collision other)
    //{
    //    if (other.transform.CompareTag("Ground"))
    //    {
    //        gameManager.isDead = true;
    //        gameObject.SetActive(false);
    //    }
    //}


    //public void Shooting()
    //    {
    //        //...

    //        // Check if the primary fire button has been pressed
    //        if (Input.GetKey(KeyCode.Mouse0)&& fireRate<=0)
    //        {
    //            // Aim the gun cylinder at the crosshair
    //            gunCylinder.transform.LookAt(aim);
                
    //            // Determine which projectile prefab to use
    //            GameObject projectilePrefabToUse = isNetting ? nettingProjectilePrefab : defaultProjectilePrefab;

    //            // Create a new projectile at the position of the gun cylinder
    //            GameObject bullet = Instantiate(projectilePrefabToUse, gunCylinder.transform.position, Quaternion.identity);

    //            // Aim the projectile at the crosshair
    //            bullet.transform.LookAt(aim);

    //            // Get the rigidbody component of the projectile
    //            Rigidbody rb = bullet.GetComponent<Rigidbody>();

    //            // Calculate the direction from the gun cylinder to the crosshair
    //            Vector3 direction = aim - gunCylinder.transform.position;

    //            // Calculate the force to apply to the projectile
    //            Vector3 movingStep = direction.normalized * force;

    //            // Apply the force to the projectile
    //            rb.AddForce(movingStep, ForceMode.Impulse);
    //            fireRate += 0.2f;
    //        }
    //    }

    //public void Netting()
    //{
    //    // Check if the netting button has been pressed
    //    if (Input.GetKeyDown(KeyCode.N))
    //    {
    //        // If the netting button is pressed, toggle the isNetting flag
    //        isNetting = !isNetting;
    //    }
    //    if (isNetting)
    //    {
    //        // If netting is active, set the crosshair sprite to the netting crosshair
    //        crossHair.sprite = nettingCrossHairSprite;
    //    }
    //    else
    //    {
    //        // If netting is not active, set the crosshair sprite to the default crosshair
    //        crossHair.sprite = defaultCrossHairSprite;
    //    }

    //    SlowTime(isNetting);
    //}
    //public void SlowTime(bool slow)
    //{
    //    if (slow)
    //    {
    //        // If slow is true, check if there is enough energy to slow time
    //        if (currentEnergy > 0)
    //        {
    //            // If there is enough energy, decrease the energy level and slow time
    //            currentEnergy -= Time.deltaTime*energyDepletionRate;
    //            Time.timeScale = 0.5f;
    //        }
    //        else
    //        {
    //            // If there is not enough energy, set the time scale back to 1
    //            Time.timeScale = 1;
    //        }
    //    }
    //    else
    //    {
    //        // If slow is false, recharge the energy level and set the time scale back to 1
    //        currentEnergy += Time.deltaTime;
    //        Time.timeScale = 1;
    //    }
    //}
    public void DoBarrelRoll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isPerformingBarrelRoll = true;
            rollDirection = Random.Range(0f, 1f);
        }
        if (isPerformingBarrelRoll == true)
        {
            barrelRollTimer += Time.deltaTime;

            if (barrelRollTimer < barrelRollMaxTime)
            {


                if (rollDirection > 0.5f)
                {
                    transform.Rotate(Vector3.forward, barrelRollSpeed * Time.deltaTime);
                    rightEngine.startSize = 2f;
                    rightEngine.startColor = Color.cyan;
                }
                else
                {
                    transform.Rotate(Vector3.back, barrelRollSpeed * Time.deltaTime);
                    leftEngine.startSize = 2f;
                    leftEngine.startColor = Color.cyan;
                }
            }
            else
            {
                rightEngine.startSize = 0;
                rightEngine.startColor = Color.yellow;
                leftEngine.startSize = 0;
                leftEngine.startColor = Color.yellow;
                isPerformingBarrelRoll = false;
                barrelRollTimer = 0f;
            }
        }

        

    }
    //public void CameraSwitch()
    //{
       
    //        // Switch the cameras on button click
    //        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
    //        backCamera.gameObject.SetActive(!backCamera.gameObject.activeSelf);
        
    //}
}
    


