using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using Instagram.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Instagram.Controllers
{
    public class ProfileController : Controller
    {
        private readonly string url;
        public ProfileController(IConfiguration configuration)
        {
            url = configuration["ApiSettings:url"];
        }
        public ActionResult Index()
        {
            return View();
        }
        // GET: ProfileController
        public async Task<ActionResult> Profile(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");
                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var profileTask = httpClient.GetAsync(url + "/user/userProfile");
                var postsTask = httpClient.GetAsync(url + "/posts/getOwnPost");

                await Task.WhenAll(profileTask, postsTask);

                var viewModel = new UserProfilePageViewModel();

                if (profileTask.Result.IsSuccessStatusCode)
                {
                    string apiResponse1 = await profileTask.Result.Content.ReadAsStringAsync();
                    viewModel.UserProfile = JsonConvert.DeserializeObject<Profile>(apiResponse1);
                }

                if (postsTask.Result.IsSuccessStatusCode)
                {
                    string apiResponse2 = await postsTask.Result.Content.ReadAsStringAsync();
                    viewModel.UserPosts = JsonConvert.DeserializeObject<FeedViewModel>(apiResponse2);
                }
                if (id != 0)
                {
                    ViewBag.postId = id;
                    return View("MyPosts",viewModel);
                }
                else
                {
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Response = ex.Message;
                return View();
            }
        }

        public async Task<ActionResult> LoadPosts()
        {
            var token = HttpContext.Session.GetString("Token");
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            
                var postsResponse = await httpClient.GetAsync(url + "/posts/getOwnPost");
                if (postsResponse.IsSuccessStatusCode)
                {
                    string apiResponse = await postsResponse.Content.ReadAsStringAsync();
                    var post = JsonConvert.DeserializeObject<FeedViewModel>(apiResponse);
                    return PartialView("_PostPartial", post);
                }
            return PartialView("_PostPartial", new FeedViewModel());
        }

        public async Task<ActionResult> LoadReels()
        {
            var token = HttpContext.Session.GetString("Token");
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var reelsResponse = await httpClient.GetAsync(url + "/posts/getOwnReel"); // <-- your reels API
            if (reelsResponse.IsSuccessStatusCode)
            {
                string apiResponse = await reelsResponse.Content.ReadAsStringAsync();
                var reel = JsonConvert.DeserializeObject<FeedViewModel>(apiResponse);
                return PartialView("_ReelPartial", reel);
            }

            return PartialView("_ReelPartial", new UserProfilePageViewModel());
        }



        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(IFormFile profileImg)
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");
                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login");
                if (profileImg == null || profileImg.Length == 0)
                    return RedirectToAction(nameof(Profile)); 

                using var content = new MultipartFormDataContent();
                using var stream = profileImg.OpenReadStream();
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(profileImg.ContentType);

                content.Add(fileContent, "profile", profileImg.FileName);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PutAsync(url + "/user/editProfile",content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var userProfile = JsonConvert.DeserializeObject<Profile>(apiResponse);
                    if (userProfile.success)
                    {
                        return RedirectToAction(nameof(Profile));
                    }
                }
                return RedirectToAction(nameof(Profile));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfileController/Edit/5
        public async Task<ActionResult> UpdateUserDetails()
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");
                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(url + "/user/userProfile"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var userProfile = JsonConvert.DeserializeObject<Profile>(apiResponse);
                    if (userProfile.success)
                    {
                        return View(userProfile);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Response = ex.Message;
                return View();
            }
        }

        // POST: ProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserDetails(int id, Profile p)
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");
                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(p.data), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync(url + "/user/updateUserDetails", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Response r = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (r.success)
                            return RedirectToAction(nameof(Profile));
                        else
                        {
                            ViewBag.error = r.msg;
                        }
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
