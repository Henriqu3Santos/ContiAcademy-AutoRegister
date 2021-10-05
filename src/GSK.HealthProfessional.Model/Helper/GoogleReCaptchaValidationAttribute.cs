using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSK.HealthProfessional.Model.Helper
{
    public class GoogleReCaptchaValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Lazy<ValidationResult> errorResult = new Lazy<ValidationResult>(() => new ValidationResult("Google reCAPTCHA validation failed", new String[] { validationContext.MemberName }));

            if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            {
                return errorResult.Value;
            }

            IConfiguration configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
            String reCaptchResponse = value.ToString();
            String reCaptchaSecret = configuration.GetSection("GoogleReCaptcha:SecretKey").Value;


            HttpClient httpClient = new HttpClient();
            var httpResponse = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?onload=onloadCallback&render=explicit&secret={reCaptchaSecret}&response={reCaptchResponse}").Result;
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return errorResult.Value;
            }

            String jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(jsonResponse);
            if (jsonData.success != true.ToString().ToLower())
            {
                return errorResult.Value;
            }

            return ValidationResult.Success;

        }
    }
}
