using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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

namespace WebApplication8.Controllers.Api
{
    public class BUPAPharmacyServiceController : ApiController
    {
        IRestResponse response;


        private bool isNullResponse = true;
        private int returnCode = 0;
        private string returnMsgs = "";


        [System.Web.Http.Route("api/BUPAPharmacyService/ApprovalRequest")]
        public async Task<ActionResult> PostPharmacyApprovalRequest([FromBody] BupaPharmacyRequestObject jsonRequest)
        {
            var baseUrl = "https://test-api.bupa.com.sa/bupa-organization/point/preauth/pbmPharmacyDetails";
            var clientID = "352d6131c9d92ddd38378548e631495e";
            var clientSecret = "c7b0436b3a10bc3817d49578b94f1cc7";
            var providerId = "XD3cN8+lQgNehTbmSf7LI97P3sqGbu0gfU88kxIBMic=";
            HttpStatusCode status;

            var client = new RestClient(baseUrl);
            var request = new RestRequest(Method.POST);


            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            BupaPharmacyRequestObject requestObject = jsonRequest;
            //BupaPharmacyRequestObject requestBody = getRequestBodyWithMultipleItems();

            request.AddJsonBody(requestObject);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("x-ibm-client-id", clientID);
            request.AddHeader("x-ibm-client-secret", clientSecret);
            request.AddHeader("providerid", providerId);

            ValidateRequestParams(requestObject);

            if (returnCode == 0)
            {
                response = client.Execute(request);

                
                if (response != null)
                {
                    isNullResponse = false;
                }
            }
            else
            {
                isNullResponse = true;
            }
            

            //var reponseObject = JsonConvert.DeserializeObject<BupaPharmacyResponseObject>(response.Content);

            //BupaPharmacyRequestObject requestObject = jsonRequest;
            //BupaPharmacyRequestObject requestB = JsonConvert.DeserializeObject<BupaPharmacyRequestObject>(jsonRequest.ToString());
            //BupaPharmacyRequestObject requestBody = getRequestBodyWithMultipleItems();
            //BupaPharmacyRequestObject requestBody = getRequestBodyWithSingleItems();


            //var response = RestUtility.CallService<BupaPharmacyResponseObject>(baseUrl, null, requestObject, "POST", clientID, clientSecret, providerId, out status) as BupaPharmacyResponseObject;

            var result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(new
                {
                    ReturnCode = returnCode,
                    ReturnMsgs = returnMsgs,
                    ReturnData = !isNullResponse ? response.Content : ""
                },
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }

                ),
                ContentType = "application/json"
            };

