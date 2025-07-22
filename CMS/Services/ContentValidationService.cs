using CMS.Models;
using CMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services
{
    public class ContentValidationService:IContentValidationService
    {
        public ValidationResult ValidationSuccessful(List<ContentItem> items, string title, string? image, bool hasText,ContentItem? currentItem = null)
        {

            if (string.IsNullOrWhiteSpace(title))
            {
                return new ValidationResult
                {
                    Success = false,
                    IsValidationError = true,
                    Message = "Title can't be empty"
                };
            }

            if (string.IsNullOrWhiteSpace(image))
            {
                return new ValidationResult
                {
                    Success = false,
                    IsValidationError = true,
                    Message = "Image path can't be empty"
                };
            }

            if (hasText==false)
            {
                return new ValidationResult
                {
                    Success = false,
                    IsValidationError = true,
                    Message = "Content must have at least one word"
                };
            }

            bool titleExists = items.Any(c =>!object.ReferenceEquals(c, currentItem) && string.Equals(c.Text, title, StringComparison.OrdinalIgnoreCase));

            if (titleExists)
            {
                return new ValidationResult
                {
                    Success = false,
                    IsValidationError = false,
                    Message = "Title already exists. Please choose a different title"
                };
            }

            return new ValidationResult
            {
                Success = true,
                IsValidationError = false,
                Message = "Validation successful"
            };

        }
    }
}
