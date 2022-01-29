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

        protected RadzenGrid<Company> grid0;

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

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<CompanyAdd>("Add Company", null);
            grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Company args)
        {
            //var result = await DialogService.OpenAsync<EditCategory>("Edit Category", new Dictionary<string, object>() { { "CategoryID", args.CategoryID } });
            //await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                int comapnyId = data.Id;

                var item = context.Companies
                              .Where(i => i.Id == comapnyId)
                              .FirstOrDefault();

                context.Companies.Remove(item);
                context.SaveChanges();

                if (item != null)
                {
                    grid0.Reload();
                }

                NotificationService.Notify(NotificationSeverity.Info, $"Error", $"Go baby go");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Category");
            }
        }
    }
}
