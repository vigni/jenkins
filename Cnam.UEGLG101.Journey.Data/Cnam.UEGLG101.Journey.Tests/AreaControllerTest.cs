using Cnam.UEGLG101.Journey.App;
using Cnam.UEGLG101.Journey.Data;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace Cnam.UEGLG101.Journey.Tests
{
    [TestFixture]
    public class AreaControllerTest
    {
        private AreaController _controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _controller = new AreaController();
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Request.Headers.Add("Authorization", "token");
        }

        [Test]
        public void AssertThatGetAreasByDepartmentReturnsBadRequestIfGivenDepartmentIsInvalid()
        {
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Request.Headers.Add("Authorization", "token");
            var result = _controller.GetAreasByDepartment("toto");
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result, "Result should be an instance of <BadRequestErrorMessageResult>");
        }

        [Test]
        public void AssertThatGetAreasByDepartmentReturnsOkAndValidListOfAreas()
        {
            var result = _controller.GetAreasByDepartment("60");
            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<Area>>>(result,
                "Result should be an instance of <BadRequestErrorMessageResult>");

            var castedResult = result as OkNegotiatedContentResult<IEnumerable<Area>>;
            var areas = castedResult.Content;

            Assert.IsNotNull(areas, "Areas returned by GetAreasByDepartment should not be empty");
            CollectionAssert.IsNotEmpty(areas, "Areas returned by GetAreasByDepartment should not be empty");
        }

        private void AreaIsValid(Area area)
        {
            var properties = typeof(Area).GetProperties();
        }
    }
}
