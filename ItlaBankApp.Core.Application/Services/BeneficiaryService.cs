
using AutoMapper;
using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Helpers;
using ItlaBankApp.Core.Application.Interfaces.Repositories;
using ItlaBankApp.Core.Application.Interfaces.Services;
using ItlaBankApp.Core.Application.Services.Generic;
using ItlaBankApp.Core.Application.ViewModels.Beneficiary;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ItlaBankApp.Core.Application.Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly AuthenticationResponse userViewModel;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IMapper mapper, 
                                IHttpContextAccessor httpContextAccessor, IProductService productService, IUserService userService) : base(beneficiaryRepository, mapper)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _productService = productService;
            _userService = userService;
        }

        public async Task PopulateBeneficiaryData(BeneficiaryViewModel viewModel)
        {
            var userData = await _userService.GetUserById(viewModel.BeneficiaryId);
            viewModel.Beneficiary = userData;
        }


        public async Task<ProductViewModel> GetProductByAccount(int accountNumber)
        {
            return await _productService.GetByAccount(accountNumber);
        }

        public async Task<List<BeneficiaryViewModel>> GetAllCurrentUserBeneficiaries()
        {
            var beneficiaries = await _beneficiaryRepository.GetAllAsync();
            var currentUserBeneficiaries = beneficiaries.Where(b => b.UserId == userViewModel.Id).ToList();

            return _mapper.Map<List<BeneficiaryViewModel>>(currentUserBeneficiaries);
        }
    }
}
