﻿using System;
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
    public partial class CompanyDetailComponent : ComponentBase
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




        [Parameter]
        public int? Id { get; set; }

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

        RepositoryBase<Company, int> repo;

        protected override async Task OnInitializedAsync()
        {
            repo = repoProvider.GetCustomRepository<Company, int>();
            await Load();
        }
        protected async Task Load()
        {
            if ((Id ?? 0) == 0)
            {
                company = new Company();
            }
            else
            {
                company = repo.FindSingle(Id ?? 0);
                if(company == null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Entity doesnt exist");
                    DialogService.Close(null);
                }
            }
        }

        protected async Task FormSubmit(Company args)
        {
            try
            {
                if(company.Id == 0)
                {
                    repo.Add(company);
                }
                repo.SaveChanges();
                DialogService.Close(company);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Category!");
            }
        }

        protected async Task CancelClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
