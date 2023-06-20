using System;
using Blazored.LocalStorage;
using PrivacyPolicyGeneratorFrontend.SharedKernel.Wrapper;
using PrivacyPolicyGeneratorFrontend.WebService.Services.Base;
using PrivacyPolicyGeneratorFrontend.WebService.Interfaces;

namespace PrivacyPolicyGeneratorFrontend.WebService.Services
{
    public class TelebirrPaymentService : BaseDataService, ITelebirrPaymentService
    {
        public TelebirrPaymentService(IClient client, ILocalStorageService localStorage) : base(client, localStorage)
        {

        }

        public async Task<Result<TelebirrResponseDto>> MakeTelebirrPayment(TelebirrPaymentCommand command)
        {
            try
            {
                var response = await _client.MakeTelebirrPaymentAsync(version, command);
                if (response.Succeeded == true)
                {
                    return await Result<TelebirrResponseDto>.SuccessAsync(response.Data, response.Messages.ToList());
                }
                else
                {

                    if (response.ValidationErrors.Count > 0)
                    {
                        var validationErrors = new List<string>();
                        foreach (var error in response.ValidationErrors)
                        {
                            validationErrors.Add(error);
                        }
                        return await Result<TelebirrResponseDto>.ValidationFailAsync(validationErrors, response.Messages.ToList().FirstOrDefault());
                    }
                    else
                    {
                        return await Result<TelebirrResponseDto>.FailAsync(response.Messages.ToList().FirstOrDefault());

                    }
                }
            }
            catch (ApiException ex)
            {
                return await Result<TelebirrResponseDto>.FailAsync(ex.Response);
            }
        }
    }
}

