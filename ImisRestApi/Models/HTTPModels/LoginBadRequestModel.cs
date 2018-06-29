using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.HTTPModels
{
    /// <summary>
    /// Error messages for Login Bad Request
    /// </summary>
    public class LoginBadRequestModel: BadRequestModel
    {
        public static readonly string USERNAME_REQUIRED_ERROR = "The Username field is required";
        public static readonly string PASSWORD_REQUIRED_ERROR = "The Password field is required";
        public static readonly string INVALID_REQUEST_BODY_ERROR = "The request body is invalid";

        public static List<string> GetBadRequestErrors(LoginRequestModel request)
        {
            List<string> errors = new List<string>();

            if (request.Username == null)
            {
                errors.Add(USERNAME_REQUIRED_ERROR);
            }

            if (request.Password == null)
            {
                errors.Add(PASSWORD_REQUIRED_ERROR);
            }

            return errors;
        }


    }
}
