// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using ProtectedNumbers.Protection;
using static UnitTestProtectionAlgorithm;

public static class SubstituteExtensions
{
    public static void SetupAllServiceThrows(this IServiceProvider serviceProvider)
    {
        serviceProvider
            .GetService(Arg.Any<Type>())
            .Throws(ci =>
            {
                Type? type = ci.ArgAt<Type>(0);
                Exception exception =
                    new InvalidOperationException(
                        $"Service provider was not setup to provider service of type {type.FullName}");

                return exception;
            })
            ;
    }

    public static void SetupForApplicationDataProtector(this IApplicationDataProtector applicationDataProtector)
    {
        // Not Initialized
        applicationDataProtector
            .Protect(Arg.Is<ProtectedNumber>(i => !i.IsInitialized()))
            .Throws(new ArgumentException("can't protect an uninitialized value"))
            ;
        applicationDataProtector
            .Unprotect(Arg.Is<ProtectedNumber>(i => !i.IsInitialized()))
            .Throws(new ArgumentException("can't unprotect an uninitialized value"))
            ;
        applicationDataProtector
          .TryUnprotect(Arg.Is<ProtectedNumber>(i => !i.IsInitialized()), out Arg.Any<ProtectedNumber?>())
          .Returns(ci =>
          {
            ci[1] = (ProtectedNumber?)null;
            return false;
          })
          ;
        // No value
        applicationDataProtector
            .Protect(Arg.Is<ProtectedNumber>(i => !i.HasValue))
            .Throws(new ArgumentException("can't protect without a value"))
            ;
        // No protected value
        applicationDataProtector
            .Unprotect(Arg.Is<ProtectedNumber>(i => !i.HasProtectedValue))
            .Throws(new ArgumentException("can't unprotect without a protected value"))
            ;
        applicationDataProtector
          .TryUnprotect(Arg.Is<ProtectedNumber>(i => !i.HasProtectedValue), out Arg.Any<ProtectedNumber?>())
          .Returns(ci =>
          {
            ci[1] = (ProtectedNumber?)null;
            return false;
          })
          ;

        // Value
        applicationDataProtector
            .Protect(Arg.Is<ProtectedNumber>(i => i.IsInitialized() && i.HasValue))
            .Returns(ci =>
            {
                ProtectedNumber input = ci.ArgAt<ProtectedNumber>(0);
                long value = input.Value;
                string protectedValue = value.ProtectUsingUnitTestAlgorithm();
                ProtectedNumber output = ProtectedNumber.From(value, protectedValue);

                return output;
            })
            ;
        // Protected Value
        applicationDataProtector
            .Unprotect(Arg.Is<ProtectedNumber>(i => i.IsInitialized() && i.HasProtectedValue))
            .Returns(ci =>
            {
                ProtectedNumber input = ci.ArgAt<ProtectedNumber>(0);
                string protectedValue = input.ProtectedValue;

                if (!TryUnprotectUsingUnitTestAlgorithm(protectedValue, out long value))
                {
                    return input;
                }

                ProtectedNumber output = ProtectedNumber.From(value, protectedValue);

                return output;
            })
            ;
        applicationDataProtector
          .TryUnprotect(Arg.Is<ProtectedNumber>(i => i.IsInitialized() && i.HasProtectedValue), out Arg.Any<ProtectedNumber?>())
          .Returns(ci =>
          {
            ProtectedNumber input = ci.ArgAt<ProtectedNumber>(0);
            string protectedValue = input.ProtectedValue;

            if (!TryUnprotectUsingUnitTestAlgorithm(protectedValue, out long value))
            {
              ci[1] = (ProtectedNumber?)null;
              return false;
            }

            ci[1] = ProtectedNumber.From(value, protectedValue);

            return true;
          })
          ;
    }

    public static void SetupForService<T>(this IServiceProvider serviceProvider, T? service)
    {
        serviceProvider
            .GetService(Arg.Is(typeof(T)))
            .Returns(service)
            ;
    }

    public static void SetupHttpContextAsDefault(this IHttpContextAccessor substituteForHttpContextAccessor)
    {
        substituteForHttpContextAccessor.HttpContext.Returns(new DefaultHttpContext());
    }

    public static void SetupHttpContextAsNull(this IHttpContextAccessor substituteForHttpContextAccessor)
    {
        substituteForHttpContextAccessor.HttpContext.ReturnsNull();
    }
}
