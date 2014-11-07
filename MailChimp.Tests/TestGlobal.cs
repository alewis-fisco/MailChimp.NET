using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MailChimp.Helper;

namespace MailChimp.Tests
{
    /// <summary>
    /// Global test information is set in this class
    /// </summary>
    [TestClass]
    public class TestGlobal
    {
        /// <summary>
        /// The global mailchimp API key
        /// </summary>
        public static string Test_APIKey = "2b86828a0ff0f296c521e7bf9897908f-us3";

        [AssemblyInitialize()]
        public static void AllTestInit(TestContext testContext)
        {
            //  Set this to your Mailchimp API key for testing
            //  See http://kb.mailchimp.com/article/where-can-i-find-my-api-key
            //  for help finding your API key
            Test_APIKey = "2b86828a0ff0f296c521e7bf9897908f-us3";
        }


        public static EmailParameter KnownEmail0
        {
            get
            {
                return new EmailParameter()
                {
                    Email = "customeremail@righthere.com"
                };
            }
        }

        public static EmailParameter KnownEmail1
        {
            get
            {
                return new EmailParameter()
                {
                    Email = "customeremail1@righthere.com"
                };
            }

        }

        public static EmailParameter KnownEmail2
        {
            get
            {
                return new EmailParameter()
                {
                    Email = "customeremail2@righthere.com"
                };
            }

        }

        public static string KnownListName
        {
            get
            {
                return "Test";
            }
        }

        public static string KnownGrouping
        {
            get
            {
                return "testGrouping";
            }
        }

        public static string KnownGroup
        {
            get
            {
                return "testGroup";
            }
        }
    }
}
