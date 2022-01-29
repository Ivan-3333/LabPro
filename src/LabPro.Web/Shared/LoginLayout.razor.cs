using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace LabPro.Web.Shared
{
    public partial class LoginLayoutComponent : LayoutComponentBase
    {
        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }



        protected RadzenBody body0;

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
        }
    }
}
