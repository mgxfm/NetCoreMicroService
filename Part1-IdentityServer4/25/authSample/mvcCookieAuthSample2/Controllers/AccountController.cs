using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvcCookieAuthSample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using mvcCookieAuthSample.ViewModels;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Test;
using IdentityServer4.Services;

namespace mvcCookieAuthSample.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IIdentityServerInteractionService _interaction;

        //private readonly TestUserStore _users;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
        }

        private IActionResult RedirectToLoacl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        //public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}


        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl  = null)
        {
            //if (ModelState.IsValid)
            //{
            //    ViewData["ReturnUrl"] = returnUrl;
            //    var identityUser = new ApplicationUser
            //    {
            //        Email = registerViewModel.Email,
            //        UserName = registerViewModel.Email,
            //        NormalizedUserName = registerViewModel.Email,
            //    };

            //    var identityResult = await _userManager.CreateAsync(identityUser, registerViewModel.Password);
            //    if (identityResult.Succeeded)
            //    {
            //        await _signInManager.SignInAsync(identityUser, new AuthenticationProperties { IsPersistent = true });
            //        return RedirectToLoacl(returnUrl);
            //    }
            //    else
            //    {
            //        AddErrors(identityResult);
            //    }
            //}

            return View();

        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string returnUrl)
        {
            //if (ModelState.IsValid)
            //{
            //    ViewData["ReturnUrl"] = returnUrl;
            //    var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            //    if (user == null)
            //    {
            //        ModelState.AddModelError(nameof(loginViewModel.Email), "Email not exists");
            //    }
            //    else
            //    {
            //        await _signInManager.SignInAsync(user, new AuthenticationProperties { IsPersistent = true });
            //        return RedirectToLoacl(returnUrl);
            //    }
            //}

            #region ��ʱ����
            //if (ModelState.IsValid)
            //{
            //    ViewData["ReturnUrl"] = returnUrl;
            //    var user = _users.FindByUsername(loginViewModel.UserName);
            //    if(user == null)
            //    {
            //        ModelState.AddModelError(nameof(loginViewModel.UserName), "UserName not Exist");
            //    }
            //    else
            //    {
            //        if (_users.ValidateCredentials(loginViewModel.UserName, loginViewModel.Password))
            //        {
            //            var props = new AuthenticationProperties
            //            {
            //                IsPersistent = true,
            //                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
            //            };

            //            await Microsoft.AspNetCore.Http.AuthenticationManagerExtensions.SignInAsync(
            //                  HttpContext,
            //                  user.SubjectId,
            //                  user.Username,
            //                  props
            //                );

            //            return RedirectToLoacl(returnUrl);
            //        }

            //        ModelState.AddModelError(nameof(loginViewModel.Password), "Wrong Password");
            //    }
            //}
            #endregion

            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if(user == null)
                {
                    ModelState.AddModelError(nameof(loginViewModel.Email), "Email not exists");
                }
                else
                {
                    if(await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
                    {
                        AuthenticationProperties props = null;
                        if (loginViewModel.RememberMe)
                        {
                            props = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                            };
                        }

                        await _signInManager.SignInAsync(user, props);

                        if (_interaction.IsValidReturnUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return Redirect("~/");
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(loginViewModel.Email), "Password is wrong");
                    }
                }
            }

            return View(loginViewModel);
        }

        public IActionResult MakeLogin()
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,"mark"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            //await HttpContext.SignOutAsync();
            //return RedirectToAction("Index", "Home");
        }
    }
}
