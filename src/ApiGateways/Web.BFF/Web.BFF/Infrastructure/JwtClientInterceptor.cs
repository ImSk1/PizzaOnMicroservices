using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Authentication;

namespace Web.BFF.Infrastructure
{
    public class JwtClientInterceptor : Interceptor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public JwtClientInterceptor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request, 
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {

            var headers = new Metadata
            {
                { "Authorization", $"Bearer {GetToken()}" }
            };

            var updatedContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method,
                context.Host,
                context.Options.WithHeaders(headers));

            return continuation(request, updatedContext);
        }
        string GetToken()
        {
            const string ACCESS_TOKEN = "access_token";
            var token = _contextAccessor.HttpContext
                .GetTokenAsync(ACCESS_TOKEN).Result;

            return token;
        }
    }
}
