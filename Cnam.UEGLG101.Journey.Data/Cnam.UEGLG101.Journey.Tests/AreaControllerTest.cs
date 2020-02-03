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
        private AreaController _controller;[OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _controller = new AreaController()
            {
                Request = new System.Net.Http.HttpRequestMessage()
            };
            _controller.Request.Headers.Add("Authorization", "token"); AreaRepository.Current.Areas = DataReader.GetAllAreas();
        }
        [Test]
        public void AssertThatGetAreasByDepartmentReturnsBadRequestIfGivenDepartmentIsInvalid()
        {
            var result = _controller.GetAreasByDepartment("toto");
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result,
                "Result should be an instance of <BadRequestErrorMessageResult>");
        }
        [Test]
        public void AssertThatGetAreasByDepartmentReturnsOkAndValidListOfAreas()
        {
            var result = _controller.GetAreasByDepartment("60");
            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<Area>>>(result,
                "Result should be an instance of <BadRequestErrorMessageResult>"); var castedResult = result as OkNegotiatedContentResult<IEnumerable<Area>>;
            var areas = castedResult.Content; Assert.IsNotNull(areas, "Areas returned by GetAreasByDepartment should not be empty");
            CollectionAssert.IsNotEmpty(areas,
                "Areas returned by GetAreasByDepartment should not be empty"); foreach (var area in areas)
            {
                Assert.IsTrue(area.IsValid(out var errors), errors);
            }
        }
        [Test]
        public void AsserThatControllerReturnsUnauthorizedWhenTokenIsNull()
        {
            var controller = new AreaController();
            var resut = controller.GetAreasByDistance(50);
            Assert.IsInstanceOf<UnauthorizedResult>(resut, "Result should be an instance of <UnauthorizedResult>");
        }
        [TestCase("token7")]
        [TestCase("token8")]
        [TestCase("token9")]
        [TestCase("token19")]
        public void AsserThatControllerReturnsUnauthorizedWhenTokenIsInvalid(string token)
        {
            var controller = new AreaController()
            {
                Request = new System.Net.Http.HttpRequestMessage()
            };
            controller.Request.Headers.Add("Authorization", token); var resut = controller.GetAreasByDistance(50);
            Assert.IsInstanceOf<UnauthorizedResult>(resut, "Result should be an instance of <UnauthorizedResult>");
        }
    }
}