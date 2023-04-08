namespace HomeApp.Shared;

public enum EPatchOperation
{
    Add,
    Remove,
    Update
}

public class ShoppingListItemPatchRequest
{
    public EPatchOperation Operation { get; set; }
}

public class ShoppingListItemDto
{
    public string ItemName { get; set; }
    public string AmountValueString { get; set; }
    public int Count { get; set; }
    public bool IsChecked { get; set; }
    public Guid Id { get; set; }
}

public class ShoppingListDto
{
    public List<ShoppingListItemDto> Items { get; set; }
}

