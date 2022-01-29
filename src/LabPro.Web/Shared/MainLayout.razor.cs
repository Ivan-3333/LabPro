using LabPro.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;


namespace LabPro.Web.Shared
{
    public partial class MainLayoutComponent : LayoutComponentBase
    {
        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }


        protected RadzenBody body0;

        protected RadzenSidebar sidebar0;

        protected override async Task OnInitializedAsync()
        {
        }


        protected async Task SidebarToggle0Click(dynamic args)
        {
            sidebar0.Toggle();

            body0.Toggle();
        }
    }
}
