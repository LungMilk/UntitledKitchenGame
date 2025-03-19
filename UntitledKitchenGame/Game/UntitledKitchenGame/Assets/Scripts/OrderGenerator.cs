using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class OrderGenerator : MonoBehaviour
{
    //this item will return a set amount of orders and we can modify it to be either focused on orders or all items.
    //for now we will make this able to set the UI elements to their respective texts.
    public Canvas orderCanvas;
    public List<TextMeshProUGUI> orderText = new List<TextMeshProUGUI>();
    public List<Image> orderImages = new List<Image>();

    public ItemCollection itemDatabase;

    List<FoodItem> foodItems = new List<FoodItem>();
    //being a prefab means the object cannot be scene referenced so we gots to find a score manager
    public int itemMax = 3;

    [SerializeField]
    private float pointData;

    public FoodType onlySelect;
    //maybe we could have the variables change as well depending on what kind of object it is?

    Dictionary<FoodItem, float> probablity = new Dictionary<FoodItem, float>();

    private void Awake()
    {
        caculateProbability();
    }

    void Start()
    {
        //GameObject tempObj = GameObject.Find("EndConManager");
        //if (tempObj != null)
        //{
        //    scoreManage = tempObj.GetComponent<ScoreManager>();
        //}
        GenerateOrder();
        
    }
    [System.Serializable]
    public struct OrderCompletionData
    {
        public string type;
        public string playerPoints;
        public string pointsFromOrder;
    }
    private void Update()
    {
        //PopulateUI();
        //if (foodItems.Count <= 0)
        //{
        //    OrderReset();
        //}
    }
    [ContextMenu("Order Reset")]
    private void OrderReset()
    {
            var data = new OrderCompletionData()
            {
                type = onlySelect.ToString(),
                playerPoints = ScoreManager.score.ToString(),
                pointsFromOrder = pointData.ToString(),
            };
        TelemetryLogger.Log(this, "order completed", data);

        //foodItems.Clear();
        GenerateOrder();
    }

    public void OrderPointCalculation()
    {
        pointData = 0;
        foreach (FoodItem item in foodItems)
        {
            pointData += item.pointValue;
        }
    }
    public void GenerateOrder()
    {
        PopulateOrder();

        PopulateUI();
        
        OrderPointCalculation();
    }

    //no longer needed but why not keep them
    [ContextMenu("Populate UI")]
    void PopulateUI()
    {
        // Adding null checks to avoid errors if the UI elements are not set up
        //if (orderCanvas == null)
        //{
        //    //Debug.LogWarning("Order Canvas is not assigned.");
        //    return;
        //}

        //if (orderText == null || orderImages == null)
        //{
        //    //Debug.LogWarning("UI lists are not assigned.");
        //    return;
        //}

        // Initialize order images and texts if not already set
        Image[] tempImageList = this.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] tempList = this.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < itemMax; i++)
        {
            orderImages.Add(tempImageList[i]);
            orderText.Add(tempList[i]);
        }
        


        for (int i = 0; i < itemMax; i++)
        {
            if (i < foodItems.Count && foodItems[i] != null && orderText[i] != null)
            {
                orderText[i].gameObject.SetActive(true);
                orderImages[i].gameObject.SetActive(true);
                orderText[i].text = foodItems[i].displayName;
                orderImages[i].sprite = foodItems[i].displayImage;
            }
            else if (orderText[i] != null)
            {
                orderText[i].gameObject.SetActive(false);
                orderImages[i].gameObject.SetActive(false);
            }
        }
    }


    // Start is called before the first frame update
    [ContextMenu("Populate Order")]
    void PopulateOrder()
    {
        //if (foodItems.Count >= itemMax)
        //{
        //    foodItems.Clear();
        //}

        for (int i = 0; i < itemMax; i++)
        {
            var selectedItem = SelectRandomItem();
            foodItems.Add(selectedItem);
        }

        if (foodItems.Count >= itemMax)
        {
            var data = new ItemSubmitData()
            {
                requiredObject = foodItems[0].name,
                requiredObject1 = foodItems[1].name,
                requiredObject2 = foodItems[2].name,
            };
            TelemetryLogger.Log(this, "order generated", data);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Ingredient>(out Ingredient ing))
        {
            CheckItemSubmission(ing.fooditem);
            Destroy(other.gameObject);
        }
    }

    //please convert the object so it is visible from the save file.
    [System.Serializable]
    public struct ItemSubmitData
    {
        public string objectName;
        public string requiredObject1;
        public string requiredObject2;
        public string requiredObject;
    }
    //we want to record if the player submits the proper item

    public void CheckItemSubmission(FoodItem submitItem)
    {
        foreach (FoodItem item in foodItems)
        {
            if (item == submitItem) // Fixed = to == here
            {
                var data = new ItemSubmitData()
                {
                    objectName = submitItem.name
                };
                TelemetryLogger.Log(this, "Correct Submission", data);
                ScoreManager.score += item.pointValue;
                foodItems.Remove(item);
                if (foodItems.Count == 0)
                {
                    OrderReset();
                    return;
                }
                PopulateUI();
                break; // Item found, break out of the loop
            }
            else
            {
                var data = new ItemSubmitData()
                {
                    objectName = submitItem.name
                };
                TelemetryLogger.Log(this, "Incorrect Submission", data);
            }
        }
        
    }
   
    public void caculateProbability()
    {
        int sum = 0;
        foreach (FoodItem item in itemDatabase.Items)
        {
            sum += item.rarity;
        }
        var items = itemDatabase.Items;
        probablity.Add(items[0], items[0].rarity*100f / sum);
        for (int i = 1; i < items.Count; i++)
        {
            probablity.Add(items[i], probablity[items[i-1]] + items[i].rarity * 100f / sum);
        }
    }

    public FoodItem SelectRandomItem()
    {
        int randomItem = Random.Range(0, 101);
        
        foreach (FoodItem item in itemDatabase.Items)
        {
            if (randomItem <= probablity[item] && (item.type == onlySelect || onlySelect == FoodType.Unset))
            {
                return item;
            }
        }
        Debug.LogError("failed to selected a item");
        return null;
    }
}

