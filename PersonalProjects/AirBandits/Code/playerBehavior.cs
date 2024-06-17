using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class playerBehavior : NetworkBehaviour
{
    public GameObject Camera;
    public GameObject networkManager;

    [Space(15)]

    //UI
    public GameObject Canvas;
    public GameObject interactText;
    [Space(10)]
    public GameObject takeOffButton;
    [Space(10)]
    public GameObject cargoMenuButton;
    public GameObject cargoPanel;
    [Space(10)]
    public GameObject orderButton;
    public GameObject orderPanel;
    [Space(10)]
    //claim order buttons on cargo panel
    public GameObject nevinButton;
    public GameObject westmountButton;
    public GameObject cliffordButton;
    public GameObject cancelButton;

    //checkboxes for cargo panel
    public GameObject nevinCheckBox;
    public GameObject westmountCheckBox;
    public GameObject cliffordCheckBox;
    [Space(10)]
    //displays the current order information on the order panel
    public GameObject orderLocationText;
    public GameObject cargoOrderText;
    public GameObject deliveryButton;
    [Space(10)]
    public GameObject deathPanel;
    public GameObject deathGoldText;
    public GameObject respawnButton;
    public GameObject deathMainMenuButton;
    [Space(10)]
    public GameObject bullet;
    public GameObject bulletSpawn;

    [Space(15)]

    //HUD elements on the cargo panel
    public Sprite boxSprite;
    public Sprite checkedBoxSprite;

    //Player Sprites
    public Sprite playerSprite;
    public Sprite airplaneSprite;

    [Space(15)]

    [SyncVar]
    public uint playerId;

    [SyncVar]
    public int myGold;

    [SyncVar]
    public int airplaneHealth;

    [SyncVar]
    public int spriteNum;

    public int respawnGold;



    public int curCargoCount;
    public int cargoCapacity;

    [Space(15)]

    public float playerSpeed;

    //airplane
    public float airplaneSpeed;
    public float turnSpeed;

    //timer used to delay the player from taking off instantly
    private float curDelayTimer;
    public float maxDelayTime;

    public float playerCameraSize;
    public float airplaneCameraSize;

    public float bulletTimer;
    public float fireRate = .5f;

    [Space(15)]

    public Text goldText;

    [Space(15)]

    public string goldString;
    public string cargoDestination;
    public string currentIsland;

    [SyncVar]
    public string playerTag;

    [Space(15)]

    public SpriteRenderer playerRenderer;

    [Space(15)]

    public Transform playerPositionAfterLanding;

    [Space(15)]

    public Vector3 playerScale;
    public Vector3 airplaneScale;

    [Space(15)]

    public bool hasOrder;

    //true when the player is flying the airplane
    public bool airplaneMode;

    //initiates the take off timer
    public bool takingOff;

    public bool hasLeftAirspace;
    public bool hasEnteredAirspace;
    public bool startLandingTimer;

    public bool isDead;



    // ***START OF CLIENT***
    public void Start()
    {
        Camera = GameObject.Find("Main Camera");
        networkManager = GameObject.Find("NetworkManager");

        Canvas = GameObject.Find("Canvas");


        bulletSpawn = gameObject.transform.GetChild(0).gameObject;

        if (Canvas != null)
        {
                //canvas interaction setup
                interactText = Canvas.transform.GetChild(0).gameObject;
                interactText.SetActive(false);
                goldText = Canvas.transform.GetChild(1).gameObject.GetComponent<Text>();
                takeOffButton = Canvas.transform.GetChild(2).gameObject;
                cargoMenuButton = Canvas.transform.GetChild(3).gameObject;
                cargoPanel = Canvas.transform.GetChild(4).gameObject;
                orderButton = Canvas.transform.GetChild(5).gameObject;
                orderPanel = Canvas.transform.GetChild(6).gameObject;
                deathPanel = Canvas.transform.GetChild(7).gameObject;
                cargoPanel.SetActive(true);
                orderPanel.SetActive(true);
                deathPanel.SetActive(true);

                //cargo panel buttons
                nevinButton = cargoPanel.transform.GetChild(0).gameObject;
                westmountButton = cargoPanel.transform.GetChild(1).gameObject;
                cliffordButton = cargoPanel.transform.GetChild(2).gameObject;
                cancelButton = cargoPanel.transform.GetChild(3).gameObject;
                deliveryButton = cargoPanel.transform.GetChild(4).gameObject;

                //island checkboxes
                nevinCheckBox = cargoPanel.transform.GetChild(5).gameObject;
                westmountCheckBox = cargoPanel.transform.GetChild(6).gameObject;
                cliffordCheckBox = cargoPanel.transform.GetChild(7).gameObject;

                //sets up current order panel
                orderLocationText = orderPanel.transform.GetChild(0).gameObject;

                //sets up death panel 
                respawnButton = deathPanel.transform.GetChild(0).gameObject;
                deathMainMenuButton = deathPanel.transform.GetChild(1).gameObject;

                deathGoldText = respawnButton.transform.GetChild(0).gameObject;


                //setting up button functions
                takeOffButton.GetComponent<Button>().onClick.AddListener(StartTakeOffButton);
                cargoMenuButton.GetComponent<Button>().onClick.AddListener(OpenCargoMenuButton);
                nevinButton.GetComponent<Button>().onClick.AddListener(NevinOrderButton);
                westmountButton.GetComponent<Button>().onClick.AddListener(WestmountOrderButton);
                cliffordButton.GetComponent<Button>().onClick.AddListener(CliffordOrderButton);
                cancelButton.GetComponent<Button>().onClick.AddListener(CancelOrderButton);
                orderButton.GetComponent<Button>().onClick.AddListener(OrderMenuButton);
                deliveryButton.GetComponent<Button>().onClick.AddListener(DeliverOrderButton);
                respawnButton.GetComponent<Button>().onClick.AddListener(RespawnButton);


                cargoOrderText = GameObject.Find("CurrentOrder");
                cargoOrderText.GetComponent<Text>().text = "You do not have an order";
                cargoPanel.SetActive(false);
                orderPanel.SetActive(false);
                deathPanel.SetActive(false);
        }

        hasOrder = false;
        hasLeftAirspace = false;
        startLandingTimer = false;
        Camera.GetComponent<Camera>().orthographicSize = playerCameraSize;

        takingOff = false;
        airplaneMode = false;

        playerScale = gameObject.transform.localScale;
        airplaneScale = new Vector3(.5f, .5f, 1);

        playerRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteNum = 1;

        playerTag = "Player";

        if (isLocalPlayer)
        {
            SetPlayerID();
            isDead = false;

            if (!hasAuthority)
            {
                gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            }
        }
    }

    //In Game

    void Update()
    {
        gameObject.tag = playerTag;
        DisplayGold();

        ShowSprite();

        CurrentOrderUpdate();
        ShowOrderButton();


        if (isLocalPlayer)
        { 
            ShootBullet();
            CheckHealth();
        }

        if (takingOff == true)
        {
            TakeOffTimer();
        }

        if (hasEnteredAirspace == true && airplaneMode == true)
        {
            LandingTimer(currentIsland);
        }
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        AirplaneMovement();
    }

    public void PlayerMovement()
    {
        if (isLocalPlayer && airplaneMode == false && isDead == false)
        {
            float vertical = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;
            float horizontal = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
            Vector3 movement = new Vector3(horizontal, vertical, 0);
            transform.position += movement;
            Camera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);
        }
    }

    public void AirplaneMovement()
    {
        if (isLocalPlayer && airplaneMode == true && isDead == false)
        {
            transform.Translate(Vector2.up * Time.deltaTime * airplaneSpeed);

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * turnSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(-Vector3.forward * Time.deltaTime * turnSpeed);
            }

            Camera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);
        }
    }

    //Functions//

    [Command]
    public void SetPlayerID()
    {
        playerId = gameObject.GetComponent<NetworkIdentity>().netId;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Nevin")
        {
            currentIsland = "Nevin Airport";
            playerPositionAfterLanding = collision.transform.Find("LandSpawn");

            if (airplaneMode == true && hasLeftAirspace == true)
            {
                hasEnteredAirspace = true;
            }
        }

        if (collision.tag == "Westmount")
        {
            currentIsland = "West Mount";
            playerPositionAfterLanding = collision.transform.Find("LandSpawn");

            if (airplaneMode == true && hasLeftAirspace == true)
            {
                hasEnteredAirspace = true;
            }
        }

        if (collision.tag == cargoDestination && airplaneMode == false)
        {
            deliveryButton.SetActive(true);
            cancelButton.SetActive(false);
        }

        else
        {
            deliveryButton.SetActive(false);
            cancelButton.SetActive(true);
        }

        if (collision.tag == "Nevin")
        {
            gameObject.GetComponent<Rigidbody2D>().WakeUp();
            nevinButton.SetActive(false);
        }

        if (collision.tag == "Westmount")
        {
            gameObject.GetComponent<Rigidbody2D>().WakeUp();
            westmountButton.SetActive(false);
        }

        if (collision.tag == "Clifford")
        {
            gameObject.GetComponent<Rigidbody2D>().WakeUp();
            cliffordButton.SetActive(false);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Nevin" || collision.tag == "Westmount")
        {
            hasLeftAirspace = true;
            hasEnteredAirspace = false;
            startLandingTimer = false;
            interactText.SetActive(false);          
        }
    }

    public void DisplayGold()
    {
        if (isLocalPlayer)
        {
            goldString = myGold.ToString();
            goldText.text = goldString;
        }
    }

    public void ShowSprite()
    {
        if (spriteNum == 1)
        {
            playerRenderer.sprite = playerSprite;
        }

        if (spriteNum == 2)
        {
            playerRenderer.sprite = airplaneSprite;
        }

        if (spriteNum == 3)
        {
            playerRenderer.sprite = null;
        }
    }

    [Command]
    public void SwitchSprite(int num)
    {
        spriteNum = num;
    }

    [Command]
    public void SwitchTag(string tag)
    {
        playerTag = tag;
    }

    [Command]
    public void SetHealth(int health)
    {
        airplaneHealth = health;
    }

    public void TakeOffTimer()
    {
        interactText.GetComponent<Text>().text = "You will take off in: " + Mathf.CeilToInt(curDelayTimer).ToString();

        curDelayTimer -= Time.deltaTime;
        if (curDelayTimer <= 0)
        {
            TakeOff();
        }
    }

    public void LandingTimer(string airport)
    {
        if (startLandingTimer == false)
        {
            curDelayTimer = 10;
            startLandingTimer = true;
        }
        interactText.SetActive(true);
        curDelayTimer -= Time.deltaTime;

        if (curDelayTimer <= 0)
        {
            interactText.GetComponent<Text>().text = "Press 'F' to land";
            if (Input.GetKeyDown(KeyCode.F))
            {
                LandAirplane();
            }
        }
        else
        {
            interactText.GetComponent<Text>().text = "You have entered the airspace of " + airport + ". You are clear to land in " + Mathf.CeilToInt(curDelayTimer).ToString() + " seconds";
        }     
    }

    public void TakeOff()
    {
        gameObject.transform.localScale = airplaneScale;
        
        airplaneMode = true;
        takingOff = false;
        interactText.SetActive(false);
        cargoMenuButton.SetActive(false);
        cargoPanel.SetActive(false);
        TakeOffForServer(true);

        Physics2D.IgnoreLayerCollision(9, 8, true);
        SwitchSprite(2);
        Camera.GetComponent<Camera>().orthographicSize = airplaneCameraSize;
    }

    public void LandAirplane()
    {
        gameObject.transform.localScale = playerScale;
        gameObject.transform.rotation = Quaternion.identity;
        
        gameObject.transform.position = playerPositionAfterLanding.transform.position;

        airplaneMode = false;
        startLandingTimer = false;
        interactText.SetActive(false);
        cargoMenuButton.SetActive(true);
        takeOffButton.SetActive(true);
        cargoPanel.SetActive(false);
        hasLeftAirspace = false;
        hasEnteredAirspace = false;
        TakeOffForServer(false);

        Physics2D.IgnoreLayerCollision(9, 8, false);
        SwitchSprite(1);
        Camera.GetComponent<Camera>().orthographicSize = playerCameraSize;
    }

    public void RemoveCheckmarks()
    {
        nevinCheckBox.GetComponent<Image>().sprite = boxSprite;
        westmountCheckBox.GetComponent<Image>().sprite = boxSprite;
        cliffordCheckBox.GetComponent<Image>().sprite = boxSprite;
    }

    public void CurrentOrderUpdate()
    {
        orderLocationText.GetComponent<Text>().text = cargoOrderText.GetComponent<Text>().text;
    }

    public void ShowOrderButton()
    {
        if (hasOrder == true)
        {
            orderButton.SetActive(true);
        }

        else
        {
            orderButton.SetActive(false);
            orderPanel.SetActive(false);
        }
    }

    public void ReactivateOrderButtons()
    {
        nevinButton.SetActive(true);
        westmountButton.SetActive(true);
        cliffordButton.SetActive(true);
    }

    public void DisableOrderButtons()
    {
        nevinButton.SetActive(false);
        westmountButton.SetActive(false);
        cliffordButton.SetActive(false);
    }

    [Command]
    public void TakeOffForServer(bool x)
    {
        Physics2D.IgnoreLayerCollision(9, 8, x);
    }
    
    public void ShootBullet()
    {
        if (airplaneMode == true)
        { 
            if (bulletTimer <= fireRate)
            {
                bulletTimer += Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (bulletTimer >= fireRate)
                {
                    bulletTimer = 0;
                    GameObject Bullet = Instantiate(bullet, bulletSpawn.transform.position, transform.rotation);
                    NetworkServer.Spawn(Bullet);
                    Bullet.GetComponent<BulletBehavior>().fromPlayerId = playerId;
                }
            }
        }
    }

    public void CheckHealth()
    {
        if (airplaneHealth <= 0)
        {
                isDead = true;
                deathPanel.SetActive(true);
                respawnGold = (int)Mathf.Round(myGold * .25f);
                deathGoldText.GetComponent<Text>().text = respawnGold.ToString() + " gold";
                DeathBehavior();        
        }
    }

    public void DeathBehavior()
    {
        SwitchTag("dead");
        SwitchSprite(3);
    }

    [Command]
    public void UpdateGoldOnRespawn(int spawnGold)
    {
        myGold = spawnGold;
    }

    //Buttons//
    public void StartTakeOffButton()
    {
        if (isLocalPlayer)
        {
            takingOff = true;
            interactText.SetActive(true);
            curDelayTimer = maxDelayTime;
            takeOffButton.SetActive(false);          
        }
    }

    public void OpenCargoMenuButton()
    {
        if (isLocalPlayer)
        {
            if (cargoPanel.activeInHierarchy == false)
            {
                cargoPanel.SetActive(true);
            }

            else
                cargoPanel.SetActive(false);
        }
    }

    public void NevinOrderButton()
    {
        if (isLocalPlayer)
        {
            curCargoCount = cargoCapacity;
            cargoDestination = "Nevin";
            cargoOrderText.GetComponent<Text>().text = "Delivering " + curCargoCount + " crates to Nevin Airport";
            RemoveCheckmarks();
            nevinCheckBox.GetComponent<Image>().sprite = checkedBoxSprite;
            hasOrder = true;
            DisableOrderButtons();

        }
    }

    public void WestmountOrderButton()
    {
        if (isLocalPlayer)
        {
            curCargoCount = cargoCapacity;
            cargoDestination = "Westmount";
            cargoOrderText.GetComponent<Text>().text = "Delivering " + curCargoCount + " crates to Westmount";
            RemoveCheckmarks();
            westmountCheckBox.GetComponent<Image>().sprite = checkedBoxSprite;
            hasOrder = true;
            DisableOrderButtons();

        }
    }

    public void CliffordOrderButton()
    {
        if (isLocalPlayer)
        {
            curCargoCount = cargoCapacity;
            cargoDestination = "Clifford";
            cargoOrderText.GetComponent<Text>().text = "Delivering " + curCargoCount + " crates to Clifford Airfield";
            RemoveCheckmarks();
            cliffordCheckBox.GetComponent<Image>().sprite = checkedBoxSprite;
            hasOrder = true;
            DisableOrderButtons();
        }
    }

    public void CancelOrderButton()
    {
        curCargoCount = 0;
        cargoDestination = "";
        cargoOrderText.GetComponent<Text>().text = "You do not have an order";
        RemoveCheckmarks();
        hasOrder = false;
        ReactivateOrderButtons();
    }

    public void DeliverOrderButton()
    {
        curCargoCount = 0;
        cargoDestination = "";
        cargoOrderText.GetComponent<Text>().text = "You do not have an order";
        RemoveCheckmarks();
        hasOrder = false;
        ReactivateOrderButtons();
        myGold += 1000;
    }

    public void OrderMenuButton()
    {
        if (isLocalPlayer)
        {
            if (orderPanel.activeInHierarchy == false)
            {
                orderPanel.SetActive(true);
            }

            else
                orderPanel.SetActive(false);
        }
    }

    public void RespawnButton()
    {
        if (isLocalPlayer)
        {
            airplaneHealth = 100;
            deathPanel.SetActive(false);
            gameObject.transform.position = playerPositionAfterLanding.transform.position;
            gameObject.transform.localScale = playerScale;
            gameObject.transform.rotation = Quaternion.identity;
            Camera.GetComponent<Camera>().orthographicSize = playerCameraSize;
            Physics2D.IgnoreLayerCollision(9, 8, false);
            interactText.SetActive(false);
            airplaneMode = false;
            isDead = false;
            SetHealth(100);
            SwitchTag("Player");
            SwitchSprite(1);
            UpdateGoldOnRespawn(respawnGold);
            hasLeftAirspace = false;
            hasEnteredAirspace = false;
            cargoMenuButton.SetActive(true);
            takeOffButton.SetActive(true);
        }
    }
}