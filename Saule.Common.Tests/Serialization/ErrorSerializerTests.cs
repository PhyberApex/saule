﻿using System;
using Saule.Serialization;
using Xunit;

namespace Saule.Common.Tests.Serialization
{
    public class ErrorSerializerTests
    {
        [Fact(DisplayName = "Serializes common Exception properties")]
        public void SerializesProperties()
        {
            var exception = new InvalidOperationException("Some message") { HelpLink = "http://example.com" };
            var errors = new ErrorSerializer().Serialize(new[] { new ApiError(exception) })["errors"][0];

            Assert.Equal(exception.Message, errors.Value<string>("title"));
            Assert.Equal(exception.HelpLink, errors["links"].Value<string>("about"));
            Assert.Equal(exception.GetType().FullName, errors.Value<string>("code"));
            Assert.Equal(exception.ToString(), errors.Value<string>("detail"));
        }

        [Fact(DisplayName = "Does not serialize links when HelpLink is null")]
        public void DoesNotPutNullInALink()
        {
            var exception = new JsonApiException(ErrorType.Server, "Some message");
            var errors = new ErrorSerializer().Serialize(new [] { new ApiError(exception) })["errors"][0];

            Assert.Null(errors["links"]);
        }
    }
}