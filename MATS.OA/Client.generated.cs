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
	public partial class Client
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
		
		private string _clientCode;
		public virtual string ClientCode 
		{ 
		    get
		    {
		        return this._clientCode;
		    }
		    set
		    {
		        this._clientCode = value;
		    }
		}
		
		private string _displayName;
		public virtual string DisplayName 
		{ 
		    get
		    {
		        return this._displayName;
		    }
		    set
		    {
		        this._displayName = value;
		    }
		}
		
		private string _contactName;
		public virtual string ContactName 
		{ 
		    get
		    {
		        return this._contactName;
		    }
		    set
		    {
		        this._contactName = value;
		    }
		}
		
		private string _contactTitle;
		public virtual string ContactTitle 
		{ 
		    get
		    {
		        return this._contactTitle;
		    }
		    set
		    {
		        this._contactTitle = value;
		    }
		}
		
		private decimal? _outStandingDeposit;
		public virtual decimal? OutStandingDeposit 
		{ 
		    get
		    {
		        return this._outStandingDeposit;
		    }
		    set
		    {
		        this._outStandingDeposit = value;
		    }
		}
		
		private int _clientStatus;
		public virtual int ClientStatus 
		{ 
		    get
		    {
		        return this._clientStatus;
		    }
		    set
		    {
		        this._clientStatus = value;
		    }
		}
		
		private string _productKey;
		public virtual string ProductKey 
		{ 
		    get
		    {
		        return this._productKey;
		    }
		    set
		    {
		        this._productKey = value;
		    }
		}
		
		private int _expiryDuration;
		public virtual int ExpiryDuration 
		{ 
		    get
		    {
		        return this._expiryDuration;
		    }
		    set
		    {
		        this._expiryDuration = value;
		    }
		}
		
		private string _bIOS_SN;
		public virtual string BIOS_SN 
		{ 
		    get
		    {
		        return this._bIOS_SN;
		    }
		    set
		    {
		        this._bIOS_SN = value;
		    }
		}
		
		private int _noOfActivations;
		public virtual int NoOfActivations 
		{ 
		    get
		    {
		        return this._noOfActivations;
		    }
		    set
		    {
		        this._noOfActivations = value;
		    }
		}
		
		private DateTime? _activationDate;
		public virtual DateTime? ActivationDate 
		{ 
		    get
		    {
		        return this._activationDate;
		    }
		    set
		    {
		        this._activationDate = value;
		    }
		}
		
		private DateTime? _expirationDate;
		public virtual DateTime? ExpirationDate 
		{ 
		    get
		    {
		        return this._expirationDate;
		    }
		    set
		    {
		        this._expirationDate = value;
		    }
		}
		
		private int? _addressId;
		public virtual int? AddressId 
		{ 
		    get
		    {
		        return this._addressId;
		    }
		    set
		    {
		        this._addressId = value;
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
		
		private int _noOfAllowedPcs;
		public virtual int NoOfAllowedPcs 
		{ 
		    get
		    {
		        return this._noOfAllowedPcs;
		    }
		    set
		    {
		        this._noOfAllowedPcs = value;
		    }
		}
		
		private Address _address;
		public virtual Address Address 
		{ 
		    get
		    {
		        return this._address;
		    }
		    set
		    {
		        this._address = value;
		    }
		}
		
		private IList<Ticket> _tickets = new List<Ticket>();
		public virtual IList<Ticket> Tickets 
		{ 
		    get
		    {
		        return this._tickets;
		    }
		}
		
	}
}
