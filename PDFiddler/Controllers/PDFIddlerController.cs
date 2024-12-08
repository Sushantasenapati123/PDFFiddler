using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;

namespace PDFiddler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFIddlerController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PDFIddlerController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("FillPdfForm")]
        public IActionResult FillPdfForm([FromBody] PdfFormModel formData)
        {
            try
            {
               
                string inputFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "File\\InputFile", "Inputform.pdf");

                string outputFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "File\\OutputFile", "OutputForm.pdf");

               
                // Load the PDF document
                using (var pdfDocument = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Modify))
                {
                    // Get the AcroForm
                    var form = pdfDocument.AcroForm;

                    // Fill out the form fields by name
                    FillTextField(form, "Signature", formData.Signature);
                    FillTextField(form, "Officer Name", formData.OfficerName);
                    FillTextField(form, "Owner Name 1", formData.OwnerName1);
                    FillTextField(form, "Due Date", formData.DueDate);
                    FillTextField(form, "Owner Address", formData.OwnerAddress);
                    FillTextField(form, "Email", formData.Email);
                    FillTextField(form, "Phone#", formData.Phone);
                    FillTextField(form, "City, Prov PC", formData.City);

                    // Save the filled PDF document
                    pdfDocument.Save(outputFilePath);
                }

                // Return the filled PDF as a byte array in the response
                byte[] fileBytes = System.IO.File.ReadAllBytes(outputFilePath);
                return File(fileBytes, "application/pdf", "OutputForm.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"An error occurred: {ex.Message}" });
            }
        }

        private void FillTextField(PdfAcroForm form, string fieldName, string fieldValue)
        {
            try
            {
                if (form.Fields[fieldName] is PdfTextField textField)
                {
                    // Set the field text
                    textField.Text = fieldValue;

                    // Manually set font size and font type using PDF annotations (indirect method)
                    // We create a font with a specified size (e.g., 8)
                    XFont font = new XFont("Arial", 8, XFontStyle.Regular);

                    // This will set the text field appearance by modifying the font of the field's annotation
                    // This is a low-level approach to change the appearance of the form field
                    //textField.SetAppearance("Sig", font);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filling field {fieldName}: {ex.Message}");
            }
        }
       
        public class PdfFormModel
        {
            public string Signature { get; set; }
            public string OfficerName { get; set; }
            public string OwnerName1 { get; set; }
            public string DueDate { get; set; }
            public string OwnerAddress { get; set; }

            public string Email { get; set; }
            public string Phone { get; set; }
            public string City { get; set; }


        }           


        public class CityData
        {
            public int CityId { get; set; }
            public int StateId { get; set; }
            public string CityName { get; set; }

        }
        [HttpGet("Get_City")]
        public async Task<IActionResult> Get_City()
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Ok(new { success = false, responseMessage = message, responseText = "Model State is invalid", data = "" });
            }
            else
            {
                // Create a list of CityData with static data
                var cityList = new List<CityData>
     {
         // Cities in Odisha (StateId = 10)
         new CityData { CityId = 1, StateId = 10, CityName = "Bhubaneswar" },
         new CityData { CityId = 2, StateId = 10, CityName = "Cuttack" },
         new CityData { CityId = 3, StateId = 10, CityName = "Brahmapur" },

         // Cities in Bihar (StateId = 11)
         new CityData { CityId = 4, StateId = 11, CityName = "Patna" },
         new CityData { CityId = 5, StateId = 11, CityName = "Gaya" },
         new CityData { CityId = 6, StateId = 11, CityName = "Bhagalpur" },

         // Cities in Chhattisgarh (StateId = 12)
         new CityData { CityId = 7, StateId = 12, CityName = "Raipur" },
         new CityData { CityId = 8, StateId = 12, CityName = "Bhilai" },
         new CityData { CityId = 9, StateId = 12, CityName = "Bilaspur" },

         // Cities in England (StateId = 20)
         new CityData { CityId = 10, StateId = 20, CityName = "London" },
         new CityData { CityId = 11, StateId = 20, CityName = "Manchester" },
         new CityData { CityId = 12, StateId = 20, CityName = "Birmingham" },

         // Cities in Scotland (StateId = 21)
         new CityData { CityId = 13, StateId = 21, CityName = "Edinburgh" },
         new CityData { CityId = 14, StateId = 21, CityName = "Glasgow" },
         new CityData { CityId = 15, StateId = 21, CityName = "Aberdeen" },

         // Cities in Wales (StateId = 22)
         new CityData { CityId = 16, StateId = 22, CityName = "Cardiff" },
         new CityData { CityId = 17, StateId = 22, CityName = "Swansea" },
         new CityData { CityId = 18, StateId = 22, CityName = "Newport" },

         // Cities in Île-de-France (StateId = 30)
         new CityData { CityId = 19, StateId = 30, CityName = "Paris" },
         new CityData { CityId = 20, StateId = 30, CityName = "Boulogne-Billancourt" },
         new CityData { CityId = 21, StateId = 30, CityName = "Saint-Denis" },

         // Cities in Provence-Alpes-Côte d'Azur (StateId = 31)
         new CityData { CityId = 22, StateId = 31, CityName = "Marseille" },
         new CityData { CityId = 23, StateId = 31, CityName = "Nice" },
         new CityData { CityId = 24, StateId = 31, CityName = "Toulon" },

         // Cities in Normandy (StateId = 32)
         new CityData { CityId = 25, StateId = 32, CityName = "Rouen" },
         new CityData { CityId = 26, StateId = 32, CityName = "Caen" },
         new CityData { CityId = 27, StateId = 32, CityName = "Le Havre" },
     };

                var jsonres = JsonConvert.SerializeObject(cityList);
                return Ok(jsonres);
            }
        }

        #region CMT


        //[HttpPost]
        //public IActionResult FillPdfForm([FromBody] PdfFormModel formData)
        //{
        //    try
        //    {
        //        // Define file paths
        //        string inputFilePath = @"C:\Users\sushanta.senapati\Desktop\Waterlooo.pdf";
        //        string outputFilePath = @"C:\Users\sushanta.senapati\Desktop\FilledForm.pdf";

        //        // Load the PDF document
        //        using (var pdfDocument = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Modify))
        //        {
        //            // Get the AcroForm
        //            var form = pdfDocument.AcroForm;

        //            // Fill out the form fields by name
        //            if (form.Fields["Signature"] is PdfTextField signatureField)
        //                signatureField.Text = formData.Signature;

        //            if (form.Fields["Officer Name"] is PdfTextField officerNameField)
        //                officerNameField.Text = formData.OfficerName;

        //            if (form.Fields["Owner Name 1"] is PdfTextField ownerNameField)
        //                ownerNameField.Text = formData.OwnerName1;

        //            if (form.Fields["Due Date"] is PdfTextField dueDateField)
        //                dueDateField.Text = formData.DueDate;

        //            if (form.Fields["Owner Address"] is PdfTextField ownerAddressField)
        //                ownerAddressField.Text = formData.OwnerAddress;



        //            if (form.Fields["Email"] is PdfTextField Email)
        //                Email.Text = formData.Email;

        //            if (form.Fields["Phone#"] is PdfTextField Phone)
        //                Phone.Text = formData.Phone;






        //            // Save the filled PDF document
        //            pdfDocument.Save(outputFilePath);
        //        }

        //        // Return the filled PDF as a byte array in the response
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(outputFilePath);
        //        return File(fileBytes, "application/pdf", "FilledForm.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = $"An error occurred: {ex.Message}" });
        //    }
        //}




        // Model to hold the form data
        #endregion

    }
}
