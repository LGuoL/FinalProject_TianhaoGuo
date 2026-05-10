using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Shop Unlock")]
    public bool hasSeenDay3ShopDialogue = false;
    public bool specialShopUnlocked = false;

    [Header("Progress")]
    public int currentDay = 1;
    public int money = 0;

    [Header("Order")]
    public int currentOrderRequired = 8;
    public int currentOrderSubmitted = 0;

    [Header("Fishing Day Settings")]
    public float fishingTimeLimit = 180f;

    private string targetSpawnPointName = "ShopSpawnPoint";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        GenerateOrderForDay();

        // 初始场景也连接一次
        MovePlayerToSpawnPoint();
        ReconnectSceneReferences();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public bool NeedDay3ShopDialogue()
    {
        return currentDay >= 3 && !hasSeenDay3ShopDialogue;
    }

    public void UnlockSpecialShop()
    {
        hasSeenDay3ShopDialogue = true;
        specialShopUnlocked = true;

        Debug.Log("Special shop unlocked.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MovePlayerToSpawnPoint();
        ReconnectSceneReferences();
    }

    public void GenerateOrderForDay()
    {
        currentOrderSubmitted = 0;

        switch (currentDay)
        {
            case 1:
                currentOrderRequired = 8;
                break;
            case 2:
                currentOrderRequired = 12;
                break;
            case 3:
                currentOrderRequired = 20;
                break;
            case 4:
                currentOrderRequired = 45;
                break;
            case 5:
                currentOrderRequired = 80;
                break;
            default:
                currentOrderRequired = 120 + (currentDay - 6) * 30;
                break;
        }
    }

    public bool IsOrderComplete()
    {
        return currentOrderSubmitted >= currentOrderRequired;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount)
            return false;

        money -= amount;
        return true;
    }

    public void SubmitFish(int amount)
    {
        currentOrderSubmitted += amount;

        if (currentOrderSubmitted > currentOrderRequired)
            currentOrderSubmitted = currentOrderRequired;
    }

    public void GoToFishingScene()
    {
        targetSpawnPointName = "FishingSpawnPoint";
        SceneManager.LoadScene("FishingArea");
    }

    public void ReturnToShopScene()
    {
        targetSpawnPointName = "ShopSpawnPoint";
        SceneManager.LoadScene("ShopArea");
    }

    public void AdvanceToNextDay()
    {
        currentDay++;
        GenerateOrderForDay();
        ReturnToShopScene();
    }

    public void FailDayAndBackToMenu()
    {

        hasSeenDay3ShopDialogue = false;
        specialShopUnlocked = false;

        Debug.Log("订单失败，重置游戏");

        currentDay = 1;
        money = 0;
        GenerateOrderForDay();

        ReturnToShopScene();
    }

    private void MovePlayerToSpawnPoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("没有找到 Player，请确认 Player 的 Tag 是 Player");
            return;
        }

        if (string.IsNullOrEmpty(targetSpawnPointName))
            return;

        GameObject spawnObj = GameObject.Find(targetSpawnPointName);

        if (spawnObj == null)
        {
            Debug.LogWarning("没有找到出生点：" + targetSpawnPointName);
            return;
        }

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.transform.position = spawnObj.transform.position;
        player.transform.rotation = spawnObj.transform.rotation;

        if (controller != null)
            controller.enabled = true;
    }

    private void ReconnectMerchant(BucketSystem bucketSystem)
    {
        MerchantNPC merchant = FindFirstObjectByType<MerchantNPC>();

        if (merchant == null)
            return;

        merchant.playerBucket = bucketSystem;
    }

    private void ReconnectPlayerEquipment(GameObject player)
    {
        PlayerEquipment equipment = player.GetComponent<PlayerEquipment>();

        if (equipment == null)
            return;

        equipment.fishingRodController = player.GetComponent<FishingRodController>();
        equipment.bucketCollector = player.GetComponent<BucketCollector>();
        equipment.weaponFishingController = player.GetComponent<WeaponFishingController>();
        equipment.inventory = player.GetComponent<InventorySystem>();
        equipment.weaponVisuals = player.GetComponent<PlayerWeaponVisuals>();

        GameObject equippedTextObj = GameObject.Find("EquippedText");

        if (equippedTextObj != null)
        {
            equipment.equipmentText = equippedTextObj.GetComponent<TMPro.TextMeshProUGUI>();
            equipment.RefreshUI();
        }
    }

    private void ReconnectSceneReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Reconnect 失败：没有找到 Player");
            return;
        }

        Camera cam = player.GetComponentInChildren<Camera>();

        if (cam == null)
        {
            Debug.LogWarning("Reconnect 失败：Player 下面没有 Camera");
        }

        InventorySystem inventory = player.GetComponent<InventorySystem>();
        BucketSystem bucketSystem = player.GetComponent<BucketSystem>();

        ReconnectPlayerInteractor(player, cam);
        ReconnectHUD(inventory, bucketSystem);
        ReconnectFishingRod(player, cam, inventory);
        ReconnectBucketCollector(player, cam, bucketSystem);
        ReconnectWeaponFishing(player, cam, inventory);
        ReconnectPlayerEquipment(player);
        ReconnectDeliveryCrate(bucketSystem);
        ReconnectFishingManager();
        ReconnectMerchant(bucketSystem);
        ReconnectPlayerEquipment(player);
        ReconnectMysteryBoxEffect(player);
    }

    private void ReconnectMysteryBoxEffect(GameObject player)
    {
        MysteryBoxEffect effect = player.GetComponent<MysteryBoxEffect>();

        if (effect == null)
            return;

        effect.inventory = player.GetComponent<InventorySystem>();

        GameObject spawnObj = GameObject.Find("MysteryFishSpawnPoint");
        GameObject targetObj = GameObject.Find("MysteryFishTargetPoint");

        if (spawnObj != null)
            effect.fishSpawnPoint = spawnObj.transform;

        if (targetObj != null)
            effect.fishTargetPoint = targetObj.transform;
    }

    private void ReconnectWeaponFishing(GameObject player, Camera cam, InventorySystem inventory)
    {
        WeaponFishingController weapon = player.GetComponent<WeaponFishingController>();

        if (weapon == null)
            return;

        weapon.playerCamera = cam;
        weapon.inventory = inventory;

        GameObject shoreSpawn = GameObject.Find("ShoreSpawnPoint");

        if (shoreSpawn != null)
        {
            weapon.shoreSpawnPoint = shoreSpawn.transform;
        }
    }

    private void ReconnectPlayerInteractor(GameObject player, Camera cam)
    {
        PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();

        if (interactor == null)
            return;

        interactor.playerCamera = cam;

        GameObject interactTextObj = GameObject.Find("InteractText");

        if (interactTextObj != null)
        {
            interactor.interactText = interactTextObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("没有找到 InteractText");
        }
    }

    private void ReconnectHUD(InventorySystem inventory, BucketSystem bucketSystem)
    {
        HUDController hud = FindFirstObjectByType<HUDController>();

        if (hud == null)
            return;

        AssignText("OrderText", ref hud.orderText);
        AssignText("MoneyText", ref hud.moneyText);
        AssignText("DayText", ref hud.dayText);
        AssignText("BucketText", ref hud.bucketText);
        AssignText("InventoryText", ref hud.inventoryText);
    }

    private void ReconnectFishingRod(GameObject player, Camera cam, InventorySystem inventory)
    {
        FishingRodController rod = player.GetComponent<FishingRodController>();

        if (rod == null)
            return;

        rod.playerCamera = cam;
        rod.inventory = inventory;

        GameObject shoreSpawn = GameObject.Find("ShoreSpawnPoint");

        if (shoreSpawn != null)
        {
            rod.shoreSpawnPoint = shoreSpawn.transform;
        }

        FishingQTE qte = FindFirstObjectByType<FishingQTE>();

        if (qte != null)
        {
            rod.fishingQTE = qte;
        }
    }

    private void ReconnectBucketCollector(GameObject player, Camera cam, BucketSystem bucketSystem)
    {
        BucketCollector collector = player.GetComponent<BucketCollector>();

        if (collector == null)
            return;

        collector.playerCamera = cam;
        collector.bucketSystem = bucketSystem;
    }

    private void ReconnectDeliveryCrate(BucketSystem bucketSystem)
    {
        DeliveryCrate crate = FindFirstObjectByType<DeliveryCrate>();

        if (crate == null)
            return;

        crate.playerBucket = bucketSystem;

        FishingManager fishingManager = FindFirstObjectByType<FishingManager>();

        if (fishingManager != null)
        {
            crate.fishingManager = fishingManager;
        }
    }

    private void ReconnectFishingManager()
    {
        FishingManager fishingManager = FindFirstObjectByType<FishingManager>();

        if (fishingManager == null)
            return;

        GameObject timerTextObj = GameObject.Find("TimerText");

        if (timerTextObj != null)
        {
            fishingManager.timerText = timerTextObj.GetComponent<TextMeshProUGUI>();
        }
    }

    private void AssignText(string objectName, ref TextMeshProUGUI targetField)
    {
        GameObject obj = GameObject.Find(objectName);

        if (obj != null)
        {
            targetField = obj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("没有找到 UI 文本：" + objectName);
        }
    }
}