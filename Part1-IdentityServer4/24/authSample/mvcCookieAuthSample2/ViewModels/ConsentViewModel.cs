﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcCookieAuthSample.ViewModels
{
    public class ConsentViewModel:InputConsentViewModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public IEnumerable<ScopesViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopesViewModel> ResourceScopes { get; set; }
    }
}
