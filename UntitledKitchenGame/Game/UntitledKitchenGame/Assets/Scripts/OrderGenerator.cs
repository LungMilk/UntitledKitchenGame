using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrderGenerator : MonoBehaviour
{
    //this item will return a set amount of orders and we can modify it to be either focused on orders or all items.
    //for now we will make this able to set the UI elements to their respective texts.
    public Canvas orderCanvas;
    public List<TextMeshProUGUI> orderText = new List<TextMeshProUGUI>();
    public List<Image> orderImages = new List<Image>();

    public ItemCollection itemDatabase;

    public List<FoodItem> foodItems = new List<FoodItem>();
    //being a prefab means the object cannot be scene referenced so we gots to find a score manager
    public ScoreManager scoreManage;
    public int itemMax = 3;

    [SerializeField]
    private float pointData;

    public type onlySelect = new type();
    //maybe we could have the variables change as well depending on what kind of object it is?
    public enum type
    {
        Unlisted,
        Meat,
        Grain,
        Veggie,
        Fish
    }
    public enum operation
    {
        Add,Subtract
    }

    public void Start()
    {
        GameObject tempObj = GameObject.Find("EndConManager");
        if (tempObj != null)
        {
            scoreManage = tempObj.GetComponent<ScoreManager>();
        }
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
        PopulateUI();
        if (foodItems.Count <= 0)
        {
            OrderReset();
        }
    }
    [ContextMenu("Order Reset")]
    private void OrderReset()
    {
            var data = new OrderCompletionData()
            {
                type = onlySelect.ToString(),
                playerPoints = scoreManage.score.ToString(),
                pointsFromOrder = pointData.ToString(),
            };
        TelemetryLogger.Log(this, "order completed", data);
        foodItems.Clear();
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
        PopulateUI();
        PopulateOrder();
        OrderPointCalculation();
    }

    //no longer needed but why not keep them
    [ContextMenu("Populate UI")]
    void PopulateUI()
    {
        // Adding null checks to avoid errors if the UI elements are not set up
        if (orderCanvas == null)
        {
            //Debug.LogWarning("Order Canvas is not assigned.");
            return;
        }

        if (orderText == null || orderImages == null)
        {
            //Debug.LogWarning("UI lists are not assigned.");
            return;
        }

        // Adding checks to avoid exceptions if UI components are not present
        if (orderCanvas != null && orderText != null && orderImages != null)
        {
            // Initialize order images and texts if not already set
            if (orderText.Count == 0 || orderImages.Count == 0)
            {
                Image[] tempImageList = this.GetComponentsInChildren<Image>();
                TextMeshProUGUI[] tempList = this.GetComponentsInChildren<TextMeshProUGUI>();

                for (int i = 0; i < Mathf.Min(itemMax, tempImageList.Length); i++)
                {
                    if (tempImageList[i] != null)
                        orderImages.Add(tempImageList[i]);
                }

                for (int i = 0; i < Mathf.Min(itemMax, tempList.Length); i++)
                {
                    if (tempList[i] != null)
                        orderText.Add(tempList[i]);
                }
            }

            for (int i = 0; i < orderText.Count; i++)
            {
                if (i < foodItems.Count && foodItems[i] != null && orderText[i] != null)
                {
                    orderText[i].gameObject.SetActive(true);
                    orderText[i].text = foodItems[i].displayName;
                }
                else if (orderText[i] != null)
                {
                    orderText[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < orderImages.Count; i++)
            {
                if (i < foodItems.Count && foodItems[i] != null && orderImages[i] != null)
                {
                    orderImages[i].gameObject.SetActive(true);
                    orderImages[i].sprite = foodItems[i].displayImage;
                }
                else if (orderImages[i] != null)
                {
                    orderImages[i].gameObject.SetActive(false);
                }
            }
        }
    }

    //I want to make a event that adds objects or removes objects from the list
    public void ChangeFoodCollection(operation op, FoodItem changeObject)
    {
        if(op == operation.Add) { itemDatabase.Items.Add(changeObject); }

        if (op == operation.Subtract) { itemDatabase.Items.Remove(changeObject); }
    }
    // Start is called before the first frame update
    [ContextMenu("Populate Order")]
    void PopulateOrder()
    {
        if (foodItems.Count >= itemMax)
        {
            foodItems.Clear();
        }

        for (int i = 0; i < itemMax; i++)
        {
            var selectedItem = SelectRandomItem();
            if (selectedItem != null)
            {
                foodItems.Add(selectedItem);
            }
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
        CheckItemSubmission(other.gameObject);
        if (other.gameObject.CompareTag("Food"))
        {
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

    public void CheckItemSubmission(GameObject receivedObject)
    {
        foreach (FoodItem item in foodItems)
        {
            if (item.foodObject == receivedObject) // Fixed = to == here
            {
                var data = new ItemSubmitData()
                {
                    objectName = receivedObject.name
                };
                TelemetryLogger.Log(this, "Correct Submission", data);
                scoreManage.score += item.pointValue;
                foodItems.Remove(item);
                break; // Item found, break out of the loop
            }
            else
            {
                var data = new ItemSubmitData()
                {
                    objectName = receivedObject.name
                };
                TelemetryLogger.Log(this, "Incorrect Submission", data);
            }
        }
    }

    public FoodItem SelectRandomItem()
    {
        int randomItem = Random.Range(0, 101);

        foreach (FoodItem item in itemDatabase.Items)
        {
            if (randomItem <= item.rarity)
            {
                if (item.type.ToString() == onlySelect.ToString())
                {
                    return item;
                }
                else if (onlySelect == type.Unlisted)
                {
                    return item;
                }
            }
        }
        return null;
    }
}

