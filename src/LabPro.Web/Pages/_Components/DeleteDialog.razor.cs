using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using LabPro.Web.Models;
using LabPro.Web.Data;

namespace LabPro.Web.Pages._Components
{
    public partial class DeleteDialogComponent : ComponentBase
    {
        [Inject]
        protected DialogService DialogService { get; set; }

        [Parameter]
        public string Msg { get; set; }


        protected async Task OkClick(MouseEventArgs args)
        {
            DialogService.Close(true);
        }

        protected async Task CancelClick(MouseEventArgs args)
        {
            DialogService.Close(false);
        }

        public static Dictionary<string, object> DeleteDialogParams(string msg)
        {
            return new Dictionary<string, object>() {{ "Msg",  msg}};
        }

    }


}
