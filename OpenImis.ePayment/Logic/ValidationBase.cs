using Microsoft.Extensions.Configuration;
using OpenImis.ePayment.Data;
using OpenImis.ePayment.Models.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Logic
{
    public class ValidationBase
    {
        public virtual ValidationResult InsureeNumber(string insureeNumber)
        {
            if (insureeNumber != null)
            {
                if (insureeNumber.Length < 12 && insureeNumber.Length > 0)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("1:Wrong format of insurance number ");
            }

            return ValidationResult.Success;
        }

        public virtual ValidationResult OfficerCode(object value)
        {
            if (value != null)
            {
                if (value.ToString().Length < 8)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("3:Not valid enrolment officer code");
            }
            return ValidationResult.Success;
        }

        public virtual ValidationResult ReconciliationData(ReconciliationRequest model)
        {
            if (model != null)
            {
                DateTime? startDate = null;
                DateTime? endDate = null;
                DateTime Odate;

                // Conversion to the appropriate date format
                if (DateTime.TryParseExact(model.date_from, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out Odate))
                {
                    startDate = DateTime.ParseExact(model.date_from, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (DateTime.TryParseExact(model.date_to, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out Odate))
                {
                    endDate = DateTime.ParseExact(model.date_to, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                // Validation
                if (startDate == null || endDate == null)
                {
                    return new ValidationResult("1-Missing specification of the period");
                }
                else if (startDate > DateTime.Now && endDate > startDate)
                {
                    return new ValidationResult("3-The period is in the future");
                }
                else if (startDate >= endDate)
                {
                    return new ValidationResult("2-Beginning date follows closing date of the period");
                }

            }
            return ValidationResult.Success;
        }

        public int GetOfficerIdByUserUUID(Guid userUUID, IConfiguration configuration)
        {
            try
            {
                var sSQL = @"SELECT O.OfficerID
                            FROM tblUsers U
                            INNER JOIN tblOfficer O ON U.LoginName = O.Code
                            WHERE U.ValidityTo IS NULL
                            AND O.ValidityTo IS NULL
                            AND U.UserUUID = @UserUUID";

                var dh = new DataHelper(configuration);
                SqlParameter[] parameters = { new SqlParameter("@UserUUID", userUUID) };
                var dt = dh.GetDataTable(sSQL, parameters, System.Data.CommandType.Text);

                if (dt.Rows.Count > 0)
                    return (int)dt.Rows[0]["OfficerId"];

            }
            catch (Exception)
            {

                throw;
            }
            return 0;
        }
    }
}
