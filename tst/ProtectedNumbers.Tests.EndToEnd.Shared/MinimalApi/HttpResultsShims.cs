// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
#if NET6_0
#pragma warning disable CS9113 // Parameter is unread - we may only need to store some values for parity
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

// This namespace must match newer ASP.NET Core typed results
namespace Microsoft.AspNetCore.Http.HttpResults
{
    // --------- Primitive typed results ---------
    public readonly struct Ok<T> : IResult
    {
        private readonly T _value;
        public Ok(T value) => _value = value;
        public Task ExecuteAsync(HttpContext httpContext) => Results.Ok(_value).ExecuteAsync(httpContext);
    }

    public readonly struct NotFound : IResult
    {
        public Task ExecuteAsync(HttpContext httpContext) => Results.NotFound().ExecuteAsync(httpContext);
    }

    public readonly struct NoContent : IResult
    {
        public Task ExecuteAsync(HttpContext httpContext) => Results.NoContent().ExecuteAsync(httpContext);
    }

    public readonly struct BadRequest : IResult
    {
      public BadRequest(){}
      public Task ExecuteAsync(HttpContext httpContext)
        => Results.BadRequest().ExecuteAsync(httpContext);
    }

    public readonly struct BadRequest<T> : IResult
    {
        private readonly T? _error;
        public BadRequest(T? error = default) => _error = error;
        public Task ExecuteAsync(HttpContext httpContext)
            => (_error is null ? Results.BadRequest() : Results.BadRequest(_error)).ExecuteAsync(httpContext);
    }

    public readonly struct Created<T> : IResult
    {
        private readonly string _uri;
        private readonly T _value;
        public Created(string uri, T value) { _uri = uri; _value = value; }
        public Task ExecuteAsync(HttpContext httpContext)
            => Results.Created(_uri, _value).ExecuteAsync(httpContext);
    }

    public readonly struct CreatedAtRoute<T> : IResult
    {
        private readonly string? _routeName;
        private readonly object? _routeValues;
        private readonly T _value;
        public CreatedAtRoute(string? routeName, object? routeValues, T value)
        { _routeName = routeName; _routeValues = routeValues; _value = value; }
        public Task ExecuteAsync(HttpContext httpContext)
            => Results.CreatedAtRoute(_routeName, _routeValues, _value).ExecuteAsync(httpContext);
    }

    public readonly struct ValidationProblem : IResult
    {
        private readonly IDictionary<string, string[]> _errors;
        private readonly int? _statusCode;
        private readonly string? _title;
        private readonly string? _detail;
        private readonly string? _instance;
        private readonly string? _type;
        private readonly IDictionary<string, object?>? _extensions;

        public ValidationProblem(
            IDictionary<string, string[]> errors,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null,
            IDictionary<string, object?>? extensions = null)
        {
            _errors = errors;
            _statusCode = statusCode;
            _title = title; _detail = detail; _instance = instance; _type = type;
            _extensions = extensions;
        }

        public Task ExecuteAsync(HttpContext httpContext)
            => Results.ValidationProblem(_errors,
                                         statusCode: _statusCode,
                                         title: _title,
                                         type: _type,
                                         detail: _detail,
                                         instance: _instance,
                                         extensions: _extensions)
                       .ExecuteAsync(httpContext);
    }

    public readonly struct ProblemHttpResult : IResult
    {
        private readonly string? _detail;
        private readonly string? _instance;
        private readonly int? _statusCode;
        private readonly string? _title;
        private readonly string? _type;
        private readonly IDictionary<string, object?>? _extensions;

        public ProblemHttpResult(
            string? detail = null,
            string? instance = null,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            IDictionary<string, object?>? extensions = null)
        {
            _detail = detail; _instance = instance; _statusCode = statusCode; _title = title; _type = type; _extensions = extensions;
        }

        public Task ExecuteAsync(HttpContext httpContext)
            => Results.Problem(
                    detail: _detail,
                    instance: _instance,
                    statusCode: _statusCode,
                    title: _title,
                    type: _type,
                    extensions: _extensions)
               .ExecuteAsync(httpContext);
    }

