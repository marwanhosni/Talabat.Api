using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services.Interface;
using Stripe;

namespace Talabat.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentController : BaseApiController
{
    private readonly IPaymentServices _paymentServices;
    private readonly string _stripeWebhookSecret;
    public PaymentController(IPaymentServices paymentServices, IConfiguration configuration)
    {
        _paymentServices = paymentServices;
        _stripeWebhookSecret =
            configuration["Stripe:WebhookSecret"]
            ?? throw new InvalidOperationException(
                "Stripe webhook secret is missing from configuration.");
    }
    [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiRequest),StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent (string BasketId)
    {
        var basket = await _paymentServices.CreateOrUpdatePaymentIntent(BasketId);
        if (basket == null) return BadRequest(new ApiRequest (400, "Problem with your basket"));
        return Ok(basket);
    }
    //[HttpPost("WebHock")]
    //public async Task<IActionResult> StripeWebhock()
    //{
    //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

    //    try
    //    {
    //        var stripeEvent = EventUtility.ConstructEvent(
    //            json,
    //            Request.Headers["Stripe-Signature"],
    //            endpointSecret);

    //        // Handle the event
    //        if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
    //        {
    //            _paymentServices.UpdateOrderPaymentIntentToSucceededOrFailed
    //        }
    //        else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
    //        {

    //        }
    //        // ... handle other event types
    //        else
    //        {
    //            Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
    //        }

    //        return Ok();
    //    }
    //    catch (StripeException e)
    //    {
    //        return BadRequest();
    //    }
    //}
    // Stripe must be able to call this endpoint without an authenticated user.
    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        using var reader = new StreamReader(HttpContext.Request.Body);
        var json = await reader.ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _stripeWebhookSecret);

            if (stripeEvent.Data.Object is not PaymentIntent paymentIntent)
                return Ok();

            switch (stripeEvent.Type)
            {
                case "payment_intent.payment_failed":
                    await _paymentServices
                        .UpdateOrderPaymentIntentToSucceededOrFailed(
                            paymentIntent.Id,
                            false);
                    break;

                case "payment_intent.succeeded":
                    await _paymentServices
                        .UpdateOrderPaymentIntentToSucceededOrFailed(
                            paymentIntent.Id,
                            true);
                    break;
            }

            return Ok();
        }
        catch (StripeException)
        {
            return BadRequest();
        }
    }

}
