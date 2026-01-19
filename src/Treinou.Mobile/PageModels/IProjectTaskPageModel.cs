using CommunityToolkit.Mvvm.Input;
using Treinou.Mobile.Models;

namespace Treinou.Mobile.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}