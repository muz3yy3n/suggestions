﻿using AutoMapper;
using Backend.AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suggestions.Business.Abstract;
using Suggestions.Entities.Dto;
using Suggestions.Entities.Models;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]

    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult UserSettings()
        {
            string Email = User.Claims.Select(c => c.Type == ClaimTypes.Email).FirstOrDefault().ToString();

            User user=_service.GetUser(Email);
            UserDtoForUpdate userDtoForUpdate = _mapper.Map<UserDtoForUpdate>(user);

            return View(userDtoForUpdate);
        }
        [HttpPost]
        public IActionResult UserSettings(UserDtoForUpdate userDto)
        {
            string email = User.Claims.Select(c => c.Type == ClaimTypes.Email).FirstOrDefault().ToString();

            User entity = _service.GetUser(email);
            User user= _mapper.Map(userDto, entity);
            _service.UpdateUser(user,email);
            TempData["Email"] = user.Email;

            return RedirectToAction("Index","Home");
        }

    }
}
