[System.Serializable]
public class OtherItem
{
    public ItemSO itemSO { set; get; }
    public OtherItemStatus status { set; get; }

    public OtherItem(ItemSO itemSO, OtherItemStatus status){
        this.itemSO = itemSO;
        this.status = status;
    }
}
