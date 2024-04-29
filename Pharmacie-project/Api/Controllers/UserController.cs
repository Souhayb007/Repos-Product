using Api.Dtos;
using Api.Enums;
using Api.Models;
using Api.Repos;
using APi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize] 
public class UserController : Controller
{
    private readonly IUserRepo _userRepo;

    public UserController(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    [HttpGet("{id}")]
    [Admin]
    public async Task<IActionResult> GetById(Guid id)
    {
        var users = await _userRepo.GetUserByIdAsync(id);
        if (users == null)
        {
            return NotFound();
        }
        return Ok(users);
    }

    [HttpGet("{role}")]
    [Admin]
    public async Task<IActionResult> Get(UserRole role)
    {
        var users = await _userRepo.AllAsync(role);

        return Ok(users);
    }

    [HttpPost]
    [Admin]
    public async Task<IActionResult> createUser(DRegister register)
    {
        var user = new User
        {
            Name = register.Name,
            Phone = register.Phone,
            Email = register.Email,
            Address = register.Address,
            Username = register.Username,
            Role = register.Role,
            PharmacyId = register.PharmacyId,
            CostPerKM = register.CostPerKM
        };
         var result = await _userRepo.SaveAsync(user);
            if (!result)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof (GetById), new { id = user.Id }, user);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        [Admin]
        public async Task<IActionResult> UpdateUser(Guid id, DRegister register)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Mettre à jour les champs nécessaires
            existingUser.Name = register.Name;
            existingUser.Phone = register.Phone;
            existingUser.Email = register.Email;
            existingUser.Address = register.Address;
            existingUser.Username = register.Username;
            existingUser.Role = register.Role;
            existingUser.PharmacyId = register.PharmacyId;
            existingUser.CostPerKM = register.CostPerKM;

            var result = await _userRepo.UpdateAsync(existingUser);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            var result = await _userRepo.DeleteAsync(existingUser);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
