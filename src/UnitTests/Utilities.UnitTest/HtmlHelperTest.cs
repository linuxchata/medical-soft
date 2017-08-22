using System;
using System.Net.Mime;
using NUnit.Framework;

namespace Utilities.UnitTest
{
    [TestFixture]
    public class HtmlHelperTest
    {
        [Test]
        public void ParseHtml_WhenHtmlIsNull_Test()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => HtmlHelper.ParseHtml(null));
        }

        [Test]
        public void ParseHtml_WhenHtmlIsEmpty_Test()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => HtmlHelper.ParseHtml(string.Empty));
        }

        [Test]
        public void ParseHtml_WhenHtmlIsFakeHtml_Test()
        {
            // Arrange
            const string Html = "fake_html";

            // Act
            var result = HtmlHelper.ParseHtml(Html);

            // Assert
            var images = result.Item1;
            var html = result.Item2;

            Assert.AreEqual(0, images.Count, "Unexpected amount of images.");

            Assert.AreEqual(Html, html.Html, "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, html.Type, "Unexpected type.");
            Assert.AreEqual(null, html.Content, "Unexpected content.");
            Assert.AreEqual(null, html.ContentId, "Unexpected content id.");
        }

        [Test]
        public void ParseHtml_WhenHtmlWithoutImages_Test()
        {
            // Arrange
            const string Html = "<html><body><h1>Test</h1><br><p>Text</p></body></html>";

            // Act
            var result = HtmlHelper.ParseHtml(Html);

            // Assert
            var images = result.Item1;
            var html = result.Item2;

            Assert.AreEqual(0, images.Count, "Unexpected amount of images.");

            Assert.AreEqual(Html, html.Html, "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, html.Type, "Unexpected type.");
            Assert.AreEqual(null, html.Content, "Unexpected content.");
            Assert.AreEqual(null, html.ContentId, "Unexpected content id.");
        }

        [Test]
        public void ParseHtml_WhenHtmlWithOneImage_Test()
        {
            // Arrange
            const string Html = "<html><body><h1>Test</h1><br><img src=\"data:image/gif;base64,some_base_64_string\"></body></html>";

            // Act
            var result = HtmlHelper.ParseHtml(Html);

            // Assert
            var images = result.Item1;
            var html = result.Item2;

            Assert.AreEqual(1, images.Count);

            Assert.IsTrue(images[0].Html.StartsWith("<img src=\"cid:"), "Unexpected image html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, images[0].Type, "Unexpected type.");
            Assert.AreEqual("some_base_64_string", images[0].Content, "Unexpected content.");
            Assert.AreNotEqual(null, images[0].ContentId, "Unexpected content id.");

            Assert.IsFalse(html.Html.IsNullOrEmpty(), "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, html.Type, "Unexpected type.");
            Assert.AreEqual(null, html.Content, "Unexpected content.");
            Assert.AreEqual(null, html.ContentId, "Unexpected content id.");
        }

        [Test]
        public void ParseHtml_WhenHtmlWithOneImage_Test2()
        {
            // Arrange
            const string Html = "<html><body><h1>Test</h1><br><IMG src=\"data:image/gif;base64,some_base_64_string\"></body></html>";

            // Act
            var result = HtmlHelper.ParseHtml(Html);

            // Assert
            var images = result.Item1;
            var html = result.Item2;

            Assert.AreEqual(1, images.Count);

            Assert.IsTrue(images[0].Html.StartsWith("<IMG src=\"cid:"), "Unexpected image html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, images[0].Type, "Unexpected type.");
            Assert.AreEqual("some_base_64_string", images[0].Content, "Unexpected content.");
            Assert.AreNotEqual(null, images[0].ContentId, "Unexpected content id.");

            Assert.IsFalse(html.Html.IsNullOrEmpty(), "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, html.Type, "Unexpected type.");
            Assert.AreEqual(null, html.Content, "Unexpected content.");
            Assert.AreEqual(null, html.ContentId, "Unexpected content id.");
        }

        [Test]
        public void ParseHtml_WhenFakeHtmlWithOneImage_Test()
        {
            // Arrange
            const string Html = "<html><body><h1>Test</h1><br><img src=\"data:image/gif;base64,some_base_64_string\"><p><imganition</p></body></html>";

            // Act
            var result = HtmlHelper.ParseHtml(Html);

            // Assert
            var images = result.Item1;
            var html = result.Item2;

            Assert.AreEqual(1, images.Count);

            Assert.IsTrue(images[0].Html.StartsWith("<img src=\"cid:"), "Unexpected image html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, images[0].Type, "Unexpected type.");
            Assert.AreEqual("some_base_64_string", images[0].Content, "Unexpected content.");
            Assert.AreNotEqual(null, images[0].ContentId, "Unexpected content id.");

            Assert.IsFalse(html.Html.IsNullOrEmpty(), "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, html.Type, "Unexpected type.");
            Assert.AreEqual(null, html.Content, "Unexpected content.");
            Assert.AreEqual(null, html.ContentId, "Unexpected content id.");
        }

        [Test]
        public void ParseHtml_WhenHtmlWithTwoImages_Test()
        {
            // Arrange
            const string Html = "<html><body><h1>Test</h1><br><img src=\"data:image/gif;base64,some_base_64_string\"><p>Text</p><img src=\"data:image/gif;base64,some_base_64_string2\"></body></html>";

            // Act
            var result = HtmlHelper.ParseHtml(Html);

            // Assert
            var images = result.Item1;
            var html = result.Item2;

            Assert.AreEqual(2, images.Count);

            Assert.IsTrue(images[0].Html.StartsWith("<img src=\"cid:"), "Unexpected image html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, images[0].Type, "Unexpected type.");
            Assert.AreEqual("some_base_64_string", images[0].Content, "Unexpected content.");
            Assert.AreNotEqual(null, images[0].ContentId, "Unexpected content id.");

            Assert.IsTrue(images[1].Html.StartsWith("<img src=\"cid:"), "Unexpected image html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, images[1].Type, "Unexpected type.");
            Assert.AreEqual("some_base_64_string2", images[1].Content, "Unexpected content.");
            Assert.AreNotEqual(null, images[1].ContentId, "Unexpected content id.");

            Assert.IsFalse(html.Html.IsNullOrEmpty(), "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, html.Type, "Unexpected type.");
            Assert.AreEqual(null, html.Content, "Unexpected content.");
            Assert.AreEqual(null, html.ContentId, "Unexpected content id.");
        }

        [Test]
        public void ParseHtml_WhenHtmlWithThreeImages_Test()
        {
            // Arrange
            const string Html = "<html><body><h1>Test</h1><br><img src=\"data:image/gif;base64,some_base_64_string\"><p>Text</p><img src=\"data:image/gif;base64,some_base_64_string2\"><img src=\"data:image/gif;base64,some_base_64_string3\"></body></html>";

            // Act
            var result = HtmlHelper.ParseHtml(Html);

            // Assert
            var images = result.Item1;
            var html = result.Item2;

            Assert.AreEqual(3, images.Count);

            Assert.IsTrue(images[0].Html.StartsWith("<img src=\"cid:"), "Unexpected image html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, images[0].Type, "Unexpected type.");
            Assert.AreEqual("some_base_64_string", images[0].Content, "Unexpected content.");
            Assert.AreNotEqual(null, images[0].ContentId, "Unexpected content id.");

            Assert.IsTrue(images[1].Html.StartsWith("<img src=\"cid:"), "Unexpected image html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, images[1].Type, "Unexpected type.");
            Assert.AreEqual("some_base_64_string2", images[1].Content, "Unexpected content.");
            Assert.AreNotEqual(null, images[1].ContentId, "Unexpected content id.");

            Assert.IsTrue(images[2].Html.StartsWith("<img src=\"cid:"), "Unexpected image html.");
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, images[2].Type, "Unexpected type.");
            Assert.AreEqual("some_base_64_string3", images[2].Content, "Unexpected content.");
            Assert.AreNotEqual(null, images[2].ContentId, "Unexpected content id.");

            Assert.IsFalse(html.Html.IsNullOrEmpty(), "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, html.Type, "Unexpected type.");
            Assert.AreEqual(null, html.Content, "Unexpected content.");
            Assert.AreEqual(null, html.ContentId, "Unexpected content id.");
        }

        [Test]
        public void ParseHtml_WhenHtmlWithFakeImage_Test()
        {
            // Arrange
            const string Html = "<html><body><h1>Test</h1><br><p><imganition</p></body></html>";

            // Act
            var result = HtmlHelper.ParseHtml(Html);

            // Assert
            var images = result.Item1;
            var html = result.Item2;

            Assert.AreEqual(0, images.Count, "Unexpected amount of images.");

            Assert.AreEqual(Html, html.Html, "Unexpected html.");
            Assert.AreEqual(MediaTypeNames.Text.Html, html.Type, "Unexpected type.");
            Assert.AreEqual(null, html.Content, "Unexpected content.");
            Assert.AreEqual(null, html.ContentId, "Unexpected content id.");
        }
    }
}
