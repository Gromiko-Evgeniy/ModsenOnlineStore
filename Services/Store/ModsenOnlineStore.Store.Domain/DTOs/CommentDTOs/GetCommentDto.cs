﻿namespace ModsenOnlineStore.Store.Domain.DTOs.CommentDTOs
{
    public class GetCommentDTO
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public int ProductId { get; set; }

        public int UserId { get; set; }
    }
}
