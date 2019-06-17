using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using mvcCookieAuthSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcCookieAuthSample.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(
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


        private ConsentViewModel CreateConsentViewModel(
            AuthorizationRequest request, 
            Client client, 
            Resources resources,
            InputConsentViewModel model
            )
        {
            var selectedScopes = model?.ScopesConsented ?? Enumerable.Empty<string>();
            var rememberConsent = model?.RememberConsent ?? true;

            var consentViewModel = new ConsentViewModel
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ClientLogoUrl = client.LogoUri,
                ClientUrl = client.ClientUri,
                RememberConsent = client.AllowRememberConsent,
                IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i, selectedScopes.Contains(i.Name) || model == null)),
                ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes).Select(x => CreateConsentViewModel(x, selectedScopes.Contains(x.Name) || model == null))
            };

            return consentViewModel;
        }

        private ScopesViewModel CreateConsentViewModel(Scope scope,bool check)
        {
            return new ScopesViewModel
            {
                Name = scope.Name,
                DisPlayName = scope.DisplayName,
                Description = scope.Description,
                Required = scope.Required,
                Checked = check || scope.Required, //如果你是check或者是必选的情况下就给你选中
                Emphasize = scope.Emphasize
            };
        }

        private ScopesViewModel CreateScopeViewModel(IdentityResource identityResource,bool check)
        {
            var scopeViewModel = new ScopesViewModel
            {
                Name = identityResource.Name,
                DisPlayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Required = identityResource.Required,
                Checked = check || identityResource.Required, //如果你是check或者是必选的情况下就给你选中
                Emphasize = identityResource.Emphasize
            };
            return scopeViewModel;
        }
        #endregion

        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl,InputConsentViewModel model = null)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
                return null;

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            var consentViewModel = CreateConsentViewModel(request, client, resources,model);
            consentViewModel.ReturnUrl = returnUrl;
            return consentViewModel;
        }

        public async Task<ProcessConsentProcess> ProcessConsent(InputConsentViewModel model)
        {
            ConsentResponse consentResponse = null;
            var result = new ProcessConsentProcess();
            if (model.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if (model.Button == "yes")
            {
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    consentResponse = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = model.ScopesConsented
                    };
                }
            }

            if (consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(model.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);

                result.ReturnUrl = model.ReturnUrl;
            }
            else
            {
                result.ViewModel = await BuildConsentViewModel(model.ReturnUrl, model);
            }
            return result;
        }
    }
}
