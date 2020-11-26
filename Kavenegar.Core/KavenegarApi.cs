using Kavenegar.Core.Exceptions;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;
using Kavenegar.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Kavenegar
{
    internal class ReturnResult
    {
        public Result @Return { get; set; }

        public object Entries { get; set; }
    }

    internal class Result
    {
        public int Status { get; set; }

        public string Message { get; set; }
    }

    internal class ReturnSend
    {
        public Result @Return { get; set; }

        public List<SendResult> Entries { get; set; }
    }

    internal class ReturnStatus
    {
        public Result Result { get; set; }

        public List<StatusResult> Entries { get; set; }
    }

    internal class ReturnStatusLocalMessageId
    {
        public Result Result { get; set; }

        public List<StatusLocalMessageIdResult> Entries { get; set; }
    }

    internal class ReturnReceive
    {
        public Result Result { get; set; }

        public List<ReceiveResult> Entries { get; set; }
    }

    internal class ReturnCountOutbox
    {
        public Result Result { get; set; }

        public List<CountOutboxResult> Entries { get; set; }
    }

    internal class ReturnCountInbox
    {
        public Result Result { get; set; }

        public List<CountInboxResult> Entries { get; set; }
    }

    internal class ReturnCountPostalCode
    {
        public Result Result { get; set; }

        public List<CountPostalCodeResult> Entries { get; set; }
    }

    internal class ReturnAccountInfo
    {
        public Result Result { get; set; }

        public AccountInfoResult Entries { get; set; }
    }

    internal class ReturnAccountConfig
    {
        public Result Result { get; set; }

        public AccountConfigResult Entries { get; set; }
    }

    public class KavenegarApi
    {
        private static HttpClient _client;
        private const string Apipath = "{0}/{1}.{2}";
        private const string BaseUrl = "http://api.kavenegar.com/v1";

        public KavenegarApi(string apikey)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{BaseUrl}/{apikey}/")
            };
        }

        public async Task<List<SendResult>> Send(string sender, List<string> receptor, string message)
        {
            return await Send(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue);
        }

        public async Task<SendResult> Send(string sender, string receptor, string message)
        {
            return await Send(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue);
        }

        public async Task<SendResult> Send(string sender, string receptor, string message, MessageType type, DateTime date)
        {
            List<string> receptors = new List<string> { receptor };
            return (await Send(sender, receptors, message, type, date))[0];
        }

        public async Task<List<SendResult>> Send(string sender, List<string> receptor, string message, MessageType type, DateTime date)
        {
            return await Send(sender, receptor, message, type, date, null);
        }

        public async Task<SendResult> Send(string sender, string receptor, string message, MessageType type, DateTime date, string localId)
        {
            var receptors = new List<string> { receptor };
            var localids = new List<string> { localId };
            return (await Send(sender, receptors, message, type, date, localids))[0];
        }

        public async Task<SendResult> Send(string sender, string receptor, string message, string localId)
        {
            return await Send(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue, localId);
        }

        public async Task<List<SendResult>> Send(string sender, List<string> receptors, string message, string localId)
        {
            List<string> localids = new List<string>();
            for (var i = 0; i <= receptors.Count - 1; i++)
            {
                localids.Add(localId);
            }
            return await Send(sender, receptors, message, MessageType.MobileMemory, DateTime.MinValue, localids);
        }

        public async Task<List<SendResult>> Send(string sender, List<string> receptor, string message, MessageType type, DateTime date, List<string> localids)
        {
            var path = GetApiPath("sms", "send", "json");
            var param = new Dictionary<string, object>
        {
            {"sender", System.Net.WebUtility.HtmlEncode(sender)},
            {"receptor", System.Net.WebUtility.HtmlEncode(StringHelper.Join(",", receptor.ToArray()))},
            {"message", System.Net.WebUtility.HtmlEncode(message)},
            {"type", (int) type},
            {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
        };
            if (localids != null && localids.Count > 0)
            {
                param.Add("localid", StringHelper.Join(",", localids.ToArray()));
            }
            var responseBody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responseBody);
            return l.Entries;
        }

        public async Task<List<SendResult>> SendArray(List<string> senders, List<string> receptors, List<string> messages)
        {
            var types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            return await SendArray(senders, receptors, messages, types, DateTime.MinValue, null);
        }

        public async Task<List<SendResult>> SendArray(string sender, List<string> receptors, List<string> messages, MessageType type, DateTime date)
        {
            var senders = new List<string>();
            for (var i = 0; i < receptors.Count; i++)
            {
                senders.Add(sender);
            }
            var types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            return await SendArray(senders, receptors, messages, types, date, null);
        }

        public async Task<List<SendResult>> SendArray(string sender, List<string> receptors, List<string> messages, MessageType type, DateTime date, string localmessageids)
        {
            var senders = new List<string>();
            for (var i = 0; i < receptors.Count; i++)
            {
                senders.Add(sender);
            }
            List<MessageType> types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            return await SendArray(senders, receptors, messages, types, date, new List<string>() { localmessageids });
        }

        public async Task<List<SendResult>> SendArray(string sender, List<string> receptors, List<string> messages, string localMessageId)
        {
            List<string> senders = new List<string>();
            for (var i = 0; i < receptors.Count; i++)
            {
                senders.Add(sender);
            }

            return await SendArray(senders, receptors, messages, localMessageId);
        }

        public async Task<List<SendResult>> SendArray(List<string> senders, List<string> receptors, List<string> messages, string localMessageId)
        {
            var types = new List<MessageType>();
            for (var i = 0; i <= receptors.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            var localmessageids = new List<string>();
            for (var i = 0; i <= receptors.Count - 1; i++)
            {
                localmessageids.Add(localMessageId);
            }
            return await SendArray(senders, receptors, messages, types, DateTime.MinValue, localmessageids);
        }

        public async Task<List<SendResult>> SendArray(List<string> senders, List<string> receptors, List<string> messages, List<MessageType> types, DateTime date, List<string> localmessageids)
        {
            string path = GetApiPath("sms", "sendarray", "json");
            var jsonSenders = JsonConvert.SerializeObject(senders);
            var jsonReceptors = JsonConvert.SerializeObject(receptors);
            var jsonMessages = JsonConvert.SerializeObject(messages);
            var jsonTypes = JsonConvert.SerializeObject(types);
            var param = new Dictionary<string, object>
        {
            {"message", jsonMessages},
            {"sender", jsonSenders},
            {"receptor", jsonReceptors},
            {"type", jsonTypes},
            {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
        };
            if (localmessageids != null && localmessageids.Count > 0)
            {
                param.Add("localmessageids", StringHelper.Join(",", localmessageids.ToArray()));
            }

            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            if (l.Entries == null)
            {
                return new List<SendResult>();
            }
            return l.Entries;
        }

        public async Task<List<StatusResult>> Status(List<string> messageIds)
        {
            string path = GetApiPath("sms", "status", "json");
            var param = new Dictionary<string, object>
        {
            {"messageid", StringHelper.Join(",", messageIds.ToArray())}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnStatus>(responsebody);
            if (l.Entries == null)
            {
                return new List<StatusResult>();
            }
            return l.Entries;
        }

        public async Task<StatusResult> Status(string messageId)
        {
            var ids = new List<string> { messageId };
            var result = await Status(ids);
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(List<string> messageIds)
        {
            string path = GetApiPath("sms", "statuslocalmessageid", "json");
            var param = new Dictionary<string, object> { { "localid", StringHelper.Join(",", messageIds.ToArray()) } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnStatusLocalMessageId>(responsebody);
            return l.Entries;
        }

        public async Task<StatusLocalMessageIdResult> StatusLocalMessageId(string messageId)
        {
            List<StatusLocalMessageIdResult> result = await StatusLocalMessageId(new List<string>() { messageId });
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<SendResult>> Select(List<string> messageIds)
        {
            var path = GetApiPath("sms", "select", "json");
            var param = new Dictionary<string, object> { { "messageId", StringHelper.Join(",", messageIds.ToArray()) } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            if (l.Entries == null)
            {
                return new List<SendResult>();
            }
            return l.Entries;
        }

        public async Task<SendResult> Select(string messageId)
        {
            var ids = new List<string> { messageId };
            var result = await Select(ids);
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<SendResult>> SelectOutbox(DateTime startDate)
        {
            return await SelectOutbox(startDate, DateTime.MaxValue);
        }

        public async Task<List<SendResult>> SelectOutbox(DateTime startDate, DateTime endDate)
        {
            return await SelectOutbox(startDate, endDate, null);
        }

        public async Task<List<SendResult>> SelectOutbox(DateTime startDate, DateTime endDate, string sender)
        {
            string path = GetApiPath("sms", "selectoutbox", "json");
            var param = new Dictionary<string, object>
         {
             {"startdate", startDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startDate)},
             {"enddate", endDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(endDate)},
             {"sender", sender}
         };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            return l.Entries;
        }

        public async Task<List<SendResult>> LatestOutbox(long pageSize)
        {
            return await LatestOutbox(pageSize, "");
        }

        public async Task<List<SendResult>> LatestOutbox(long pageSize, string sender)
        {
            var path = GetApiPath("sms", "latestoutbox", "json");
            var param = new Dictionary<string, object> { { "pagesize", pageSize }, { "sender", sender } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            return l.Entries;
        }

        public async Task<CountOutboxResult> CountOutbox(DateTime startDate)
        {
            return await CountOutbox(startDate, DateTime.MaxValue, 10);
        }

        public async Task<CountOutboxResult> CountOutbox(DateTime startDate, DateTime endDate)
        {
            return await CountOutbox(startDate, endDate, 0);
        }

        public async Task<CountOutboxResult> CountOutbox(DateTime startDate, DateTime endDate, int status)
        {
            string path = GetApiPath("sms", "countoutbox", "json");
            var param = new Dictionary<string, object>
         {
             {"startdate", startDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startDate)},
             {"enddate", endDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(endDate)},
             {"status", status}
         };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnCountOutbox>(responsebody);
            if (l.Entries == null || l.Entries[0] == null)
            {
                return new CountOutboxResult();
            }
            return l.Entries[0];
        }

        public async Task<List<StatusResult>> Cancel(List<string> ids)
        {
            string path = GetApiPath("sms", "cancel", "json");
            var param = new Dictionary<string, object>
        {
            {"messageid", StringHelper.Join(",", ids.ToArray())}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnStatus>(responsebody);
            return l.Entries;
        }

        public async Task<StatusResult> Cancel(string messageId)
        {
            var ids = new List<string> { messageId };
            var result = await Cancel(ids);
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<ReceiveResult>> Receive(string line, int isRead)
        {
            string path = GetApiPath("sms", "receive", "json");
            var param = new Dictionary<string, object> { { "linenumber", line }, { "isread", isRead } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnReceive>(responsebody);
            if (l.Entries == null)
            {
                return new List<ReceiveResult>();
            }
            return l.Entries;
        }

        public async Task<CountInboxResult> CountInbox(DateTime startDate, string lineNumber)
        {
            return await CountInbox(startDate, DateTime.MaxValue, lineNumber, 0);
        }

        public async Task<CountInboxResult> CountInbox(DateTime startDate, DateTime endDate, string lineNumber)
        {
            return await CountInbox(startDate, endDate, lineNumber, 0);
        }

        public async Task<CountInboxResult> CountInbox(DateTime startDate, DateTime endDate, string lineNumber, int isRead)
        {
            var path = GetApiPath("sms", "countoutbox", "json");
            var param = new Dictionary<string, object>
        {
            {"startdate", startDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startDate)},
            {"enddate", endDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(endDate)},
            {"linenumber", lineNumber},
            {"isread", isRead}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnCountInbox>(responsebody);
            return l.Entries[0];
        }

        public async Task<List<CountPostalCodeResult>> CountPostalCode(long postalcode)
        {
            string path = GetApiPath("sms", "countpostalcode", "json");
            var param = new Dictionary<string, object> { { "postalcode", postalcode } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnCountPostalCode>(responsebody);
            return l.Entries;
        }

        public async Task<List<SendResult>> SendByPostalCode(long postalcode, string sender, string message, long mciStartIndex, long mciCount, long mtnstartindex, long mtncount)
        {
            return await SendByPostalCode(postalcode, sender, message, mciStartIndex, mciCount, mtnstartindex, mtncount, DateTime.MinValue);
        }

        public async Task<List<SendResult>> SendByPostalCode(long postalcode, string sender, string message, long mciStartIndex, long mciCount, long mtnstartindex, long mtncount, DateTime date)
        {
            var path = GetApiPath("sms", "sendbypostalcode", "json");
            var param = new Dictionary<string, object>
        {
            {"postalcode", postalcode},
            {"sender", sender},
            {"message", System.Net.WebUtility.HtmlEncode(message)},
            {"mcistartIndex", mciStartIndex},
            {"mcicount", mciCount},
            {"mtnstartindex", mtnstartindex},
            {"mtncount", mtncount},
            {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            return l.Entries;
        }

        public async Task<AccountInfoResult> AccountInfo()
        {
            var path = GetApiPath("account", "info", "json");
            var responsebody = await Execute(path, null);
            var l = JsonConvert.DeserializeObject<ReturnAccountInfo>(responsebody);
            return l.Entries;
        }

        public async Task<AccountConfigResult> AccountConfig(string apilogs, string dailyReport, string debugmode, string defaultsender, int? mincreditalarm, string resendfailed)
        {
            var path = GetApiPath("account", "config", "json");
            var param = new Dictionary<string, object>
        {
            {"apilogs", apilogs},
            {"dailyreport", dailyReport},
            {"debugmode", debugmode},
            {"defaultsender", defaultsender},
            {"mincreditalarm", mincreditalarm},
            {"resendfailed", resendfailed}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnAccountConfig>(responsebody);
            return l.Entries;
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string template)
        {
            return await VerifyLookup(receptor, token, null, null, template, VerifyLookupType.Sms);
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string template, VerifyLookupType type)
        {
            return await VerifyLookup(receptor, token, null, null, template, type);
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string template)
        {
            return await VerifyLookup(receptor, token, token2, token3, template, VerifyLookupType.Sms);
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string token10, string template)
        {
            return await VerifyLookup(receptor, token, token2, token3, token10, template, VerifyLookupType.Sms);
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string template, VerifyLookupType type)
        {
            return await VerifyLookup(receptor, token, token2, token3, null, template, type);
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string token10, string template, VerifyLookupType type)
        {
            return await VerifyLookup(receptor, token, token2, token3, token10, null, template, type);
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string token10, string token20, string template, VerifyLookupType type)
        {
            var path = GetApiPath("verify", "lookup", "json");
            var param = new Dictionary<string, object>
            {
                {"receptor", receptor},
                {"template", template},
                {"token", token},
                {"token2", token2},
                {"token3", token3},
                {"token10", token10},
                {"token20", token20},
                {"type", type},
            };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            return l.Entries[0];
        }

        public async Task<SendResult> CallMakeTts(string message, string receptor)
        {
            return (await CallMakeTts(message, new List<string> { receptor }, null, null))[0];
        }

        public async Task<List<SendResult>> CallMakeTts(string message, List<string> receptor)
        {
            return await CallMakeTts(message, receptor, null, null);
        }

        public async Task<List<SendResult>> CallMakeTts(string message, List<string> receptor, DateTime? date, List<string> localId)
        {
            var path = GetApiPath("call", "maketts", "json");
            var param = new Dictionary<string, object>
            {
                {"receptor", StringHelper.Join(",", receptor.ToArray())},
                {"message", System.Net.WebUtility.HtmlEncode(message)},
            };
            if (date != null)
                param.Add("date", DateHelper.DateTimeToUnixTimestamp(date.Value));
            if (localId != null && localId.Count > 0)
                param.Add("localid", StringHelper.Join(",", localId.ToArray()));
            var responseBody = await Execute(path, param);

            return JsonConvert.DeserializeObject<ReturnSend>(responseBody).Entries;
        }

        public Task<SendResult> SendAsync(string sender, string receptor, string message)
        {
            return SendAsync(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue);
        }

        public async Task<SendResult> SendAsync(string sender, string receptor, string message, MessageType type, DateTime date)
        {
            var receptors = new List<string> { receptor };

            return (await SendAsync(sender, receptors, message, type, date))[0];
        }

        public Task<List<SendResult>> SendAsync(string sender, List<string> receptor, string message, MessageType type, DateTime date)
        {
            return SendAsync(sender, receptor, message, type, date, null);
        }

        public async Task<SendResult> SendAsync(string sender, string receptor, string message, MessageType type, DateTime date, string localId)
        {
            var receptors = new List<string> { receptor };
            var localids = new List<string> { localId };

            return (await SendAsync(sender, receptors, message, type, date, localids))[0];
        }

        public Task<SendResult> SendAsync(string sender, string receptor, string message, string localId)
        {
            return SendAsync(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue, localId);
        }

        public Task<List<SendResult>> SendAsync(string sender, List<string> receptor, string message)
        {
            return SendAsync(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue);
        }

        public Task<List<SendResult>> SendAsync(string sender, List<string> receptors, string message, string localId)
        {
            var localIds = new List<string>();
            for (var i = 0; i <= receptors.Count - 1; i++)
                localIds.Add(localId);

            return SendAsync(sender, receptors, message, MessageType.MobileMemory, DateTime.MinValue, localIds);
        }

        public async Task<List<SendResult>> SendAsync(string sender, List<string> receptor, string message, MessageType type, DateTime date, List<string> localids)
        {
            var path = GetApiPath("sms", "send", "json");
            var param = new Dictionary<string, object>
            {
                {"sender", WebUtility.UrlEncode(sender)},
                {"receptor", WebUtility.UrlEncode(StringHelper.Join(",", receptor.ToArray()))},
                {"message", WebUtility.UrlEncode(message)},
                {"type", (int) type},
                {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
            };

            if (localids != null && localids.Count > 0)
                param.Add("localId", StringHelper.Join(",", localids.ToArray()));

            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnSend>(responseBody);
            return result.Entries;
        }

        public Task<List<SendResult>> SendArrayAsync(List<string> senders, List<string> receptors, List<string> messages)
        {
            var types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
                types.Add(MessageType.MobileMemory);

            return SendArrayAsync(senders, receptors, messages, types, DateTime.MinValue, null);
        }

        public Task<List<SendResult>> SendArrayAsync(string sender, List<string> receptors, List<string> messages, MessageType type, DateTime date)
        {
            var senders = new List<string>();
            for (var i = 0; i < receptors.Count; i++)
                senders.Add(sender);

            var types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
                types.Add(MessageType.MobileMemory);

            return SendArrayAsync(senders, receptors, messages, types, date, null);
        }

        public Task<List<SendResult>> SendArrayAsync(string sender, List<string> receptors, List<string> messages, MessageType type, DateTime date, string localmessageids)
        {
            var senders = new List<string>();
            for (var i = 0; i < receptors.Count; i++)
                senders.Add(sender);

            var types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
                types.Add(MessageType.MobileMemory);

            return SendArrayAsync(senders, receptors, messages, types, date, new List<string>() { localmessageids });
        }

        public Task<List<SendResult>> SendArrayAsync(string sender, List<string> receptors, List<string> messages, string localMessageId)
        {
            var senders = new List<string>();
            for (var i = 0; i < receptors.Count; i++)
                senders.Add(sender);

            return SendArrayAsync(senders, receptors, messages, localMessageId);
        }

        public Task<List<SendResult>> SendArrayAsync(List<string> senders, List<string> receptors, List<string> messages, string localMessageId)
        {
            var types = new List<MessageType>();
            for (var i = 0; i <= receptors.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            var localmessageids = new List<string>();
            for (var i = 0; i <= receptors.Count - 1; i++)
            {
                localmessageids.Add(localMessageId);
            }
            return SendArrayAsync(senders, receptors, messages, types, DateTime.MinValue, localmessageids);
        }

        public async Task<List<SendResult>> SendArrayAsync(List<string> senders, List<string> receptors, List<string> messages, List<MessageType> types, DateTime date, List<string> localmessageids)
        {
            var path = GetApiPath("sms", "sendarray", "json");
            var jsonSenders = JsonConvert.SerializeObject(senders);
            var jsonReceptors = JsonConvert.SerializeObject(receptors);
            var jsonMessages = JsonConvert.SerializeObject(messages);
            var jsonTypes = JsonConvert.SerializeObject(types);
            var param = new Dictionary<string, object>
            {
                {"message", jsonMessages},
                {"sender", jsonSenders},
                {"receptor", jsonReceptors},
                {"type", jsonTypes},
                {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
            };

            if (localmessageids != null && localmessageids.Count > 0)
                param.Add("localmessageids", StringHelper.Join(",", localmessageids.ToArray()));

            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnSend>(responseBody);
            if (result.Entries == null)
                return new List<SendResult>();

            return result.Entries;
        }

        public async Task<StatusResult> StatusAsync(string messageId)
        {
            var ids = new List<string> { messageId };
            var result = await StatusAsync(ids);
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<StatusResult>> StatusAsync(List<string> messageIds)
        {
            var path = GetApiPath("sms", "status", "json");
            var param = new Dictionary<string, object>
            {
                {"messageId", StringHelper.Join(",", messageIds.ToArray())}
            };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnStatus>(responseBody);
            if (result.Entries == null)
                return new List<StatusResult>();

            return result.Entries;
        }

        public async Task<StatusLocalMessageIdResult> StatusLocalMessageIdAsync(string messageId)
        {
            var result = await StatusLocalMessageIdAsync(new List<string>() { messageId });
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<StatusLocalMessageIdResult>> StatusLocalMessageIdAsync(List<string> messageIds)
        {
            var path = GetApiPath("sms", "statuslocalmessageid", "json");
            var param = new Dictionary<string, object> { { "localId", StringHelper.Join(",", messageIds.ToArray()) } };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnStatusLocalMessageId>(responseBody);

            return result.Entries;
        }

        public async Task<SendResult> SelectAsync(string messageId)
        {
            var ids = new List<string> { messageId };
            var result = await SelectAsync(ids);

            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<SendResult>> SelectAsync(List<string> messageIds)
        {
            var path = GetApiPath("sms", "select", "json");
            var param = new Dictionary<string, object> { { "messageId", StringHelper.Join(",", messageIds.ToArray()) } };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnSend>(responseBody);
            if (result.Entries == null)
                return new List<SendResult>();

            return result.Entries;
        }

        public Task<List<SendResult>> SelectOutboxAsync(DateTime startDate)
        {
            return SelectOutboxAsync(startDate, DateTime.MaxValue);
        }

        public Task<List<SendResult>> SelectOutboxAsync(DateTime startDate, DateTime endDate)
        {
            return SelectOutboxAsync(startDate, endDate, null);
        }

        public async Task<List<SendResult>> SelectOutboxAsync(DateTime startDate, DateTime endDate, string sender)
        {
            string path = GetApiPath("sms", "selectoutbox", "json");
            var param = new Dictionary<string, object>
             {
                 {"startDate", startDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startDate)},
                 {"endDate", endDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(endDate)},
                 {"sender", sender}
             };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnSend>(responseBody);

            return result.Entries;
        }

        public Task<List<SendResult>> LatestOutboxAsync(long pageSize)
        {
            return LatestOutboxAsync(pageSize, "");
        }

        public async Task<List<SendResult>> LatestOutboxAsync(long pageSize, string sender)
        {
            var path = GetApiPath("sms", "latestoutbox", "json");
            var param = new Dictionary<string, object> { { "pageSize", pageSize }, { "sender", sender } };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnSend>(responseBody);

            return result.Entries;
        }

        public Task<CountOutboxResult> CountOutboxAsync(DateTime startDate)
        {
            return CountOutboxAsync(startDate, DateTime.MaxValue, 10);
        }

        public Task<CountOutboxResult> CountOutboxAsync(DateTime startDate, DateTime endDate)
        {
            return CountOutboxAsync(startDate, endDate, 0);
        }

        public async Task<CountOutboxResult> CountOutboxAsync(DateTime startDate, DateTime endDate, int status)
        {
            var path = GetApiPath("sms", "countoutbox", "json");
            var param = new Dictionary<string, object>
             {
                 {"startDate", startDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startDate)},
                 {"endDate", endDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(endDate)},
                 {"status", status}
             };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnCountOutbox>(responseBody);
            if (result.Entries?[0] == null)
                return new CountOutboxResult();

            return result.Entries[0];
        }

        public async Task<StatusResult> CancelAsync(string messageId)
        {
            var ids = new List<string> { messageId };
            var result = await CancelAsync(ids);

            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<StatusResult>> CancelAsync(List<string> ids)
        {
            var path = GetApiPath("sms", "cancel", "json");
            var param = new Dictionary<string, object>
            {
                {"messageId", StringHelper.Join(",", ids.ToArray())}
            };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnStatus>(responseBody);

            return result.Entries;
        }

        public async Task<List<ReceiveResult>> ReceiveAsync(string line, int isRead)
        {
            var path = GetApiPath("sms", "receive", "json");
            var param = new Dictionary<string, object> { { "lineNumber", line }, { "isRead", isRead } };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnReceive>(responseBody);
            if (result.Entries == null)
                return new List<ReceiveResult>();

            return result.Entries;
        }

        public Task<CountInboxResult> CountInboxACountInboxAsync(DateTime startDate, string lineNumber)
        {
            return CountInboxAsync(startDate, DateTime.MaxValue, lineNumber, 0);
        }

        public Task<CountInboxResult> CountInboxAsync(DateTime startDate, DateTime endDate, string lineNumber)
        {
            return CountInboxAsync(startDate, endDate, lineNumber, 0);
        }

        public async Task<CountInboxResult> CountInboxAsync(DateTime startDate, DateTime endDate, string lineNumber, int isRead)
        {
            var path = GetApiPath("sms", "countoutbox", "json");
            var param = new Dictionary<string, object>
            {
                {"startDate", startDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startDate)},
                {"endDate", endDate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(endDate)},
                {"lineNumber", lineNumber},
                {"isRead", isRead}
            };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnCountInbox>(responseBody);

            return result.Entries[0];
        }

        public async Task<List<CountPostalCodeResult>> CountPostalCodeAsync(long postalCode)
        {
            var path = GetApiPath("sms", "countpostalcode", "json");
            var param = new Dictionary<string, object> { { "postalcode", postalCode } };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnCountPostalCode>(responseBody);

            return result.Entries;
        }

        public Task<List<SendResult>> SendByPostalCodeAsync(long postalcode, string sender, string message, long mciStartIndex, long mciCount, long mtnStartIndex, long mtnCount)
        {
            return SendByPostalCodeAsync(postalcode, sender, message, mciStartIndex, mciCount, mtnStartIndex, mtnCount, DateTime.MinValue);
        }

        public async Task<List<SendResult>> SendByPostalCodeAsync(long postalcode, string sender, string message, long mciStartIndex, long mciCount, long mtnStartIndex, long mtnCount, DateTime date)
        {
            var path = GetApiPath("sms", "sendbypostalcode", "json");
            var param = new Dictionary<string, object>
            {
                {"postalcode", postalcode},
                {"sender", sender},
                {"message", WebUtility.UrlEncode(message)},
                {"mcistartindex", mciStartIndex},
                {"mcicount", mciCount},
                {"mtnstartindex", mtnStartIndex},
                {"mtncount", mtnCount},
                {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
            };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnSend>(responseBody);

            return result.Entries;
        }

        public async Task<AccountInfoResult> AccountInfoAsync()
        {
            var path = GetApiPath("account", "info", "json");
            var responseBody = await ExecuteAsync(path, null);
            var result = JsonConvert.DeserializeObject<ReturnAccountInfo>(responseBody);

            return result.Entries;
        }

        public async Task<AccountConfigResult> AccountConfigAsync(string apiLogs, string dailyReport, string debugMode, string defaultSender, int? minCreditAlarm, string resendFailed)
        {
            var path = GetApiPath("account", "config", "json");
            var param = new Dictionary<string, object>
            {
                {"apilogs", apiLogs},
                {"dailyReport", dailyReport},
                {"debugmode", debugMode},
                {"defaultsender", defaultSender},
                {"mincreditalarm", minCreditAlarm},
                {"resendfailed", resendFailed}
            };

            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnAccountConfig>(responseBody);

            return result.Entries;
        }

        public Task<SendResult> VerifyLookupAsync(string receptor, string token, string template)
        {
            return VerifyLookupAsync(receptor, token, null, null, template, VerifyLookupType.Sms);
        }

        public Task<SendResult> VerifyLookupAsync(string receptor, string token, string template, VerifyLookupType type)
        {
            return VerifyLookupAsync(receptor, token, null, null, template, type);
        }

        public Task<SendResult> VerifyLookupAsyncAsync(string receptor, string token, string token2, string token3, string template)
        {
            return VerifyLookupAsync(receptor, token, token2, token3, template, VerifyLookupType.Sms);
        }

        public Task<SendResult> VerifyLookupAsync(string receptor, string token, string token2, string token3, string token10, string template)
        {
            return VerifyLookupAsync(receptor, token, token2, token3, token10, template, VerifyLookupType.Sms);
        }

        public Task<SendResult> VerifyLookupAsync(string receptor, string token, string token2, string token3, string template, VerifyLookupType type)
        {
            return VerifyLookupAsync(receptor, token, token2, token3, null, template, type);
        }

        public Task<SendResult> VerifyLookupAsync(string receptor, string token, string token2, string token3, string token10, string template, VerifyLookupType type)
        {
            return VerifyLookupAsync(receptor, token, token2, token3, token10, null, template, type);
        }

        public async Task<SendResult> VerifyLookupAsync(string receptor, string token, string token2, string token3, string token10, string token20, string template, VerifyLookupType type)
        {
            var path = GetApiPath("verify", "lookup", "json");
            var param = new Dictionary<string, object>
            {
                {"receptor", receptor},
                {"template", template},
                {"token", token},
                {"token2", token2},
                {"token3", token3},
                {"token10", token10},
                {"token20", token20},
                {"type", type},
            };
            var responseBody = await ExecuteAsync(path, param);
            var result = JsonConvert.DeserializeObject<ReturnSend>(responseBody);

            return result.Entries[0];
        }

        public async Task<SendResult> CallMakeTtsAsync(string message, string receptor)
        {
            return (await CallMakeTtsAsync(message, new List<string> { receptor }, null, null))[0];
        }

        public Task<List<SendResult>> CallMakeTtsAsync(string message, List<string> receptor)
        {
            return CallMakeTtsAsync(message, receptor, null, null);
        }

        public async Task<List<SendResult>> CallMakeTtsAsync(string message, List<string> receptor, DateTime? date, List<string> localId)
        {
            var path = GetApiPath("call", "maketts", "json");
            var param = new Dictionary<string, object>
            {
                {"receptor", StringHelper.Join(",", receptor.ToArray())},
                {"message", WebUtility.UrlEncode(message)},
            };

            if (date != null)
                param.Add("date", DateHelper.DateTimeToUnixTimestamp(date.Value));

            if (localId != null && localId.Count > 0)
                param.Add("localId", StringHelper.Join(",", localId.ToArray()));

            var responseBody = await ExecuteAsync(path, param);

            return JsonConvert.DeserializeObject<ReturnSend>(responseBody).Entries;
        }

        private static async Task<string> ExecuteAsync(string path, Dictionary<string, object> parameters, CancellationToken cancellationToken = default)
        {
            var keyValues = parameters?.Select(x => new KeyValuePair<string, string>(x.Key, x.Value?.ToString()));
            var content = keyValues == null ? null : new FormUrlEncodedContent(keyValues);
            var request = new HttpRequestMessage(HttpMethod.Post, path) { Content = content };

            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    response = await client.SendAsync(request, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();

                    var body = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                        return body;

                    var result = JsonConvert.DeserializeObject<ReturnResult>(body);
                    throw new ApiException(result.Return.Message, result.Return.Status);
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (response != null)
                    throw new HttpException(ex.Message, (int)response.StatusCode);

                throw new HttpException(ex.Message, 500);
            }
        }

        private string GetApiPath(string @base, string method, string output)
        {
            return string.Format(Apipath, @base, method, output);
        }

        private async Task<string> Execute(string path, Dictionary<string, object> @params)
        {
            var nvc = @params?.Select(x => new KeyValuePair<string, string>(x.Key, x.Value?.ToString()));

            var postdata = new FormUrlEncodedContent(nvc);

            var response = await _client.PostAsync(path, postdata);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ReturnResult>(responseBody);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ApiException(result.Return.Message, result.Return.Status);

            return responseBody;
        }
    }
}