using Client.Cache.Interface;
using Client.Providers;
using Client.ViewModel;
using Common.Builder;
using DataAccess;
using Moq;
using NUnit.Framework;

namespace Client.UnitTests
{
    [TestFixture]
    public class EducationViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;

        private Mock<IViewModelBuilder> viewModelBuilderMock;

        private Mock<IViewBuilder> viewBuilderMock;

        private Mock<IMessageBoxProvider> messageBoxProviderMock;

        private Mock<IEducationCache> educationCacheMock;

        public void Init()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.viewModelBuilderMock = new Mock<IViewModelBuilder>();
            this.viewBuilderMock = new Mock<IViewBuilder>();
            this.messageBoxProviderMock = new Mock<IMessageBoxProvider>();
            this.educationCacheMock = new Mock<IEducationCache>();
        }

        [Test]
        public void CtorEducationViewModel_ShouldReturnNotNull_Test()
        {
            // Arrange
            // Act
            var viewModel = this.CreateEducationViewModel();

            // Assert
            Assert.That(viewModel, Is.Not.Null);
        }

        private EducationViewModel CreateEducationViewModel()
        {
            var viewModel = new EducationViewModel(
                unitOfWorkMock.Object,
                viewModelBuilderMock.Object,
                viewBuilderMock.Object,
                messageBoxProviderMock.Object,
                educationCacheMock.Object);

            return viewModel;
        }
    }
}
