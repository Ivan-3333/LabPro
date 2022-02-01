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
    public class BasicEntityListComponent<T,TDetail> : BasicComponent where T : DatabaseEntity<int> where TDetail: ComponentBase
    {
        [Inject]
        protected RepositoryProvider repoProvider { get; set; }

        RepositoryBase<T, int> repo { get; set; }

        protected RadzenGrid<T> dgData;

        #region OVERRIDE
        public virtual string Title()
        {
            return "Entity";
        }

        public virtual string EditTitle()
        {
            return "Edit";
        }

        public virtual string CreateTitle()
        {
            return "Create";
        }

        public virtual string DeleteMsg(T item)
        {
            return "Delete confirm?";
        }

        public virtual string IncludeProperties()
        {
            return string.Empty;
        }
        #endregion


        IEnumerable<T> _getResult;
        protected IEnumerable<T> getResult
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

        protected override async Task OnInitializedAsync()
        {
            repo = repoProvider.GetCustomRepository<T, int>();
            await Load();
        }
        protected async Task Load()
        {
            getResult = repo.GetActive(includeProperties: IncludeProperties());
        }

        protected async Task AddClick(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<TDetail>(CreateTitle(), new Dictionary<string, object>() { { "Id", 0 } });
            await dgData.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task GridRowSelect(T args)
        {
            var result = await DialogService.OpenAsync<TDetail>(EditTitle(), new Dictionary<string, object>() { { "Id", args.Id } });
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
                    bool result = await DialogService.OpenAsync<DeleteDialog>("Confirm", DeleteDialogComponent.DeleteDialogParams(DeleteMsg(item)));
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
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete");
            }
        }
    }
}
