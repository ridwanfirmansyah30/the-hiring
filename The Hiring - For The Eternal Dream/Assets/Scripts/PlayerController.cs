using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Private
    private DataCenter dataCenter;
    private CinemachineImpulseSource impulseSource;
    private Animator anim;
    private GameObject interactableObject;
    private float horiz;
    private float defaultShowTextDuration = 0.0f;
    private Collider2D[] coll2D;
    private Vector2 turnOn = Vector2.zero;
    private int itemCounter = 0;

    [Header("Object")]
    public MainManager mainManager;
    public BoundariesManager boundariesManager;
    public Rigidbody2D rb;
    public GameObject character;
    public GameObject exclamationMark;
    public GameObject informationPanel;
    public TextMeshProUGUI textUI;

    [Space(10)]
    [Header("Boolean")]
    public bool notAllowedMove = false;
    public bool finish = false;
    public bool showText = false;

    [Space(10)]
    [Header("Number")]
    public float movementSpeed;
    public float showTextDuration = 0.0f;

    [Space(10)]
    [Header("Array")]
    public List<AudioSource> soundEffects;


    private void Awake() {
        dataCenter = GameObject.FindGameObjectWithTag("DataCenter").GetComponent<DataCenter>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        anim = character.GetComponent<Animator>();

        defaultShowTextDuration = showTextDuration;
        this.transform.position = DataCenter.activeSpawnPos;

        for (int i = 0; i < dataCenter.playerSpawnPos.Length; i++) {
            if (dataCenter.playerSpawnPos[i] == DataCenter.activeSpawnPos) {
                switch (i) {
                    default:
                        character.transform.eulerAngles = new Vector2(0, 0);
                    break;
                    case 1:
                        character.transform.eulerAngles = new Vector2(0, 180);
                    break;
                }
            }
        }
    }

    private void Start() {
        coll2D = this.gameObject.GetComponents<Collider2D>();

        notAllowedMove = DataCenter.onCutScene;
        if (DataCenter.onCutScene) {
            StartCoroutine(AwakeAnimation());
        } else {
            mainManager.SetGameplayUI(true);
        }
    }

    private void FixedUpdate() {
        if (!finish) { rb.velocity = new Vector2(horiz, rb.velocity.y); }

        informationPanel.SetActive(showText);
    }

    private void Update() {
        if (!finish) {
            this.transform.position = new Vector2(
                Mathf.Clamp(
                    this.transform.position.x, 
                    boundariesManager.points[0].x, 
                    boundariesManager.points[1].x
                ), 
                this.transform.position.y
            );
        } else {
            rb.velocity = new Vector2(turnOn.x * movementSpeed, rb.velocity.y);
        }

        if (!notAllowedMove) {
            horiz = Controller() * movementSpeed;

            switch (horiz) {
                default:
                    anim.SetBool("Run", false);
                break;
                case < -0.1f or > 0.1f:
                    anim.SetBool("Run", true);
                    switch (horiz) {
                        case < -0.1f:
                            character.transform.eulerAngles = new Vector2(0, 180);
                        break;
                        case > 0.1f:
                            character.transform.eulerAngles = new Vector2(0, 0);
                        break;
                    }
                break;
            }
        } else {
            horiz = 0.0f;
        }

        GlitchHandler();

        if (exclamationMark.activeSelf) {
            if (Input.GetKeyDown(KeyCode.F) || CrossPlatformInputManager.GetButtonDown("Interact")) {
                Interact();
            }
        }

        if (showText) {
            if (showTextDuration > 0.0f) {
                showTextDuration -= Time.deltaTime;
            } else {
                showTextDuration = defaultShowTextDuration;
                showText = false;
            }
        }
        
    }

    private IEnumerator AwakeAnimation() {
        mainManager.SetGameplayUI(false);
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.SetBool("Awake", true);

        yield return new WaitForSeconds(5.0f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.SetBool("Awake", false);
        anim.SetBool("Stand", true);

        yield return new WaitForSeconds(2.5f);
        anim.SetBool("Stand", false);

        yield return new WaitUntil(() => mainManager.camAnimFinished);
        DataCenter.onCutScene = false;
        notAllowedMove = false;
        mainManager.SetGameplayUI(true);

        switch (SystemInfo.deviceType) {
            case DeviceType.Unknown or DeviceType.Handheld:
                mainManager.instruction.SetActive(false);
            break;
            case DeviceType.Desktop:
                mainManager.instruction.SetActive(true);
            break;
        }

        yield break;
    }

    private void Interact() {
        if (interactableObject != null) {
            switch (interactableObject.tag) {
                case "InteractableObject":
                    Interactable interactable = interactableObject.GetComponent<Interactable>();

                    if (interactable.attachment.Count > 0) {
                        interactable.SpawnItem();
                    } else {
                        showText = true;
                        textUI.text = interactable.information;
                    }

                    exclamationMark.SetActive(false);

                    if (mainManager.gameplayUI.activeSelf) {
                        mainManager.interactUI.SetActive(false);
                    }
                break;
                case "Item":
                    ItemWorld itemWorld = interactableObject.GetComponent<ItemWorld>();

                    if (itemCounter < dataCenter.items.Length - 1) {
                        int limit = 1;
                        while (limit > 0) {
                            if (dataCenter.items[itemCounter].itemID == DataCenter.Items.ItemID.Empty) {
                                dataCenter.items[itemCounter].itemID = (DataCenter.Items.ItemID)itemWorld.itemID + 1;
                                itemWorld.DestroyThisObject();
                                limit = 0;
                            } else {
                                itemCounter++;
                            }
                        }
                    } else {
                        showText = true;
                        textUI.text = "Penyimpanan Sudah Penuh";
                    }
                    
                break;
            }
        } else {
            Debug.LogWarning("Interactable Object is null but you still tried to access it!");
        }
    }

    private IEnumerator Finished() {
        anim.SetBool("Run", true);

        yield return new WaitForSeconds(3f);
        LoadingManager.sceneName = "Game";
        SceneManager.LoadScene("Loading");
    }

    public AudioSource SoundEffect(string idSound) {
        AudioSource thisSound = new AudioSource();

        for (int i = 0; i < soundEffects.Count; i++) {
            if (soundEffects[i].gameObject.name.Contains(idSound)) {
                thisSound = soundEffects[i];
            }
        }

        return thisSound;
    }

    private void GlitchHandler() {
        if (this.transform.position.y > 50) {
            transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -100, 50));
        }

        if (this.transform.position.y < -50 || this.transform.position.y > 50) {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    private void SetInteractionGraphic(GameObject collidedObject, int wichIcon) {
        interactableObject = collidedObject;

        if (mainManager.gameplayUI.activeSelf) {
            for (int i = 0; i < mainManager.interactButtonIcons.Length; i++) {
                if (i == wichIcon) {
                    mainManager.interactButtonIcons[i].SetActive(true);
                } else {
                    mainManager.interactButtonIcons[i].SetActive(false);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "InteractableObject":
                SetInteractionGraphic(other.gameObject, 0);
            break;
            case "Item":
                SetInteractionGraphic(other.gameObject, 1);
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "WorldEndLeft":
                turnOn = Vector2.left;
                finish = true;
                notAllowedMove = true;
                //rb.bodyType = RigidbodyType2D.Kinematic;
                StartCoroutine(Finished());
                DataCenter.stage--;
                DataCenter.activeSpawnPos = dataCenter.playerSpawnPos[1];
            break;
            case "WorldEndRight":
                turnOn = Vector2.right;
                finish = true;
                notAllowedMove = true;
                //rb.bodyType = RigidbodyType2D.Kinematic;
                StartCoroutine(Finished());
                DataCenter.stage++;
                DataCenter.activeSpawnPos = dataCenter.playerSpawnPos[0];
            break;
            case "InteractableObject" or "Item":
                exclamationMark.SetActive(true);

                if (mainManager.gameplayUI.activeSelf) {
                    mainManager.interactUI.SetActive(true);
                }
            break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "InteractableObject" or "Item":
                exclamationMark.SetActive(false);
                interactableObject = null;

                if (mainManager.gameplayUI.activeSelf) {
                    mainManager.interactUI.SetActive(false);
                }
            break;
        }
    }

    public float Controller() {
        float thisController = 0.0f;

        switch (SystemInfo.deviceType) {
            case DeviceType.Unknown or DeviceType.Handheld:
                thisController = CrossPlatformInputManager.GetAxis("Horizontal");
            break;
            case DeviceType.Desktop:
                thisController = Input.GetAxis("Horizontal");
            break;
        }

        return thisController;
    }

}
