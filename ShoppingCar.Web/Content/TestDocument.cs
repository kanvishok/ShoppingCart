using System;
using System.Collections.Generic;
using System.Text;

namespace Awarious.Core.Repository.Mongo.Test
{
	public class TestDocument : IMongoDocument
	{
		public Guid TestDocumentId { get; set; }
		public string FirstName { get; set; }
		public int Age { get; set; }
		public string Designation { get; set; }
		public DateTime InsertedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public DateTime? DeletedDate { get; set; }
		public Guid? RequestedBy { get; set; }
		public Guid? RequestorTenantId { get; set; }
		public int ProductId { get; set; }
	}
}
