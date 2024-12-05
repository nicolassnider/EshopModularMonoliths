﻿global using Basket.Basket.Dtos;
global using Basket.Basket.Exceptions;
global using Basket.Basket.Features.GetBasket;
global using Basket.Basket.Models;
global using Basket.Data;
global using Basket.Data.Repository;
global using Carter;
global using Catalog.Contracts.Products.Features.GetProductById;
global using FluentValidation;
global using Mapster;
global using MassTransit;
global using MediatR;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Shared.Contracts.CQRS;
global using Shared.Data;
global using Shared.Data.Interceptors;
global using Shared.Data.Seed;
global using Shared.DDD;
global using Shared.Exceptions;
global using Shared.Messaging.Events;
global using System.Reflection;
global using System.Security.Claims;
