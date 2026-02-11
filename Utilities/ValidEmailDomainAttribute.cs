using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            char[] delimiterChars = {'@'};
            string[] strings = value.ToString().Split(delimiterChars);
            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}
