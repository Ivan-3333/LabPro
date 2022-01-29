using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using LabPro.Web.Data;
using LabPro.Web.Models;

namespace LabPro.Web.Pages.Companies
{
    public partial class CompanyAddComponent : ComponentBase
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

        Company _company;

        protected Company company
        {
            get
            {
                return _company;
            }
            set
            {
                if (!object.Equals(_company, value))
                {
                    _company = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            company = new Company();
        }

        protected async System.Threading.Tasks.Task Form0Submit(Company args)
        {
            try
            {
                var northwindCreateCategoryResult = context.Companies.Add(company);
                context.SaveChanges();
                DialogService.Close(company);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Category!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
