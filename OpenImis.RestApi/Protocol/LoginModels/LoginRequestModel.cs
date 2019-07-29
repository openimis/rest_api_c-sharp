using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Protocol.LoginModels
{
    /// <summary>
    /// This class serves as a parameter class for the Login call 
    /// </summary>
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "The Username field is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The Password field is required")]
        public string Password { get; set; }

        //private List<string> ErrorMessages { get; set; }

        //private Boolean IsValidCalled { get; set; }

        //public readonly string USERNAME_REQUIRED_ERROR = "The Username field is required";
        //public readonly string PASSWORD_REQUIRED_ERROR = "The Password field is required";
        //public readonly string INVALID_REQUEST_BODY_ERROR = "The request body is invalid";

        //public LoginRequestModel()
        //{
        //    IsValidCalled = false;
        //}

        //public Boolean IsValid()
        //{
        //    Boolean IsValid = true;

        //    IsValidCalled = true;

        //    ErrorMessages = new List<string>();

        //    if (Username == null)
        //    {
        //        ErrorMessages.Add(USERNAME_REQUIRED_ERROR);
        //        IsValid = false;
        //    }

        //    if (Password == null)
        //    {
        //        ErrorMessages.Add(PASSWORD_REQUIRED_ERROR);
        //        IsValid = false;
        //    }

        //    return IsValid;
        //}

        ///// <summary>
        ///// Returns the error messages after calling validation method
        ///// </summary>
        ///// <returns></returns>
        //public string[] GetErrorMessages()
        //{
        //    if (!IsValidCalled)
        //    {
        //        IsValid();
        //    }

        //    return ErrorMessages.ToArray();
        //}


    }
}
