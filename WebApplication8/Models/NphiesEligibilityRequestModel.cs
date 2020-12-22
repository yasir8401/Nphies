using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class NphiesEligibilityRequestModel
    {

        public string bundeId { get; set; }
        public string messageHeaderId { get; set; }
        public DateTime bundeTimeStamp { get; set; }
        public string bundleMetaProfile { get; set; }
        public string messageHeaderMetaProfile { get; set; }
        public string messageHeaderEventCodingSystem { get; set; }
        public string messageHeaderEventCodingCode { get; set; }
        public string messageHeaderDestinationEndPoint { get; set; }
        public string messageDestinationReceiverIdentifierSystem { get; set; }
        public string messageDestinationReceiverIdentifierValue { get; set; }
        public string messageDestinationReceiverType { get; set; }
        public string messageSenderResourceIdentifierSystem { get; set; }
        public string messageSenderResourceIdentifierValue { get; set; }
        public string messageSenderResourceType { get; set; }

    }
}