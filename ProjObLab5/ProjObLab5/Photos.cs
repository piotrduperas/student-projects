//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using TravelAgencies.DataAccess;

namespace TravelAgencies.Agencies
{
	public interface IPhoto
	{
		string Name { get; }
		int WidthPx { get; }
		int HeightPx { get; }
	}

	class Photo : IPhoto
	{
		public string Name { get; }
		public int WidthPx { get; }
		public int HeightPx { get; }

		public Photo(string Name, int WidthPx, int HeightPx) 
		{
			this.Name = Name;
			this.WidthPx = WidthPx;
			this.HeightPx = HeightPx;
		}

		public static Photo FromPhotMetadata(PhotMetadata p)
		{
			return new Photo(p.Name, int.Parse(p.WidthPx), int.Parse(p.HeightPx));
		}
	}

	abstract class PhotoDecorator : IPhoto
	{
		protected IPhoto photo;
		public PhotoDecorator(IPhoto photo)
		{
			this.photo = photo;
		}
		public virtual string Name
		{
			get => photo.Name;
		}
		public virtual int WidthPx
		{
			get => photo.WidthPx;
		}
		public virtual int HeightPx
		{
			get => photo.HeightPx;
		}
	}
}
