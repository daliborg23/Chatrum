using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Chatrum.Models {
	public class MessageViewModel {
		public string Id { get; set; }
		public string PostBy { get; set; }
		[Required]
		public string Message { get; set; }
	}
}
