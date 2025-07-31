using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.EventUseCases
{
    internal class CreateEvent : ICreateEvent
    {
        private readonly IEventRepository _repository;
        private readonly FirebaseStorageService _firebase;
        private readonly IEventImageRepository _imageRepository;

        public CreateEvent(IEventRepository repository, IEventImageRepository imageRepository, FirebaseStorageService firebase)
        {
            _repository = repository;
            _firebase = firebase;
            _imageRepository = imageRepository;
        }

        public async Task ExecuteAsync(Event createEvent, Role role, Stream avatar, List<EventImage> additionalImages)
        {
            //await _repository.
            if (role.Equals(Role.Admin))
            {
                createEvent.Status = EventStatus.APPROVED;
            } 
            else if (role.Equals(Role.Club))
            {
                createEvent.Status = EventStatus.PENDING;
            } 
            else
            {
                throw new ArgumentException("You don't have permission to create event!");
            }
            string avatarPath = await _firebase.UploadFileAsync(FileType.Image, avatar, createEvent.AvatarPath);
            createEvent.AvatarPath = await _firebase.GetSignedFileUrlAsync(avatarPath);
            Event newEvent = await _repository.AddAsync(createEvent);

            foreach (EventImage image in additionalImages)
            {
                string path = await _firebase.UploadFileAsync(FileType.Image, image.Stream, image.Path);
                image.Path = await _firebase.GetSignedFileUrlAsync(path);
                image.EventId = newEvent.Id;
            }
            await _imageRepository.AddImagesAsync(additionalImages);
        }
    }
}
