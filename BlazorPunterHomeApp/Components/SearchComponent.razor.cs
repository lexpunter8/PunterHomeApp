using Microsoft.AspNetCore.Components;

namespace BlazorPunterHomeApp.Components
{
    public class SearchModel
    {
        public string SearchText { get; set; } = string.Empty;
        public ESortOrder SortOrder { get; set; }
    }
    public partial class SearchComponent : ComponentBase
    {
        private ESortOrder mySortOrder;

        [Parameter] public string SearchText { get; set; }
        [Parameter] public ESortOrder SortOrder { get => mySortOrder; set { mySortOrder = value; OnSortOrderChanged.InvokeAsync(mySortOrder); } }
        [Parameter] public EventCallback<SearchModel> OnSearch { get; set; }
        [Parameter] public EventCallback OnBarcodePressed { get; set; }
        [Parameter] public EventCallback<ESortOrder> OnSortOrderChanged{ get; set; }
        [Parameter] public string TextPlaceholder { get; set; }
        [Parameter] public bool ShowBarcodeButton { get; set; }
        public SearchModel SearchModel { get; set; } = new SearchModel();
    }

    public enum ESortOrder
    {
        Ascending,
        Descending
    }
}
