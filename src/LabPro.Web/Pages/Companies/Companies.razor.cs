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

namespace LabPro.Web.Pages.Companies
{
    public partial class CompaniesComponent : ComponentBase
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
        protected LabProContext context { get; set; }

        protected RadzenGrid<Company> dgData;

        IEnumerable<Company> _getCompaniesResult;
        protected IEnumerable<Company> getCompaniesResult
        {
            get
            {
                return _getCompaniesResult;
            }
            set
            {
                if (!object.Equals(_getCompaniesResult, value))
                {
                    _getCompaniesResult = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            getCompaniesResult = context.Companies.AsQueryable();
        }

        protected async Task AddClick(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<CompanyDetail>("Add Company", new Dictionary<string, object>() { { "Id", 0 } });
            await dgData.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task GridRowSelect(Company args)
        {
            var result = await DialogService.OpenAsync<CompanyDetail>("Edit Category", new Dictionary<string, object>() { { "Id", args.Id } });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                int comapnyId = data.Id;

                var item = context.Companies
                              .Where(i => i.Id == comapnyId)
                              .FirstOrDefault();

                if (item != null)
                {
                    bool result = await DialogService.OpenAsync<DeleteDialog>("Confirm", DeleteDialogComponent.DeleteDialogParams(item.Id.ToString(), item.Name));
                    if (result)
                    {
                        context.Companies.Remove(item);
                        context.SaveChanges();

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
