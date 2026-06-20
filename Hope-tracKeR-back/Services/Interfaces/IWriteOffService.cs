using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IWriteOffService
{
    Task<Result> WriteOff(WriteOffDeviceRequest writeOffDeviceRequest);
}