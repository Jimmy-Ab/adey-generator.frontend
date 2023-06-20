using System;
using PrivacyPolicyGeneratorFrontend.SharedKernel.Wrapper;
using PrivacyPolicyGeneratorFrontend.WebService.Services;

namespace PrivacyPolicyGeneratorFrontend.WebService.Interfaces
{
	public interface ITelebirrPaymentService
	{
            Task<Result<TelebirrResponseDto>> MakeTelebirrPayment(TelebirrPaymentCommand command);
    }
}

