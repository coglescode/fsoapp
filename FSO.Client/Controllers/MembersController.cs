using Microsoft.AspNetCore.Mvc;
using FSO.Client.Models;
using FSO.Client.Services;
using Microsoft.Identity.Client;
namespace FSO.Client.Controllers;

public class MembersController : Controller
{
    private readonly MembersApiService _membersApiService;

    public MembersController(MembersApiService membersApiService)
    {
        _membersApiService = membersApiService;
    }
       
    // Get all members from the API service
    public async Task<IActionResult> Index()
    {
        var membersFromApiService = await _membersApiService.GetMembersAsync();
    
        //if (!membersFromApiService.Any())
        if (membersFromApiService != null)
        {
            var members = membersFromApiService.Select(member => new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Lastname = member.Lastname
            }).ToList();


            return View(members);
        }
        else
        {
            var members = new MemberViewModel();
            return View(members);
        }

        //var members = membersFromApiService.Select(member => new MemberViewModel
        //{
        //    Id = member.Id,
        //    Name = member.Name
        //}).ToList();

        //if (members == null || members.Count == 0)
        //{
        //    return NotFound("Lista vacia.");
        //}        
                
    }

    public async Task<IActionResult> DeleteMember(MemberViewModel member)
    {
        //var memberId = new MembersApiDTO
        //{
        //    Id = memberView.Id
        //};

        var _memberId = await _membersApiService.DeleteMembersAsync(member.Id);
        return RedirectToAction("Index", "Members");
  
    }

    public async Task<IActionResult> PostMember(MemberViewModel memberView)
    {
        var member = new MembersApiDTO
        {
            //Id = memberView.Id,
            Name = memberView.Name,
            Lastname = memberView.Lastname
        };

        var memberName = await _membersApiService.PostMembersAsync(member);
        return RedirectToAction("Index", "Members");
    }

    public async Task<IActionResult> EditMember(string id, MemberViewModel memberView)
    {
        var memberId = await _membersApiService.GetMemberAsync(id);

        var member = new MembersApiDTO
        {
            //Id = memberView.Id,
            Name = memberView.Name,
            Lastname = memberView.Lastname
        };


        //if (member is )
        //{
        //    return NotFound();
        //}

        //memberView.Id = member.Id;

        var memberName = await _membersApiService.PutMembersAsync(id, member);
        return RedirectToAction("Index", "Members");
    }


}