using UnityEngine;

[CreateAssetMenu(menuName = "Shopping List/Available Items")]
public class AvailableShoppingListItemsSO : ScriptableObject
{
    public ItemSO[] availableItems;
}
