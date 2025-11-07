using FsoAvalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsoAvalonia.Models;


namespace FsoAvalonia.ViewModels;

public partial class MemberViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isChecked;

    [ObservableProperty]
    private string? _name;

    //[ObservableProperty]
    //private string? _lastname;

    public MemberViewModel()
    {

    }

    public MemberViewModel(Member member)
    {
        IsChecked = member.IsChecked;
        Name = member.Name;
        //Lastname = member.Lastname;
    }

    public Member GetMember()
    {
        return new Member
        {
            IsChecked = this.IsChecked,
            Name = this.Name,
            //Lastname = this.Lastname
        };
    }
}
