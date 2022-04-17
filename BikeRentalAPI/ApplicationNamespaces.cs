// .NET
global using System;
global using Microsoft.Extensions.Options;

// NUGET
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization.Attributes;
global using MongoDB.Driver;
global using FluentValidation;
global using FluentValidation.AspNetCore;

// LOCAL
global using BikeRentalAPI.Configuration;
global using BikeRentalAPI.Context;
global using BikeRentalAPI.Models;
global using BikeRentalAPI.Repositories;
global using BikeRentalAPI.Services;
global using BikeRentalAPI.Validation;
global using BikeRentalAPI.GraphQL.Queries;
global using BikeRentalAPI.GraphQL.Mutations;