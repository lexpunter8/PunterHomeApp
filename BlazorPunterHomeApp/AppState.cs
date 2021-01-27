using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp
{
    public enum EPage
    {
        HomePage,
        Other
    }
    public class AppState
    {
        public EPage CurrentPage { get; set; }

        public event EventHandler PageChanged;

        public void SetCurrentPage(EPage page)
        {
            CurrentPage = page;
            PageChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
