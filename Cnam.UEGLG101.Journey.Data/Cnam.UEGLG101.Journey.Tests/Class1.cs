using System.Device.Location;
using System.Linq;
using System.Net;
using Cnam.UEGLG101.Journey.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace Cnam.UEGLG101.Journey.Tests
{
    [TestFixture]
    public class Class1
    {
        private string _departementRoute;
        private string _distanceRoute;
        private IRestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _departementRoute = "Value/GetAreasWithDepartmentParam?postCodes=60";
            _distanceRoute = "Value/GetAreasWithKilometersInParam?nbKm=50";
            _restClient = new RestClient("http://localhost:9000/");
        }

        [Test]
        public void AssertThatEachRouteWorks()
        {
            var departmentsResponse = _restClient.Get(new RestRequest(_departementRoute, Method.GET));
            var distanceRespone = _restClient.Get(new RestRequest(_distanceRoute, Method.GET));
            
            Assert.AreEqual(HttpStatusCode.OK, departmentsResponse.StatusCode, "Department route does not work");
            Assert.AreEqual(HttpStatusCode.OK, distanceRespone.StatusCode, "Range route does not work");
        }

        [Test]
        public void AssertThatDepartmentsRouteResultsMatchWithBusinessRules()
        {
            var response = _restClient.Get(new RestRequest(_departementRoute, Method.GET));
            var results = JsonConvert.DeserializeObject<Area[]>(response.Content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Invalid response code");
            Assert.IsNotNull(results, "Get by departments returns null");
            Assert.IsNotEmpty(results, "Get by departments returns empty array");
            Assert.IsTrue(results.All(res => !string.IsNullOrEmpty(res.Address)), "Results contain ares with empty address");
            Assert.IsTrue(results.All(res => res.PostalCode.StartsWith("60")), "Result contain areas that dos not belong to specified department");
        }

        [Test]
        public void AssertThatRangeRouteResultsMatchWithBusinessRules()
        {
            var currentLocation = new GeoCoordinate(49.2032994, 2.5866091);
            var response = _restClient.Get(new RestRequest(_distanceRoute, Method.GET));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Invalid response code");

            var results = JsonConvert.DeserializeObject<Area[]>(response.Content);

            Assert.IsNotNull(results, "Get by range returns null");
            Assert.IsNotEmpty(results, "Get by range returns empty array");
            Assert.IsTrue(results.All(res => !string.IsNullOrEmpty(res.Address)), "Results contain ares with empty address");
            Assert.IsTrue(results.All(res => currentLocation.GetDistanceTo(new GeoCoordinate(res.Latitude, res.Longitude)) / 1000 <= 50), "Result contain areas too far from current location");

        }
    }
}