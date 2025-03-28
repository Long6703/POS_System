using Microsoft.AspNetCore.Components;
using POS.Shared.RequestModel;
using POS.Shared.ResponseModel;
using POS.Web.Exceptions;
using POS.Web.Services;

namespace POS.Web.Pages.Auth
{
    public partial class Login
    {
        private LoginRequest _loginRequest = new();
        private bool _isSubmitting = false;
        private string? _errorMessage;

        private async Task HandleLogin()
        {
            _isSubmitting = true;
            _errorMessage = null;
            try
            {
                var response = await ApiService.PostAsync<LoginRequest, LoginResponse>("api/auth/login", _loginRequest);
                if (response != null && !string.IsNullOrEmpty(response.Token))
                {

                    if (AuthStateProvider is CustomAuthStateProvider customAuth)
                    {
                        customAuth.SetUser(_loginRequest.Email, response.Token);
                    }

                    Navigation.NavigateTo("/shops");
                }
                else
                {
                    _errorMessage = "Invalid login credentials.";
                }
            }
            catch (ApiException ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}