using FluentValidation;
using System;
using Warren.OrderService.Application.Models;

namespace Warren.OrderService.Application.Commands.CreateOrderCommand
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(command => command.shipper_number).NotNull().WithMessage("缺少单号字段");
            RuleFor(command => command.shipper_number).NotEmpty().WithMessage("单号不能为空");
        }
    }


    public class CreateOrderCommand : CreateOrderCommandValidatorRequest, MediatR.IRequest<IApiResult>
    {

    }

    public class CreateOrderCommandValidatorRequest
    {
        public string shipper_number { get; set; }
        public DateTime forecast_date { get; set; } = DateTime.UtcNow;
    }
}
