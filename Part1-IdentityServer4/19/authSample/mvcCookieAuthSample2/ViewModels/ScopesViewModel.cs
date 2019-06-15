using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcCookieAuthSample.ViewModels
{
    public class ScopesViewModel
    {
        public string Name { get; set; }
        public string DisPlayName { get; set; }
        public bool Emphasize { get; set; }
        public string Required { get; set; }
        public bool Checked { get; set; }
        public string Description { get; set; }
    }
}