            return result;
        }

        private void ValidateRequestParams(BupaPharmacyRequestObject requestObject)
        {
           
            if (requestObject.dateOfAdmission == null || requestObject.dateOfAdmission == DateTime.MinValue)
            {
                returnCode = 1;
                returnMsgs += "Date of Admission cannot be null or less than min value";
            }

            if (requestObject.departmentType == null || requestObject.departmentType == "")
            {
                returnCode = 1;
                returnMsgs += "Department Type cannot be null or empty";
            }

            if (requestObject.detail_DayOfSupply == null || !requestObject.detail_DayOfSupply.Any())
            {
                returnCode = 1;
                returnMsgs += "Day of Supply is required";
            }

            if (requestObject.detail_DiagnosisCode == null || !requestObject.detail_DiagnosisCode.Any())
            {
                returnCode = 1;
                returnMsgs += "Diagnosis Code is required";
            }

            if (requestObject.detail_EstimatedCost == null || !requestObject.detail_EstimatedCost.Any())
            {
                returnCode = 1;
                returnMsgs += "Estimated Cost is required";
            }

            if (requestObject.detail_ItemNo == null || !requestObject.detail_ItemNo.Any())
            {
                returnCode = 1;
                returnMsgs += "ItemNo is required";
            }

            if (requestObject.detail_Per == null || !requestObject.detail_Per.Any())
            {
                returnCode = 1;
                returnMsgs += "Detail Per is required";
            }

            if (requestObject.detail_Quantity == null || !requestObject.detail_Quantity.Any())
            {
                returnCode = 1;
                returnMsgs += "Detail Quantity is required";
            }

            if (requestObject.detail_ServiceCode == null || !requestObject.detail_ServiceCode.Any())
            {
                returnCode = 1;
                returnMsgs += "Detail Service Code(s) is required";
            }

            if (requestObject.detail_ServiceDescription == null || !requestObject.detail_ServiceDescription.Any())
            {
                returnCode = 1;
                returnMsgs += "Detail Description Code(s) is required";
            }

            if (requestObject.detail_ServiceType == null || !requestObject.detail_ServiceType.Any())
            {
                returnCode = 1;
                returnMsgs += "Detail Service Type(s) is required";
            }

            if (requestObject.detail_SupplyDateFrom == null || !requestObject.detail_SupplyDateFrom.Any())
            {
                returnCode = 1;
                returnMsgs += "Supply From Date is required";
            }

            if (requestObject.detail_SupplyDateTo == null || !requestObject.detail_SupplyDateTo.Any())
            {
                returnCode = 1;
                returnMsgs += "Supply From To is required";
            }

            if (requestObject.detail_Times == null || !requestObject.detail_Times.Any())
            {
                returnCode = 1;
                returnMsgs += "Detail Times is required";
            }

            if (requestObject.detail_Unit == null || !requestObject.detail_Unit.Any())
            {
                returnCode = 1;
                returnMsgs += "Detail Unit is required";
            }

            if (requestObject.detail_UnitType == null || !requestObject.detail_UnitType.Any())
            {
                returnCode = 1;
                returnMsgs += "Detail Unit Type is required";
            }


            if (requestObject.diagnosisCode == null || requestObject.diagnosisCode == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Diagnosis Code is required";
            }

            if (requestObject.diagnosisDescription == null || requestObject.diagnosisDescription == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Diagnosis Description is required";
            }

            if (requestObject.estimatedAmount == null || requestObject.estimatedAmount == 0.00)
            {
                returnCode = 1;
                returnMsgs += "Estimated Amount is required";
            }

            if (requestObject.memberID_Igama == null || requestObject.memberID_Igama == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Either Membership number or Iqama/ID number is required";
            }

            if (requestObject.memberName == null || requestObject.memberName == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Memeber Name is required";
            }

            if (requestObject.membershipNo == null || requestObject.membershipNo == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Memebership Number is required";
            }

            if (requestObject.patientFileNo == null || requestObject.patientFileNo == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Patient File No is required";
            }

            if (requestObject.physician_Name == null || requestObject.physician_Name == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Physician Name is required";
            }

            if (requestObject.providerCode == null || requestObject.providerCode == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Provider Code is required";
            }

            if (requestObject.treatmentType == null || requestObject.treatmentType == string.Empty)
            {
                returnCode = 1;
                returnMsgs += "Treatment Type is required";
            }

        }

        private BupaPharmacyRequestObject getRequestBodyWithMultipleItems()
        {
            BupaPharmacyRequestObject requestBody = new BupaPharmacyRequestObject()
            {
                IVF_Pregnancy = null,
                RTA = null,
                abortion = null,
                bloodPressure = "",
                cardIssueNumber = 0,
                checkUp = null,
                chiefComplaintsAndMainSymptoms = "",
                chronic = null,
                congenital = null,
                dateOfAdmission = new DateTime(2020, 05, 03, 00, 00, 00),
                death = null,
                departmentType = "GEN",
                detail = null,
                detail_BenefitHead = new List<string> { "", "" },
                detail_DayOfSupply = new List<int> { 1, 2 },
                detail_DiagnosisCode = new List<string> { "R51", "R51" },
                detail_Dosage = new List<string> { "test please ignore", "test please ignore" },
                detail_EstimatedCost = new List<double> { 20.0, 5.0 },
                detail_ExemptCat = new List<string> { "N", "N" },
                detail_ItemNo = new List<int> { 1, 2 },
                detail_Per = new List<string> { "1", "1" },
                detail_Quantity = new List<long> { 4, 1 },
                detail_Referral_Ind = new List<string> { "N", "N" },
                detail_Remark = new List<string> { "test please ignore", "test please ignore" },
                detail_ServiceCode = new List<string> { "1-288-98", "5-288-99" },
                detail_ServiceDescription = new List<string> { "PANADOL 500MG TAB, 500 mg, Tablet, 24", "PANADOL 500 mg film-coated tablet, 500 mg, Film-coated tablet, 24" },
                detail_ServiceType = new List<string> { "ME", "ME" },
                detail_SupplyDateFrom = new List<DateTime> { new DateTime(), new DateTime() },
                detail_SupplyDateTo = new List<DateTime> { new DateTime(), new DateTime() },
                detail_SupplyPeriod = new List<string> { "", "" },
                detail_Times = new List<int> { 1, 1 },
                detail_Unit = new List<int> { 96, 24 },
                detail_UnitType = new List<string> { "Tablet", "Film-coated tablet" },
                diagnosisCode = "R51",
                diagnosisDescription = "test please ignore",
                durationOfIllness = "",
                estimatedAmount = 25.00,
                exDetail = null,
                exempted = "N",
                expected_Delivery_Date = null,
                gender = "M",
                gravida = null,
                infertility = null,
                last_Menstrual_Period = null,
                lengthOfStay = 1,
                live = null,
                medAutoValidateCode = "ENT",
                memberID_Igama = "2325351134",
                memberMobileNo = null,
                memberName = "Mahmood Ahmed Hussain",
                membershipNo = "17604697",
                normal_Pregnancy = null,
                otherConditions = "",
                para = null,
                patientFileNo = "test",
                physician_Name = "KAMAL MOHAMMED  AL SHAMI",
                policyNo = "",
                possibleLineOfTreatment = "",
                preauthorisation_ID = 0,
                processUser = null,
                providerCode = "20134",
                providerFaxNo = "123456",
                psychiatric = null,
                pulse = null,
                quantity = 5,
                referral = "Y",
                temperature = null,
                transactionID = 0,
                transactionType = "N",
                treatmentType = "I",
                trustedDoctor = "N",
                vaccination = null,
                verificationID = 0,
                workRelated = null,
                Username = "",
                Password = ""
            };

            return requestBody;
        }

        private BupaPharmacyRequestObject getRequestBodyWithSingleItems()
        {
            BupaPharmacyRequestObject requestBody = new BupaPharmacyRequestObject()
            {
                IVF_Pregnancy = null,
                RTA = null,
                abortion = null,
                bloodPressure = "",
                cardIssueNumber = 0,
                checkUp = null,
                chiefComplaintsAndMainSymptoms = "",
                chronic = null,
                congenital = null,
                dateOfAdmission = new DateTime(2020, 05, 03, 00, 00, 00),
                death = null,
                departmentType = "GEN",
                detail = null,
                detail_BenefitHead = new List<string> { "" },
                detail_DayOfSupply = new List<int> { 1 },
                detail_DiagnosisCode = new List<string> { "R51" },
                detail_Dosage = new List<string> { "test please ignore" },
                detail_EstimatedCost = new List<double> { 20.0 },
                detail_ExemptCat = new List<string> { "N" },
                detail_ItemNo = new List<int> { 1 },
                detail_Per = new List<string> { "1" },
                detail_Quantity = new List<long> { 4 },
                detail_Referral_Ind = new List<string> { "N" },
                detail_Remark = new List<string> { "test please ignore" },
                detail_ServiceCode = new List<string> { "1-288-98" },
                detail_ServiceDescription = new List<string> { "PANADOL 500MG TAB, 500 mg, Tablet, 24" },
                detail_ServiceType = new List<string> { "ME" },
                detail_SupplyDateFrom = new List<DateTime> { new DateTime() },
                detail_SupplyDateTo = new List<DateTime> { new DateTime() },
                detail_SupplyPeriod = new List<string> { "" },
                detail_Times = new List<int> { 1 },
                detail_Unit = new List<int> { 96 },
                detail_UnitType = new List<string> { "Tablet" },
                diagnosisCode = "R51",
                diagnosisDescription = "test please ignore",
                durationOfIllness = "",
                estimatedAmount = 25.00,
                exDetail = null,
                exempted = "N",
                expected_Delivery_Date = null,
                gender = "M",
                gravida = null,
                infertility = null,
                last_Menstrual_Period = null,
                lengthOfStay = 1,
                live = null,
                medAutoValidateCode = "ENT",
                memberID_Igama = "2325351134",
                memberMobileNo = null,
                memberName = "Mahmood Ahmed Hussain",
                membershipNo = "17604697",
                normal_Pregnancy = null,
                otherConditions = "",
                para = null,
                patientFileNo = "test",
                physician_Name = "KAMAL MOHAMMED  AL SHAMI",
                policyNo = "",
                possibleLineOfTreatment = "",
                preauthorisation_ID = 0,
                processUser = null,
                providerCode = "20134",
                providerFaxNo = "123456",
                psychiatric = null,
                pulse = null,
                quantity = 5,
                referral = "Y",
                temperature = null,
                transactionID = 0,
                transactionType = "N",
                treatmentType = "I",
                trustedDoctor = "N",
                vaccination = null,
                verificationID = 0,
                workRelated = null,
                Username = "",
                Password = ""
            };

            return requestBody;
        }

       
    }
}
//            request.AddParameter("IVF_Pregnancy", requestObject.IVF_Pregnancy);
//            request.AddParameter("RTA", requestObject.RTA);
//            request.AddParameter("abortion", requestObject.abortion);
//            request.AddParameter("bloodPressure", requestObject.bloodPressure);
//            request.AddParameter("cardIssueNumber", requestObject.cardIssueNumber);
//            request.AddParameter("checkUp", requestObject.checkUp);

