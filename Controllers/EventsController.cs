using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GoogleCalendarAPI.Services;
using GoogleCalendarAPI.Interfaces;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarAPI;
using GoogleCalendarAPI.DTOs;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace GoogleCalendarAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {


        //private IGoogleAuthProvider _auth;
        private string _calendarID = "primary";
        private CalendarService _service;


        public EventsController(ICalendarServiceGen service)
        {
            _service = service.GenerateCalendarService();
        }


        [HttpPost("/api/events")]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] GoogleCalendarEventDTO eventDTO)
        {

            try
            {
                List<EventAttachment> eventAttachments = new();
                if(eventDTO.AttachmentURLs.Count > 0)
                {
                    for(int i = 0; i < eventDTO.AttachmentURLs.Count; i++) 
                    {
                        eventAttachments.Add(new EventAttachment() 
                            { 
                            FileUrl = eventDTO.AttachmentURLs[i],
                            }
                        );
                    }
                }
                Event newEvent = new()
                {
                    Summary = eventDTO.Summary,
                    Description = eventDTO.Description,
                    Start = new EventDateTime()
                    {

                        DateTime = eventDTO.StartTime,
                        

                    },
                    End = new EventDateTime()
                    {
                        DateTime = eventDTO.EndTime,
                        
                    },
                    
                    Attachments = eventAttachments.Count>0 ? eventAttachments : null,
                    
                    

                };
                // Use Google Calendar API to create event
                EventsResource.InsertRequest request = _service.Events.Insert(newEvent, _calendarID);
                request.SupportsAttachments = true;
                Event createdEvent = await request.ExecuteAsync();

                // Return the created event details
                return Created("api/events/{eventId}", _service.SerializeObject(createdEvent));

            }
            catch(Exception ex) 
            {
                return BadRequest(ex.ToString());
            }
        }


        [HttpDelete("/api/events/{eventId}")]
        public async Task<ActionResult> DeleteEvent([FromRoute] string eventId)
        {
            try
            {
                // Use Google Calendar API to delete event
                EventsResource.DeleteRequest request = _service.Events.Delete(_calendarID, eventId);
                var result = await request.ExecuteAsync();

                // Return 204 No Content if deletion is successful
                return NoContent();
            }
            catch (Exception ex)
            {
                // Handle API call errors
                return BadRequest(ex.ToString());
            }
        }


        [HttpGet("/api/events")]
        public async Task<ActionResult<Events>> GetEvents([FromQuery]string? searchString = "")
        {
            // Use Google Calendar API to list events
            EventsResource.ListRequest request = _service.Events.List(_calendarID);
            string? pageToken = null;
            Events events;
            List <IList<Event>>  eventsList = new();
            do
            {
                request.Q = searchString;
                request.PageToken = pageToken;
                request.MaxResults = 10;
                events =  request.Execute();
                pageToken = events.NextPageToken;
                eventsList.Add(events.Items);
            } while (pageToken != null);

            var jsonRes = _service.SerializeObject(eventsList);
            // Return list of events
            return Ok(jsonRes);
            
        }
    }
}

