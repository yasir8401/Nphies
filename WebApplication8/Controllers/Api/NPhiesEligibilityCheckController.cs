using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication8.Helpers;
using WebApplication8.Models;
using static WebApplication8.Models.BupaPharmacyRequest;
using RestSharp;
using System.Web.Mvc;
using Newtonsoft.Json;
using Hl7.Fhir.Model;
using System;
using System.Collections;
using Hl7.Fhir.Serialization;
using System.IO;

namespace WebApplication8.Controllers.Api
{
    public class NPhiesEligibilityCheckController : ApiController
    {

        [System.Web.Http.Route("api/NPhiesService/EligibilityRequest")]

        public async Task<ActionResult> PostNPhiesEligibilityCheckRequest([FromBody] BupaPharmacyRequestObject jsonRequest)
        {

            string ServiceRootUrl = "http://sgw.s.nphies.sa/tmb/$process-message";

            string messageHeaderId = Guid.NewGuid().ToString();
            string bundleId = Guid.NewGuid().ToString();

            List<Bundle.EntryComponent> _entry = new List<Bundle.EntryComponent>();

            //var FhirClient = new Hl7.Fhir.Rest.FhirClient(ServiceRootUrl);

            //Bundle Initilization

            #region "Bundle Definition"
            Bundle _bundle = new Bundle();
            _bundle.Type = Bundle.BundleType.Message;

            //Bundle Metadata
            _bundle.Timestamp = DateTime.Now;
            _bundle.Id = bundleId;
            _bundle.Meta = new Meta()
            {
                Profile = new List<string>() { "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/bundle|1.0.0" }
            };

            #endregion

            #region MessageHeader
            //MessageHeader resource
            MessageHeader _header = new MessageHeader();
            _header.Id = messageHeaderId;

            //MessageHeader Meta
            _header.Meta = new Meta()
            {
                Profile = new List<string>() { "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/message-header|1.0.0" }
            };

            
            //MessageHeader EventCoding
            var messageEventCoding = new Coding();
            messageEventCoding.System = "http://nphies.sa/terminology/CodeSystem/ksa-message-events";
            messageEventCoding.Code = "eligibility-request";
            _header.Event = messageEventCoding;

            //MessageHeader Destination
            MessageHeader.MessageDestinationComponent messageHeaderDestination = new MessageHeader.MessageDestinationComponent();
            messageHeaderDestination.Endpoint = "http://nphies.sa/license/payer-license/TMB-INS";
            ResourceReference messageDestinationReceiver = new ResourceReference();
            messageDestinationReceiver.Identifier = getIdentifier("http://nphies.sa/license/payer-license", "TMB-INS");
            messageDestinationReceiver.Type = ResourceType.Organization.ToString();
            messageHeaderDestination.Receiver = messageDestinationReceiver;
            _header.Destination = new List<MessageHeader.MessageDestinationComponent> { messageHeaderDestination };

            //MessageHeader Sender
            ResourceReference messageSenderResource = new ResourceReference();
            messageSenderResource.Type = ResourceType.Organization.ToString();
            messageSenderResource.Identifier = getIdentifier("http://nphies.sa/license/provider-license", "PR-FHIR");
            _header.Sender = messageSenderResource;

            //MessageHeader Source
            MessageHeader.MessageSourceComponent messageSourceComponent = new MessageHeader.MessageSourceComponent();
            messageSourceComponent.Endpoint = "http://nphies.sa/license/provider-license/PR-FHIR";
            _header.Source = messageSourceComponent;

            //MessageHeader Focus
            _header.Focus = new List<ResourceReference> { getResource("http://pr-fhir.com.sa/CoverageEligibilityRequest/1001") };

            //Adding MessageHeader Entry
            _bundle.AddResourceEntry(_header, "urn:uuid:" + messageHeaderId);

            #endregion

            #region CoverageEligibilityRequest
            //CoverageEligibilityRequest entry
            CoverageEligibilityRequest coverageEligibilityRequest = new CoverageEligibilityRequest();
            coverageEligibilityRequest.Id = "1001";
            coverageEligibilityRequest.Meta = new Meta() { Profile = new List<string>() { } };
            coverageEligibilityRequest.Status = FinancialResourceStatusCodes.Active;
            coverageEligibilityRequest.Serviced = new Date("2020-12-25");
            coverageEligibilityRequest.Created = DateTime.UtcNow.ToString("yyyy-MM-dd");
            //CoverageEligibilityRequest identifier
            coverageEligibilityRequest.Identifier = new List<Identifier>() { getIdentifier("http://pr-fhir.com.sa/CoverageEligibilityRequest", RandomNumber(13)) };

            //CoverageEligibilityRequest Insurance
            CoverageEligibilityRequest.InsuranceComponent insuranceComponent = new CoverageEligibilityRequest.InsuranceComponent();
            insuranceComponent.Coverage = getResource("http://provider.com/Coverage/21");
            coverageEligibilityRequest.Insurance = new  List<CoverageEligibilityRequest.InsuranceComponent>() { insuranceComponent };

            //CoverageEligibilityRequest Priority
            coverageEligibilityRequest.Priority = getCodeableConcept("http://terminology.hl7.org/CodeSystem/processpriority", "normal");

            //CoverageEligibilityRequest Purpose
            coverageEligibilityRequest.Purpose = new List<CoverageEligibilityRequest.EligibilityRequestPurpose?>() { CoverageEligibilityRequest.EligibilityRequestPurpose.Benefits };
            
            //CoverageEligibilityRequest Patient Reference
            coverageEligibilityRequest.Patient = getResource("Patient/5588");

            //CoverageEligibilityRequest Provider Reference
            coverageEligibilityRequest.Provider = getResource("Organization/10");

            //CoverageEligibilityRequest Insurer Reference
            coverageEligibilityRequest.Insurer = getResource("Organization/11");

            _bundle.AddResourceEntry(coverageEligibilityRequest, "http://pr-fhir.com.sa/CoverageEligibilityRequest/1001");

            #endregion

            #region Coverage
            //Coverage entry
            Coverage coverage = new Coverage();
            coverage.SubscriberId = "0000000099";
            coverage.Id = "21";
            coverage.Status = FinancialResourceStatusCodes.Active;

            //Coverage Identifier
            coverage.Identifier = new List<Identifier>() { getIdentifier("http://payer.com/memberid", "0000000099") };

            //Coverage Period
            coverage.Period = getPeriod("2020-09-28", "2020-09-30");

            //Coverage Subscriber
            coverage.Subscriber = getResource("http://provider.com/Patient/5588");

            //Coverage Type
            coverage.Type = getCodeableConceptWithDisplay("http://nphies.sa/terminology/CodeSystem/coverage-type", "EHCPOL", "extended healthcare");


            //Coverage Payer
            coverage.Payor = new List<ResourceReference>() { getResource("http://provider.com/Organization/11") };

            //Coverage Beneficiary
            coverage.Beneficiary = getResource("http://provider.com/Patient/5588");

            //Coverage Meta
            coverage.Meta = getMeta("http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/coverage");

            //Covrage Relationship
            coverage.Relationship = getCodeableConceptWithDisplay("http://terminology.hl7.org/CodeSystem/subscriber-relationship", "self", "self");

            //Coverage Policyholder
            coverage.PolicyHolder = getResource("http://provider.com/Organization/10");

            _bundle.AddResourceEntry(coverage, "http://provider.com/Coverage/21");

            #endregion

            #region Patient
            //Patient Entry
            Patient patient = new Patient();
            patient.Id = "5588";
            patient.Meta = getMeta("http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/patient|1.0.0");
            patient.Gender = AdministrativeGender.Male;
            patient.Active = true;
            patient.BirthDate ="1949-08-04";

            //Patient Identifier
            patient.Identifier = new List<Identifier>() { getPatientIdentifier("http://moi.gov.sa/id/iqama", "00000000003", "http://nphies.sa/terminology/CodeSystem/patient-identifier-type", "DP") };
            //Patient ManageOrganization
            patient.ManagingOrganization = getResource("http://provider.com/Organization/10");

            //Patient _gender
            CodeableConcept genderExtension = new CodeableConcept();
            Coding genderExtensionCoding = new Coding();
            genderExtensionCoding.System = "http://nphies.sa/terminology/CodeSystem/ksa-administrative-gender";
            genderExtensionCoding.Code = "male";
            genderExtension.Coding = new List<Coding>() { genderExtensionCoding };

            Extension extension = new Extension();
            extension.Value = genderExtension;
            extension.Url = "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-ksa-administrative-gender";

            patient.GenderElement.Extension.Add(extension);

            //Patient Name
            patient.Name = new List<HumanName>() { getName(new List<string>() { "Sara", "Bashir", "Ahmad" }, HumanName.NameUse.Official, "Sara Bashir Ahmad Anabtawi") };

            //Patient Telecom
            patient.Telecom = new List<ContactPoint>() { getContactPoint(ContactPoint.ContactPointSystem.Phone, "0099656856") };

            //Patient Marital Status
            patient.MaritalStatus = getCodeableConcept("http://terminology.hl7.org/CodeSystem/v3-MaritalStatus", "Married");

            _bundle.AddResourceEntry(patient, "http://provider.com/Patient/5588");

            #endregion

            #region Organization/Provider 
            //Provider Info
            Organization porviderOrganization = new Organization();
            porviderOrganization.Identifier = new List<Identifier>() { getIdentifierWithUse("http://nphies.sa/license/provider-license", "PR-FHIR", Identifier.IdentifierUse.Official) };
            porviderOrganization.Meta = getMeta("http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/provider-organization");
            porviderOrganization.Name = "Ibby Davydoch";
            porviderOrganization.Active = true;
            porviderOrganization.Id = "10";
            porviderOrganization.Type = new List<CodeableConcept>() { getCodeableConceptWithText("http://nphies.sa/terminology/CodeSystem/organization-type", "prov", "PR-34-309") };

            _bundle.AddResourceEntry(porviderOrganization, "http://provider.com/Organization/10");

            #endregion

            #region Organization/Payer 
            //Provider Info
            Organization payerOrganization = new Organization();
            payerOrganization.Identifier = new List<Identifier>() { getIdentifierWithUse("http://nphies.sa/license/payer-license", "TMB-INS", Identifier.IdentifierUse.Official) };
            payerOrganization.Meta = getMeta("http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/insurer-organization");
            payerOrganization.Name = "Insurance Company 18";
            payerOrganization.Active = true;
            payerOrganization.Id = "11";
            payerOrganization.Type = new List<CodeableConcept>() { getCodeableConceptWithText("http://nphies.sa/terminology/CodeSystem/organization-type", "ins", "TMB-INS") };

            _bundle.AddResourceEntry(payerOrganization, "http://provider.com/Organization/11");


            #endregion

            var requestJson = _bundle.ToJson();
            var fhirSerialzer = new FhirJsonParser();
            Bundle resultBundle;
            bool? coverageInforce = false;
            List<string> errorCodes = new List<string>();
            List<string> errorMessages = new List<string>();

            HttpWebResponse httpResponse = null;
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://sgw.s.nphies.sa/tmb/$process-message");
                httpWebRequest.ContentType = "application/fhir+json";
                httpWebRequest.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                httpWebRequest.Headers["username"] = "Provider";
                httpWebRequest.Headers["password"] = "P@ssw0rd";
                httpWebRequest.Headers["Authorization"] = "Bearer ABC";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(requestJson);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    
                    resultBundle = fhirSerialzer.Parse<Bundle>(result);

                    if (httpResponse.StatusCode == HttpStatusCode.Created || httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var entry in resultBundle.Entry)
                        {
                            if (entry.Resource.TypeName == "CoverageEligibilityResponse") 
                            {
                                var coverageEligibilityResponse = (CoverageEligibilityResponse)entry.Resource;

                                coverageInforce = coverageEligibilityResponse.Insurance[0].Inforce;
                                errorCodes = null;
                                errorMessages = null;
                            }
                        }
                    }


                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorContennt = reader.ReadToEnd().Trim();

