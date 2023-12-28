using Demo_Detai12.Model;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using Neo4jClient;

namespace Demo_Detai12.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LienKetvaTruyVanController : Controller
    {
        private readonly IGraphClient _client;
        public LienKetvaTruyVanController(IGraphClient client)
        {
            _client = client;
        }

        [HttpGet]
        [Route("/api/([controller])/get-relationship-Congdan&Baohiem")]
        public async Task<IActionResult> GetrelationshipCongdan(string Macc, string Mabh)
        {
            await _client.Cypher.Match("(a: Congdan), (b: BaoHiiem)")
                                       .Where((Congdan a, Baohiem b) => a.Macc == Macc && b.Mabh == Mabh)
                                       .Create("(a) - [r: GRANT] -> (b)")
                                       .ExecuteWithoutResultsAsync();
            return Ok();
        }

        [HttpGet]
        [Route("/api/([controller])/get-demtbaohiem-theoten")]
        public async Task<IActionResult> Getsoluong(string Tenbh)
        {
            var result = await _client.Cypher.Match("(bh:Baohiem)")
                                                .Where((Baohiem bh) => bh.Tenbh == Tenbh)
                                                .Return(bh => bh.Count()).ResultsAsync;
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/([controller])/get-Nhapso-hienthihotencongdan")]
        public async Task<IActionResult> GetByBaso(string Baso)
        {
            var congdan = await _client.Cypher.Match("(cd: Congdan)")
                                                  .Where((Congdan cd) => cd.Sodienthoai.EndsWith(Baso))
                                                  .Return(cd => cd.As<Congdan>().Hoten).ResultsAsync;

            return Ok(congdan.LastOrDefault());
        }

    
        [HttpGet]
        [Route("/api/([controller])/get-Hotenbatdaubangchucai")]
        public async Task<IActionResult> GetByHoTen(string Hoten)
        {
            var congdan = await _client.Cypher.Match("(cd: Congdan)")
                                                  .Where((Congdan cd) => cd.Hoten.StartsWith(Hoten))
                                                  .Return(cd => cd.As<Congdan>().Hoten).ResultsAsync;

            return Ok(congdan.LastOrDefault());
        }

        [HttpGet]
        [Route("/api/([controller])/get-Congdan-GRANT-BaoHiem")]
        public async Task<IActionResult> GetCongDanWithBaoHiem(string Tenbh)
        {
            var Congdan = await _client.Cypher.Match("(a:Congdan)-[:GRANT]->(b:Baohiem)")
                                              .Where((Congdan a, Baohiem b) => b.Tenbh == Tenbh)
                                              .Return(a => a.As<Congdan>().Hoten).ResultsAsync;

            return Ok(Congdan);

        }

    }
}
