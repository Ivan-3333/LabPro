using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using LabPro.Web.Models;
using LabPro.Web.Data;
using LabPro.Web.Pages._Components;

namespace LabPro.Web.Pages.ContactPersons
{
    public partial class ContactPersonsComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected RepositoryProvider repoProvider { get; set; }

        RepositoryBase<ContactPerson, int> repo { get; set; }

        protected RadzenGrid<ContactPerson> dgData;

        IEnumerable<ContactPerson> _getResult;
        protected IEnumerable<ContactPerson> getResult
        {
            get
            {
                return _getResult;
            }
            set
            {
                if (!object.Equals(_getResult, value))
                {
                    _getResult = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            repo = repoProvider.GetCustomRepository<ContactPerson, int>();
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            getResult = repo.GetActive(includeProperties: "Company");
        }

        protected async Task AddClick(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<ContactPersonDetail>("Add Contact", new Dictionary<string, object>() { { "Id", 0 } });
            await dgData.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task GridRowSelect(ContactPerson args)
        {
            var result = await DialogService.OpenAsync<ContactPersonDetail>("Edit Contact", new Dictionary<string, object>() { { "Id", args.Id } });
            await dgData.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                int comapnyId = data.Id;

                var item = repo.FindSingle(data.Id);

                if (item != null)
                {
                    bool result = await DialogService.OpenAsync<DeleteDialog>("Confirm", DeleteDialogComponent.DeleteDialogParams(item.Id.ToString(), item.Name));
                    if (result)
                    {
                        repo.Delete(item);
                        repo.SaveChanges();

                        NotificationService.Notify(NotificationSeverity.Info, $"Info", $"Deleted");
                        await dgData.Reload();
                    }
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Category");
            }
        }
    }
}
