using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UnchainedBackend.Helpers;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.Repos;

namespace UnchainedBackend.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepo _usersRepo;
        private readonly IEthHelper _ethHelper;


        public UsersController(IUsersRepo usersRepo, IEthHelper ethHelper) {
            _usersRepo = usersRepo;
            _ethHelper = ethHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _usersRepo.GetUsers();
            if (users == null) return NoContent();
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> GetUser([FromBody] SignatureModel model)
        {
            var user = await _usersRepo.GetUser(model);
            if (user == null) return NoContent();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            var result = await _usersRepo.PostUser(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] IdModel model)
        {
            var result = await _usersRepo.DeleteUser(model.PublicAddress);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var result = await _usersRepo.UpdateUser(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] SignatureModel model)
        {
            try
            {
                var user = await _usersRepo.GetUser(model);
                // Add user to the DB if doesnt exist
                if (user == null)
                {
                    var newUser = new User()
                    {
                        PublicAddress = model.PublicAddress,
                        Signature = model.Signature
                    };
                    await _usersRepo.PostUser(newUser);
                    return Ok(newUser);
                };
                var verification = _ethHelper.VerifySignature(model);
                if (verification == false)
                    return BadRequest();
                return Ok(user);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

    }
}
