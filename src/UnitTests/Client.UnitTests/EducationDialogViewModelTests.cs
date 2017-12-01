using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.ViewModel.Dialogs;
using Common.Communication;
using Common.Enumeration;
using Contracts;
using DataAccess;
using Models;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Client.UnitTests
{
    [TestFixture]
    public class EducationDialogViewModelTests
    {
        private IFixture fixture;

        [SetUp]
        public void Init()
        {
            this.fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
        }

        [Test]
        public void HandleCommand_WhenSuccessful_ShouldPopulateModel_Test()
        {
            // Arrange
            var id = this.fixture.Create<int>();
            var repository = this.CreateEducationRepositoryMock();
            var unitOfWorkMock = this.CreateUnitOfWorkMock(id);
            var sut = this.fixture.Create<EducationDialogViewModel>();

            // Act
            this.ConcurrentExecute(sut.HandleCommand);

            // Assert
            Assert.That(sut.Status, Is.EqualTo(LoadingStatus.Added));
            Assert.That(sut.Model, Is.Not.Null);
            Assert.That(sut.Model.Id, Is.EqualTo(id));

            repository.Verify(m => m.Add(It.IsAny<EducationModel>()), Times.Once);
            unitOfWorkMock.Verify(m => m.Save(), Times.Once);
        }

        [Test]
        public void HandleCommand_WhenFailed_ShouldPopulateModel_Test()
        {
            // Arrange
            var id = this.fixture.Create<int>();
            var repository = this.CreateEducationRepositoryMock();
            var unitOfWorkMock = this.CreateUnitOfWorkMock(id, true);
            var sut = this.fixture.Create<EducationDialogViewModel>();

            // Act
            this.ConcurrentExecute(sut.HandleCommand);

            // Assert
            Assert.That(sut.Status, Is.EqualTo(LoadingStatus.Failed));

            repository.Verify(m => m.Add(It.IsAny<EducationModel>()), Times.Once);
            unitOfWorkMock.Verify(m => m.Save(), Times.Once);
        }

        private Mock<IEducationRepository<EducationModel, int>> CreateEducationRepositoryMock()
        {
            var repository = this.fixture.Freeze<Mock<IEducationRepository<EducationModel, int>>>();
            repository.Setup(a => a.Add(It.IsAny<EducationModel>()));
            return repository;
        }

        private Mock<IUnitOfWork> CreateUnitOfWorkMock(int id, bool isFailed = false)
        {
            var response = this.CreateSaveChangesResponse(id, isFailed);
            var unitOfWorkMock = this.fixture.Freeze<Mock<IUnitOfWork>>();
            unitOfWorkMock.Setup(a => a.Save()).Returns(response);
            return unitOfWorkMock;
        }

        private SaveChangesResponse CreateSaveChangesResponse(int id, bool isFailed)
        {
            SaveChangesResponse response;
            if (isFailed)
            {
                var excepton = this.fixture.Create<Exception>();
                response = new SaveChangesResponse(excepton);
            }
            else
            {
                response = new SaveChangesResponse();
                response.TryAddResult(DatabaseEntity.Educations.ToString(), id);
            }

            return response;
        }

        private void ConcurrentExecute(ICommand command)
        {
            var scheduler = new SynchronousTaskScheduler();
            Task.Factory.StartNew(
                () =>
                {
                    command.Execute(null);
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);
        }
    }
}