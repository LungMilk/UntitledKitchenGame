using NodeCanvas.Tasks.Conditions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class OrderGenerator : MonoBehaviour
{
    public AudioSource SourceOrdered;
    public AudioSource SourceWrongO;
    public AudioSource SourceScored;

    //this item will return a set amount of orders and we can modify it to be either focused on orders or all items.
    //for now we will make this able to set the UI elements to their respective texts.
    public Canvas orderCanvas;
    public List<TextMeshProUGUI> orderText = new List<TextMeshProUGUI>();
    public List<Image> orderImages = new List<Image>();

    public ItemCollection itemDatabase;
    public List<FoodItem> objectsinDataBase;

    public List<FoodItem> foodItems = new List<FoodItem>();
    //being a prefab means the object cannot be scene referenced so we gots to find a score manager
    public ScoreManager ScoreManager;
    public int itemMax = 3;

    [SerializeField]
    private float pointData;
    public float negativeModifer = 0.5f;

    [SerializeField] GameObject MouseOpen;
    [SerializeField] GameObject MouseClose;
    [SerializeField] float MouseCloseTime = 2f;

    public FoodType onlySelect;
    //public type onlySelect = new type();
    //maybe we could have the variables change as well depending on what kind of object it is?
    public enum FoodType
    {
        Unlisted,
        Meat,
        Grain,
        Veggie,
        Fish
    }

    Dictionary<FoodItem, float> probablity = new Dictionary<FoodItem, float>();

    private void Awake()
    {

        caculateProbability();
    }

    [System.Serializable]
    public enum operation
    {
        Add, Subtract
    }

    void Start()
    {
        //GameObject tempObj = GameObject.Find("EndConManager");
        //if (tempObj != null)
        //{
        //    scoreManage = tempObj.GetComponent<ScoreManager>();
        //}
        GenerateOrder();
        foreach (FoodItem item in itemDatabase.Items)
        {
            objectsinDataBase.Add(item);
        }

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
            //how can we get the score now?
            playerPoints = ScoreManager.score.ToString(),
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
        //dont know if we might need this
        //var data = new OrderCompletionData()
        //{
        //    type = onlySelect.ToString(),
        //    playerPoints = scoreManage.score.ToString(),
        //    pointsFromOrder = pointData.ToString(),
        //};
    }
    public IEnumerator NewCustomer()
    {
        MouseClose.SetActive(true);
        MouseOpen.SetActive(false);
        yield return new WaitForSeconds(MouseCloseTime);
        GenerateOrder();
        MouseClose.SetActive(false);
        MouseOpen.SetActive(true);
    }

    public void GenerateOrder()
    {
        PopulateUI();
        PopulateOrder();
        OrderPointCalculation();
        SourceOrdered.Play();
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
    //I want to make it so in the unityEvent in timed event I can in the inspector drag the order generator object into the timed event, access a method from the order genrator
    //and give it a parameter. This parameter is a foodItem object which is a scriptable object. I want to modify the referenced items database and add items to it.
    //so what is happening is these need isolated collections;
    public void AddToFoodCollection(FoodItem changeObject)
    {
        if (!itemDatabase.Items.Contains(changeObject))
        {
            itemDatabase.Items.Add(changeObject);
            print($"{this.name} has added a {changeObject.displayName}");
        }
        else { print("object is already in the collection"); }
    }
    public void RemoveFromFoodCollection(FoodItem changeObject)
    {
        if (itemDatabase.Items.Contains(changeObject))
        {
            itemDatabase.Items.Remove(changeObject);
            print($"{this.name} has removed a {changeObject.displayName}");
        }
        else { print("object does not exist in the collection"); }
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
                //pointsFromOrder = pointData.ToString(),
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
            StartCoroutine(NewCustomer());
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
            if (item.foodObjectPrefab == receivedObject) // Fixed = to == here
            {
                var data = new ItemSubmitData()
                {
                    objectName = receivedObject.name,
                    requiredObject = foodItems[0].name,
                    requiredObject1 = foodItems[1].name,
                    requiredObject2 = foodItems[2].name,
                };
                TelemetryLogger.Log(this, "Correct Submission", data);
                ScoreManager.AddScore(item.pointValue);
                //scoreManage.score += item.pointValue;
                foodItems.Remove(item);
                SourceScored.Play();
                break; // Item found, break out of the loop
            }
            else
            {
                var data = new ItemSubmitData()
                {
                    objectName = receivedObject.name,
                    requiredObject = foodItems[0].name,
                    requiredObject1 = foodItems[1].name,
                    requiredObject2 = foodItems[2].name,
                };
                ScoreManager.AddScore((-item.pointValue *negativeModifer));
                //scoreManage.score -= item.pointValue * negativeModifer;
                TelemetryLogger.Log(this, "Incorrect Submission", data);
                SourceWrongO.Play();
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
        probablity.Add(items[0], items[0].rarity * 100f / sum);
        for (int i = 1; i < items.Count; i++)
        {
            probablity.Add(items[i], probablity[items[i - 1]] + items[i].rarity * 100f / sum);
        }
    }

    public FoodItem SelectRandomItem()
    {
        int randomItem = Random.Range(0, 101); // Random number between 0 and 100
        List<FoodItem> weightedItems = new List<FoodItem>();

        // Create a list of items weighted by their rarity
        foreach (FoodItem item in itemDatabase.Items)
        {
            // Only consider items that match the 'onlySelect' criteria
            if (item.type.ToString() == onlySelect.ToString() || onlySelect == FoodType.Unlisted)
            {
                // Add the item multiple times to the list based on its rarity percentage
                for (int i = 0; i < item.rarity; i++)
                {
                    weightedItems.Add(item);
                }
            }
        }

        // If the weightedItems list is empty, return null
        if (weightedItems.Count == 0)
        {
            return null;
        }

        // Select a random item from the weightedItems list
        int randomIndex = Random.Range(0, weightedItems.Count);
        return weightedItems[randomIndex];
    }
}

