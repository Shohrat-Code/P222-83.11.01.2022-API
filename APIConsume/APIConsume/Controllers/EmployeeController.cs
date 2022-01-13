using APIConsume.Helpers;
using APIConsume.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIConsume.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpClientFactory _client;

        public EmployeeController(IHttpClientFactory client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            var client = _client.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, SD.APIPathEmployee);
            var response = await client.SendAsync(request);

            List<VmEmployee> employees = new List<VmEmployee>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var employeeString = await response.Content.ReadAsStringAsync();
                employees = JsonConvert.DeserializeObject<List<VmEmployee>>(employeeString);
            }

            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            var client = _client.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, SD.APIPathPosition);
            var response = await client.SendAsync(request);

            VmCreateEmployee model = new VmCreateEmployee();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var positionString = await response.Content.ReadAsStringAsync();
                model.Positions = JsonConvert.DeserializeObject<List<VmPosition>>(positionString);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VmCreateEmployee model)
        {
            if (ModelState.IsValid)
            {
                var client = _client.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, SD.APIPathEmployee);

                using (var ms = new MemoryStream())
                {
                    model.Employee.ImageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    model.Employee.ImageBase64 = Convert.ToBase64String(fileBytes);
                }

                request.Content = new StringContent(JsonConvert.SerializeObject(model.Employee), Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    ModelState.AddModelError("", "Yaradammadim!");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            VmCreateEmployee model = new VmCreateEmployee();

            var client = _client.CreateClient();

            //Get position
            var request = new HttpRequestMessage(HttpMethod.Get, SD.APIPathPosition);
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var positionString = await response.Content.ReadAsStringAsync();
                model.Positions = JsonConvert.DeserializeObject<List<VmPosition>>(positionString);
            }

            //Get employee
            var requestEmployee = new HttpRequestMessage(HttpMethod.Get, SD.APIPathEmployee + id);
            var responseEmployee = await client.SendAsync(requestEmployee);

            if (responseEmployee.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var positionStringEmployee = await responseEmployee.Content.ReadAsStringAsync();
                model.Employee = JsonConvert.DeserializeObject<VmEmployee>(positionStringEmployee);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VmCreateEmployee model)
        {
            var client = _client.CreateClient();

            if (ModelState.IsValid)
            {
                var requestPost = new HttpRequestMessage(HttpMethod.Patch, SD.APIPathEmployee);
                requestPost.Content = new StringContent(JsonConvert.SerializeObject(model.Employee), Encoding.UTF8, "application/json");
                var responsePost = await client.SendAsync(requestPost);

                if (responsePost.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    ModelState.AddModelError("", "Yaradammadim!");

                    //Get position
                    var requestPosition = new HttpRequestMessage(HttpMethod.Get, SD.APIPathPosition);
                    var responsePosition = await client.SendAsync(requestPosition);

                    if (responsePosition.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var positionString = await responsePosition.Content.ReadAsStringAsync();
                        model.Positions = JsonConvert.DeserializeObject<List<VmPosition>>(positionString);
                    }
                    return View(model);
                }
            }

            //Get position
            var request = new HttpRequestMessage(HttpMethod.Get, SD.APIPathPosition);
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var positionString = await response.Content.ReadAsStringAsync();
                model.Positions = JsonConvert.DeserializeObject<List<VmPosition>>(positionString);
            }
            ModelState.AddModelError("", "Model valid doyul");
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {

            var client = _client.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Delete, SD.APIPathEmployee + id);
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("index");
            }

            TempData["error"] = "Cannot delete";
            return RedirectToAction("index");
        }
    }
}
