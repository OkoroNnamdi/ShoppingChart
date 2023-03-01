using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using CmsShoppingChart.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace CmsShoppingChart.Extensions
{
    public class FileExtensionAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // var context = (CmsShoppingContext)validationContext.GetService(typeof(CmsShoppingContext)); 

            var file = value as IFormFile;
            if ( file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "Jpg", "Png" };
                bool result = extensions.Any(x=>extension.EndsWith (x));
                if (!result)
                {

                    return new ValidationResult(GetErrorMessage());
                }
              
            }
            return ValidationResult.Success;

        }
        private string GetErrorMessage()
        {
            return "Allowed extensions are jpg and png";
        }
    }
}
