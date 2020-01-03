﻿using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Factories
{
    public interface IProductTypeFactory : IFactory<ProductTypeDto, ProductType>
    {
        
    }
}