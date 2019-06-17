using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using mvcCookieAuthSample.Services;
using mvcCookieAuthSample.ViewModels;

namespace mvcCookieAuthSample.Controllers
{
    public class ConsentController : Controller
    {
        private ConsentService _consentService;
        
        public ConsentController(
            ConsentService consentService
            )
        {
            _consentService = consentService;
        }

        


        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await _consentService.BuildConsentViewModel(returnUrl,null);
            if(model == null)
            {
                //todo 异常处理
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel viewModel)
        {
            var result = await _consentService.ProcessConsent(viewModel);
            if (result.IsRedirect)
            {
               return Redirect(result.ReturnUrl);
            }
            if (!string.IsNullOrEmpty(result.ValidationError))
            {
                ModelState.AddModelError("", result.ValidationError);
            }
            return View();
        }
    }
}