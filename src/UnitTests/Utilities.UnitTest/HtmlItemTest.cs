using System;
using System.Net.Mime;
using NUnit.Framework;

namespace Utilities.UnitTest
{
    [TestFixture]
    public class HtmlItemTest
    {
        [Test]
        public void NullHtmlItem_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlItem(null, string.Empty));
        }

        [Test]
        public void EmptyHtmlItem_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlItem(string.Empty, string.Empty));
        }

        [Test]
        public void NullType_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlItem("Html", null));
        }

        [Test]
        public void EmptyType_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlItem("Html", string.Empty));
        }

        [Test]
        public void FakeType_Test()
        {
            Assert.Throws<ArgumentException>(() => new HtmlItem("Html", "Type"));
        }

        [Test]
        public void ImageTypeNullContentNullContentId_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlItem("Html", MediaTypeNames.Image.Jpeg));
        }

        [Test]
        public void ImageTypeEmptyContentNullContentId_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlItem("Html", MediaTypeNames.Image.Jpeg, string.Empty));
        }

        [Test]
        public void ImageTypeNullContentId_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlItem("Html", MediaTypeNames.Image.Jpeg, "Content"));
        }

        [Test]
        public void ImageTypeEmptyContentId_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlItem("Html", MediaTypeNames.Image.Jpeg, "Content", string.Empty));
        }

        [Test]
        public void HtmlType_Test()
        {
            var result = new HtmlItem("Html", MediaTypeNames.Text.Html);

            Assert.AreEqual("Html", result.Html, "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, result.Type, "Unexpected type.");
            Assert.AreEqual(null, result.Content, "Unexpected content.");
            Assert.AreEqual(null, result.ContentId, "Unexpected content id.");
        }

        [Test]
        public void ImageType_Test()
        {
            var result = new HtmlItem("<img />", MediaTypeNames.Image.Jpeg, "content", "id");

            Assert.AreEqual("<img />", result.Html, "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, result.Type, "Unexpected type.");
            Assert.AreEqual("content", result.Content, "Unexpected content.");
            Assert.AreEqual("id", result.ContentId, "Unexpected content id.");
        }
    }
}
