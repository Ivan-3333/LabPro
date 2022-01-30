using LabPro.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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


        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }



        protected RadzenBody body0;

        private void Authenticated()
        {
            StateHasChanged();
        }


        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (Security != null)
            {
                Security.Authenticated += Authenticated;

                await Security.InitializeAsync(AuthenticationStateProvider);
            }
        }
    }
}
