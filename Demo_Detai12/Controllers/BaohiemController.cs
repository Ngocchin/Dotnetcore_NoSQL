using Demo_Detai12.Model;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using System;
using System.Diagnostics.Metrics;

namespace Demo_Detai12.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaohiemController : Controller
    {
        private readonly IGraphClient _client;
        
        public BaohiemController(IGraphClient client)
        {
            _client = client;
        }
        [HttpGet]
        [Route("/api/([controller])/get-all")]
        public async Task<IActionResult> Get()
        {
            var baohiem = await _client.Cypher.Match("(bh: Baohiem)")
                                                  .Return(bh => bh.As<Baohiem>()).ResultsAsync;

            return Ok(baohiem);
        }

        [HttpGet]
        [Route("/api/([controller])/get-Mabh")]
        public async Task<IActionResult> GetById(string Mabh)
        {
            var baohiem = await _client.Cypher.Match("(bh: Baohiem)")
                                                  .Where((Baohiem bh) => bh.Mabh == Mabh)
                                                  .Return(bh => bh.As<Baohiem>()).ResultsAsync;

            return Ok(baohiem.LastOrDefault());
        }

        [HttpPost]
        [Route("/api/([controller])/get-insert")]
        public async Task<IActionResult> GetCreate([FromBody] Baohiem baohiemn)
        {
            await _client.Cypher.Create("(bh: Baohiem $baohiem)")
                                .WithParam("baohiem", baohiemn)
                                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpPut]
        [Route("/api/([controller])/get-update-Mabh")]
        public async Task<IActionResult> GetUpdateById(string Mabh, Baohiem baohiem)
        {
            await _client.Cypher.Match("(bh: Baohiem)")
                                      .Where((Baohiem bh) => bh.Mabh == Mabh)
                                      .Set("bh = $baohiem")
                                      .WithParam("baohiem", baohiem)
                                      .ExecuteWithoutResultsAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("/api/([controller])/get-delete-Mabh")]
        public async Task<IActionResult> GetDeleteId(string Mabh)
        {
            var baohiem = await _client.Cypher.Match("(bh: Baohiem)")
                                                  .Where((Baohiem bh) => bh.Mabh == Mabh)
                                                  .Delete("bh")
                                                  .Return(bh => bh.As<Baohiem>()).ResultsAsync;
            return Ok(Get());
        }

    }
}
