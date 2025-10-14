using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DCandidateController : ControllerBase
    {
        private readonly DonationDBContext _context;

        public DCandidateController(DonationDBContext context)
        {
            _context = context;
        }

        //public DCandidateController()
        //{
        //}

        // GET: api/DCandidate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DCandidate>>> GetDCandidates()
        {
            return await _context.DCandidates.ToListAsync();
        }

        // GET: api/DCandidate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DCandidate>> GetDCandidate(int id)
        {
            var dCandidate = await _context.DCandidates.FindAsync(id);

            if (dCandidate == null)
            {
                return NotFound();
            }

            return dCandidate;
        }

        // PUT: api/DCandidate/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDCandidate(int id, DCandidate dCandidate)
        {
            dCandidate.id = id;

            _context.Entry(dCandidate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DCandidateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DCandidate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DCandidate>> PostDCandidate(DCandidate dCandidate)
        {
            string xmlString = dCandidate.emailtext;
            XmlDocument xmlDoc = new XmlDocument();
            if (IsValidXml(dCandidate.emailtext))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNodeList parentNode = doc.GetElementsByTagName("total");
                if (parentNode.Count != 0)
                {
                    foreach (XmlNode childrenNode in parentNode)
                    {
                        dCandidate.total = childrenNode.InnerText;
                    }
                }
            }
            else
            {
                dCandidate.total = "Rejected";
            }
            _context.DCandidates.Add(dCandidate);
            await _context.SaveChangesAsync();
           

            return CreatedAtAction("GetDCandidate", new { id = dCandidate.id, total= dCandidate.total }, dCandidate);
        }
        public static bool IsValidXml(string xmlString)
        {
            try
            {
                // Attempt to parse the XML string
                XDocument.Parse(xmlString);
                return true; // If parsing succeeds, it's valid XML
            }
            catch (XmlException ex)
            {
                // Catch specific XML parsing errors
                Console.WriteLine($"XML parsing error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors during parsing
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }
        // DELETE: api/DCandidate/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DCandidate>> DeleteDCandidate(int id)
        {
            var dCandidate = await _context.DCandidates.FindAsync(id);
            if (dCandidate == null)
            {
                return NotFound();
            }

            _context.DCandidates.Remove(dCandidate);
            await _context.SaveChangesAsync();

            return dCandidate;
        }

        private bool DCandidateExists(int id)
        {
            return _context.DCandidates.Any(e => e.id == id);
        }
    }
}
