using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    internal class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository repository, IMapper iMapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
        }

        public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _repository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<OrdersVm>>(orderList);
        }
    }
}