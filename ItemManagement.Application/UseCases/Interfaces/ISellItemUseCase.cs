using AutoMapper;
using ItemManagement.Application.UseCaseInterfaces;
 
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Application.UseCaseInterfaces
{


    public interface ISellItemUseCase
    {
        Task ExecuteAsync(string cashierName, int itemId, int qtyToSell, CancellationToken cancellationToken);
    }
}