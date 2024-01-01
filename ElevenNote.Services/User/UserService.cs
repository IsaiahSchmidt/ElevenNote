using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.User;
using Microsoft.AspNetCore.Identity;

namespace ElevenNote.Services.User;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;

    public UserService(ApplicationDbContext context,
                        UserManager<UserEntity> userManager,
                        SignInManager<UserEntity> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<bool> RegisterUserAsync(UserRegister model)
    {
        if (await CheckEmailAvailability(model.Email) == false)
        {
            Console.WriteLine("Invalid email, already in use");
            return false;
        }

        if (await CheckUserNameAvailability(model.UserName) == false)
        {
            Console.WriteLine("Invalid username, already in use");
            return false;
        }
    
        UserEntity entity = new()
        {
            Email = model.Email,
            UserName = model.UserName,
            DateCreated = DateTime.Now
        };

        IdentityResult registerResult = await _userManager.CreateAsync(entity, model.Password);
        return registerResult.Succeeded;
    }

    private async Task<bool> CheckUserNameAvailability(string userName)
    {
        UserEntity? existingUser = await _userManager.FindByNameAsync(userName);
        return existingUser is null;
    }

    private async Task<bool> CheckEmailAvailability(string  email)
    {
        UserEntity? existingUser = await _userManager.FindByEmailAsync(email);
        return existingUser is null;
    }
}