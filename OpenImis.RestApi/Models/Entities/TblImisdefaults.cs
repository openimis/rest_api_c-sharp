using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblImisdefaults
    {
        public int DefaultId { get; set; }
        public int? PolicyRenewalInterval { get; set; }
        public string Ftphost { get; set; }
        public string Ftpuser { get; set; }
        public string Ftppassword { get; set; }
        public int? Ftpport { get; set; }
        public string FtpenrollmentFolder { get; set; }
        public string AssociatedPhotoFolder { get; set; }
        public string FtpclaimFolder { get; set; }
        public string FtpfeedbackFolder { get; set; }
        public string FtppolicyRenewalFolder { get; set; }
        public string FtpphoneExtractFolder { get; set; }
        public string FtpoffLineExtractFolder { get; set; }
        public decimal? AppVersionBackEnd { get; set; }
        public decimal? AppVersionEnquire { get; set; }
        public decimal? AppVersionEnroll { get; set; }
        public decimal? AppVersionRenewal { get; set; }
        public decimal? AppVersionFeedback { get; set; }
        public decimal? AppVersionClaim { get; set; }
        public int? OffLineHf { get; set; }
        public string WinRarFolder { get; set; }
        public string DatabaseBackupFolder { get; set; }
        public int? OfflineChf { get; set; }
        public string Smslink { get; set; }
        public string Smsip { get; set; }
        public string SmsuserName { get; set; }
        public string Smspassword { get; set; }
        public string Smssource { get; set; }
        public int? Smsdlr { get; set; }
        public int? Smstype { get; set; }
        public decimal? AppVersionFeedbackRenewal { get; set; }
        public decimal? AppVersionImis { get; set; }
    }
}
