using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;
    [SerializeField] float playerScale;
    

    [Header("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
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
    public bool isBoosting;
    
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
    public bool isSlowingTime = false;
    public GameObject nettingProjectilePrefab;
    public float maxEnergy = 100;
    public float currentEnergy;
    public float energyDepletionRate;

    [Header("Environment")]
    public int topBounds = 3000;
    public GameObject activeTerrain;
    [SerializeField] GameObject miniMapSprite;
    [SerializeField] GameObject explosionVFX;


    void Start()
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
        gameManager = FindObjectOfType<GameManager>();
        mainCamera.gameObject.SetActive(true);
        backCamera.gameObject.SetActive(false);
        
        activeTerrain = FindObjectOfType<PerlinNoiseMapChatGPT>().gameObject;
        topBounds *= FindObjectOfType<PerlinNoiseMapChatGPT>().mapScale;
        
        crossHair = GetComponentInChildren<Image>();
        Vector3 playerSpawnPos = new Vector3(activeTerrain.gameObject.GetComponent<Renderer>().bounds.center.x , 30 * activeTerrain.GetComponent<PerlinNoiseMapChatGPT>().mapScale, activeTerrain.gameObject.GetComponent<Renderer>().bounds.center.z-30* FindObjectOfType<PerlinNoiseMapChatGPT>().mapScale);

        transform.position = playerSpawnPos;
        gunCylinder.GetComponent<Transform>();
        Physics.IgnoreLayerCollision(8, 9);
        
    }

    private void Update()
    {
        miniMapSprite.transform.position = new Vector3(transform.position.x, 50, transform.position.z);
        miniMapSprite.transform.rotation = Quaternion.Euler(90, 0, 0);
        fireRate -= Time.deltaTime;
        if (fireRate<0)
        {
            fireRate = 0;
        }
        PlayAudio();
        Shooting();
        IsSlowingTime();
        DoBarrelRoll();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Bounds terrainBounds = activeTerrain.GetComponent<Renderer>().bounds;
        OutOfBounds(terrainBounds);

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
        flightDirection = Vector3.Dot(gameObject.transform.forward, Vector3.down);
        if (Mathf.Abs(flightDirection) < 0.1f)
        {
            flightDirection = Vector3.Dot(gameObject.transform.right, Vector3.down);
        }
        if (!isPerformingBarrelRoll)
        {
            
            Rotating();
            
        }
        
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        if (transform.position.y > topBounds)
        {
            transform.position = new Vector3(transform.position.x, topBounds, transform.position.z);
        }
        if (glide < 30)
        {
            glide = 30;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CameraSwitch();
        }
        
        if (mainCamera.isActiveAndEnabled)
        {
            UpdateAim();
        }
        Gliding();
        Boosting();
        
        
        


    }
   
    void UpdateAim()
    {
        
        Vector3 mousePos = Input.mousePosition;
        int screenLimitX = 700;
        int screenLimitY = 200;
        mousePos += mainCamera.transform.forward * 30f;
        //mouseX = Mathf.Clamp(mousePos.x, screenLimitX, Screen.width - screenLimitX);
        //mouseY = Mathf.Clamp(mousePos.y, screenLimitY, Screen.height - screenLimitY);
        //aim = mainCamera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 1000f));
        aim = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1000f));
        /*crossHair.transform.position = new Vector3(mouseX, mouseY, 1000f);*/
        crossHair.transform.position = mousePos;
        
        
    }



    public void Boosting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            mainEngineMat.SetColor("_EmissionColor", Color.cyan);
            mainEngine.startSize = 5f;
            mainEngine.startColor = Color.cyan;
            boost += 0.01f;
            isBoosting = true;
            
        }
        if (!Input.GetKey(KeyCode.Space))
        {
            mainEngineMat.SetColor("_EmissionColor", Color.yellow);
            mainEngine.startSize = 3f;
            mainEngine.startColor = Color.yellow;
            boost -= 0.01f;
            isBoosting = false;


        }
        if (boost<1)
        {
            boost = 1;
        }
    }
    public void Gliding()
    {

        float speedBoost = -0.2f*flightDirection;
        glide -= transform.forward.z * Time.deltaTime * speed+1*speedBoost*playerScale;
        transform.position += transform.forward * Time.deltaTime * glide * boost*playerScale;
        
    }

    
    public void Rotating()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(-verticalInput * Time.deltaTime * rotationSpeed, 0f, -horizontalInput * Time.deltaTime * rotationSpeed*playerScale);
        float leanAngle = transform.localEulerAngles.z;

        // Clamp the lean angle to a range of -45 to 45 degrees
        if (leanAngle > 180) leanAngle -= 360;
        leanAngle = Mathf.Clamp(leanAngle, -90f, 90f);

        // Add movement in the direction of lean
        transform.position -= transform.right * leanAngle * Time.deltaTime * rotationPushSpeed*playerScale;

    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground")||other.transform.CompareTag("AlienShip"))
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            gameManager.EndGame("You crashed, your score is :"+gameManager.score);
            gameObject.SetActive(false);
        }

    }


    public void Shooting()
    {
        // Check if the primary fire button (mouse0) or the secondary fire button (mouse1) has been pressed
        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) && fireRate <= 0)
        {
            // Aim the gun cylinder at the crosshair
            gunCylinder.transform.LookAt(aim);

            // Determine which projectile prefab to use based on which mouse button was pressed
            GameObject projectilePrefabToUse = Input.GetKey(KeyCode.Mouse0) ? defaultProjectilePrefab : nettingProjectilePrefab;

            // Create a new projectile at the position of the gun cylinder
            GameObject bullet = Instantiate(projectilePrefabToUse, gunCylinder.transform.position, Quaternion.identity);

            // Aim the projectile at the crosshair
            bullet.transform.LookAt(aim);

            // Get the rigidbody component of the projectile
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            // Calculate the direction from the gun cylinder to the crosshair
            Vector3 direction = aim - gunCylinder.transform.position;

            // Calculate the force to apply to the projectile
            Vector3 movingStep = direction.normalized * force;

            // Apply the force to the projectile
            rb.AddForce(movingStep, ForceMode.Impulse);

            // Play the appropriate audio clip and increment the fire rate
            if (projectilePrefabToUse == defaultProjectilePrefab)
            {
                audioSource.volume = 0.6f;
                audioSource.PlayOneShot(audioClips[1]);
            }
            if (projectilePrefabToUse == nettingProjectilePrefab)
            {
                audioSource.volume = 0.6f;
                audioSource.PlayOneShot(audioClips[3]);
            }
            fireRate += 0.2f;
        }
    }

    public void IsSlowingTime()
    {
        // Check if the netting button has been pressed
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            // If the netting button is pressed, toggle the isNetting flag
            isSlowingTime = !isSlowingTime;
        }
        if (isSlowingTime)
        {
            // If netting is active, set the crosshair sprite to the netting crosshair
            crossHair.sprite = nettingCrossHairSprite;
        }
        else
        {
            // If netting is not active, set the crosshair sprite to the default crosshair
            crossHair.sprite = defaultCrossHairSprite;
            currentEnergy += Time.deltaTime*2.5f;
        }

        SlowTime(isSlowingTime);
    }
    public void SlowTime(bool slow)
    {
        if (slow)
        {
            // If slow is true, check if there is enough energy to slow time
            if (currentEnergy > 0)
            {
                // If there is enough energy, decrease the energy level and slow time
                currentEnergy -= Time.deltaTime*energyDepletionRate;
                Time.timeScale = 0.5f;
                
            }
            else if (!gameManager.gameIsPaused)
            {
                // If there is not enough energy, set the time scale back to 1
                Time.timeScale = 1;
            }
        }
        else if (!gameManager.gameIsPaused)
        {
            // If slow is false, recharge the energy level and set the time scale back to 1
            Time.timeScale = 1;
        }
    }
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
                    //play SFX
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
    public void CameraSwitch()
    {
       
            // Switch the cameras on button click
            mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
            backCamera.gameObject.SetActive(!backCamera.gameObject.activeSelf);
        
    }
    private void PlayAudio()
    {

        // Check if the player is gliding
        if (!isBoosting)
        {
            if (!audioSource.isPlaying)
            {
                // Play the first audio clip on a loop
                audioSource.clip = audioClips[2];
                audioSource.volume = 0.3f;
                audioSource.loop = true;
                audioSource.Play();
            }
            
        }
        // Check if the player is boosting
        else if (isBoosting)
        {
            if (!audioSource.isPlaying)
            {
                // Play the third audio clip once and on a loop
                audioSource.clip = audioClips[2];
                audioSource.volume += 0.3f;
                audioSource.loop = true;
                audioSource.Play();
            }
                
            }
            
            
        }
    public void OutOfBounds(Bounds terrainBounds)
    {

        // Limit the player's position to within the bounds of the terrain
        if (transform.position.x < terrainBounds.min.x)
        {
            transform.position = new Vector3(terrainBounds.min.x, transform.position.y, transform.position.z);
        }
        if (transform.position.x > terrainBounds.max.x)
        {
            transform.position = new Vector3(terrainBounds.max.x, transform.position.y, transform.position.z);
        }
        if (transform.position.z < terrainBounds.min.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, terrainBounds.min.z);
        }
        if (transform.position.z > terrainBounds.max.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, terrainBounds.max.z);
        }
    }
        // Otherwise, stop any audio that is playing
        //else
        //{
        //    audioSource.Stop();
        //}
    }

    