    // --------- Union wrappers (Results<T1..T5>) ---------
    public readonly struct Results<T1, T2> : IResult
        where T1 : struct, IResult
        where T2 : struct, IResult
    {
        private readonly IResult _inner;
        private Results(IResult inner) => _inner = inner;
        public static implicit operator Results<T1, T2>(T1 value) => new(value);
        public static implicit operator Results<T1, T2>(T2 value) => new(value);
        public Task ExecuteAsync(HttpContext httpContext) => _inner.ExecuteAsync(httpContext);
    }

    public readonly struct Results<T1, T2, T3> : IResult
        where T1 : struct, IResult
        where T2 : struct, IResult
        where T3 : struct, IResult
    {
        private readonly IResult _inner;
        private Results(IResult inner) => _inner = inner;
        public static implicit operator Results<T1, T2, T3>(T1 value) => new(value);
        public static implicit operator Results<T1, T2, T3>(T2 value) => new(value);
        public static implicit operator Results<T1, T2, T3>(T3 value) => new(value);
        public Task ExecuteAsync(HttpContext httpContext) => _inner.ExecuteAsync(httpContext);
    }

    public readonly struct Results<T1, T2, T3, T4> : IResult
        where T1 : struct, IResult
        where T2 : struct, IResult
        where T3 : struct, IResult
        where T4 : struct, IResult
    {
        private readonly IResult _inner;
        private Results(IResult inner) => _inner = inner;
        public static implicit operator Results<T1, T2, T3, T4>(T1 value) => new(value);
        public static implicit operator Results<T1, T2, T3, T4>(T2 value) => new(value);
        public static implicit operator Results<T1, T2, T3, T4>(T3 value) => new(value);
        public static implicit operator Results<T1, T2, T3, T4>(T4 value) => new(value);
        public Task ExecuteAsync(HttpContext httpContext) => _inner.ExecuteAsync(httpContext);
    }

    public readonly struct Results<T1, T2, T3, T4, T5> : IResult
        where T1 : struct, IResult
        where T2 : struct, IResult
        where T3 : struct, IResult
        where T4 : struct, IResult
        where T5 : struct, IResult
    {
        private readonly IResult _inner;
        private Results(IResult inner) => _inner = inner;
        public static implicit operator Results<T1, T2, T3, T4, T5>(T1 value) => new(value);
        public static implicit operator Results<T1, T2, T3, T4, T5>(T2 value) => new(value);
        public static implicit operator Results<T1, T2, T3, T4, T5>(T3 value) => new(value);
        public static implicit operator Results<T1, T2, T3, T4, T5>(T4 value) => new(value);
        public static implicit operator Results<T1, T2, T3, T4, T5>(T5 value) => new(value);
        public Task ExecuteAsync(HttpContext httpContext) => _inner.ExecuteAsync(httpContext);
    }

    // --------- Factory methods (TypedResults) ---------
    public static class TypedResults
    {
        // 200
        public static Ok<T> Ok<T>(T value) => new(value);

        // 201
        public static Created<T> Created<T>(string uri, T value) => new(uri, value);
        public static CreatedAtRoute<T> CreatedAtRoute<T>(string? routeName, object? routeValues, T value)
            => new(routeName, routeValues, value);

        // 204
        public static NoContent NoContent() => new();

        // 400
        public static BadRequest BadRequest() => new();
        public static BadRequest<T> BadRequest<T>(T? error) => new(error);

        // 404
        public static NotFound NotFound() => new();

        // 422 / 400-like (depends on config)
        public static ValidationProblem ValidationProblem(
            IDictionary<string, string[]> errors,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null,
            IDictionary<string, object?>? extensions = null)
            => new(errors, statusCode, title, type, detail, instance, extensions);

        // RFC7807 Problem
        public static ProblemHttpResult Problem(
            string? detail = null,
            string? instance = null,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            IDictionary<string, object?>? extensions = null)
            => new(detail, instance, statusCode, title, type, extensions);
    }
}
#pragma warning restore CS9113
#endif
