﻿using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Response;
using DataTransferObjects.Models.Food.Request;

namespace Services.Interfaces
{
    public interface IFoodService : IBaseService
    {
        Task<ICollection<GetFoodResponse>>  GetAllAsync(string? userRole, FoodFilterRequest filterRequest);
        Task<IPaginable<GetFoodResponse>> GetPageAsync(string? userRole, FoodFilterRequest filterRequest, PaginationRequest request);
        Task<GetFoodResponse> GetFoodResponseByIdAsync(Guid id);
        Task<Food> GetByIdAsync(Guid id);
        Task CreateFoodAsync(CreateFoodRequest request, User user);

        Task UpdateFoodAsync(Guid foodId, UpdateFoodRequest request, User user);

        Task DeleteAsync(Guid guid, User user);
        Task<ICollection<GetBestSellerFoodsResponse>> GetBestSellerFoodsAsync(GetBestSellerFoodsRequest request, User manager);
    }
}
