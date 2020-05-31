//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas

namespace TravelAgencies.Agencies
{
	public interface IReview
	{
		string Content { get; }
		string UserName { get; }
	}

	class Review : IReview
	{
		public string UserName { get; }
		public string Content { get; }

		public Review(string UserName, string Content)
		{
			this.UserName = UserName;
			this.Content = Content;
		}
	}

	abstract class ReviewDecorator : IReview
	{
		protected IReview review;
		public ReviewDecorator(IReview review)
		{
			this.review = review;
		}
		public virtual string UserName
		{
			get => review.UserName;
		}
		public virtual string Content
		{
			get => review.Content;
		}
	}
}
