namespace P2.Platform.Api.Resources
{
    using AutoMapper;
    using ContactManagement;
    using Core;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        public class ContactFilter : CollectionFilter
        {
            // contact tag = home, mobile, work
            public string[] Tag { get; set; }

        }

        private readonly IContactService service;
        private readonly ILogger logger;

        static ContactsController()
        {
            // intialize mappings for all contact related dtos
            Mapper.Initialize(config =>
            {
                // initialize mapping for contact
                config.CreateMap<Contact, ContactManagement.Dto.Contact>();

                // ignore CreatedDate and Id when map between Contact
                // option: move to mapping config file
                config.CreateMap<ContactManagement.Dto.Contact, Contact>()
                        .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

                // initialize mapping for address
                config.CreateMap<Address, ContactManagement.Dto.Address>()
                        .ForSourceMember(source => source.ContactId, opt => opt.Ignore())
                        .ForSourceMember(source => source.Contact, opt => opt.Ignore());
            });
        }

        public ContactsController(IContactService service, ILogger<ContactsController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        #region Contacts
        // POST /contacts
        [Route("", Name = "CreateContact")]
        [HttpPost]
        public IActionResult CreateContact([FromBody]ContactManagement.Dto.Contact contact)
        {
            if (!this.ModelState.IsValid)
            {
                this.logger.LogInformation("Invalid Contact Format");
                return BadRequest(this.ModelState);
            }

            try
            {
                var contactToAdd = Mapper.Map<Contact>(contact);

                // initialize default
                contactToAdd.Id = 0;
                contactToAdd.CreatedDate = DateTime.UtcNow;
                contactToAdd.IsActive = true;

                this.service.AddContact(contactToAdd);

                if (contactToAdd.Id <= 0)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }

                // convert back to dto before sending a response
                var result = Mapper.Map<ContactManagement.Dto.Contact>(contactToAdd);

                // return 201 with new resource location in header
                return CreatedAtAction("GetContact", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        // GET /contacts/1
        [Route("{id}", Name = "GetContact")]
        [HttpGet]
        public IActionResult GetContact(int id)
        {
            // invalid request
            if (id <= 0)
            {
                return BadRequest();
            }

            try
            {
                // get contact from service
                var contact = this.service.GetContactById(id);

                // contact not found
                if (contact == null)
                {
                    return NotFound();
                }

                // convert to dto
                var result = Mapper.Map<ContactManagement.Dto.Contact>(contact);

                return Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        // PUT /contacts/1
        [Route("{id}")]
        [HttpPut]
        public IActionResult UpdateContact(int id, [FromBody]ContactManagement.Dto.Contact contact)
        {
            if (id <= 0 || !this.ModelState.IsValid)
            {
                return BadRequest();
            }
            
            try
            {
                // make sure contact exists
                var found = this.service.GetContactById(id);

                if(found == null)
                {
                    return NotFound();
                }

                // update propoerties to existing contact
                Mapper.Map<ContactManagement.Dto.Contact, Contact>(contact, found);

                this.service.UpdateContact(found);

                return Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        // GET /contacts?keywords=something&orderBy=firstName desc&type=home&top=10&skip=0
        [Route("", Name = "GetContacts")]
        [HttpGet]
        public IActionResult GetContacts([FromQuery]ContactFilter filter)
        {
            int count = 0;
            int total = 0;
            int skip = filter.Skip ?? 0;
            int top = filter.Top ?? 50;

            // default sorting
            string sort = "CreatedDate";
            bool ascending = false;

            // ignore type if equals 'all'
            if (filter.Tag != null && filter.Tag.Count() > 0)
            {
                if (filter.Tag.Where(a => String.IsNullOrEmpty(a) || a.ToLower() == "all").Any())
                    filter.Tag = new string[] { };
            }

            // make sure orderby has valid format
            if (!String.IsNullOrEmpty(filter.OrderBy))
            {
                if (!filter.ParseOrderBy(out sort, out ascending))
                {
                    return BadRequest("Invalid orderBy format.");
                }
            }

            try
            {
                // get results
                var contacts = this.service.GetContacts(filter.Tag, filter.Keywords,
                    out count, out total, sort, ascending, skip, top);

                // convert to dtos
                var result = Mapper.Map<IList<ContactManagement.Dto.Contact>>(contacts);

                // return results
                return Ok(new CollectionResult<ContactManagement.Dto.Contact>
                {
                    Skip = skip,
                    Top = top,
                    Count = count,
                    Total = total,
                    Results = result
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }

            // return empty collection
            return Ok(new NullCollectionResult<ContactManagement.Dto.Contact>());
        }

        #endregion

        #region Addresses

        [Route("{id}/addresses")]
        [HttpGet]
        public IActionResult GetAddresses(int id)
        {
            // invalid request
            if (id <= 0)
            {
                return BadRequest();
            }

            try
            {
                // get addresses
                var addresses = this.service.GetAddressesByContact(id);

                // convert to dto
                var result = Mapper.Map<IList<ContactManagement.Dto.Address>>(addresses);

                return Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }

            return Ok(new List<ContactManagement.Dto.Address>());
        }

        #endregion
    }
}
