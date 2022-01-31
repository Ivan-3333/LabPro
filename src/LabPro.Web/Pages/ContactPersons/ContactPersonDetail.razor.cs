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

namespace LabPro.Web.Pages.ContactPersons
{
    public partial class ContactPersonComponent : ComponentBase
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

        ContactPerson _entity;

        protected ContactPerson entity
        {
            get
            {
                return _entity;
            }
            set
            {
                if (!object.Equals(_entity, value))
                {
                    _entity = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        IEnumerable<Company> _companies;
        protected IEnumerable<Company> companies
        {
            get
            {
                return _companies;
            }
            set
            {
                if (!object.Equals(_companies, value))
                {
                    _companies = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        RepositoryBase<ContactPerson, int> repo;

        RepositoryBase<Company, int> comapnyRepo;

        protected override async Task OnInitializedAsync()
        {
            repo = repoProvider.GetCustomRepository<ContactPerson, int>();
            comapnyRepo = repoProvider.GetCustomRepository<Company, int>();
            companies = comapnyRepo.GetActive();
            await Load();
        }
        protected async Task Load()
        {
            if ((Id ?? 0) == 0)
            {
                entity = new ContactPerson();
            }
            else
            {
                entity = repo.FindSingle(Id ?? 0);
                if(entity == null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Entity doesnt exist");
                    DialogService.Close(null);
                }
            }
        }

        protected async Task FormSubmit(ContactPerson args)
        {
            try
            {
                if(entity.Id == 0)
                {
                    repo.Add(entity);
                }
                repo.SaveChanges();
                DialogService.Close(entity);
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
