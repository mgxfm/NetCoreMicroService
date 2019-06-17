using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcCookieAuthSample.ViewModels
{
    public class ProcessConsentProcess
    {
        public string ReturnUrl { get; set; }
        public bool IsRedirect => !string.IsNullOrEmpty(this.ReturnUrl);
        public ConsentViewModel ViewModel { get; set; }
        public string ValidationError { get; set; }
    }
}
