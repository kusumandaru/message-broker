using System;
using System.Collections.Generic;
using MessageBroker.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MessageBroker.Test.Model
{
    [TestClass()]
    public class RequestMessageDataTests
    {
        [TestMethod()]
        public void RequestMessageDataServiceStackTest()
        {
            var objectToSend = new RequestMessageData<SomeMessage>(new SomeMessage(1, "dua", DateTime.Now, new List<string> { "ratna", "santi" })); //some class or object
            var serializeObject = ServiceStack.Text.JsonSerializer.SerializeToString(objectToSend);

            var deserializeObject = ServiceStack.Text.JsonSerializer.DeserializeFromString<RequestMessageData<SomeMessage>>(serializeObject);

            Assert.AreEqual(objectToSend.Data.Number, deserializeObject.Data.Number);
            Assert.AreEqual(objectToSend.Data.Message, deserializeObject.Data.Message);
            DateTimeAssert.AreEqual(objectToSend.Data.StartDate, deserializeObject.Data.StartDate, TimeSpan.FromMinutes(1));
            Assert.AreEqual(objectToSend.Data.MemberList.Count, deserializeObject.Data.MemberList.Count);
            Assert.IsTrue(deserializeObject.Data.MemberList.Contains("ratna"));
            Assert.IsTrue(deserializeObject.Data.MemberList.Contains("santi"));

        }

        [TestMethod()]
        public void RequestMessageDataJsonConvertTest()
        {
            var objectToSend = new RequestMessageData<SomeMessage>(new SomeMessage(1, "dua", DateTime.Now, new List<string> { "ratna", "santi" })); //some class or object
            var serializeObject = JsonConvert.SerializeObject(objectToSend);

            var deserializeObject = JsonConvert.DeserializeObject<RequestMessageData<SomeMessage>>(serializeObject);

            Assert.AreEqual(objectToSend.Data.Number, deserializeObject.Data.Number);
            Assert.AreEqual(objectToSend.Data.Message, deserializeObject.Data.Message);
            DateTimeAssert.AreEqual(objectToSend.Data.StartDate, deserializeObject.Data.StartDate, TimeSpan.FromMinutes(1));
            Assert.AreEqual(objectToSend.Data.MemberList.Count, deserializeObject.Data.MemberList.Count);
            Assert.IsTrue(deserializeObject.Data.MemberList.Contains("ratna"));
            Assert.IsTrue(deserializeObject.Data.MemberList.Contains("santi"));

        }

        private class SomeMessage
        {
            public int Number { get; private set; }
            public string Message { get; private set; }
            public DateTime StartDate { get; private set; }
            public List<string> MemberList { get; private set; }
            public SomeMessage(int number, string message, DateTime startDate, List<string> memberList)
            {
                Number = number;
                Message = message;
                StartDate = startDate;
                MemberList = memberList;
            }
        }

        public static class DateTimeAssert
        {
            public static void AreEqual(DateTime? expectedDate, DateTime? actualDate, TimeSpan maximumDelta)
            {
                if (expectedDate == null && actualDate == null)
                {
                    return;
                }
                else if (expectedDate == null)
                {
                    throw new NullReferenceException("The expected date was null");
                }
                else if (actualDate == null)
                {
                    throw new NullReferenceException("The actual date was null");
                }
                double totalSecondsDifference = Math.Abs(((DateTime)actualDate - (DateTime)expectedDate).TotalSeconds);

                if (totalSecondsDifference > maximumDelta.TotalSeconds)
                {
                    throw new Exception(string.Format("Expected Date: {0}, Actual Date: {1} \nExpected Delta: {2}, Actual Delta in seconds- {3}",
                                                    expectedDate,
                                                    actualDate,
                                                    maximumDelta,
                                                    totalSecondsDifference));
                }
            }
        }


    }
}
