using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels
{
    public class InsureeAttachments
    {
        public int idAttachment { get; set; }
        public int? InsureeId { get; set; }
        public DateTime Date { get; set; }
        public string Folder { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public string Mime { get; set; }
        public string Document { get; set; }
    }
}
