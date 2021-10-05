using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Net;

namespace GSK.HealthProfessional.Service
{
    public class AuthenticationApiService
    {
        public CookieContainer CookieAuth { get; set; }
        private readonly IConfiguration _configuration;

        public AuthenticationApiService(IConfiguration configuration)
        {
            _configuration = configuration;
            Authenticate();
        }

        private void  Authenticate()
        {
            var urlApiNeolude = _configuration.GetSection("AppSettings").GetSection("UrlApiNeolude").Value; 
            var userNameApi = _configuration.GetSection("AppSettings:UserNameApiNeolude").Value;
            var passwordApi = _configuration.GetSection("AppSettings:PasswordApiNeolude").Value;
            var actionAuthentication = "/app/auth/";
            var client2 = new RestClient(urlApiNeolude);
            client2.CookieContainer = new CookieContainer();
            var requestAuthentication = new RestRequest(actionAuthentication, Method.POST);
            requestAuthentication.AddHeader("Content-Type", "application/json");
            requestAuthentication.AddHeader("Connection", "keep-alive");
            requestAuthentication.AddHeader("Accept", "*/*");
            requestAuthentication.AddParameter(new Parameter(
                name: "",
                value: $"{{ login:'{userNameApi}', password: '{passwordApi}' }}",
                contentType: "application/json",
                type: ParameterType.RequestBody));
            var responseAuthentication = client2.Execute(requestAuthentication);

            if (responseAuthentication.StatusCode == System.Net.HttpStatusCode.OK)
                CookieAuth = client2.CookieContainer;
            else
            CookieAuth = null ;

        }       

        public static string GenerateToken(string clientIdentifier, string clientSecret, string clientTicks)
        {

            var hashInput = clientIdentifier;
            hashInput += clientSecret;
            hashInput += clientTicks;

            var hashOutput = System.Security.Cryptography.SHA256.Create().ComputeHash(ToByteArray(hashInput));
            var token = Convert.ToBase64String(hashOutput);

            return token;
        }

        private  static byte[] ToByteArray( string text)
        {
            var bytes = new byte[text.Length * sizeof(char)];
            System.Buffer.BlockCopy(text.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }



    }
}
