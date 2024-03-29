﻿using MailChimp.Net.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IMailChimpRepo _mailChimpRepo;

        public UsersController(IUsersRepo usersRepo, IEthHelper ethHelper, IMailChimpRepo mailChimpRepo) {
            _usersRepo = usersRepo;
            _ethHelper = ethHelper;
            _mailChimpRepo = mailChimpRepo;
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
            var user = await _usersRepo.GetUser(model.PublicAddress);
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
        public async Task<IActionResult> GetArtists()
        {
            var result = await _usersRepo.GetArtists();
            if (result == null)
                return NoContent();
            return Ok(result);
        }
        

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] SignatureModel model)
        {
            var result = await _usersRepo.DeleteUser(model.PublicAddress);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var result = await _usersRepo.UpdateUser(user);
            MailChimpModel mailModel = new()
            {
                Email = user.Email,
                FirstName = user.Name,
                Tags = new List<MemberTag>() { new MemberTag() { Id = 1, Name = "Artist" } }
            };
            var mailChimpResult = await _mailChimpRepo.AddUserToList(mailModel);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] SignatureModel model)
        {
            try
            {
                var user = await _usersRepo.GetUser(model.PublicAddress);
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
        [HttpPost]
        public async Task<IActionResult> UnchainUser([FromBody] SignatureModel model)
        {
            var result = await _usersRepo.UnchainUser(model.PublicAddress);
            return Ok(result);
        }
    }
}
