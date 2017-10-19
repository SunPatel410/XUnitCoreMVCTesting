using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CreditCards.Controllers;
using CreditCards.Core.Interfaces;
using CreditCards.Core.Model;
using CreditCards.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CreditCards.Tests.Controller
{
    public class ApplyControllerShould
    {
        private readonly Mock<ICreditCardApplicationRepository> _mockRepo;
        private readonly ApplyController _sut;

        public ApplyControllerShould()
        {
             _mockRepo = new Mock<ICreditCardApplicationRepository>();
             _sut = new ApplyController(_mockRepo.Object);
        }

        [Fact]
        public void ReturnViewForIndex()
        {
            IActionResult result = _sut.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ReturnViewWhenInvalidModelState()
        {
            _sut.ModelState.AddModelError("x", "Test Error");

            var app = new NewCreditCardApplicationDetails
            {
                FirstName = "Sarah"
            };

            IActionResult result = _sut.Index();

            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<NewCreditCardApplicationDetails>(viewResult.Model);

            Assert.Equal(app.FirstName, model.FirstName);
        }

        [Fact]
        public async Task NotSaveApplicationWhenModelError()
        {
            _sut.ModelState.AddModelError("x", "Test Error");

            var app = new NewCreditCardApplicationDetails();

            await _sut.Index(app);

            _mockRepo.Verify(
                x => x.AddAsync(It.IsAny<CreditCardApplication>()), Times.Never);
        }

        [Fact]
        public async Task SaveApplicationWhenValidModel()
        {
            CreditCardApplication savedApplication = null;

            _mockRepo.Setup(x => x.AddAsync(It.IsAny<CreditCardApplication>()))
                .Returns(Task.CompletedTask)
                //when the addasync mock is executed . The callback will get us the credit card application again.
                .Callback<CreditCardApplication>(x => savedApplication = x);

            var app = new NewCreditCardApplicationDetails
            {
                FirstName = "Sunny",
                LastName = "Patel",
                Age = 29,
                FrequentFlyerNumber = "012345-A",
                GrossAnnualIncome = 100_000
            };

            await _sut.Index(app);

            _mockRepo.Verify(
                x => x.AddAsync(It.IsAny<CreditCardApplication>()), Times.Once);

            Assert.Equal(app.FirstName, savedApplication.FirstName);
            Assert.Equal(app.LastName, savedApplication.LastName);
            Assert.Equal(app.Age, savedApplication.Age);
            Assert.Equal(app.FrequentFlyerNumber, savedApplication.FrequentFlyerNumber);
            Assert.Equal(app.GrossAnnualIncome, savedApplication.GrossAnnualIncome);
        }

        [Fact]
        public async Task ReturnApplicationCompleteValidModel()
        {
            var app = new NewCreditCardApplicationDetails
            {
                FirstName = "Sunny",
                LastName = "Patel",
                Age = 29,
                FrequentFlyerNumber = "012345-A",
                GrossAnnualIncome = 100_000
            };

            var result = await _sut.Index(app);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("ApplicationComplete", viewResult.ViewName);
        }
    }
}
