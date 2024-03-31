[System.Serializable]
public class ShoppingItem
{
    public ItemSO itemSO { set; get; }
    public ShoppingItemStatus status { set; get; }

    public ShoppingItem(ItemSO itemSO, ShoppingItemStatus status){
        this.itemSO = itemSO;
        this.status = status;
    }
}
