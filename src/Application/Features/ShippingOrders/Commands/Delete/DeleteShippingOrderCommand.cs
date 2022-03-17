// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ShippingOrders.Caching;
using CleanArchitecture.Blazor.Application.Features.ShippingOrders.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.ShippingOrders.Commands.Delete;

public class DeleteShippingOrderCommand : IRequest<Result>, ICacheInvalidator
{
    public int[] Id { get; }
    public string CacheKey => ShippingOrderCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => ShippingOrderCacheKey.SharedExpiryTokenSource;
    public DeleteShippingOrderCommand(int[] id)
    {
        Id = id;
    }
}


public class DeleteShippingOrderCommandHandler :
             IRequestHandler<DeleteShippingOrderCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DeleteShippingOrderCommandHandler> _localizer;
    public DeleteShippingOrderCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<DeleteShippingOrderCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteShippingOrderCommand request, CancellationToken cancellationToken)
    {

        var items = await _context.ShippingOrders.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            _context.ShippingOrders.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }


}

