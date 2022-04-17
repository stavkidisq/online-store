using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Infrastructure
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as FormFile;

            if(file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                string[] extensions = { "png", "jpg" };

                bool isValid = extensions.Any(e => extension.EndsWith(e));

                if(!isValid)
                {
                    return new ValidationResult("Allowed extensions are png and jpg");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
