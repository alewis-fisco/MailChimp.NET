using System;
using System.Linq;
using System.Collections.Generic;
using MailChimp.Folders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MailChimp.Tests
{
    [TestClass]
    public class FolderTests
    {
        [TestMethod]
        public void GetFolders_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);

            //  Act
            List<FolderListResult> details = mc.GetFolders(FolderType.campaign);

            //  Assert
            Assert.IsNotNull(details);
        }

        [TestMethod]
        public void AddFolder_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);

            //  Act
            FolderAddResult details = mc.AddFolder("TestFolder", FolderType.campaign);

            //  Assert
            Assert.IsNotNull(details);
            Assert.AreNotEqual<int>(0, details.NewFolderId);
        }

        [TestMethod]
        public void UpdateFolder_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var folder = mc.GetFolders(FolderType.campaign).Where(f => f.FolderName.StartsWith("TestFolder")).FirstOrDefault();


            //  Act
            FolderActionResult details = mc.UpdateFolder(folder.FolderId, "TestFolderupdated", FolderType.campaign);

            //  Assert
            Assert.IsNotNull(details);
            Assert.IsTrue(details.Complete);
        }

        [TestMethod]
        public void DeleteFolder_Successful()
        {
            //  Arrange
            MailChimpManager mc = new MailChimpManager(TestGlobal.Test_APIKey);
            var folder = mc.GetFolders(FolderType.campaign).Where(f => f.FolderName.StartsWith("TestFolder")).FirstOrDefault();


            //  Act
            FolderActionResult details = mc.DeleteFolder(folder.FolderId, FolderType.campaign);

            //  Assert
            Assert.IsNotNull(details);
            Assert.IsTrue(details.Complete);
        }
    }
}
