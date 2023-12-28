using Demo_Detai12.Model;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace Demo_Detai12.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CongdanController : Controller
    {
        private readonly IGraphClient _client;
        public CongdanController(IGraphClient client)
        {
            _client = client;
        }

        [HttpGet]
        [Route("/api/([controller])/get-all")]
        public async Task<IActionResult> Get()
        {
            var congdan = await _client.Cypher.Match("(cd: Congdan)")
                                                  .Return(cd => cd.As<Congdan>()).ResultsAsync;

            return Ok(congdan);
        }

        [HttpGet]
        [Route("/api/([controller])/get-Macc")]
        public async Task<IActionResult> GetById(string Macc)
        {
            var congdan = await _client.Cypher.Match("(cd: Congdan)")
                                                  .Where((Congdan cd) => cd.Macc == Macc)
                                                  .Return(cd => cd.As<Congdan>()).ResultsAsync;

            return Ok(congdan.LastOrDefault());
        }

        [HttpPost]
        [Route("/api/([controller])/get-insert")]
        public async Task<IActionResult> GetCreate([FromBody] Congdan congdan)
        {
            await _client.Cypher.Create("(cd: Congdan $congdan)")
                                .WithParam("congdan", congdan)
                                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpPut]
        [Route("/api/([controller])/get-update-Macc")]
        public async Task<IActionResult> GetUpdateById(string Macc, Congdan congdan)
        {
            await _client.Cypher.Match("(cd: Congdan)")
                                      .Where((Congdan cd) => cd.Macc == Macc)
                                      .Set("cd = $congdan")
                                      .WithParam("congdan", congdan)
                                      .ExecuteWithoutResultsAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("/api/([controller])/get-delete-Macc")]
        public async Task<IActionResult> GetDeleteId(string Macc)
        {
            var congdan = await _client.Cypher.Match("(cd: Congdan)")
                                                  .Where((Congdan cd) => cd.Macc == Macc)
                                                  .Delete("cd")
                                                  .Return(cd => cd.As<Congdan>()).ResultsAsync;
            return Ok();
        }
        
    }
}
