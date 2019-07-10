using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using NUnit.Framework;

using oht;
using oht.Entities;

namespace UnitTestOHT
{
    [TestFixture]
    public class OhtTests
    {
        private string _publicKey;
        private string _privateKey;
        private string _englishLangCode;
        private string _germanLangCode;
        private string _englishLangName;
        private string _testContentEn;
        private int _testContentWordCount;
        private string _projectComment;
        private string _projectTag;
       
        [OneTimeSetUp]
        public void Setup()
        {
            _publicKey = "BHJDgbNRG8dZKTqc3xQ7";
            _privateKey = "25d6f32a28b8c9a4bf6ef1c41014e358";
            _englishLangCode = "en-us";
            _germanLangCode = "de-de";
            _englishLangName = "english";
            _testContentEn = "Once upon a time was girl who lived in a nice tiny house.";
            _testContentWordCount = 13;
            _projectComment = "Here is some comment about project";
            _projectTag = "ProjectTag1";
        }

        [Test]
        public void TestGetAccountDetails()
        {
            // Arrange            
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act
            var result = oht.GetAccountDetails();

            // Assert
            Assert.NotNull(result);            
        }

        [Test]
        public void TestCreateFileResource()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            var fileName = string.Format("unittest{0}.txt", Guid.NewGuid().ToString());

            // Act
            var resourceId = oht.CreateFileResource(_testContentEn, fileName);

            // Assert
            Assert.IsNotEmpty(resourceId);
        }


        [Test]
        public void TestCreateTextResource()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);            
           
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);

            // Assert
            Assert.IsNotEmpty(resourceId);
        }

        [Test]
        public void TestGetResource()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var resource = oht.GetResource(resourceId);

            // Assert
            StringAssert.Contains(resourceId, resource.DownloadUrl);
        }


        [Test]
        public void TestDownloadResource()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);            
            var tempFilePath = Path.GetTempFileName();

            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            oht.DownloadResource(resourceId, tempFilePath);
            var savedContent = File.ReadAllText(tempFilePath);

            // Assert
            StringAssert.AreEqualIgnoringCase(_testContentEn, savedContent);
        }

        [Test]
        public void TestGetSupportedLanguages()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act            
            var result = oht.GetSupportedLanguages();

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void TestGetSupportedLanguagePairs()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act            
            var result = oht.GetSupportedLanguagePairs();

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void TestDetectLanguage()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);            

            // Act            
            var result = oht.DetectLanguage(_testContentEn);

            // Assert
            StringAssert.AreEqualIgnoringCase(_englishLangName, result.Language);
        }


        [Test]
        public void TestGetSupportedExpertises()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act            
            var result = oht.GetSupportedExpertises();

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void TestMachineTranslation()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
           
            // Act            
            var result = oht.MachineTranslation(_englishLangCode, _germanLangCode, _testContentEn);

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void TestGetWordCount()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);            

            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var result = oht.GetWordCount(new List<string> { resourceId });

            // Assert
            Assert.AreEqual(_testContentWordCount, result.Total.Wordcount);
        }

        [Test]
        public void TestGetQuote()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var result = oht.GetQuote(new List<string> { resourceId }, 0, _englishLangCode, _germanLangCode);

            // Assert
            Assert.AreEqual(_testContentWordCount, result.Total.Wordcount);
        }

        [Test]
        public void TestCreateTranslationProject()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var result = oht.CreateTranslationProject(_englishLangCode, _germanLangCode, new List<string> { resourceId });
            
            // Assert
            Assert.AreEqual(_testContentWordCount, result.Wordcount);
        }


        [Test]
        public void TestCreateTransProofProject()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var result = oht.CreateTransProofProject(_englishLangCode, _germanLangCode, new List<string> { resourceId });

            // Assert
            Assert.AreEqual(_testContentWordCount, result.Wordcount);
        }

        [Test]
        public void TestCreateProofreadingProject()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var result = oht.CreateProofreadingProject(_englishLangCode, new List<string> { resourceId });

            // Assert
            Assert.AreEqual(_testContentWordCount, result.Wordcount);
        }

        [Test]
        public void TestCreateProofTranslatedProject()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var result = oht.CreateProofTranslatedProject(_englishLangCode, _germanLangCode, new List<string> { resourceId }, new List<string> { resourceId });

            // Assert
            Assert.AreEqual(_testContentWordCount, result.Wordcount);
        }

        [Test]
        public void TestCreateTranscriptionProject()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var result = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });

            // Assert
            Assert.AreEqual(_testContentWordCount, result.Wordcount);
        }

        [Test]
        public void TestCancelProject()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var project = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });
            oht.CancelProject(project.ProjectId);

            // Assert
            // Nothing to check, just making sure no exception raised.
        }

        [Test]
        public void TestGetProjectDetails()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var project = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });
            var result = oht.GetProjectDetails(project.ProjectId);

            // Assert
            Assert.AreEqual(_testContentWordCount, result.Wordcount);
        }

        [Test]
        public void TestGetProjectsList()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);
            
            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var project = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });
            var result = oht.GetProjectsList();

            // Assert
            Assert.AreNotEqual("0", result.projectsCount);
        }

        [Test]
        public void TestAddProjectComment()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var project = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });
            oht.AddProjectComment(project.ProjectId, _projectComment);

            // Assert
            // Nothing to check - just making sure no exception raised.
        }

        [Test]
        public void TestGetProjectComments()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var project = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });
            oht.AddProjectComment(project.ProjectId, _projectComment);
            var result = oht.GetProjectComments(project.ProjectId);

            // Assert
            Assert.AreNotEqual(0, result.Count);
        }

        [Test]
        public void TestAddProjectTag()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var project = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });
            oht.AddProjectTag(project.ProjectId, _projectTag);

            // Assert
            // Nothing to check - just making sure no exception raised.
        }

        [Test]
        public void TestGetProjectTags()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);

            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var project = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });
            oht.AddProjectTag(project.ProjectId, _projectTag);
            var result = oht.GetProjectTags(project.ProjectId);

            // Assert
            Assert.AreNotEqual(0, result.Count);
        }

        [Test]
        public void TestDeleteProjectTag()
        {
            // Arrange
            var oht = new OhtApi(_privateKey, _publicKey, true);            

            // Act
            var resourceId = oht.CreateTextResource(_testContentEn);
            var project = oht.CreateTranscriptionProject(_englishLangCode, new List<string> { resourceId });
            oht.AddProjectTag(project.ProjectId, _projectTag);
            var list = oht.GetProjectTags(project.ProjectId);
            var tagObj = list.SingleOrDefault(a => a.Value == _projectTag);
            oht.DeleteProjectTag(project.ProjectId, tagObj.Key);

            // Assert
            // Nothing to check - just making sure no exception raised.
        }
    }
}
