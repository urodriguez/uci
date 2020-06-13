﻿using Application.Contracts;
using Application.Contracts.AggregateUpdaters;
using Application.Contracts.DuplicateValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;

namespace Application.Services
{
    public class InventionTypeService : CrudService<InventionTypeDto, InventionType>, IInventionTypeService
    {
        public InventionTypeService(
            IRoleService roleService,
            IInventionTypeFactory factory, 
            IInventionTypeUpdater updater, 
            IAuditService auditService,
            IInventionTypeDuplicateValidator inventionTypeDuplicateValidator,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            ILogService logService,
            IAppSettingsService appSettingsService,
            IInventAppContext inventAppContext
        ) : base(
            roleService,
            factory,
            updater,
            auditService, 
            inventionTypeDuplicateValidator,
            tokenService,
            unitOfWork,
            logService,
            appSettingsService,
            inventAppContext
        )
        {
        }
    }
}