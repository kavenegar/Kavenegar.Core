﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kavenegar.Core.Dto.Result;
using Kavenegar.Core.Enums;
using Moq;
using NUnit.Framework;
using Shared.Infrastructure;
using MessageSender = Kavenegar.Core.KavenegarMessageSender;

namespace Test.Kavenegar.Core.KavenegarMessageSender.VerifyLookup;

[TestFixture]
public class VerifyLookupWithParam
{
    [SetUp]
    public void SetUp()
    {
        _mockHttpClientHelper = new Mock<IHttpClientHelper>();
        _kavenegarMessageSender = new MessageSender(_mockHttpClientHelper.Object, "");
    }

    private MessageSender _kavenegarMessageSender = null!;
    private Mock<IHttpClientHelper> _mockHttpClientHelper = null!;

    [Test]
    public async Task VerifyLookup_WhenCalled_CallsPostAsync()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "");

        _mockHttpClientHelper.Verify(
            i => i.PostAsync(
                "verify/lookup.json",
                null,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task VerifyLookup_WhenCalled_CheckReceptorQueryParam()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "receptor",
            "",
            "");

        Assert.That(passedQueryParams["receptor"], Is.EqualTo("receptor"));
    }

    [Test]
    public async Task VerifyLookup_WhenCalled_CheckTemplateQueryParam()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "template",
            "");

        Assert.That(passedQueryParams["template"], Is.EqualTo("template"));
    }

    [Test]
    public async Task VerifyLookup_WhenCalled_CheckFirstTokenQueryParam()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "token");

        Assert.That(passedQueryParams["token"], Is.EqualTo("token"));
    }

    [Test]
    public async Task VerifyLookup_WithNoSender_QueryParamsNotIncludeToken2()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "");

        Assert.That(passedQueryParams.ContainsKey("token2"), Is.False);
    }

    [Test]
    public async Task VerifyLookup_WithSender_QueryParamsIncludeToken2()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "",
            "token2");

        Assert.That(passedQueryParams["token2"], Is.EqualTo("token2"));
    }

    [Test]
    public async Task VerifyLookup_WithNoSender_QueryParamsNotIncludeToken3()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "");

        Assert.That(passedQueryParams.ContainsKey("token3"), Is.False);
    }

    [Test]
    public async Task VerifyLookup_WithSender_QueryParamsIncludeToken3()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "",
            token3: "token3");

        Assert.That(passedQueryParams["token3"], Is.EqualTo("token3"));
    }

    [Test]
    public async Task VerifyLookup_WithNoSender_QueryParamsNotIncludeToken10()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "");

        Assert.That(passedQueryParams.ContainsKey("token10"), Is.False);
    }

    [Test]
    public async Task VerifyLookup_WithSender_QueryParamsIncludeToken10()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "",
            token4: "token10");

        Assert.That(passedQueryParams["token10"], Is.EqualTo("token10"));
    }

    [Test]
    public async Task VerifyLookup_WithNoSender_QueryParamsNotIncludeToken20()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "");

        Assert.That(passedQueryParams.ContainsKey("token20"), Is.False);
    }

    [Test]
    public async Task VerifyLookup_WithSender_QueryParamsIncludeToken20()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "",
            token5: "token20");

        Assert.That(passedQueryParams["token20"], Is.EqualTo("token20"));
    }

    [Test]
    public async Task VerifyLookup_WithNoSender_QueryParamsNotIncludeType()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "");

        Assert.That(passedQueryParams.ContainsKey("type"), Is.False);
    }

    [Test]
    public async Task VerifyLookup_WithSender_QueryParamsIncludeType()
    {
        Dictionary<string, object?> passedQueryParams = null!;

        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, object?, Dictionary<string, object?>, CancellationToken>(
                (
                    _,
                    _,
                    queryParams,
                    _) =>
                {
                    passedQueryParams = queryParams;
                })
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{}")
                });

        await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "",
            type: VerifyLookupType.Call);

        Assert.That(passedQueryParams["type"], Is.EqualTo((int)VerifyLookupType.Call));
    }

    [Test]
    public async Task VerifyLookup_WhenCalled_ReturnsSendResult()
    {
        _mockHttpClientHelper.Setup(
                i => i.PostAsync(
                    "verify/lookup.json",
                    null,
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent("{\"entries\":[{}]}")
                });

        var result = await _kavenegarMessageSender.VerifyLookup(
            "",
            "",
            "");

        Assert.That(result, Is.TypeOf<SendResultDto>());
    }
}