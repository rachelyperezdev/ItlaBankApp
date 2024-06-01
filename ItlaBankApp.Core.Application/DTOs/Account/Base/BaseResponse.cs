namespace ItlaBankApp.Core.Application.DTOs.Account.Common
{
    public class BaseResponse
    {
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}