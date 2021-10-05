using GSK.HealthProfessional.Model.Helper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSK.HealthProfessional.Model
{
    public class HealthProfessionalModel: GoogleReCaptchaModelBase
    {

        [Key]
        public Guid ProfessionalId { get; set; }
        [Required(ErrorMessage ="Campo obrigatório")]
        [DisplayName("Nome")]
        [StringLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DisplayName("Sobrenome")]
        [StringLength(200)]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail em formato inválido.")]
        [StringLength(200)]      
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DisplayName("Redigitar E-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail em formato inválido.")]
        [StringLength(200)]
        [Compare("Email", ErrorMessage = "O campo E-mail e Redigitar E-mail não combinam.")]
        public string RedigitarEmail { get; set; }
        
        [DisplayName("Senha")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(20, ErrorMessage = "A senha precisa ter mais de 5 caracteres.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [DisplayName("Confirmação de senha")]
        [StringLength(20, ErrorMessage = "A confirmação de senha precisa ter mais de 5 caracteres.", MinimumLength = 5)]
        [Compare("Password", ErrorMessage="O campo senha e confirmação de senha não combinam.")]
        public string PasswordConfirmation { get; set; }


        [DisplayName("Declaro meu consentimento para o processamento dos dados pessoais fornecidos por mim para obtenção de informações e serviços solicitados.")]
        public bool Consentimento { get; set; }

        [DisplayName("Ao informar meus dados, eu concordo com o Aviso de Proteção de Dados.")]        
        public bool AcceptsTermUse { get; set; }


        [DisplayName("Código SAP")]
        [StringLength(200)]
        public string CodigoSAP { get; set; }

        public string ClientType { get; set; }

    }
}
