using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemImporter.Asset", menuName = "Data/Potion Importer")]
public class ItemImporter : ScriptableObject
{
    [Tooltip("Path to Excel file to import. Use forward slashes")]
    public string excelFilePath = "Editor/Items.xlsx";

    [ContextMenu("Import")]
    public void Import()
    {
        //TODO: call the function somehow, maybe have a button in the inspector (stretch goal but feasible)
        var excel = new ExcelImporter(excelFilePath);
        var items = DataHelper.GetAllAssetsOfType<FoodItem>();

        Debug.Log("Finished Importing potions from excel");
    }
}
