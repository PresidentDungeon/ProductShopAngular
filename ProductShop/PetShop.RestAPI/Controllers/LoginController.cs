﻿using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entities;
using PetShop.Core.Entities.Security;
using System;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IUserService UserService;

        public LoginController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginInputModel inputModel)
        {
            try
            {
                User foundUser = UserService.Login(inputModel);

                if (foundUser == null)
                {
                    return Unauthorized();
                }

                var tokenString = UserService.GenerateJWTToken(foundUser);

                return Ok(new
                {
                    ID = foundUser.ID,
                    Token = tokenString
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
