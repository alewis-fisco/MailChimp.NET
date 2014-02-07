using System;
using System.Collections.Generic;
using MailChimp.Lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using MailChimp.Helper;
using System.Diagnostics;

namespace MailChimp.Tests
{
    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void GetLists_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);

            //  Act
            ListResult details = mc.GetLists();

            //  Assert
            Assert.IsNotNull(details.Data);
        }

        [TestMethod]
        public void GetAbuseReport_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            //  Act
            AbuseResult details = mc.GetListAbuseReports(lists.Data[0].Id);

            //  Assert
            Assert.IsNotNull(details.Data);
        }

        [TestMethod]
        public void GetListActivity_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            //  Act
            List<ListActivity> results = mc.GetListActivity(lists.Data[0].Id);

            //  Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }


       

        [TestMethod]
        public void Subscribe_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();
            

            //  Act
            EmailParameter results = mc.Subscribe(lists.Data[0].Id, TestGlobal.KnownEmail0,null,"html",false,false,true,false);

            //  Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(!string.IsNullOrEmpty(results.LEId));
        }


        [TestMethod]
        public void Unsubscribe_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            var x = mc.GetAllMembersForList(lists.Data[0].Id);

            Assert.IsTrue(x.Data.Count > 0);

            //  Act
            UnsubscribeResult results = mc.Unsubscribe(lists.Data[0].Id, TestGlobal.KnownEmail0);

            //  Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Complete);
        }


        [TestMethod]
        public void BatchSubscribe_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            List<BatchEmailParameter> emails = new List<BatchEmailParameter>();

            BatchEmailParameter batchEmail1 = new BatchEmailParameter()
            {
                Email = TestGlobal.KnownEmail1
            };

            BatchEmailParameter batchEmail2 = new BatchEmailParameter()
            {
                Email = TestGlobal.KnownEmail2,
                MergVars = new MergeVar()
                {
                    Groupings = new List<Grouping>()
                     {
                         new Grouping(){ Name = "TestGrouping",GroupNames = new List<string>(){"TestGroup"}}
                     }
                }
            };

            emails.Add(batchEmail1);
            emails.Add(batchEmail2);

            //  Act
            BatchSubscribeResult results = mc.BatchSubscribe(lists.Data[0].Id, emails);

            //  Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.AddCount == 2);

        }

        

     //   [TestMethod]
        public void UnsubscribeValidUser_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            var member = mc.GetAllMembersForList(lists.Data[0].Id).Data.FirstOrDefault();

            var results = mc.Unsubscribe(lists.Data[0].Id, new EmailParameter() { Email = member.Email });
            Assert.IsNotNull(results);
         

        }

        [TestMethod]
        public void BatchUnsubscribe_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            List<EmailParameter> emails = new List<EmailParameter>();

            emails.Add(TestGlobal.KnownEmail1);
            emails.Add(TestGlobal.KnownEmail2);

            //  Act
            BatchUnsubscribeResult results = mc.BatchUnsubscribe(lists.Data[0].Id, emails);

            //  Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.SuccessCount == 2);
        }

        [TestMethod]
        public void GetMemberInfo_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            List<EmailParameter> emails = new List<EmailParameter>();

            emails.Add(TestGlobal.KnownEmail1);
            emails.Add(TestGlobal.KnownEmail2);

            //  Act
            MemberInfoResult results = mc.GetMemberInfo(lists.Data[0].Id, emails);

            //  Assert
            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Data);
            Assert.IsTrue(results.Data.Count.Equals(2));
            Assert.IsTrue(results.Data.Count(m => m.Email.Equals(TestGlobal.KnownEmail1.Email)).Equals(1));        
        }



        [TestMethod]
        public void GetAllMembersForList_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            //  For each list
            foreach(var list in lists.Data)
            {
                //  Write out the list name:
                Debug.WriteLine("Users for the list " + list.Name);

                //  Get the first 100 members of each list:
                MembersResult results = mc.GetAllMembersForList(list.Id, "subscribed", 0, 100);

                //  Write out each member's email address:
                foreach(var member in results.Data)
                {
                    Debug.WriteLine(member.Email);
                }
            }
        }

        [TestMethod]
        public void GetLocationsForList_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();

            //  For each list
            foreach(var list in lists.Data)
            {
                Debug.WriteLine("Information for " + list.Name);

                //  Get the location data for each list:
                List<SubscriberLocation> locations = mc.GetLocationsForList(list.Id);

                //  Write out each of the locations:
                foreach(var location in locations)
                {
                    Debug.WriteLine("Country: {0} - {2} users, accounts for {1}% of list subscribers", location.Country, location.Percent, location.Total);
                }
            }
        }
        [TestMethod]
        public void AddStaticSegment_Successful()
        {
            // Arrange 
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();
            // Act
            StaticSegmentAddResult result = mc.AddStaticSegment(lists.Data[0].Id, "Test Segment");
            // Assert
            Assert.IsNotNull(result.NewStaticSegmentID);
        }
        [TestMethod]
        public void GetStaticSegmentsForList_Successful()
        {
            // Arrange 
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();
            // Act
            List<StaticSegmentResult> result = mc.GetStaticSegmentsForList(lists.Data[0].Id);
            // Assert
            Assert.IsTrue(result.Count > 0);
        }
        [TestMethod]
        public void DeleteStaticSegment_Succesful()
        {
            // Arrange 
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();
            List<StaticSegmentResult> segments = mc.GetStaticSegmentsForList(lists.Data[0].Id);
            // Act
            StaticSegmentActionResult result = mc.DeleteStaticSegment(lists.Data[0].Id, segments[0].StaticSegmentId);
            Assert.IsTrue(result.Complete);
        }
        [TestMethod] 
        public void AddStaticSegmentMembers_Successful()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();
            List<StaticSegmentResult> segments = mc.GetStaticSegmentsForList(lists.Data[0].Id);
            EmailParameter email1 = TestGlobal.KnownEmail0;
            List<EmailParameter> emails = new List<EmailParameter>();
            emails.Add(email1);
            StaticSegmentMembersAddResult result = mc.AddStaticSegmentMembers(lists.Data[0].Id,segments[0].StaticSegmentId,emails);
            Assert.IsTrue(result.successCount == 1);
        }
        [TestMethod]
        public void DeleteStaticSegmentMembers_Successful()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();
            List<StaticSegmentResult> segments = mc.GetStaticSegmentsForList(lists.Data[0].Id);
            EmailParameter email1 = TestGlobal.KnownEmail0;

            List<EmailParameter> emails = new List<EmailParameter>();
            emails.Add(email1);
            StaticSegmentMembersDeleteResult result = mc.DeleteStaticSegmentMembers(lists.Data[0].Id, segments[0].StaticSegmentId, emails);
            Assert.IsTrue(result.successCount == 1);
            Assert.IsTrue(result.errorCount == 0);
        }
        [TestMethod]
        public void ResetStaticSegment_Successful()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();
            List<StaticSegmentResult> segments = mc.GetStaticSegmentsForList(lists.Data[0].Id);
            StaticSegmentActionResult result = mc.ResetStaticSegment(lists.Data[0].Id, segments[0].StaticSegmentId);
            Assert.IsTrue(result.Complete);
        }


        [TestMethod]
        public void AddInterestGrouping_Successful()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var list = mc.GetLists().Data.FirstOrDefault();

            var result = mc.AddInterestGrouping(list.Id, InterestGroupingType.hidden, "testGrouping", new List<string>() { "testGroup" });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetListInterestGroupings_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            ListResult lists = mc.GetLists();
            Assert.IsNotNull(lists);
            Assert.IsTrue(lists.Data.Any());
            //  Act
            List<InterestGrouping> results = mc.GetListInterestGroupings(lists.Data.FirstOrDefault().Id);

            //  Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void UpdateInterestGroupingName_Successful()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var list = mc.GetLists().Data.FirstOrDefault();

            var grouping = mc.GetListInterestGroupings(list.Id).Where(g => g.Name.Equals("testGrouping")).FirstOrDefault();

            var result = mc.UpdateInterestGroupingName(grouping.Id, grouping.Name + "modified");

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void AddInterestGroup_Success()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var list = mc.GetLists().Data.FirstOrDefault();

            var grouping = mc.GetListInterestGroupings(list.Id).Where(g => g.Name.Equals("testGroupingmodified")).FirstOrDefault();

            var result = mc.AddInterestGroup(list.Id, "newGroup", grouping.Id);

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void UpdateInterestGroup_Success()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var list = mc.GetLists().Data.FirstOrDefault();

            var grouping = mc.GetListInterestGroupings(list.Id).Where(g => g.Name.Equals("testGroupingmodified")).FirstOrDefault();

            var result = mc.UpdateInterestGroup(list.Id, "newGroup", "newGroupModified", grouping.Id);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteInterestGroup_Success()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var list = mc.GetLists().Data.FirstOrDefault();

            var grouping = mc.GetListInterestGroupings(list.Id).Where(g => g.Name.Equals("testGroupingmodified")).FirstOrDefault();

            var group = grouping.GroupNames.FirstOrDefault();

            var result = mc.DeleteInterestGroup(list.Id, group.Name, grouping.Id);

            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void DeleteInterestGrouping_Successful()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var list = mc.GetLists().Data.FirstOrDefault();

            var grouping = mc.GetListInterestGroupings(list.Id).Where(g => g.Name.StartsWith("testGrouping")).FirstOrDefault();


            var result = mc.DeleteInterestGrouping(grouping.Id);

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetListMergeVars_Success()
        {
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var list = mc.GetLists().Data.FirstOrDefault();

            var result = mc.GetListMergeVars(list.Id);

            Assert.IsNotNull(result);

            Assert.IsTrue(result.SuccessCount.Equals(1));
            Assert.IsTrue(result.Data[0].MergeVars.Count > 0);

        }

        [TestMethod]
        public void ListResultIndexerReturnsListByName_Success()
        {
            var listInfo = new ListInfo() { Name = TestGlobal.KnownListName };
            var result = new ListResult() { Data = new List<ListInfo>() { listInfo }, Total = 1 };

            Assert.AreEqual(listInfo, result[TestGlobal.KnownListName]);

        }

        [TestMethod]
        public void MemberInfoResultIndexerReturnsMemberByEmail_Success()
        {
            var memberInfo = new MemberInfo() { Email = TestGlobal.KnownEmail0.Email };

            var result = new MemberInfoResult() { Data = new List<MemberInfo>() { memberInfo }, SuccessCount = 1 };

            Assert.AreEqual(memberInfo, result[TestGlobal.KnownEmail0.Email]);
        }

        [TestMethod]
        public void GetSingleMemberInfoSuccess()
        {
            var mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var memberInfo = mc.GetMemberInfo(mc.GetLists()[TestGlobal.KnownListName].Id, TestGlobal.KnownEmail0);

            Assert.IsNotNull(memberInfo);
            Assert.AreEqual(memberInfo.Email, TestGlobal.KnownEmail0.Email);
        }
       
        [TestMethod]
        public void Subscribe_UpdateExistingWithNewInterestGroupings_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
           // ListResult lists = mc.GetLists();

            var list = mc.GetLists()[TestGlobal.KnownListName];

            // create the group we are going to try to add
            var interestGrouping = mc.GetListInterestGroupings(list.Id).Where(x => x.Name.Equals(TestGlobal.KnownGrouping)).FirstOrDefault();
            Grouping grouping = interestGrouping.ToGrouping();
            grouping.GroupNames = new List<string>() { TestGlobal.KnownGroup };


            // get the member and assert the precondition
            var member = mc.GetMemberInfo(list.Id, TestGlobal.KnownEmail0);
            Assert.IsTrue(member.MemberMergeInfo.Groups.Count(mg => mg.Name.Equals(TestGlobal.KnownGrouping)) == 1);
            Assert.IsTrue(member.MemberMergeInfo.Groups.FirstOrDefault().Groups.Count(mg => mg.Name.Equals(TestGlobal.KnownGroup) && mg.Interested) == 0);

            /*Test adding a group*/

            // get the current groups for this member and add a new one
            var mergeVars = member.MemberMergeInfo.ToMergeVar();
            mergeVars.Groupings.Add(grouping);

            // update the member and assert first post condition 
            EmailParameter results = mc.Subscribe(list.Id, TestGlobal.KnownEmail0,mergeVars, "html", false, true, true, false);
            Assert.IsNotNull(results);
            Assert.IsTrue(!string.IsNullOrEmpty(results.LEId));
            member = mc.GetMemberInfo(list.Id, TestGlobal.KnownEmail0);
            Assert.IsTrue(member.MemberMergeInfo.Groups.Count(mg => mg.Name.Equals(TestGlobal.KnownGrouping)) == 1);
            Assert.IsTrue(member.MemberMergeInfo.Groups.FirstOrDefault().Groups.Count(mg => mg.Name.Equals(TestGlobal.KnownGroup) && mg.Interested) == 1);


            /*Test removing group */

            // get member groups and remove the test group
            var mergeVars2 = member.MemberMergeInfo.ToMergeVar();
            mergeVars2.Groupings.Where(g => g.Name.Equals(TestGlobal.KnownGrouping)).First().GroupNames.Remove(TestGlobal.KnownGroup);

            // update the member and assert 2nd post condition
            var results2 = mc.Subscribe(list.Id, TestGlobal.KnownEmail0,mergeVars2, "html", false, true, true, false);

            Assert.IsNotNull(results2);
            Assert.IsTrue(!string.IsNullOrEmpty(results2.LEId));

            member = mc.GetMemberInfo(list.Id, TestGlobal.KnownEmail0);

            Assert.IsTrue(member.MemberMergeInfo.Groups.Count(mg => mg.Name.Equals(TestGlobal.KnownGrouping)) == 1);
            Assert.IsTrue(member.MemberMergeInfo.Groups.FirstOrDefault().Groups.Count(mg => mg.Name.Equals(TestGlobal.KnownGroup) && mg.Interested) == 0);

        }
    }
}
