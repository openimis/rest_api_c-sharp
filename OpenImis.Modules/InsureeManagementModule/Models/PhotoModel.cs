using OpenImis.DB.SqlServer;
using OpenImis.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.InsureeManagementModule.Models
{
	public class PhotoModel
	{
		/// <summary>
		/// The id of the Photo from the DB
		/// </summary>
		public int PhotoId { get; set; }

		/// <summary>
		/// The associated Insuree
		/// </summary>
		public InsureeModel Insuree { get; set; }

		/// <summary>
		/// The Insuree's security number used to match a photo with an Insuree if the photo is not sent with the insuree
		/// </summary>
		public string CHFID { get; set; }

		/// <summary>
		/// The photo folder
		/// </summary>
		public string PhotoFolder { get; set; }

		/// <summary>
		/// The photo filename 
		/// </summary>
		public string PhotoFileName { get; set; }

		/// <summary>
		/// The officer that took the photo
		/// </summary>
		/// TODO: update the id with the Officer type 
		public int OfficerId { get; set; }

		/// <summary>
		/// The date when the photo was taken 
		/// </summary>
		public DateTime PhotoDate { get; set; }

		/// <summary>
		/// The validation start date 
		/// </summary>
		public DateTime ValidFrom { get; set; }

		/// <summary>
		/// The validation end date 
		/// </summary>
		public DateTime ValidTo { get; set; }

		public static PhotoModel FromTblPhoto(TblPhotos tblPhoto)
		{
			if (tblPhoto == null)
			{
				return null;
			}

			PhotoModel photoModel = new PhotoModel()
			{
				PhotoId = tblPhoto.PhotoId,
				Insuree = InsureeModel.FromTblInsuree(tblPhoto.TblInsuree.FirstOrDefault()),
				CHFID = tblPhoto.Chfid,
				PhotoFolder = tblPhoto.PhotoFolder,
				PhotoFileName = tblPhoto.PhotoFileName,
				OfficerId = tblPhoto.OfficerId,
				PhotoDate = tblPhoto.PhotoDate,
				ValidFrom = tblPhoto.ValidityFrom,
				ValidTo = TypeCast.GetValue<DateTime>(tblPhoto.ValidityTo)
			};
			return photoModel;
		}

		public TblPhotos ToTblPhoto()
		{
			TblPhotos tblPhoto = new TblPhotos()
			{
				PhotoId = this.PhotoId,
				Chfid = this.CHFID,
				PhotoFolder = this.PhotoFolder,
				PhotoFileName = this.PhotoFileName,
				OfficerId = this.OfficerId,
				PhotoDate = this.PhotoDate,
				ValidityFrom = this.ValidFrom,
				ValidityTo = this.ValidTo
			};
			tblPhoto.TblInsuree.Add(this.Insuree.ToTblInsuree());
			return tblPhoto;
		}

	}
}
