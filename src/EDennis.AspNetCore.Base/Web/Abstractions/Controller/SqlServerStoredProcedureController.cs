using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    [ApiController]
    [Route("api/[controller]")]
    public abstract class SqlServerStoredProcedureController<TContext> : ControllerBase
        where TContext : DbContext, ISqlServerDbContext<TContext> {


        public TContext Context { get; }

        public SqlServerStoredProcedureController(TContext context) {
            Context = context;
        }


        /// <summary>
        /// Executes a stored procedure and returns the result as a json array.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("array")]
        public IActionResult GetJsonArrayFromStoredProcedure([FromQuery] string spName, [FromQuery] Dictionary<string, string> parameters) {

            try {
                var parms = ConvertParameters(parameters);
                var result = Context.GetJsonArrayFromStoredProcedure(spName, parms, null);
                return Ok(result);
            } catch (ArgumentException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result as a json array.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("array/async")]
        public async Task<IActionResult> GetJsonArrayFromStoredProcedureAsync([FromQuery] string spName, [FromQuery] Dictionary<string, string> parameters) {

            try {
                var parms = ConvertParameters(parameters);
                var result = await Context.GetJsonArrayFromStoredProcedureAsync(spName, parms, null);
                return Ok(result);
            } catch (ArgumentException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }



        /// <summary>
        /// Executes a stored procedure and returns the result as a json object.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("object")]
        public IActionResult GetJsonObjectFromStoredProcedure([FromQuery] string spName, [FromQuery] Dictionary<string, string> parameters) {

            try {
                var parms = ConvertParameters(parameters);
                var result = Context.GetJsonObjectFromStoredProcedure(spName, parms, null);
                return Ok(result);
            } catch (ArgumentException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result as a json object.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("object/async")]
        public async Task<IActionResult> GetJsonObjectFromStoredProcedureAsync([FromQuery] string spName, [FromQuery] Dictionary<string, string> parameters) {

            try {
                var parms = ConvertParameters(parameters);
                var result = await Context.GetJsonObjectFromStoredProcedureAsync(spName, parms, null);
                return Ok(result);
            } catch (ArgumentException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }



        /// <summary>
        /// Executes a stored procedure and returns the result as a json object.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("scalar")]
        public IActionResult GetScalarFromStoredProcedure([FromQuery] string spName, [FromQuery] string returnType, [FromQuery] Dictionary<string, string> parameters) {

            if (spName == null)
                return new BadRequestObjectResult("spName query parameter not provided");
            else if (returnType == null)
                return new BadRequestObjectResult("returnType query parameter not provided");

            try {
                var parms = ConvertParameters(parameters);

                if (returnType.Equals("int", StringComparison.OrdinalIgnoreCase) || returnType.Equals("integer", StringComparison.OrdinalIgnoreCase))
                    return Ok(Context.GetScalarFromStoredProcedure<TContext, int>(spName, parms));
                else if (returnType.Equals("decimal", StringComparison.OrdinalIgnoreCase) || returnType.Equals("money", StringComparison.OrdinalIgnoreCase))
                    return Ok(Context.GetScalarFromStoredProcedure<TContext, decimal>(spName, parms));
                else if (returnType.Equals("date", StringComparison.OrdinalIgnoreCase) || returnType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                    return Ok(Context.GetScalarFromStoredProcedure<TContext, DateTime>(spName, parms));
                else if (returnType.Equals("time", StringComparison.OrdinalIgnoreCase) || returnType.Equals("timespan", StringComparison.OrdinalIgnoreCase))
                    return Ok(Context.GetScalarFromStoredProcedure<TContext, TimeSpan>(spName, parms));
                else if (returnType.Equals("bool", StringComparison.OrdinalIgnoreCase) || returnType.Equals("boolean", StringComparison.OrdinalIgnoreCase))
                    return Ok(Context.GetScalarFromStoredProcedure<TContext, bool>(spName, parms));
                else
                    return Ok(Context.GetScalarFromStoredProcedure<TContext, string>(spName, parms));
            } catch (ArgumentException ex) {
                return new BadRequestObjectResult(ex.Message);
            }

        }


        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result as a json object.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("scalar/async")]
        public async Task<IActionResult> GetScalarFromStoredProcedureAsync([FromQuery] string spName, [FromQuery] string returnType, [FromQuery] Dictionary<string, string> parameters) {

            if (spName == null)
                return new BadRequestObjectResult("spName query parameter not provided");
            else if (returnType == null)
                return new BadRequestObjectResult("returnType query parameter not provided");

            try {
                var parms = ConvertParameters(parameters);

                if (returnType.Equals("int", StringComparison.OrdinalIgnoreCase) || returnType.Equals("integer", StringComparison.OrdinalIgnoreCase))
                    return Ok(await Context.GetScalarFromStoredProcedureAsync<TContext, int>(spName, parms));
                else if (returnType.Equals("decimal", StringComparison.OrdinalIgnoreCase) || returnType.Equals("money", StringComparison.OrdinalIgnoreCase))
                    return Ok(await Context.GetScalarFromStoredProcedureAsync<TContext, decimal>(spName, parms));
                else if (returnType.Equals("date", StringComparison.OrdinalIgnoreCase) || returnType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                    return Ok(await Context.GetScalarFromStoredProcedureAsync<TContext, DateTime>(spName, parms));
                else if (returnType.Equals("time", StringComparison.OrdinalIgnoreCase) || returnType.Equals("timespan", StringComparison.OrdinalIgnoreCase))
                    return Ok(await Context.GetScalarFromStoredProcedureAsync<TContext, TimeSpan>(spName, parms));
                else if (returnType.Equals("bool", StringComparison.OrdinalIgnoreCase) || returnType.Equals("boolean", StringComparison.OrdinalIgnoreCase))
                    return Ok(await Context.GetScalarFromStoredProcedureAsync<TContext, bool>(spName, parms));
                else
                    return Ok(Context.GetScalarFromStoredProcedureAsync<TContext, string>(spName, parms));

            } catch (ArgumentException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }



        /// <summary>
        /// Obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("forjson")]
        public IActionResult GetJsonFromJsonStoredProcedure([FromQuery] string spName, [FromQuery] Dictionary<string, string> parameters) {

            try {
                var parms = ConvertParameters(parameters);
                var result = Context.GetJsonFromJsonStoredProcedure(spName, parms);
                return Ok(result);
            } catch (ArgumentException ex) {
                return new BadRequestObjectResult(ex.Message);
            }

        }


        /// <summary>
        /// Asynchronously obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("forjson/async")]
        public async Task<IActionResult> GetJsonFromJsonStoredProcedureAsync([FromQuery] string spName, [FromQuery] Dictionary<string, string> parameters) {

            try {
                var parms = ConvertParameters(parameters);
                var result = await Context.GetJsonFromJsonStoredProcedureAsync(spName, parms);
                return Ok(result);
            } catch (ArgumentException ex) {
                return new BadRequestObjectResult(ex.Message);
            }

        }


        private Dictionary<string, object> ConvertParameters(Dictionary<string, string> parameters) {
            return parameters
                .Where(q => q.Key != "spName" && q.Key != "returnType")
                .ToDictionary(q => q.Key, q => (object)q.Value);
        }



    }


}
