using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Contract
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, string sourceLang = "ar", string targetLang = "en");
    }
}
