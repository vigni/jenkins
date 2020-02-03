using Cnam.UEGLG101.Journey.Data;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cnam.UEGLG101.Journey.App
{
    [Route("Areas")]
    public class AreaController : ApiController
    {
        private GeoCoordinate _currentLocation;

        public AreaController()
        {
            _currentLocation = new GeoCoordinate(49.2032994, 2.5866091);
        }

        [Route("Department/{idDepartment}")]
        [HttpGet]
        public IHttpActionResult GetAreasByDepartment(string idDepartment)
        {
            /*var results = new List<Area>();
            foreach (var area in AreaRepository.Current.Areas)
            {
                if(area.PostalCode.StartsWith(idDepartment))
                {
                    results.Add(area);
                }
            }
            return results;*/

            // Méthode plus propre et plus simple
            var req = Request;
            var context = RequestContext;
            return SafeRunHttpAction(() =>
            {
                if(!int.TryParse(idDepartment, out var postal))
                {
                    return BadRequest($"Invalid department id : '{idDepartment}'");
                }
                var enumerable = from area in AreaRepository.Current.Areas
                                 where area != null && area.PostalCode.StartsWith(idDepartment)
                                 select area;
                return Ok(enumerable);
            });
        }
        
        [Route("Distance/{distance}")]
        [HttpGet]
        public IHttpActionResult GetAreasByDistance(int distance)
        {
            return SafeRunHttpAction(() =>
            {
                var enumerable = from area in AreaRepository.Current.Areas
                                 let areaCoordinate = new GeoCoordinate(area.Latitude, area.Longitude)
                                 where _currentLocation.GetDistanceTo(areaCoordinate) / 1000 <= distance
                                 select area;
                return Ok(enumerable);
            });
            
        }

        private string GetToken()
        {
            return Request.RequestUri.Query.Replace("?api_key=", "");
        }
        private bool IsValidToken(string token)
        {
            var tokens = new[] { "token", "token1", "token2" };
            return tokens.Contains(token);
        }

        public IHttpActionResult SafeRunHttpAction(Func<IHttpActionResult> getDatas)
        {
            try
            {
                return getDatas();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult CreateArea(Area area)
        {
            return SafeRunHttpAction(() =>
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                AreaRepository.Current.Areas.Add(area);
                return Created("/",area.Name);
            });
        }
    }

    public class AreaRepository
    {
        private static AreaRepository _current;
        public static AreaRepository Current => _current ?? (_current = new AreaRepository());

        public List<Area> Areas { get; set; }

        private AreaRepository()
        {
            Areas = new List<Area>();
        }
    }
}
