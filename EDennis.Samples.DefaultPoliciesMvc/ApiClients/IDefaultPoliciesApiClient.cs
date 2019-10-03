using EDennis.Samples.DefaultPoliciesMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.DefaultPoliciesMvc.ApiClients {
    public interface IDefaultPoliciesApiClient {

        ObjectResult CreatePerson(Person person);
        ObjectResult GetPersons();
        ObjectResult GetPersonDetails(int? id);
        ObjectResult EditPerson(int id, Person person);
        StatusCodeResult DeletePerson(int id);
        StatusCodeResult PersonExists(int id);


        ObjectResult CreatePosition(Position Position);
        ObjectResult GetPositions();
        ObjectResult GetPositionDetails(int? id);
        ObjectResult EditPosition(int id, Position Position);
        StatusCodeResult DeletePosition(int id);
        StatusCodeResult PositionExists(int id);

    }
}