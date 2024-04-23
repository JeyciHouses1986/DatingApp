using API.Controllers;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _uow;
    private readonly IPhotoService _photoService;

    public AdminController(UserManager<AppUser> userManager, IUnitOfWork uow, IPhotoService photoService)
    {
        _userManager = userManager;
        _uow = uow;
        _photoService = photoService;
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await _userManager.Users
        .OrderBy(x => x.UserName)
        .Select(x => new
        {
            x.Id,
            Username = x.UserName,
            Roles = x.UserRoles.Select(y => y.Role.Name).ToList()
        })
        .ToListAsync();

        return Ok(users);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
    {
        if (string.IsNullOrEmpty(roles)) return BadRequest("You must specify at least one role");

        var selectedRoles = roles.Split(",").ToArray();

        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if (!result.Succeeded) return BadRequest("Failed to add roles");

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if (!result.Succeeded) return BadRequest("Failed to remove roles");

        return Ok(await _userManager.GetRolesAsync(user));
    }


    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public async Task<ActionResult> GetPhotosForModeratorAsync()
    {
        var photos = await _uow.PhotoRepository.GetUnapprovedPhotos();
        return Ok(photos);
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("approve-photo/{photoId}")]
    public async Task<ActionResult> ApprovePhoto(int photoId)
    {
        var photo = await
        _uow.PhotoRepository.GetPhotoById(photoId);
        if (photo == null) return NotFound("Could not find photo");
        photo.IsApproved = true;
        var user = await
        _uow.UserRepository.GetUserByPhotoId(photoId);
        if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;
        await _uow.Complete();
        return Ok();
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("reject-photo/{photoId}")]
    public async Task<ActionResult> RejectPhoto(int photoId)
    {
        var photo = await
        _uow.PhotoRepository.GetPhotoById(photoId);
        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Result == "ok")
            {
                _uow.PhotoRepository.RemovePhoto(photo);
            }
        }
        else
        {
            _uow.PhotoRepository.RemovePhoto(photo);
        }
        await _uow.Complete();
        return Ok();
    }

    
}
