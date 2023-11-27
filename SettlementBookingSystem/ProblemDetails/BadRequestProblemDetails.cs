using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace SettlementBookingSystem.ProblemDetails
{
    public class BadRequestProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public BadRequestProblemDetails(ValidationException ex)
        {
            Status = StatusCodes.Status400BadRequest;
            Title = "Bad Request";
            Detail = ex.Errors.Count() != 0 ? string.Join(";", ex.Errors) : ex.Message;
            Type = "https://httpstatuses.com/400";
        }
    }
}