//            request.AddParameter("chiefComplaintsAndMainSymptoms", requestObject.chiefComplaintsAndMainSymptoms);
//            request.AddParameter("chronic", requestObject.chronic);
//            request.AddParameter("congenital", requestObject.congenital);
//            request.AddParameter("dateOfAdmission", requestObject.dateOfAdmission);
//            request.AddParameter("death", requestObject.death);
//            request.AddParameter("departmentType", requestObject.departmentType);
//            request.AddParameter("detail", requestObject.detail);
//            request.AddParameter("detail_BenefitHead", requestObject.detail_BenefitHead);
//            request.AddParameter("detail_DayOfSupply", requestObject.detail_DayOfSupply);
//            request.AddParameter("detail_DiagnosisCode", requestObject.detail_DiagnosisCode);
//            request.AddParameter("detail_Dosage", requestObject.detail_Dosage);
//            request.AddParameter("detail_EstimatedCost", requestObject.detail_EstimatedCost);
//            request.AddParameter("detail_ExemptCat", requestObject.detail_ExemptCat);
//            request.AddParameter("detail_ItemNo", requestObject.detail_ItemNo);
//            request.AddParameter("detail_Per", requestObject.detail_Per);
//            request.AddParameter("detail_Quantity", requestObject.detail_Quantity);
//            request.AddParameter("detail_Referral_Ind", requestObject.detail_Referral_Ind);
//            request.AddParameter("detail_Remark", requestObject.detail_Remark);
//            request.AddParameter("detail_ServiceCode", requestObject.detail_ServiceCode);
//            request.AddParameter("detail_ServiceDescription", requestObject.detail_ServiceDescription);
//            request.AddParameter("detail_ServiceType", requestObject.detail_ServiceType);
//            request.AddParameter("detail_SupplyDateFrom", requestObject.detail_SupplyDateFrom);
//            request.AddParameter("detail_SupplyDateTo", requestObject.detail_SupplyDateTo);
//            request.AddParameter("detail_SupplyPeriod", requestObject.detail_SupplyPeriod);
//            request.AddParameter("detail_Times", requestObject.detail_Times);
//            request.AddParameter("detail_Unit", requestObject.detail_Unit);
//            request.AddParameter("detail_UnitType", requestObject.detail_UnitType);
//            request.AddParameter("diagnosisCode", requestObject.diagnosisCode);
//            request.AddParameter("diagnosisDescription", requestObject.diagnosisDescription);
//            request.AddParameter("durationOfIllness", requestObject.durationOfIllness);
//            request.AddParameter("estimatedAmount", requestObject.estimatedAmount);
//            request.AddParameter("exDetail", requestObject.exDetail);
//            request.AddParameter("exempted", requestObject.exempted);
//            request.AddParameter("expected_Delivery_Date", requestObject.expected_Delivery_Date);
//            request.AddParameter("gender", requestObject.gender);
//            request.AddParameter("gravida", requestObject.gravida);
//            request.AddParameter("infertility", requestObject.infertility);
//            request.AddParameter("last_Menstrual_Period", requestObject.last_Menstrual_Period);
//            request.AddParameter("lengthOfStay", requestObject.lengthOfStay);
//            request.AddParameter("live", requestObject.live);
//            request.AddParameter("medAutoValidateCode", requestObject.medAutoValidateCode);
//            request.AddParameter("memberID_Igama", requestObject.memberID_Igama);
//            request.AddParameter("memberMobileNo", requestObject.memberMobileNo);
//            request.AddParameter("memberName", requestObject.memberName);
//            request.AddParameter("membershipNo", requestObject.membershipNo);
//            request.AddParameter("normal_Pregnancy", requestObject.normal_Pregnancy);
//            request.AddParameter("otherConditions", requestObject.otherConditions);
//            request.AddParameter("para", requestObject.para);
//            request.AddParameter("patientFileNo", requestObject.patientFileNo);
//            request.AddParameter("physician_Name", requestObject.physician_Name);
//            request.AddParameter("policyNo", requestObject.policyNo);
//            request.AddParameter("possibleLineOfTreatment", requestObject.possibleLineOfTreatment);
//            request.AddParameter("preauthorisation_ID", requestObject.preauthorisation_ID);
//            request.AddParameter("processUser", requestObject.processUser);
//            request.AddParameter("providerCode", requestObject.providerCode);
//            request.AddParameter("providerFaxNo", requestObject.providerFaxNo);
//            request.AddParameter("psychiatric", requestObject.psychiatric);
//            request.AddParameter("pulse", requestObject.pulse);
//            request.AddParameter("quantity", requestObject.quantity);
//            request.AddParameter("referral", requestObject.referral);
//            request.AddParameter("temperature", requestObject.temperature);
//            request.AddParameter("transactionID", requestObject.transactionID);
//            request.AddParameter("transactionType", requestObject.transactionType);
//            request.AddParameter("treatmentType", requestObject.treatmentType);
//            request.AddParameter("trustedDoctor", requestObject.trustedDoctor);
//            request.AddParameter("vaccination", requestObject.vaccination);
//            request.AddParameter("verificationID", requestObject.verificationID);
//            request.AddParameter("workRelated", requestObject.workRelated);
//            request.AddParameter("Username", requestObject.Username);
//            request.AddParameter("Password", requestObject.Password);
