using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using mvcCookieAuthSample.ViewModels;

namespace mvcCookieAuthSample.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentController(
            IClientStore clientStore,
            IResourceStore resourceStore,
            IIdentityServerInteractionService identityServerInteractionService
            )
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        #region 私有
        private async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
                return null;

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            var consentViewModel = CreateConsentViewModel(request, client, resources);
            consentViewModel.ReturnUrl = returnUrl;
            return consentViewModel;
        }

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request,Client client,Resources resources)
        {
            var consentViewModel = new ConsentViewModel
            {
                ClientName = client.ClientName,
                ClientLogoUrl = client.LogoUri,
                ClientUrl = client.ClientUri,
                RememberConsent = client.AllowRememberConsent,
                IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i)),
                ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes).Select(x => CreateConsentViewModel(x))
            };

            return consentViewModel;
        }

        private ScopesViewModel CreateConsentViewModel(Scope scope)
        {
            return new ScopesViewModel
            {
                Name = scope.Name,
                DisPlayName = scope.DisplayName,
                Description = scope.Description,
                Required = scope.Required,
                Checked = scope.Required,
                Emphasize = scope.Emphasize
            };
        }

        private ScopesViewModel CreateScopeViewModel(IdentityResource identityResource)
        {
            var scopeViewModel = new ScopesViewModel
            {
                Name = identityResource.Name,
                DisPlayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Required = identityResource.Required,
                Checked = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
            return scopeViewModel;
        }
        #endregion


        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await BuildConsentViewModel(returnUrl);
            if(model == null)
            {
                //todo 异常处理
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel viewModel)
        {
            ConsentResponse consentResponse = null;

            if(viewModel.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if(viewModel.Button == "yes")
            {
                if(viewModel.ScopesConsented!=null && viewModel.ScopesConsented.Any())
                {
                    consentResponse = new ConsentResponse
                    {
                        RememberConsent = viewModel.RememberConsent,
                        ScopesConsented = viewModel.ScopesConsented
                    };
                }
            }

            if(consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(viewModel.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);

                Redirect(viewModel.ReturnUrl);
            }

            return View();
        }
    }
}