using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Chatrum.Models {
	public class MessagesViewModel {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
		public string PostBy { get; set; }
		[Required]
		public string Message { get; set; }
	}
}
