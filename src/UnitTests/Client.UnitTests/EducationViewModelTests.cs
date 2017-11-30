using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Cache.Interface;
using Client.Providers;
using Client.ViewModel;
using Common.Builder;
using Common.Enumeration;
using DataAccess;
using Models;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Client.UnitTests
{
    using Ploeh.AutoFixture.AutoMoq;

    [TestFixture]
    public class EducationViewModelTests
    {
        private IFixture fixture;

        private Mock<IUnitOfWork> unitOfWorkMock;

        private Mock<IViewModelBuilder> viewModelBuilderMock;

        private Mock<IViewBuilder> viewBuilderMock;

        private Mock<IMessageBoxProvider> messageBoxProviderMock;

        private Mock<IEducationCache> educationCacheMock;

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
            var viewModel = this.CreateEducationViewModel(true);

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Status, Is.EqualTo(LoadingStatus.Loaded));
        }

        [Test]
        public void CtorEducationViewModel_WhenDataIsPresent_ShouldPopulateModel_Test()
        {
            // Arrange
            var educationModels = this.fixture.Create<IEnumerable<EducationModel>>().ToList();
            this.unitOfWorkMock.Setup(a => a.EducationRepository.GetAllExceptDeleted()).Returns(educationModels);

            // Act
            var viewModel = this.CreateEducationViewModel();

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Status, Is.EqualTo(LoadingStatus.Loaded));
            Assert.That(viewModel.Model.Any(), Is.True);
            CollectionAssert.AreEqual(educationModels.ToList(), viewModel.Model.ToList());
        }

        private EducationViewModel CreateEducationViewModel(bool useAutoMock = false)
        {
            EducationViewModel viewModel = null;
            var scheduler = new SynchronousTaskScheduler();
            Task.Factory.StartNew(
                () =>
                {
                    if (useAutoMock)
                    {
                        viewModel = this.fixture.Create<EducationViewModel>();
                    }
                    else
                    {
                        viewModel = new EducationViewModel(
                            this.unitOfWorkMock.Object,
                            this.viewModelBuilderMock.Object,
                            this.viewBuilderMock.Object,
                            this.messageBoxProviderMock.Object,
                            this.educationCacheMock.Object);
                    }
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);

            return viewModel;
        }
    }
}