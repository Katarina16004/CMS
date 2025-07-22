using System;
using System.Collections.Generic;
using CMS.Models;

namespace CMS.Services.Interfaces
{
    public interface IContentValidationService
    {
        public ValidationResult ValidationSuccessful(List<ContentItem> items, string title, string? image, bool hasText);
    }
}
