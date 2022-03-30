using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Warren.OrderService.Application.Models;

namespace Warren.OrderService.Application.Commands.QueryOrderByPagingCommand
{

    public class QueryOrderByPagingCommandValidator : AbstractValidator<QueryOrderByPagingCommand>
    {
        public QueryOrderByPagingCommandValidator()
        {
            RuleFor(command => command.data).NotNull();
            RuleFor(command => command.page_index).NotNull().Custom((page_index, context) =>
            {
                if (page_index <= 0)
                {
                    context.AddFailure("page_index", "当前页不能小于0");
                }
            });
            RuleFor(command => command.page_size).NotNull().Custom((page_size, context) =>
            {
                if (page_size <= 0)
                {
                    context.AddFailure("page_size", "单页数量不能小于0");
                }
            });
            RuleFor(command => command.data).NotNull().Custom((data, context) =>
            {
                if (data != null)
                {
                    if (data.order_numbers == null || !data.order_numbers.Any())
                    {
                        if (!data.begin_date.HasValue || !data.end_date.HasValue)
                        {
                            context.AddFailure("Date", "起始或结束时间不能为空");
                        }
                        else
                        {
                            if (DateTime.Compare(data.begin_date.Value, data.end_date.Value) > 0)
                            {
                                context.AddFailure("Date", "起始时间必须小于等于结束时间");
                            }
                        }
                    }
                }
            });
        }
    }



    public class QueryOrderByPagingCommand : PagingRequest<QueryOrderByPagingCommandRequest>,
    MediatR.IRequest<OkApiResult<List<QueryOrderByPagingCommandResponse>>>
    {
    }


    public class QueryOrderByPagingCommandRequest
    {
        public List<string> order_numbers { get; set; }
        public DateTime? begin_date { get; set; }
        public DateTime? end_date { get; set; }
    }

    public class QueryOrderByPagingCommandResponse
    {
        public long order_id { get; set; }
        public string shipper_number { get; set; }
        public DateTime forecast_date { get; set; }

        public static List<QueryOrderByPagingCommandResponse> MapFrom(List<Domain.IQueries.Order.Dtos.QueryOrderInfoDto> queryOrderInfoDtos)
        {
            return queryOrderInfoDtos.Select(o => new QueryOrderByPagingCommandResponse()
            {
                order_id = o.order_id,
                forecast_date = o.forecast_date,
                shipper_number = o.shipper_number

            }).ToList();
        }
    }
}
