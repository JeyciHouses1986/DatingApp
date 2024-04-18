using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessagesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public MessagesController(IMapper mapper, IUnitOfWork uow )
    {
        _mapper = mapper;
        _uow = uow;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var userName = User.GetUserName();

        if (userName == createMessageDto.RecipientUserName)
            return BadRequest("You cannot send a message to yourself");

        var sender = await _uow.UserRepository.GetUserByUsernameAsync(userName);
        var recipient = await _uow.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUserName);

        if (recipient == null) return NotFound();

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };

        _uow.MessageRepository.AddMessage(message);

        if (await _uow.Complete()) return Ok(_mapper.Map<MessageDto>(message));

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUserName();
        var messages = await _uow.MessageRepository.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

        return messages;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var userName = User.GetUserName();
        var message = await _uow.MessageRepository.GetMessage(id);
        if (message.SenderUsername != userName && message.RecipientUsername != userName)
            return Unauthorized();

        if (message.SenderUsername == userName) message.SenderDeleted = true;
        if (message.RecipientUsername == userName) message.RecipientDeleted = true;

        if (message.SenderDeleted && message.RecipientDeleted)
        {
            _uow.MessageRepository.DeleteMessage(message);
        }
        if (await _uow.Complete()) return Ok();
        return BadRequest("Problem deleting message");
    }
}
