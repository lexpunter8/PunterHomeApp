using Microsoft.AspNetCore.Components;

namespace BlazorPunterHomeApp.Components
{
    public partial class ProductComponent : ComponentBase
    {
        private string imageUrl =
            "https://dummyimage.com/286x180/8EB1C7/FEFDFF.png&text=Awesome+Product";
        
        [Parameter]
        public string productName { get; set; }

        [Parameter]
        public string quantiyString { get; set; }

        private string productDescription =
            "You won't believe how great this product is until you actually use it yourself.";
    }
}
