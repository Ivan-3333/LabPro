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

namespace LabPro.Web.Pages._Components
{
    public class BasicEntityDetailComponent<T> : BasicComponent where T : DatabaseEntity<int>, new()
    {
        [Inject]
        protected RepositoryProvider repoProvider { get; set; }

        [Parameter]
        public int? Id { get; set; }


        T _entity;
        protected T entity
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

        RepositoryBase<T, int> repo;

        protected override async Task OnInitializedAsync()
        {
            await CreateRepos();
            await LoadEntity();
            await LoadAdditionalData();
        }

        protected virtual async Task CreateRepos()
        {
            repo = repoProvider.GetCustomRepository<T, int>();
        }

        protected virtual async Task LoadEntity()
        {
            if ((Id ?? 0) == 0)
            {
                entity = new T();
            }
            else
            {
                entity = repo.FindSingle(Id ?? 0);
                if (entity == null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Entity doesnt exist");
                    DialogService.Close(null);
                }
            }
        }

        protected virtual async Task LoadAdditionalData()
        {
            
        }

        protected async Task FormSubmit(T args)
        {
            try
            {
                if (entity.Id == 0)
                {
                    repo.Add(entity);
                }
                repo.SaveChanges();
                DialogService.Close(entity);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new entity!");
            }
        }

        protected async Task CancelClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
