﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suggestions.Business.Abstract;
using Suggestions.Entities.Models;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    public class SuggestionsController : Controller
    {
        private static readonly HttpClient _client = new HttpClient();
        private IServiceManager _serviceManager;
        private static int _queryCount = 0;

        public SuggestionsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public IActionResult Index()
        {
            var claims = User.Claims;
            var IsAuthenticated = User.Identity.IsAuthenticated;
            var Username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;

            return View(_serviceManager.SuggestionsService.GetFile());
        }
        [HttpGet]
        public IActionResult DataUpload()
        {

            return View(_serviceManager.SuggestionsService.GetFile());
        }
        [HttpPost]
        public IActionResult DataUpload(IFormFile formFile)
        {

            var dataUpload = _serviceManager.SuggestionsService.DataUpload(formFile);
            return View(dataUpload);

        }
        public IActionResult Suggestions(string fileName)
        {
            var email = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            var user = _serviceManager.UserService.GetUser(email);
            FileUploadViewModel file = new FileUploadViewModel();

            file.FieldList = _serviceManager.SuggestionsService.Header(fileName);
            file.FileNames = _serviceManager.SuggestionsService.GetFile();
            file.ThisFileName = fileName;
            return View(file);
        }
        public async Task<IActionResult> ProcessSuggestions(string fileName, List<string> selectedFeatures, string p_pk, string p_name, string p_type)
        {

            var recommendations = await _serviceManager.SuggestionsService.GetRecommendations(fileName, selectedFeatures, p_pk, p_name, p_type, _queryCount);
            return View(recommendations);
        }
        public IActionResult LogUser()
        {
            return View();
        }
    }
}
