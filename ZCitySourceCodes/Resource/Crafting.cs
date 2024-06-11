using System;
using System.Collections.Generic;

[Serializable]
public class Crafting
{
    public List<UseableItemSO> CraftableItems;

    private int CurrentRecipe;
    public int RecipeIndex;

    public void AddResource(int ResourceID)
    {
        if ((CurrentRecipe & ResourceID) == 0)
        {
            CurrentRecipe |= ResourceID;
        }
    }

    public bool TryCrafting()
    {
        for (int i = 0; i < CraftableItems.Count; i++)
        {
            if (CurrentRecipe == CraftableItems[i].Recipe)
            {
                RecipeIndex = i;
                return true;
            }
        }

        return false;
    }

    public UseableItemSO GetCraftInfo()
    {
        return CraftableItems[RecipeIndex];
    }

    public void ResetRecipe()
    {
        CurrentRecipe = 0;
    }
}
