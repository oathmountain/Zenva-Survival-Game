using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingRecipeUI : MonoBehaviour
{
    public BuildingRecipe recipe;
    public Image backgroundImage;
    public Image icon;
    public TextMeshProUGUI buildingName;
    public Image[] resourceCost;

    public Color canBuildColor;
    public Color cannotbuildColor;
    private bool canBuild;

    void OnEnable()
    {
        UpdateCanCraft();
    }

    void Start()
    {
        icon.sprite = recipe.icon;
        buildingName.text = recipe.displayName;

        for(int i = 0; i < resourceCost.Length; i++)
        {
            if (i < resourceCost.Length)
            {
                resourceCost[i].gameObject.SetActive(true);

                resourceCost[i].sprite = recipe.cost[i].item.icon;
                resourceCost[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = recipe.cost[i].quantity.ToString();
            }
            else
            {
                resourceCost[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateCanCraft()
    {
        canBuild = true;

        for(int i = 0; i < recipe.cost.Length; i++)
        {
            if(!Inventory.instance.HasItems(recipe.cost[i].item, recipe.cost[i].quantity))
            {
                canBuild = false;
                break;
            }
        }

        backgroundImage.color = canBuild ? canBuildColor : cannotbuildColor;
    }

    public void OnClickButton()
    {

    }
}
