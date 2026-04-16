global using System.Net;
global using System.Reflection;
global using System.Text;

global using Ardalis.GuardClauses;
global using AutoMapper;
global using AutoMapper.QueryableExtensions;
global using FluentValidation;
global using MediatR;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

global using Newtonsoft.Json;

global using KeyRails.BankingApi.Domain.Common;
global using KeyRails.BankingApi.Application.Common.Behaviours;
global using KeyRails.BankingApi.Application.Common.Interfaces;
global using KeyRails.BankingApi.Application.ExternalServices;

global using KeyRails.BankingApi.Domain.Entities;
global using KeyRails.BankingApi.Domain.Events;