                            resultBundle = fhirSerialzer.Parse<Bundle>(errorContennt);

                            foreach (var entry in resultBundle.Entry)
                            {
                                if (entry.Resource.TypeName == "CoverageEligibilityResponse")
                                {
                                    var coverageEligibilityResponse = (CoverageEligibilityResponse)entry.Resource;

                                    if (coverageEligibilityResponse.Error != null && coverageEligibilityResponse.Error.Count > 0)
                                    {
                                        coverageInforce = coverageEligibilityResponse.Insurance[0].Inforce;

                                        foreach (var errorMsg in coverageEligibilityResponse.Error)
                                        {
                                            errorMessages.Add(errorMsg.Code.Text);
                                        }

                                    }
                                    
                                }
                            }

                        }
                    }

                }

                
            }

            var resultObj = new ContentResult
            {
                Content = JsonConvert.SerializeObject(new
                {
                    ReturnCode = errorCodes != null ? String.Join(",", errorCodes) : null,
                    ReturnMsgs = errorMessages != null ? String.Join(",", errorMessages) : null,
                    ReturnData = (bool)coverageInforce ? "The Coverage is Inforce" : "The Coverage is not inforce. Please check errors!"
                },
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }

                ),
                ContentType = "application/json"
            };

            return resultObj;

        }

        public ResourceReference getResource(string Reference) 
        {
            ResourceReference resourceReference = new ResourceReference();
            resourceReference.Reference = Reference;

            return resourceReference;
        }

        public Meta getMeta(string profileURL)
        {
            Meta meta = new Meta();
            List<string> profiles = new List<string>();

            profiles.Add(profileURL);

            meta.Profile = profiles;

            return meta;

        }

        public CodeableConcept getCodeableConceptWithDisplay(string systemmUrl, string code, string display)
        {
            CodeableConcept codeableConcept = new CodeableConcept();
            Coding coding = new Coding();
            coding.System = systemmUrl;
            coding.Code = code;
            coding.Display = display;
            codeableConcept.Coding = new List<Coding>() { coding };

            return codeableConcept;

        }

        public CodeableConcept getCodeableConcept(string systemmUrl, string code)
        {
            CodeableConcept codeableConcept = new CodeableConcept();
            Coding coding = new Coding();
            coding.System = systemmUrl;
            coding.Code = code;
            codeableConcept.Coding = new List<Coding>() { coding };
            
            return codeableConcept;

        }

        public CodeableConcept getCodeableConceptWithText(string systemmUrl, string code, string text)
        {
            CodeableConcept codeableConcept = new CodeableConcept();
            Coding coding = new Coding();
            coding.System = systemmUrl;
            coding.Code = code;
            codeableConcept.Coding = new List<Coding>() { coding };
            codeableConcept.Text = text;

            return codeableConcept;

        }

        public Identifier getIdentifier(string systemUrl, string value)
        {
            Identifier identifier = new Identifier();
            identifier.System = systemUrl;
            identifier.Value = value;

            return identifier;
        }

        public Period getPeriod(string start, string end)
        {
            Period period = new Period();
            period.Start = start;
            period.End = end;


            return period;
        }

        public ContactPoint getContactPoint(ContactPoint.ContactPointSystem system, string value)
        {
            ContactPoint contactpoint = new ContactPoint();
            contactpoint.System = system;
            contactpoint.Value = value;

            return contactpoint;
        }

        public HumanName getName(List<string> given, HumanName.NameUse use, string text)
        {
            HumanName humanName = new HumanName();
            humanName.Given = given;
            humanName.Use = use;
            humanName.Text = text;

            return humanName;

        }
        public Identifier getIdentifierWithUse(string systemUrl, string value, Identifier.IdentifierUse use)
        {
            Identifier identifier = new Identifier();
            identifier.System = systemUrl;
            identifier.Value = value;
            identifier.Use = use;
            
            return identifier;
        }


        public Coding getCoding(string systemUrl, string code)
        {
            Coding coding = new Coding();
            coding.System = systemUrl;
            coding.Code = code;

            return coding;
        }

        public Identifier getPatientIdentifier(string identifierSytem, string identifierValue, string identifierCodingSystem, string identifierCodingCode)
        {
            Identifier patientIdentifier = new Identifier();
            patientIdentifier.System = identifierSytem;
            patientIdentifier.Value = identifierValue;
            CodeableConcept patientIdentifierType = new CodeableConcept();
            Coding patientIdentifierTypeCoding = new Coding();
            patientIdentifierTypeCoding.System = identifierCodingSystem;
            patientIdentifierTypeCoding.Code = identifierCodingCode;
            patientIdentifierType.Coding = new List<Coding> { patientIdentifierTypeCoding };
            patientIdentifier.Type = patientIdentifierType;

            return patientIdentifier;
        }

        private static Random random = new Random();
        public static string RandomNumber(int length)
        {
            const string chars = "1234567890";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
