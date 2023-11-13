using Domain.Dtos.User.Register;
using LanguageExt.Common;

namespace Application.Interface
{
    public interface IRegisterService
    {
        Task<Result<string>> RegisterAsync(RegisterRequest request, string origin);
    }
}
