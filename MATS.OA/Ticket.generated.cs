#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using MATS.OA;


namespace MATS.OA	
{
	public partial class Ticket
	{
		private int _id;
		public virtual int Id 
		{ 
		    get
		    {
		        return this._id;
		    }
		    set
		    {
		        this._id = value;
		    }
		}
		
		private string _passengerPassportNumber;
		public virtual string PassengerPassportNumber 
		{ 
		    get
		    {
		        return this._passengerPassportNumber;
		    }
		    set
		    {
		        this._passengerPassportNumber = value;
		    }
		}
		
		private string _passengerFullName;
		public virtual string PassengerFullName 
		{ 
		    get
		    {
		        return this._passengerFullName;
		    }
		    set
		    {
		        this._passengerFullName = value;
		    }
		}
		
		private string _city;
		public virtual string City 
		{ 
		    get
		    {
		        return this._city;
		    }
		    set
		    {
		        this._city = value;
		    }
		}
		
		private string _route;
		public virtual string Route 
		{ 
		    get
		    {
		        return this._route;
		    }
		    set
		    {
		        this._route = value;
		    }
		}
		
		private string _airLines;
		public virtual string AirLines 
		{ 
		    get
		    {
		        return this._airLines;
		    }
		    set
		    {
		        this._airLines = value;
		    }
		}
		
		private double _amount;
		public virtual double Amount 
		{ 
		    get
		    {
		        return this._amount;
		    }
		    set
		    {
		        this._amount = value;
		    }
		}
		
		private DateTime _requestedDate;
		public virtual DateTime RequestedDate 
		{ 
		    get
		    {
		        return this._requestedDate;
		    }
		    set
		    {
		        this._requestedDate = value;
		    }
		}
		
		private int _typeOfTrip;
		public virtual int TypeOfTrip 
		{ 
		    get
		    {
		        return this._typeOfTrip;
		    }
		    set
		    {
		        this._typeOfTrip = value;
		    }
		}
		
		private string _ticketNumber;
		public virtual string TicketNumber 
		{ 
		    get
		    {
		        return this._ticketNumber;
		    }
		    set
		    {
		        this._ticketNumber = value;
		    }
		}
		
		private DateTime? _checkInDate;
		public virtual DateTime? CheckInDate 
		{ 
		    get
		    {
		        return this._checkInDate;
		    }
		    set
		    {
		        this._checkInDate = value;
		    }
		}
		
		private DateTime? _flightDate;
		public virtual DateTime? FlightDate 
		{ 
		    get
		    {
		        return this._flightDate;
		    }
		    set
		    {
		        this._flightDate = value;
		    }
		}
		
		private float? _commisionPercent;
		public virtual float? CommisionPercent 
		{ 
		    get
		    {
		        return this._commisionPercent;
		    }
		    set
		    {
		        this._commisionPercent = value;
		    }
		}
		
		private decimal? _flightCost;
		public virtual decimal? FlightCost 
		{ 
		    get
		    {
		        return this._flightCost;
		    }
		    set
		    {
		        this._flightCost = value;
		    }
		}
		
		private int _clientId;
		public virtual int ClientId 
		{ 
		    get
		    {
		        return this._clientId;
		    }
		    set
		    {
		        this._clientId = value;
		    }
		}
		
		private int _localStatus;
		public virtual int LocalStatus 
		{ 
		    get
		    {
		        return this._localStatus;
		    }
		    set
		    {
		        this._localStatus = value;
		    }
		}
		
		private DateTime? _localStatusDate;
		public virtual DateTime? LocalStatusDate 
		{ 
		    get
		    {
		        return this._localStatusDate;
		    }
		    set
		    {
		        this._localStatusDate = value;
		    }
		}
		
		private int _serverStatus;
		public virtual int ServerStatus 
		{ 
		    get
		    {
		        return this._serverStatus;
		    }
		    set
		    {
		        this._serverStatus = value;
		    }
		}
		
		private DateTime? _serverStatusDate;
		public virtual DateTime? ServerStatusDate 
		{ 
		    get
		    {
		        return this._serverStatusDate;
		    }
		    set
		    {
		        this._serverStatusDate = value;
		    }
		}
		
		private int _enabled;
		public virtual int Enabled 
		{ 
		    get
		    {
		        return this._enabled;
		    }
		    set
		    {
		        this._enabled = value;
		    }
		}
		
		private int? _createdByUserId;
		public virtual int? CreatedByUserId 
		{ 
		    get
		    {
		        return this._createdByUserId;
		    }
		    set
		    {
		        this._createdByUserId = value;
		    }
		}
		
		private DateTime? _dateRecordCreated;
		public virtual DateTime? DateRecordCreated 
		{ 
		    get
		    {
		        return this._dateRecordCreated;
		    }
		    set
		    {
		        this._dateRecordCreated = value;
		    }
		}
		
		private int? _modifiedByUserId;
		public virtual int? ModifiedByUserId 
		{ 
		    get
		    {
		        return this._modifiedByUserId;
		    }
		    set
		    {
		        this._modifiedByUserId = value;
		    }
		}
		
		private DateTime? _dateLastModified;
		public virtual DateTime? DateLastModified 
		{ 
		    get
		    {
		        return this._dateLastModified;
		    }
		    set
		    {
		        this._dateLastModified = value;
		    }
		}
		
		private bool _serverPost;
		public virtual bool ServerPost 
		{ 
		    get
		    {
		        return this._serverPost;
		    }
		    set
		    {
		        this._serverPost = value;
		    }
		}
		
		private bool _localPost;
		public virtual bool LocalPost 
		{ 
		    get
		    {
		        return this._localPost;
		    }
		    set
		    {
		        this._localPost = value;
		    }
		}
		
		private string _rOWGUID;
		public virtual string ROWGUID 
		{ 
		    get
		    {
		        return this._rOWGUID;
		    }
		    set
		    {
		        this._rOWGUID = value;
		    }
		}
		
		private Client _client;
		public virtual Client Client 
		{ 
		    get
		    {
		        return this._client;
		    }
		    set
		    {
		        this._client = value;
		    }
		}
		
		private IList<Remark> _remarks = new List<Remark>();
		public virtual IList<Remark> Remarks 
		{ 
		    get
		    {
		        return this._remarks;
		    }
		}
		
		private IList<PaymentDTO> _paymentDTOs = new List<PaymentDTO>();
		public virtual IList<PaymentDTO> PaymentDTOs 
		{ 
		    get
		    {
		        return this._paymentDTOs;
		    }
		}
		
		private IList<Attachment> _attachments = new List<Attachment>();
		public virtual IList<Attachment> Attachments 
		{ 
		    get
		    {
		        return this._attachments;
		    }
		}
		
	}
}
