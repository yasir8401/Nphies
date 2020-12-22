using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{
    public class BupaPharmacyRequest
    {
        public class BupaPharmacyRequestObject
        {
            public string IVF_Pregnancy { get; set; }
            public string RTA { get; set; }
            public string abortion { get; set; }
            public string bloodPressure { get; set; }
            public int cardIssueNumber { get; set; }
            public string checkUp { get; set; }
            public string chiefComplaintsAndMainSymptoms { get; set; }
            public string chronic { get; set; }
            public string congenital { get; set; }
            public DateTime dateOfAdmission { get; set; }
            public int? death { get; set; }
            public string departmentType { get; set; }
            public List<string> detail { get; set; }
            public List<string> detail_BenefitHead { get; set; }
            public List<int> detail_DayOfSupply { get; set; }
            public List<string> detail_DiagnosisCode { get; set; }
            public List<string> detail_Dosage { get; set; }
            public List<double> detail_EstimatedCost { get; set; }
            public List<string> detail_ExemptCat { get; set; }
            public List<int> detail_ItemNo { get; set; }
            public List<string> detail_Per { get; set; }
            public List<long> detail_Quantity { get; set; }
            public List<string> detail_Referral_Ind { get; set; }
            public List<string> detail_Remark { get; set; }
            public List<string> detail_ServiceCode { get; set; }
            public List<string> detail_ServiceDescription { get; set; }
            public List<string> detail_ServiceType { get; set; }
            public List<DateTime> detail_SupplyDateFrom { get; set; }
            public List<DateTime> detail_SupplyDateTo { get; set; }
            public List<string> detail_SupplyPeriod { get; set; }
            public List<int> detail_Times { get; set; }
            public List<int> detail_Unit { get; set; }
            public List<string> detail_UnitType { get; set; }
            public string diagnosisCode { get; set; }
            public string diagnosisDescription { get; set; }
            public string durationOfIllness { get; set; }
            public double? estimatedAmount { get; set; }
            public List<string> exDetail { get; set; }
            public string exempted { get; set; }
            public DateTime? expected_Delivery_Date { get; set; }
            public string gender { get; set; }
            public int? gravida { get; set; }
            public string infertility { get; set; }
            public DateTime? last_Menstrual_Period { get; set; }
            public int lengthOfStay { get; set; }
            public int? live { get; set; }
            public string medAutoValidateCode { get; set; }
            public string memberID_Igama { get; set; }
            public string memberMobileNo { get; set; }
            public string memberName { get; set; }
            public string membershipNo { get; set; }
            public string normal_Pregnancy { get; set; }
            public string otherConditions { get; set; }
            public int? para { get; set; }
            public string patientFileNo { get; set; }
            public string physician_Name { get; set; }
            public string policyNo { get; set; }
            public string possibleLineOfTreatment { get; set; }
            public int preauthorisation_ID { get; set; }
            public string processUser { get; set; }
            public string providerCode { get; set; }
            public string providerFaxNo { get; set; }
            public string psychiatric { get; set; }
            public double? pulse { get; set; }
            public int quantity { get; set; }
            public string referral { get; set; }
            public double? temperature { get; set; }
            public int transactionID { get; set; }
            public string transactionType { get; set; }
            public string treatmentType { get; set; }
            public string trustedDoctor { get; set; }
            public string vaccination { get; set; }
            public int verificationID { get; set; }
            public string workRelated { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }


        public class BupaPharmacyResponseObject
        {
            public long transactionID { get; set; }
            public int preAuthorizationID { get; set; }
            public string providerName { get; set; }
            public string insuranceCompany { get; set; }
            public string patientFileNo { get; set; }
            public string department { get; set; }
            public string providerFaxNo { get; set; }
            public DateTime? dateOfAdmission { get; set; }
            public string memberName { get; set; }
            public string idCardNo { get; set; }
            public int age { get; set; }
            public string gender { get; set; }
            public string policyHolder { get; set; }
            public string membershipNo { get; set; }
            public string memberClass { get; set; }
            public DateTime? expiryDate { get; set; }
            public string preauthorisationStatus { get; set; }
            public DateTime? appValidity { get; set; }
            public string roomType { get; set; }
            public string comments { get; set; }
            public string insuranceOfficer { get; set; }
            public string addComments { get; set; }
            public DateTime? dateTime { get; set; }
            public string chronicInd { get; set; }
            public string extDate { get; set; }
            public string extInd { get; set; }
            public string status { get; set; }
            public List<string> errorID { get; set; }
            public List<string> errorMessage { get; set; }

            //List item details
            public List<string> serviceCode { get; set; }
            public List<string> serviceDesc { get; set; }
            public List<string> supplyPeriod { get; set; }
            public List<DateTime?> supplyFrom { get; set; }
            public List<DateTime?> supplyTo { get; set; }
            public List<string> notes { get; set; }



        }

    }
}