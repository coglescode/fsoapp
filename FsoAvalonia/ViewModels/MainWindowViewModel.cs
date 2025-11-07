using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace FsoAvalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        public ObservableCollection<MemberViewModel> Members { get; } = new ObservableCollection<MemberViewModel>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddItemCommand))] // This attribute will invalidate the command each time this property changes
        private string? _newItemContent;

        private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);

        [RelayCommand(CanExecute = nameof(CanAddItem))]
        private void AddItem()
        {
            // Add a new item to the list
            Members.Add(new MemberViewModel() { Name = NewItemContent});

            // reset the NewItemContent
            NewItemContent = null;
        }

        [RelayCommand]
        private void RemoveItem(MemberViewModel member)
        {
            // Remove the given item from the list
            Members.Remove(member);
        }
    }
}
