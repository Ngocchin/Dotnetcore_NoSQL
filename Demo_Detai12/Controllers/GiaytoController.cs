using Demo_Detai12.Model;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace Demo_Detai12.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GiaytoController : Controller
    {
        private readonly IGraphClient _client;
        public GiaytoController(IGraphClient client)
        {
            _client = client;
        }

        [HttpGet]
        [Route("/api/([controller])/get-all")]
        public async Task<IActionResult> Get()
        {
            var giayto = await _client.Cypher.Match("(gt: Giayto)")
                                                  .Return(gt => gt.As<Giayto>()).ResultsAsync;

            return Ok(giayto);
        }

        [HttpGet]
        [Route("/api/([controller])/get-Mabh")]
        public async Task<IActionResult> GetById(string Magt)
        {
            var giayto = await _client.Cypher.Match("(gt: Giayto)")
                                                  .Where((Giayto gt) => gt.Magt == Magt)
                                                  .Return(gt => gt.As<Giayto>()).ResultsAsync;

            return Ok(giayto.LastOrDefault());
        }

        [HttpPost]
        [Route("/api/([controller])/get-insert")]
        public async Task<IActionResult> GetCreate([FromBody] Giayto giayto)
        {
            await _client.Cypher.Create("(gt: Giayto $giayto)")
                                .WithParam("giayto", giayto)
                                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpPut]
        [Route("/api/([controller])/get-update-Magt")]
        public async Task<IActionResult> GetUpdateById(string Magt, Giayto giayto)
        {
            await _client.Cypher.Match("(gt: Giayto)")
                                      .Where((Giayto gt) => gt.Magt == Magt)
                                      .Set("gt = $giayto")
                                      .WithParam("giayto", giayto)
                                      .ExecuteWithoutResultsAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("/api/([controller])/get-delete-Magt")]
        public async Task<IActionResult> GetDeleteId(string Magt)
        {
            var giayto = await _client.Cypher.Match("(gt: Giayto)")
                                                  .Where((Giayto gt) => gt.Magt == Magt)
                                                  .Delete("gt")
                                                  .Return(gt => gt.As<Giayto>()).ResultsAsync;
            return Ok();
        }
    }
}
