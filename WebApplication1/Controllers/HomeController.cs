using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WebApplication1.Models;
using static PDFiddler.Controllers.PDFIddlerController;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DownloadPDF()
        {
            try
            {

                //string apiUrl = "http://localhost:5144/api/PDFIddler/FillPdfForm"; // LOcal

                string apiUrl = "http://172.27.28.106:1234/api/PDFIddler/FillPdfForm"; // IIS API URL
                              
                //http://172.27.28.106:1234/api/PDFIddler/Get_City
               
                var formData = new PdfFormModel
                {
                    Signature = "S.K. Senapati",
                    OfficerName = "Smruti Ranjan Nayak",
                    OwnerName1 = "Nanu Pany",
                    DueDate = "25/12/2026",
                    OwnerAddress = "OCAC Tower",
                    City = "BBSR",
                    Email = "SUshanta",
                    Phone = "8658193889"
                };


                var jsonData = JsonConvert.SerializeObject(formData);

                // Create HttpClient
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Add("Accept", "application/json");


                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");


                    var response = await client.PostAsync(apiUrl, content);

                    // Ensure successful status code
                    if (response.IsSuccessStatusCode)
                    {

                        var fileBytes = await response.Content.ReadAsByteArrayAsync();


                        return File(fileBytes, "application/pdf");
                    }
                    else
                    {
                        // If the request fails, return a bad request response with the error
                        return BadRequest(new { message = $"An error occurred: {response.ReasonPhrase}" });
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                return BadRequest(new { message = $"An error occurred: {ex.Message}" });
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
