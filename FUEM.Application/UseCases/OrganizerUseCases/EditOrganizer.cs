using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.OrganizerUseCases
{
    public class EditOrganizer : IEditOrganizer
    {
        private readonly IOrganizerRepository _repository;
        private readonly FirebaseStorageService _firebase;

        public EditOrganizer(IOrganizerRepository repository, FirebaseStorageService firebase)
        {
            _repository = repository;
            _firebase = firebase;
        }

        public async Task EditOrganizerAsync(Organizer organizer, Stream? avatarStream, Stream? coverStream)
        {
            if (avatarStream != null)
            {
                string avatarLink = await _firebase.UploadFileAsync(FileType.Image, avatarStream, organizer.AvatarPath);
                organizer.AvatarPath = await _firebase.GetSignedFileUrlAsync(avatarLink);
            }
            if (coverStream != null)
            {
                string coverLink = await _firebase.UploadFileAsync(FileType.Image, coverStream, organizer.CoverPath);
                organizer.CoverPath = await _firebase.GetSignedFileUrlAsync(coverLink);
            }
            await _repository.EditAsync(organizer);
        }
    }
}
