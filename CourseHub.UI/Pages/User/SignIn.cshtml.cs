using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.Core.Services.Domain.UserServices.TempModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace CourseHub.UI.Pages.User;

public class SignInModel : PageModel
{
    private readonly IUserApiService _userApiService;

    [BindProperty]
    public SignInDto Dto { get; set; }
    [BindProperty]
    public bool RememberMe { get; set; }



    public string SignInPath { get; set; }
    public string GoogleOAuthBasePath { get; set; }



    public SignInModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;

        string basePath = Configurer.GetApiClientOptions().ApiServerPath;
        SignInPath = basePath + "/api/auth/signin";
        GoogleOAuthBasePath = basePath + "/api/auth/google-oauth/";
    }



    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        // Defaults to UserName
        if (Dto.UserName is null)
            return Page();

        if (IsEmailAddress(Dto.UserName))
        {
            Dto.Email = Dto.UserName;
            Dto.UserName = null;
        }



        HttpResponseMessage response = await _userApiService.SignInAsync(Dto);
        if (!response.IsSuccessStatusCode)
        {
            string? responseMessage = await response.Content.ReadFromJsonAsync<string>();
            if (responseMessage is not null)
            {
                if (responseMessage.StartsWith("400") ||
                    responseMessage.StartsWith("401") ||
                    responseMessage.StartsWith("403"))
                    ModelState.AddModelError(string.Empty, responseMessage.Substring(5));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error logging in.");
            }
            return Page();
        }

        // set userData in the session
        // if "RememberMe" is true, set userId in the cookie

        string sResponse = await response.Content.ReadAsStringAsync();
        AuthModel authData = JsonSerializer.Deserialize<AuthModel>(sResponse, SerializeOptions.JsonOptions)!;
        HttpContext.SetClientData(JsonSerializer.Serialize(authData.User));

        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            foreach (var header in SetCookieHeaderValue.ParseList(cookies.ToList()))
                if (header.Name == "Bearer" || header.Name == "Refresh")
				{
                    HttpContext.Response.SetAuthCookie(header.Name.ToString(), header.Value.ToString());
				}
        }

        if (RememberMe)
            Response.AddRememberCookie(authData.User!.Id);
        return RedirectToPage(Global.PAGE_INDEX);
    }

    private bool IsEmailAddress(string valueAsString)
    {
        int index = valueAsString.IndexOf('@');

        return
            index > 0 &&
            index != valueAsString.Length - 1 &&
            index == valueAsString.LastIndexOf('@');
    }
}
