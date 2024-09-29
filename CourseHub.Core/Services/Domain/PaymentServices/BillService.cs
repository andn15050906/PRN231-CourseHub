using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Payment;
using CourseHub.Core.RequestDtos.Payment.BillDtos;
using CourseHub.Core.Services.Domain.PaymentServices.Contracts;
using CourseHub.Core.Services.Domain.PaymentServices.TempModels;
using System.Linq.Expressions;

namespace CourseHub.Core.Services.Domain.PaymentServices;

public class BillService : DomainService, IBillService
{
    public BillService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }

    public async Task<ServiceResult<PagedResult<BillModel>>> Get(QueryBillDto dto)
    {
        var query = _uow.BillRepo.GetPagingQuery(GetPredicate(dto), dto.PageIndex, dto.PageSize);

        PagedResult<BillModel> result = await query.ExecuteWithOrderBy(_ => _.CreationTime, ascending: false);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<Guid>> Create(Guid id, CreateBillDto dto, PaymentResponse response, Guid clientId)
    {
        var entity = Adapt(id, dto, response, clientId);
        try
        {
            await _uow.BillRepo.Insert(entity);
            return Created(entity.Id);
        }
        catch
        {
            return ServerError<Guid>();
        }
    }






    private Expression<Func<Bill, bool>>? GetPredicate(QueryBillDto dto)
    {
        return null;
    }

    private Bill Adapt(Guid id, CreateBillDto dto, PaymentResponse paymentResponse, Guid clientId)
    {
        return new Bill(
            id, dto.Action, dto.Note, paymentResponse.Amount, dto.Gateway, paymentResponse.TransactionId,
            paymentResponse.ClientTransactionId, paymentResponse.Token, paymentResponse.IsSuccessful, clientId);
    }
}
