using AutoMapper;
using AutoMapper.Features;
using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.ViewModels.Beneficiary;
using ItlaBankApp.Core.Application.ViewModels.Payment;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Application.ViewModels.User;
using ItlaBankApp.Core.Domain.Entities;

namespace ItlaBankApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region User Profile
            CreateMap<UserDTO, UserViewModel>()
                .ReverseMap();

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
                .ReverseMap();

            CreateMap<SaveUserViewModel, RegisterRequest>()
                .ReverseMap();
            #endregion

            #region "Product Profile"
            CreateMap<Product, ProductViewModel>()
                .ReverseMap();
            CreateMap<Product, SaveProductViewModel>()
               .ReverseMap();
            #endregion

            #region "Payment Profile"
            CreateMap<Payment, PaymentViewModel>()
                .ReverseMap();

            CreateMap<Payment, SavePaymentViewModel>()
                .ForMember(dest => dest.ErrorMessage, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.DestinationId, opt => opt.MapFrom(src => src.DestinationId.ToString()));
            #endregion

            #region "Beneficiary Profile"
            CreateMap<Beneficiary, SaveBeneficiaryViewModel>()
                .ReverseMap();


            CreateMap<Beneficiary, BeneficiaryViewModel>()
                .ReverseMap();
            #endregion



            #region password



            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
               .ForMember(x => x.HasError, opt => opt.Ignore())
               .ForMember(x => x.Error, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            #endregion
        }
    }
}
