using System.Threading;
using System.Threading.Tasks;
using Client.Cache.Interface;
using Client.Providers;
using Client.ViewModel;
using Common.Builder;
using Common.Enumeration;
using DataAccess;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

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

        private IFixture fixture;

        [SetUp]
        public void Init()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());

            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.viewModelBuilderMock = new Mock<IViewModelBuilder>();
            this.viewBuilderMock = new Mock<IViewBuilder>();
            this.messageBoxProviderMock = new Mock<IMessageBoxProvider>();
            this.educationCacheMock = new Mock<IEducationCache>();
        }

        [Test]
        public void CtorEducationViewModel_ShouldReturnLoadedStatus_Test()
        {
            // Arrange
            // Act
            var viewModel = this.CreateEducationViewModel();

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Status, Is.EqualTo(LoadingStatus.Loaded));
        }

        private EducationViewModel CreateEducationViewModel()
        {
            EducationViewModel viewModel = null;
            var scheduler = new SynchronousTaskScheduler();
            Task.Factory.StartNew(
                () =>
                {
                    viewModel = this.fixture.Create<EducationViewModel>();
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);

            return viewModel;
        }
    }
}
