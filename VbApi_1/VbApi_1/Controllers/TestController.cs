using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    // GET: api/TestController.cs
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET: api/TestController.cs/5
    [HttpGet("{id}", Name = "Get")]
    public string Get(int id)
    {
        return "value";
    }

    // POST: api/TestController.cs
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT: api/TestController.cs/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE: api/TestController.cs/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
    
    
    
    [HttpGet("GetIdByQuery")] //nulable
    public string GetIdByQuery([FromQuery] int? id)
    {
        return $"{id}";
    }
    [HttpGet("GetIdByRoute/{id}")] //nulable de�il
    public string GetIdByRoute(int? id)
    {
        return $"{id}";
    }
     //?id=1&lat=2&lng=3 // 1-2-3
    [HttpGet("GetIdByQueryParameter")] // default 0 0 0 
    public string GetIdByQueryParameter([FromQuery] int? id,int? lat,int? lng)
    {
        return $"{id}-{lat}-{lng}";
    }
    // 11/22/33
    [HttpGet("GetIdByRoutePatameter/{id}/{lat}/{lng}")] 
    public string GetIdByRoutePatameter(int? id,int? lat,int? lng)
    {
        return $"{id}-{lat}-{lng}";
    }
    
    [HttpGet("GetIdByRouteAndQueryPatameter/{id}/{lat}/{lng}")]
    public string GetIdByRouteAndQueryPatameter([FromRoute]int? id,int? lat,int? lng,[FromQuery] string? name,string? lastname)
    {
        return $"{id}-{lat}-{lng}-{name}-{lastname}";
    }
}