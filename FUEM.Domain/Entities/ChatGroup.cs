using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Entities
{
    public class ChatGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Name { get; set; } = null!;
        public int CreatorId { get; set; } // Organizer (Admin/Club)
        public int EventId { get; set; }  // Associated event ID
        public bool IsHidden { get; set; } = false;

        public virtual Organizer Creator { get; set; } = null!;
        public virtual Event Event { get; set; } = null!;
        public virtual ICollection<ChatGroupMember> Members { get; set; } = new List<ChatGroupMember>();
    }

    public class ChatGroupMember
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string? GroupId { get; set; }
        public int StudentId { get; set; } // Student Collaborator

        public virtual Student? Student { get; set; } = null!;
        public virtual ChatGroup Group { get; set; } = null!;
    }

    public class ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string? GroupId { get; set; }

        public int? SenderStudentId { get; set; }  // Nullable FK to Student
        public int? SenderOrganizerId { get; set; }  // Nullable FK to Organizer

        public string Content { get; set; } = null!;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ChatGroup Group { get; set; } = null!;
        public virtual Student? SenderStudent { get; set; }
        public virtual Organizer? SenderOrganizer { get; set; }
    }
}
