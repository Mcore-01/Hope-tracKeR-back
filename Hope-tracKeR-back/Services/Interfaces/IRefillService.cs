using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IRefillService
{
    Task<Result<int>> StartRefill(StartRefillRequest refillRequest);
    Task<Result> CompleteRefill(CompleteRefillRequest refillRequest);
    Task<Result<byte[]>> GenerateRefillToDocx(int itemId);
}