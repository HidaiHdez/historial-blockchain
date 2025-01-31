using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using historial_blockchain.Entities;
using historial_blockchain.Models;
using historial_blockchain.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace historial_blockchain.Contexts
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet("GetRoles")]
        public ActionResult<IEnumerable<IdentityRole>>  GetRoles()
        {
            return context.Roles.ToList();
        }

        [HttpPut("AsignHospital")]
        public async Task<ActionResult> AsignHospital(HospitalAdminDTO hospitalIdentification)
        {
            var user = await userManager.FindByIdAsync(hospitalIdentification.UserId);
            if(user is null) 
                return BadRequest();
            user.HospitalId = hospitalIdentification.HospitalId;
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("AsignRole")]
        public async Task<ActionResult> AsignRole(EditRoleDTO editRoleDTO)
        {
            var user = await userManager.FindByIdAsync(editRoleDTO.UserId);
            if(user is null) 
                return NotFound();
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDTO.RoleName));
            await userManager.AddToRoleAsync(user, editRoleDTO.RoleName);
            return Ok();
        }

        [HttpPost("RemoveRole")]
        public async Task<ActionResult> RemoveRole(EditRoleDTO editRoleDTO)
        {
            var user = await userManager.FindByIdAsync(editRoleDTO.UserId);
            if(user is null) 
                return NotFound();
            await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDTO.RoleName));
            await userManager.RemoveFromRoleAsync(user, editRoleDTO.RoleName);
            return Ok();
        }
    }
}